using DarkUI.Controls;
using DarkUI.Forms;
using Intersect.Editor.Content;
using Intersect.Editor.Core;
using Intersect.Editor.General;
using Intersect.Editor.Localization;
using Intersect.Editor.Networking;
using Intersect.Enums;
using Intersect.Framework.Core.GameObjects.Animations;
using Intersect.Framework.Core.GameObjects.Events;
using Intersect.Framework.Core.GameObjects.Items;
using Intersect.Framework.Core.GameObjects.Maps.MapList;
using Intersect.GameObjects;
using Intersect.Utilities;
using Graphics = System.Drawing.Graphics;

namespace Intersect.Editor.Forms.Editors;


public partial class FrmSpell : EditorForm
{

    private List<SpellDescriptor> mChanged = new List<SpellDescriptor>();

    private string mCopiedItem;

    private SpellDescriptor mEditorItem;

    private List<string> mKnownFolders = new List<string>();

    private List<string> mKnownCooldownGroups = new List<string>();

    public FrmSpell()
    {
        ApplyHooks();
        InitializeComponent();
        Icon = Program.Icon;
        _btnSave = btnSave;
        _btnCancel = btnCancel;

        cmbScalingStat.Items.Clear();
        for (var i = 0; i < Enum.GetValues<Stat>().Length; i++)
        {
            cmbScalingStat.Items.Add(Globals.GetStatName(i));
        }

        lstGameObjects.Init(UpdateToolStripItems, AssignEditorItem, toolStripItemNew_Click, toolStripItemCopy_Click, toolStripItemUndo_Click, toolStripItemPaste_Click, toolStripItemDelete_Click);
    }
    private void AssignEditorItem(Guid id)
    {
        mEditorItem = SpellDescriptor.Get(id);
        UpdateEditor();
    }

    protected override void GameObjectUpdatedDelegate(GameObjectType type)
    {
        if (type == GameObjectType.Spell)
        {
            InitEditor();
            if (mEditorItem != null && !SpellDescriptor.Lookup.Values.Contains(mEditorItem))
            {
                mEditorItem = null;
                UpdateEditor();
            }
        }
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
        foreach (var item in mChanged)
        {
            item.RestoreBackup();
            item.DeleteBackup();
        }

        Hide();
        Globals.CurrentEditor = -1;
        Dispose();
    }

    private void btnSave_Click(object sender, EventArgs e)
    {
        //Send Changed items
        foreach (var item in mChanged)
        {
            PacketSender.SendSaveObject(item);
            item.DeleteBackup();
        }

        Hide();
        Globals.CurrentEditor = -1;
        Dispose();
    }

    private void frmSpell_Load(object sender, EventArgs e)
    {
        cmbProjectile.Items.Clear();
        cmbProjectile.Items.AddRange(ProjectileDescriptor.Names);
        cmbCastAnimation.Items.Clear();
        cmbCastAnimation.Items.Add(Strings.General.None);
        cmbCastAnimation.Items.AddRange(AnimationDescriptor.Names);
        cmbHitAnimation.Items.Clear();
        cmbHitAnimation.Items.Add(Strings.General.None);
        cmbHitAnimation.Items.AddRange(AnimationDescriptor.Names);
        cmbEvent.Items.Clear();
        cmbEvent.Items.Add(Strings.General.None);
        cmbEvent.Items.AddRange(EventDescriptor.Names);
        cmbTickAnimation.Items.Clear();
        cmbTickAnimation.Items.Add(Strings.General.None);
        cmbTickAnimation.Items.AddRange(AnimationDescriptor.Names);

        cmbSprite.Items.Clear();
        cmbSprite.Items.Add(Strings.General.None);
        var spellNames = GameContentManager.GetSmartSortedTextureNames(GameContentManager.TextureType.Spell);
        cmbSprite.Items.AddRange(spellNames);

        cmbTransform.Items.Clear();
        cmbTransform.Items.Add(Strings.General.None);
        var spriteNames = GameContentManager.GetSmartSortedTextureNames(GameContentManager.TextureType.Entity);
        cmbTransform.Items.AddRange(spriteNames);

        cmbCastSprite.Items.Clear();
        cmbCastSprite.Items.Add(Strings.General.None);
        cmbCastSprite.Items.AddRange(
            GameContentManager.GetOverridesFor(GameContentManager.TextureType.Entity, "cast").ToArray()
        );

        nudWarpX.Maximum = (int)Options.Instance.Map.MapWidth;
        nudWarpY.Maximum = (int)Options.Instance.Map.MapHeight;

        cmbWarpMap.Items.Clear();
        cmbWarpMap.Items.AddRange(MapList.OrderedMaps.Select(map => map?.Name).ToArray());
        cmbWarpMap.SelectedIndex = 0;

        nudStr.Maximum = Options.Instance.Player.MaxStat;
        nudMag.Maximum = Options.Instance.Player.MaxStat;
        nudDef.Maximum = Options.Instance.Player.MaxStat;
        nudMR.Maximum = Options.Instance.Player.MaxStat;
        nudSpd.Maximum = Options.Instance.Player.MaxStat;
        nudStr.Minimum = -Options.Instance.Player.MaxStat;
        nudMag.Minimum = -Options.Instance.Player.MaxStat;
        nudDef.Minimum = -Options.Instance.Player.MaxStat;
        nudMR.Minimum = -Options.Instance.Player.MaxStat;
        nudSpd.Minimum = -Options.Instance.Player.MaxStat;

        nudCastDuration.Maximum = Int32.MaxValue;
        nudCooldownDuration.Maximum = Int32.MaxValue;

        InitLocalization();
        UpdateEditor();
    }

    private void InitLocalization()
    {
        Text = Strings.SpellEditor.title;
        toolStripItemNew.Text = Strings.SpellEditor.New;
        toolStripItemDelete.Text = Strings.SpellEditor.delete;
        toolStripItemCopy.Text = Strings.SpellEditor.copy;
        toolStripItemPaste.Text = Strings.SpellEditor.paste;
        toolStripItemUndo.Text = Strings.SpellEditor.undo;

        grpSpells.Text = Strings.SpellEditor.spells;

        grpGeneral.Text = Strings.SpellEditor.general;
        lblName.Text = Strings.SpellEditor.name;
        lblType.Text = Strings.SpellEditor.type;
        cmbType.Items.Clear();
        for (var i = 0; i < Strings.SpellEditor.types.Count; i++)
        {
            cmbType.Items.Add(Strings.SpellEditor.types[i]);
        }

        lblIcon.Text = Strings.SpellEditor.icon;
        lblDesc.Text = Strings.SpellEditor.description;
        lblCastAnimation.Text = Strings.SpellEditor.castanimation;
        lblSpriteCastAnimation.Text = Strings.SpellEditor.CastSpriteOverride;
        lblHitAnimation.Text = Strings.SpellEditor.hitanimation;
        chkBound.Text = Strings.SpellEditor.bound;

        grpRequirements.Text = Strings.SpellEditor.requirements;
        lblCannotCast.Text = Strings.SpellEditor.cannotcast;
        btnDynamicRequirements.Text = Strings.SpellEditor.requirementsbutton;

        grpSpellCost.Text = Strings.SpellEditor.cost;
        lblHPCost.Text = Strings.SpellEditor.hpcost;
        lblMPCost.Text = Strings.SpellEditor.manacost;
        lblCastDuration.Text = Strings.SpellEditor.casttime;
        lblCooldownDuration.Text = Strings.SpellEditor.cooldown;
        lblCooldownGroup.Text = Strings.SpellEditor.CooldownGroup;
        chkIgnoreGlobalCooldown.Text = Strings.SpellEditor.IgnoreGlobalCooldown;
        chkIgnoreCdr.Text = Strings.SpellEditor.IgnoreCooldownReduction;

        grpTargetInfo.Text = Strings.SpellEditor.targetting;
        lblTargetType.Text = Strings.SpellEditor.targettype;
        cmbTargetType.Items.Clear();
        for (var i = 0; i < Strings.SpellEditor.targettypes.Count; i++)
        {
            cmbTargetType.Items.Add(Strings.SpellEditor.targettypes[i]);
        }

        lblCastRange.Text = Strings.SpellEditor.castrange;
        lblProjectile.Text = Strings.SpellEditor.projectile;
        lblHitRadius.Text = Strings.SpellEditor.hitradius;
        lblDuration.Text = Strings.SpellEditor.duration;

        grpCombat.Text = Strings.SpellEditor.combatspell;
        grpDamage.Text = Strings.SpellEditor.damagegroup;
        lblCritChance.Text = Strings.SpellEditor.critchance;
        lblCritMultiplier.Text = Strings.SpellEditor.critmultiplier;
        lblDamageType.Text = Strings.SpellEditor.damagetype;
        lblHPDamage.Text = Strings.SpellEditor.hpdamage;
        lblManaDamage.Text = Strings.SpellEditor.mpdamage;
        chkFriendly.Text = Strings.SpellEditor.friendly;
        cmbDamageType.Items.Clear();
        for (var i = 0; i < Strings.Combat.damagetypes.Count; i++)
        {
            cmbDamageType.Items.Add(Strings.Combat.damagetypes[i]);
        }

        lblScalingStat.Text = Strings.SpellEditor.scalingstat;
        lblScaling.Text = Strings.SpellEditor.scalingamount;

        grpHotDot.Text = Strings.SpellEditor.hotdot;
        chkHOTDOT.Text = Strings.SpellEditor.ishotdot;
        lblTick.Text = Strings.SpellEditor.hotdottick;
        lblTickAnimation.Text = Strings.SpellEditor.TickAnimation;

        grpStats.Text = Strings.SpellEditor.stats;
        lblStr.Text = Strings.SpellEditor.strength;
        lblDef.Text = Strings.SpellEditor.defense;
        lblSpd.Text = Strings.SpellEditor.agility;
        lblMag.Text = Strings.SpellEditor.intelligence;
        lblMR.Text = Strings.SpellEditor.faith;

        grpEffectDuration.Text = Strings.SpellEditor.boostduration;
        lblBuffDuration.Text = Strings.SpellEditor.duration;
        grpEffect.Text = Strings.SpellEditor.effectgroup;
        lblEffect.Text = Strings.SpellEditor.effectlabel;
        cmbExtraEffect.Items.Clear();
        for (var i = 0; i < Strings.SpellEditor.effects.Count; i++)
        {
            cmbExtraEffect.Items.Add(Strings.SpellEditor.effects[i]);
        }

        lblSprite.Text = Strings.SpellEditor.transformsprite;

        grpDash.Text = Strings.SpellEditor.dash;
        lblRange.Text = Strings.SpellEditor.dashrange.ToString(scrlRange.Value);
        grpDashCollisions.Text = Strings.SpellEditor.dashcollisions;
        chkIgnoreMapBlocks.Text = Strings.SpellEditor.ignoreblocks;
        chkIgnoreActiveResources.Text = Strings.SpellEditor.ignoreactiveresources;
        chkIgnoreInactiveResources.Text = Strings.SpellEditor.ignoreinactiveresources;
        chkIgnoreZDimensionBlocks.Text = Strings.SpellEditor.ignorezdimension;

        grpWarp.Text = Strings.SpellEditor.warptomap;
        lblMap.Text = Strings.Warping.map.ToString("");
        lblX.Text = Strings.Warping.x.ToString("");
        lblY.Text = Strings.Warping.y.ToString("");
        lblWarpDir.Text = Strings.Warping.direction.ToString("");
        cmbDirection.Items.Clear();
        for (var i = -1; i < 4; i++)
        {
            cmbDirection.Items.Add(Strings.Direction.dir[(Direction)i]);
        }

        btnVisualMapSelector.Text = Strings.Warping.visual;

        grpEvent.Text = Strings.SpellEditor.Event;

        //Searching/Sorting
        btnAlphabetical.ToolTipText = Strings.SpellEditor.sortalphabetically;
        txtSearch.Text = Strings.SpellEditor.searchplaceholder;
        lblFolder.Text = Strings.SpellEditor.folderlabel;

        btnSave.Text = Strings.SpellEditor.save;
        btnCancel.Text = Strings.SpellEditor.cancel;
    }

    private void UpdateEditor()
    {
        if (mEditorItem != null)
        {
            pnlContainer.Show();

            txtName.Text = mEditorItem.Name;
            cmbFolder.Text = mEditorItem.Folder;
            txtDesc.Text = mEditorItem.Description;
            cmbType.SelectedIndex = (int)mEditorItem.SpellType;

            nudCastDuration.Value = mEditorItem.CastDuration;
            nudCooldownDuration.Value = mEditorItem.CooldownDuration;
            cmbCooldownGroup.SelectedItem = mEditorItem.CooldownGroup;
            chkIgnoreGlobalCooldown.Checked = mEditorItem.IgnoreGlobalCooldown;
            chkIgnoreCdr.Checked = mEditorItem.IgnoreCooldownReduction;

            cmbCastAnimation.SelectedIndex = AnimationDescriptor.ListIndex(mEditorItem.CastAnimationId) + 1;
            cmbHitAnimation.SelectedIndex = AnimationDescriptor.ListIndex(mEditorItem.HitAnimationId) + 1;
            cmbTickAnimation.SelectedIndex = AnimationDescriptor.ListIndex(mEditorItem.TickAnimationId) + 1;
            cmbCastSprite.SelectedIndex = cmbCastSprite.FindString(
                    TextUtils.NullToNone(mEditorItem.CastSpriteOverride)
            );

            chkBound.Checked = mEditorItem.Bound;

            cmbSprite.SelectedIndex = cmbSprite.FindString(TextUtils.NullToNone(mEditorItem.Icon));
            picSpell.BackgroundImage?.Dispose();
            picSpell.BackgroundImage = null;
            if (cmbSprite.SelectedIndex > 0)
            {
                picSpell.BackgroundImage = Image.FromFile("resources/spells/" + cmbSprite.Text);
            }

            nudHPCost.Value = mEditorItem.VitalCost[(int)Vital.Health];
            nudMpCost.Value = mEditorItem.VitalCost[(int)Vital.Mana];

            txtCannotCast.Text = mEditorItem.CannotCastMessage;

            UpdateSpellTypePanels();
            if (mChanged.IndexOf(mEditorItem) == -1)
            {
                mChanged.Add(mEditorItem);
                mEditorItem.MakeBackup();
            }
        }
        else
        {
            pnlContainer.Hide();
        }

        var hasItem = mEditorItem != null;
        UpdateEditorButtons(hasItem);
        UpdateToolStripItems();
    }

    private void UpdateSpellTypePanels()
    {
        grpTargetInfo.Hide();
        grpCombat.Hide();
        grpWarp.Hide();
        grpDash.Hide();
        grpEvent.Hide();
        cmbTargetType.Enabled = true;

        // Reset our combat data location, since event type spells can move it.
        grpCombat.Location = new System.Drawing.Point(grpEvent.Location.X, grpEvent.Location.Y);

        if (cmbType.SelectedIndex == (int)SpellType.CombatSpell ||
            cmbType.SelectedIndex == (int)SpellType.WarpTo ||
            cmbType.SelectedIndex == (int)SpellType.Event)
        {
            grpTargetInfo.Show();
            grpCombat.Show();
            cmbTargetType.SelectedIndex = (int)mEditorItem.Combat.TargetType;
            UpdateTargetTypePanel();

            nudHPDamage.Value = mEditorItem.Combat.VitalDiff[(int)Vital.Health];
            nudMPDamage.Value = mEditorItem.Combat.VitalDiff[(int)Vital.Mana];

            nudStr.Value = mEditorItem.Combat.StatDiff[(int)Stat.Strength];
            nudDef.Value = mEditorItem.Combat.StatDiff[(int)Stat.Defense];
            nudSpd.Value = mEditorItem.Combat.StatDiff[(int)Stat.Agility];
            nudMag.Value = mEditorItem.Combat.StatDiff[(int)Stat.Intelligence];
            nudMR.Value = mEditorItem.Combat.StatDiff[(int)Stat.Faith];

            nudStrPercentage.Value = mEditorItem.Combat.PercentageStatDiff[(int)Stat.Strength];
            nudDefPercentage.Value = mEditorItem.Combat.PercentageStatDiff[(int)Stat.Defense];
            nudMagPercentage.Value = mEditorItem.Combat.PercentageStatDiff[(int)Stat.Intelligence];
            nudMRPercentage.Value = mEditorItem.Combat.PercentageStatDiff[(int)Stat.Faith];
            nudSpdPercentage.Value = mEditorItem.Combat.PercentageStatDiff[(int)Stat.Agility];

            chkFriendly.Checked = Convert.ToBoolean(mEditorItem.Combat.Friendly);
            cmbDamageType.SelectedIndex = mEditorItem.Combat.DamageType;
            cmbScalingStat.SelectedIndex = mEditorItem.Combat.ScalingStat;
            nudScaling.Value = mEditorItem.Combat.Scaling;
            nudCritChance.Value = mEditorItem.Combat.CritChance;
            nudCritMultiplier.Value = (decimal)mEditorItem.Combat.CritMultiplier;

            chkHOTDOT.Checked = mEditorItem.Combat.HoTDoT;
            nudBuffDuration.Value = mEditorItem.Combat.Duration;
            nudTick.Value = mEditorItem.Combat.HotDotInterval;
            cmbExtraEffect.SelectedIndex = (int)mEditorItem.Combat.Effect;
            cmbExtraEffect_SelectedIndexChanged(null, null);
        }
        else if (cmbType.SelectedIndex == (int)SpellType.Warp)
        {
            grpWarp.Show();
            for (var i = 0; i < MapList.OrderedMaps.Count; i++)
            {
                if (MapList.OrderedMaps[i].MapId == mEditorItem.Warp.MapId)
                {
                    cmbWarpMap.SelectedIndex = i;

                    break;
                }
            }

            nudWarpX.Value = mEditorItem.Warp.X;
            nudWarpY.Value = mEditorItem.Warp.Y;
            cmbDirection.SelectedIndex = mEditorItem.Warp.Dir;
        }
        else if (cmbType.SelectedIndex == (int)SpellType.Dash)
        {
            grpDash.Show();
            scrlRange.Value = mEditorItem.Combat.CastRange;
            lblRange.Text = Strings.SpellEditor.dashrange.ToString(scrlRange.Value);
            chkIgnoreMapBlocks.Checked = mEditorItem.Dash.IgnoreMapBlocks;
            chkIgnoreActiveResources.Checked = mEditorItem.Dash.IgnoreActiveResources;
            chkIgnoreInactiveResources.Checked = mEditorItem.Dash.IgnoreInactiveResources;
            chkIgnoreZDimensionBlocks.Checked = mEditorItem.Dash.IgnoreZDimensionAttributes;
        }

        if (cmbType.SelectedIndex == (int)SpellType.Event)
        {
            grpEvent.Show();
            cmbEvent.SelectedIndex = EventDescriptor.ListIndex(mEditorItem.EventId) + 1;
            // Move our combat data down a little bit, it's not a very clean solution but it'll let us display it properly.
            grpCombat.Location = new System.Drawing.Point(grpEvent.Location.X, grpEvent.Location.Y + grpEvent.Size.Height + 5);
        }

        if (cmbType.SelectedIndex == (int)SpellType.WarpTo)
        {
            grpTargetInfo.Show();
            cmbTargetType.SelectedIndex = (int)SpellTargetType.Single;
            cmbTargetType.Enabled = false;
            UpdateTargetTypePanel();
        }
    }

    private void UpdateTargetTypePanel()
    {
        lblHitRadius.Hide();
        nudHitRadius.Hide();
        lblCastRange.Hide();
        nudCastRange.Hide();
        lblProjectile.Hide();
        cmbProjectile.Hide();
        lblDuration.Hide();
        nudDuration.Hide();

        if (cmbTargetType.SelectedIndex == (int)SpellTargetType.Single)
        {
            lblCastRange.Show();
            nudCastRange.Show();
            nudCastRange.Value = mEditorItem.Combat.CastRange;
            if (cmbType.SelectedIndex == (int)SpellType.CombatSpell)
            {
                lblHitRadius.Show();
                nudHitRadius.Show();
                nudHitRadius.Value = mEditorItem.Combat.HitRadius;
            }
        }

        if (cmbTargetType.SelectedIndex == (int)SpellTargetType.AoE &&
            cmbType.SelectedIndex == (int)SpellType.CombatSpell)
        {
            lblHitRadius.Show();
            nudHitRadius.Show();
            nudHitRadius.Value = mEditorItem.Combat.HitRadius;
        }

        if (cmbTargetType.SelectedIndex < (int)SpellTargetType.Self)
        {
            lblCastRange.Show();
            nudCastRange.Show();
            nudCastRange.Value = mEditorItem.Combat.CastRange;
        }

        if (cmbTargetType.SelectedIndex == (int)SpellTargetType.Projectile)
        {
            lblProjectile.Show();
            cmbProjectile.Show();
            cmbProjectile.SelectedIndex = ProjectileDescriptor.ListIndex(mEditorItem.Combat.ProjectileId);
        }

        if (cmbTargetType.SelectedIndex == (int)SpellTargetType.OnHit)
        {
            lblDuration.Show();
            nudDuration.Show();
            nudDuration.Value = mEditorItem.Combat.OnHitDuration;
        }

        if (cmbTargetType.SelectedIndex == (int)SpellTargetType.Trap)
        {
            lblDuration.Show();
            nudDuration.Show();
            nudDuration.Value = mEditorItem.Combat.TrapDuration;
        }
    }

    private void txtName_TextChanged(object sender, EventArgs e)
    {
        mEditorItem.Name = txtName.Text;
        lstGameObjects.UpdateText(txtName.Text);
    }

    private void cmbType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (cmbType.SelectedIndex != (int)mEditorItem.SpellType)
        {
            mEditorItem.SpellType = (SpellType)cmbType.SelectedIndex;
            UpdateSpellTypePanels();
        }
    }

    private void cmbSprite_SelectedIndexChanged(object sender, EventArgs e)
    {
        mEditorItem.Icon = cmbSprite.Text;
        picSpell.BackgroundImage?.Dispose();
        picSpell.BackgroundImage = null;
        picSpell.BackgroundImage = cmbSprite.SelectedIndex > 0
            ? Image.FromFile("resources/spells/" + cmbSprite.Text)
            : null;
    }

    private void cmbTargetType_SelectedIndexChanged(object sender, EventArgs e)
    {
        mEditorItem.Combat.TargetType = (SpellTargetType)cmbTargetType.SelectedIndex;
        UpdateTargetTypePanel();
    }

    private void chkHOTDOT_CheckedChanged(object sender, EventArgs e)
    {
        mEditorItem.Combat.HoTDoT = chkHOTDOT.Checked;
    }

    private void txtDesc_TextChanged(object sender, EventArgs e)
    {
        mEditorItem.Description = txtDesc.Text;
    }

    private void cmbExtraEffect_SelectedIndexChanged(object sender, EventArgs e)
    {
        mEditorItem.Combat.Effect = (SpellEffect)cmbExtraEffect.SelectedIndex;

        lblSprite.Visible = false;
        cmbTransform.Visible = false;
        picSprite.Visible = false;

        if (cmbExtraEffect.SelectedIndex == 6) //Transform
        {
            lblSprite.Visible = true;
            cmbTransform.Visible = true;
            picSprite.Visible = true;

            cmbTransform.SelectedIndex =
                cmbTransform.FindString(TextUtils.NullToNone(mEditorItem.Combat.TransformSprite));

            if (cmbTransform.SelectedIndex > 0)
            {
                var bmp = new Bitmap(picSprite.Width, picSprite.Height);
                var g = Graphics.FromImage(bmp);
                var src = Image.FromFile("resources/entities/" + cmbTransform.Text);
                g.DrawImage(
                    src,
                    new Rectangle(
                        picSprite.Width / 2 - src.Width / (Options.Instance.Sprites.NormalFrames * 2), picSprite.Height / 2 - src.Height / (Options.Instance.Sprites.Directions * 2), src.Width / Options.Instance.Sprites.NormalFrames,
                        src.Height / Options.Instance.Sprites.Directions
                    ), new Rectangle(0, 0, src.Width / Options.Instance.Sprites.NormalFrames, src.Height / Options.Instance.Sprites.Directions), GraphicsUnit.Pixel
                );

                g.Dispose();
                src.Dispose();
                picSprite.BackgroundImage = bmp;
            }
            else
            {
                picSprite.BackgroundImage = null;
            }
        }
    }

    private void frmSpell_FormClosed(object sender, FormClosedEventArgs e)
    {
        btnCancel_Click(null, null);
    }

    private void scrlRange_Scroll(object sender, ScrollValueEventArgs e)
    {
        lblRange.Text = Strings.SpellEditor.dashrange.ToString(scrlRange.Value);
        mEditorItem.Combat.CastRange = scrlRange.Value;
    }

    private void chkIgnoreMapBlocks_CheckedChanged(object sender, EventArgs e)
    {
        mEditorItem.Dash.IgnoreMapBlocks = chkIgnoreMapBlocks.Checked;
    }

    private void chkIgnoreActiveResources_CheckedChanged(object sender, EventArgs e)
    {
        mEditorItem.Dash.IgnoreActiveResources = chkIgnoreActiveResources.Checked;
    }

    private void chkIgnoreInactiveResources_CheckedChanged(object sender, EventArgs e)
    {
        mEditorItem.Dash.IgnoreInactiveResources = chkIgnoreInactiveResources.Checked;
    }

    private void chkIgnoreZDimensionBlocks_CheckedChanged(object sender, EventArgs e)
    {
        mEditorItem.Dash.IgnoreZDimensionAttributes = chkIgnoreZDimensionBlocks.Checked;
    }

    private void cmbTransform_SelectedIndexChanged(object sender, EventArgs e)
    {
        mEditorItem.Combat.TransformSprite = cmbTransform.Text;
        if (cmbTransform.SelectedIndex > 0)
        {
            var bmp = new Bitmap(picSprite.Width, picSprite.Height);
            var g = Graphics.FromImage(bmp);
            var src = Image.FromFile("resources/entities/" + cmbTransform.Text);
            g.DrawImage(
                src,
                new Rectangle(
                    picSprite.Width / 2 - src.Width / (Options.Instance.Sprites.NormalFrames * 2), picSprite.Height / 2 - src.Height / (Options.Instance.Sprites.Directions * 2), src.Width / Options.Instance.Sprites.NormalFrames,
                    src.Height / Options.Instance.Sprites.Directions
                ), new Rectangle(0, 0, src.Width / Options.Instance.Sprites.NormalFrames, src.Height / Options.Instance.Sprites.Directions), GraphicsUnit.Pixel
            );

            g.Dispose();
            src.Dispose();
            picSprite.BackgroundImage = bmp;
        }
        else
        {
            picSprite.BackgroundImage = null;
        }
    }

    private void toolStripItemNew_Click(object sender, EventArgs e)
    {
        PacketSender.SendCreateObject(GameObjectType.Spell);
    }

    private void toolStripItemDelete_Click(object sender, EventArgs e)
    {
        if (mEditorItem != null && lstGameObjects.Focused)
        {
            if (DarkMessageBox.ShowWarning(
                    Strings.SpellEditor.deleteprompt, Strings.SpellEditor.deletetitle, DarkDialogButton.YesNo,
                    Icon
                ) ==
                DialogResult.Yes)
            {
                PacketSender.SendDeleteObject(mEditorItem);
            }
        }
    }

    private void toolStripItemCopy_Click(object sender, EventArgs e)
    {
        if (mEditorItem != null && lstGameObjects.Focused)
        {
            mCopiedItem = mEditorItem.JsonData;
            toolStripItemPaste.Enabled = true;
        }
    }

    private void toolStripItemPaste_Click(object sender, EventArgs e)
    {
        if (mEditorItem != null && mCopiedItem != null && lstGameObjects.Focused)
        {
            mEditorItem.Load(mCopiedItem, true);
            UpdateEditor();
        }
    }

    private void toolStripItemUndo_Click(object sender, EventArgs e)
    {
        if (mChanged.Contains(mEditorItem) && mEditorItem != null)
        {
            if (DarkMessageBox.ShowWarning(
                    Strings.SpellEditor.undoprompt, Strings.SpellEditor.undotitle, DarkDialogButton.YesNo,
                    Icon
                ) ==
                DialogResult.Yes)
            {
                mEditorItem.RestoreBackup();
                UpdateEditor();
            }
        }
    }

    private void UpdateToolStripItems()
    {
        toolStripItemCopy.Enabled = mEditorItem != null && lstGameObjects.Focused;
        toolStripItemPaste.Enabled = mEditorItem != null && mCopiedItem != null && lstGameObjects.Focused;
        toolStripItemDelete.Enabled = mEditorItem != null && lstGameObjects.Focused;
        toolStripItemUndo.Enabled = mEditorItem != null && lstGameObjects.Focused;
    }

    private void form_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Control)
        {
            if (e.KeyCode == Keys.N)
            {
                toolStripItemNew_Click(null, null);
            }
        }
    }

    private void chkFriendly_CheckedChanged(object sender, EventArgs e)
    {
        mEditorItem.Combat.Friendly = chkFriendly.Checked;
    }

    private void cmbDamageType_SelectedIndexChanged(object sender, EventArgs e)
    {
        mEditorItem.Combat.DamageType = cmbDamageType.SelectedIndex;
    }

    private void cmbScalingStat_SelectedIndexChanged(object sender, EventArgs e)
    {
        mEditorItem.Combat.ScalingStat = cmbScalingStat.SelectedIndex;
    }

    private void btnDynamicRequirements_Click(object sender, EventArgs e)
    {
        var frm = new FrmDynamicRequirements(mEditorItem.CastingRequirements, RequirementType.Spell);
        frm.ShowDialog();
    }

    private void cmbCastSprite_SelectedIndexChanged(object sender, EventArgs e)
    {
        mEditorItem.CastSpriteOverride = TextUtils.SanitizeNone(cmbCastSprite?.Text);
    }

    private void cmbCastAnimation_SelectedIndexChanged(object sender, EventArgs e)
    {
        mEditorItem.CastAnimation = AnimationDescriptor.Get(AnimationDescriptor.IdFromList(cmbCastAnimation.SelectedIndex - 1));
    }

    private void cmbHitAnimation_SelectedIndexChanged(object sender, EventArgs e)
    {
        mEditorItem.HitAnimation = AnimationDescriptor.Get(AnimationDescriptor.IdFromList(cmbHitAnimation.SelectedIndex - 1));
    }

    private void cmbProjectile_SelectedIndexChanged(object sender, EventArgs e)
    {
        mEditorItem.Combat.ProjectileId = ProjectileDescriptor.IdFromList(cmbProjectile.SelectedIndex);
    }

    private void cmbEvent_SelectedIndexChanged(object sender, EventArgs e)
    {
        mEditorItem.EventId = EventDescriptor.IdFromList(cmbEvent.SelectedIndex - 1);
    }

    private void btnVisualMapSelector_Click(object sender, EventArgs e)
    {
        var frmWarpSelection = new FrmWarpSelection();
        frmWarpSelection.SelectTile(
            MapList.OrderedMaps[cmbWarpMap.SelectedIndex].MapId, (int)nudWarpX.Value, (int)nudWarpY.Value
        );

        frmWarpSelection.ShowDialog();
        if (frmWarpSelection.GetResult())
        {
            for (var i = 0; i < MapList.OrderedMaps.Count; i++)
            {
                if (MapList.OrderedMaps[i].MapId == frmWarpSelection.GetMap())
                {
                    cmbWarpMap.SelectedIndex = i;
                    mEditorItem.Warp.MapId = MapList.OrderedMaps[i].MapId;

                    break;
                }
            }

            nudWarpX.Value = frmWarpSelection.GetX();
            mEditorItem.Warp.X = frmWarpSelection.GetX();
            nudWarpY.Value = frmWarpSelection.GetY();
            mEditorItem.Warp.Y = frmWarpSelection.GetY();
        }
    }

    private void cmbWarpMap_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (cmbWarpMap.SelectedIndex > -1 && mEditorItem != null)
        {
            mEditorItem.Warp.MapId = MapList.OrderedMaps[cmbWarpMap.SelectedIndex].MapId;
        }
    }

    private void nudWarpX_ValueChanged(object sender, EventArgs e)
    {
        mEditorItem.Warp.X = (byte)nudWarpX.Value;
    }

    private void nudWarpY_ValueChanged(object sender, EventArgs e)
    {
        mEditorItem.Warp.Y = (byte)nudWarpY.Value;
    }

    private void cmbDirection_SelectedIndexChanged(object sender, EventArgs e)
    {
        mEditorItem.Warp.Dir = (byte)cmbDirection.SelectedIndex;
    }

    private void nudCastDuration_ValueChanged(object sender, EventArgs e)
    {
        mEditorItem.CastDuration = (int)nudCastDuration.Value;
    }

    private void nudCooldownDuration_ValueChanged(object sender, EventArgs e)
    {
        mEditorItem.CooldownDuration = (int)nudCooldownDuration.Value;
    }

    private void nudHitRadius_ValueChanged(object sender, EventArgs e)
    {
        mEditorItem.Combat.HitRadius = (int)nudHitRadius.Value;
    }

    private void nudHPCost_ValueChanged(object sender, EventArgs e)
    {
        mEditorItem.VitalCost[(int)Vital.Health] = (int)nudHPCost.Value;
    }

    private void nudMpCost_ValueChanged(object sender, EventArgs e)
    {
        mEditorItem.VitalCost[(int)Vital.Mana] = (int)nudMpCost.Value;
    }

    private void nudHPDamage_ValueChanged(object sender, EventArgs e)
    {
        mEditorItem.Combat.VitalDiff[(int)Vital.Health] = (int)nudHPDamage.Value;
    }

    private void nudMPDamage_ValueChanged(object sender, EventArgs e)
    {
        mEditorItem.Combat.VitalDiff[(int)Vital.Mana] = (int)nudMPDamage.Value;
    }

    private void nudStr_ValueChanged(object sender, EventArgs e)
    {
        mEditorItem.Combat.StatDiff[(int)Stat.Strength] = (int)nudStr.Value;
    }

    private void nudMag_ValueChanged(object sender, EventArgs e)
    {
        mEditorItem.Combat.StatDiff[(int)Stat.Intelligence] = (int)nudMag.Value;
    }

    private void nudDef_ValueChanged(object sender, EventArgs e)
    {
        mEditorItem.Combat.StatDiff[(int)Stat.Defense] = (int)nudDef.Value;
    }

    private void nudMR_ValueChanged(object sender, EventArgs e)
    {
        mEditorItem.Combat.StatDiff[(int)Stat.Faith] = (int)nudMR.Value;
    }

    private void nudSpd_ValueChanged(object sender, EventArgs e)
    {
        mEditorItem.Combat.StatDiff[(int)Stat.Agility] = (int)nudSpd.Value;
    }

    private void nudStrPercentage_ValueChanged(object sender, EventArgs e)
    {
        mEditorItem.Combat.PercentageStatDiff[(int)Stat.Strength] = (int)nudStrPercentage.Value;
    }

    private void nudMagPercentage_ValueChanged(object sender, EventArgs e)
    {
        mEditorItem.Combat.PercentageStatDiff[(int)Stat.Intelligence] = (int)nudMagPercentage.Value;
    }

    private void nudDefPercentage_ValueChanged(object sender, EventArgs e)
    {
        mEditorItem.Combat.PercentageStatDiff[(int)Stat.Defense] = (int)nudDefPercentage.Value;
    }

    private void nudMRPercentage_ValueChanged(object sender, EventArgs e)
    {
        mEditorItem.Combat.PercentageStatDiff[(int)Stat.Faith] = (int)nudMRPercentage.Value;
    }

    private void nudSpdPercentage_ValueChanged(object sender, EventArgs e)
    {
        mEditorItem.Combat.PercentageStatDiff[(int)Stat.Agility] = (int)nudSpdPercentage.Value;
    }

    private void nudBuffDuration_ValueChanged(object sender, EventArgs e)
    {
        mEditorItem.Combat.Duration = (int)nudBuffDuration.Value;
    }

    private void nudTick_ValueChanged(object sender, EventArgs e)
    {
        mEditorItem.Combat.HotDotInterval = (int)nudTick.Value;
    }

    private void nudCritChance_ValueChanged(object sender, EventArgs e)
    {
        mEditorItem.Combat.CritChance = (int)nudCritChance.Value;
    }

    private void nudScaling_ValueChanged(object sender, EventArgs e)
    {
        mEditorItem.Combat.Scaling = (int)nudScaling.Value;
    }

    private void nudCastRange_ValueChanged(object sender, EventArgs e)
    {
        mEditorItem.Combat.CastRange = (int)nudCastRange.Value;
    }

    private void nudCritMultiplier_ValueChanged(object sender, EventArgs e)
    {
        mEditorItem.Combat.CritMultiplier = (double)nudCritMultiplier.Value;
    }

    private void nudOnHitDuration_ValueChanged(object sender, EventArgs e)
    {
        if (cmbTargetType.SelectedIndex == (int)SpellTargetType.OnHit)
        {
            mEditorItem.Combat.OnHitDuration = (int)nudDuration.Value;
        }

        if (cmbTargetType.SelectedIndex == (int)SpellTargetType.Trap)
        {
            mEditorItem.Combat.TrapDuration = (int)nudDuration.Value;
        }
    }

    private void chkBound_CheckedChanged(object sender, EventArgs e)
    {
        mEditorItem.Bound = chkBound.Checked;
    }

    private void btnAddCooldownGroup_Click(object sender, EventArgs e)
    {
        var cdGroupName = string.Empty;
        var result = DarkInputBox.ShowInformation(
            Strings.SpellEditor.CooldownGroupPrompt, Strings.SpellEditor.CooldownGroupTitle, ref cdGroupName,
            DarkDialogButton.OkCancel
        );

        if (result == DialogResult.OK && !string.IsNullOrEmpty(cdGroupName))
        {
            if (!cmbCooldownGroup.Items.Contains(cdGroupName))
            {
                mEditorItem.CooldownGroup = cdGroupName;
                mKnownCooldownGroups.Add(cdGroupName);
                InitEditor();
                cmbCooldownGroup.Text = cdGroupName;
            }
        }
    }

    private void cmbCooldownGroup_SelectedIndexChanged(object sender, EventArgs e)
    {
        mEditorItem.CooldownGroup = cmbCooldownGroup.Text;
    }

    private void chkIgnoreGlobalCooldown_CheckedChanged(object sender, EventArgs e)
    {
        mEditorItem.IgnoreGlobalCooldown = chkIgnoreGlobalCooldown.Checked;
    }

    private void chkIgnoreCdr_CheckedChanged(object sender, EventArgs e)
    {
        mEditorItem.IgnoreCooldownReduction = chkIgnoreCdr.Checked;
    }

    private void txtCannotCast_TextChanged(object sender, EventArgs e)
    {
        mEditorItem.CannotCastMessage = txtCannotCast.Text;
    }

    #region "Item List - Folders, Searching, Sorting, Etc"

    public void InitEditor()
    {
        //Collect folders
        var mFolders = new List<string>();
        foreach (var itm in SpellDescriptor.Lookup)
        {
            if (!string.IsNullOrEmpty(((SpellDescriptor)itm.Value).Folder) &&
                !mFolders.Contains(((SpellDescriptor)itm.Value).Folder))
            {
                mFolders.Add(((SpellDescriptor)itm.Value).Folder);
                if (!mKnownFolders.Contains(((SpellDescriptor)itm.Value).Folder))
                {
                    mKnownFolders.Add(((SpellDescriptor)itm.Value).Folder);
                }
            }

            if (!string.IsNullOrWhiteSpace(((SpellDescriptor)itm.Value).CooldownGroup) &&
                !mKnownCooldownGroups.Contains(((SpellDescriptor)itm.Value).CooldownGroup))
            {
                mKnownCooldownGroups.Add(((SpellDescriptor)itm.Value).CooldownGroup);
            }
        }

        // Do we add item cooldown groups as well?
        if (Options.Instance.Combat.LinkSpellAndItemCooldowns)
        {
            foreach (var itm in ItemDescriptor.Lookup)
            {
                if (!string.IsNullOrWhiteSpace(((ItemDescriptor)itm.Value).CooldownGroup) &&
                !mKnownCooldownGroups.Contains(((ItemDescriptor)itm.Value).CooldownGroup))
                {
                    mKnownCooldownGroups.Add(((ItemDescriptor)itm.Value).CooldownGroup);
                }
            }
        }

        mFolders.Sort();
        mKnownFolders.Sort();
        cmbFolder.Items.Clear();
        cmbFolder.Items.Add("");
        cmbFolder.Items.AddRange(mKnownFolders.ToArray());

        mKnownCooldownGroups.Sort();
        cmbCooldownGroup.Items.Clear();
        cmbCooldownGroup.Items.Add(string.Empty);
        cmbCooldownGroup.Items.AddRange(mKnownCooldownGroups.ToArray());

        var items = SpellDescriptor.Lookup.OrderBy(p => p.Value?.Name).Select(pair => new KeyValuePair<Guid, KeyValuePair<string, string>>(pair.Key,
            new KeyValuePair<string, string>(((SpellDescriptor)pair.Value)?.Name ?? Models.DatabaseObject<SpellDescriptor>.Deleted, ((SpellDescriptor)pair.Value)?.Folder ?? ""))).ToArray();
        lstGameObjects.Repopulate(items, mFolders, btnAlphabetical.Checked, CustomSearch(), txtSearch.Text);
    }

    private void btnAddFolder_Click(object sender, EventArgs e)
    {
        var folderName = string.Empty;
        var result = DarkInputBox.ShowInformation(
            Strings.SpellEditor.folderprompt, Strings.SpellEditor.foldertitle, ref folderName,
            DarkDialogButton.OkCancel
        );

        if (result == DialogResult.OK && !string.IsNullOrEmpty(folderName))
        {
            if (!cmbFolder.Items.Contains(folderName))
            {
                mEditorItem.Folder = folderName;
                lstGameObjects.ExpandFolder(folderName);
                InitEditor();
                cmbFolder.Text = folderName;
            }
        }
    }

    private void cmbFolder_SelectedIndexChanged(object sender, EventArgs e)
    {
        mEditorItem.Folder = cmbFolder.Text;
        InitEditor();
    }

    private void btnAlphabetical_Click(object sender, EventArgs e)
    {
        btnAlphabetical.Checked = !btnAlphabetical.Checked;
        InitEditor();
    }

    private void txtSearch_TextChanged(object sender, EventArgs e)
    {
        InitEditor();
    }

    private void txtSearch_Leave(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtSearch.Text))
        {
            txtSearch.Text = Strings.SpellEditor.searchplaceholder;
        }
    }

    private void txtSearch_Enter(object sender, EventArgs e)
    {
        txtSearch.SelectAll();
        txtSearch.Focus();
    }

    private void btnClearSearch_Click(object sender, EventArgs e)
    {
        txtSearch.Text = Strings.SpellEditor.searchplaceholder;
    }

    private bool CustomSearch()
    {
        return !string.IsNullOrWhiteSpace(txtSearch.Text) &&
               txtSearch.Text != Strings.SpellEditor.searchplaceholder;
    }

    private void txtSearch_Click(object sender, EventArgs e)
    {
        if (txtSearch.Text == Strings.SpellEditor.searchplaceholder)
        {
            txtSearch.SelectAll();
        }
    }

    #endregion

    private void cmbTickAnimation_SelectedIndexChanged(object sender, EventArgs e)
    {
        Guid animationId = AnimationDescriptor.IdFromList(cmbTickAnimation.SelectedIndex - 1);
        mEditorItem.TickAnimation = AnimationDescriptor.Get(animationId);
    }
}
