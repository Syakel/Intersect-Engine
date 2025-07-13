using DarkUI.Forms;
using Intersect.Editor.Core;
using Intersect.Editor.General;
using Intersect.Editor.Localization;
using Intersect.Editor.Networking;
using Intersect.Enums;
using Intersect.Framework.Core.GameObjects.Skills;
using Intersect.GameObjects;
using Intersect.Models;
using System.Diagnostics;

namespace Intersect.Editor.Forms.Editors;

public partial class FrmSkill : EditorForm
{
    private List<SkillDescriptor> mChanged = new List<SkillDescriptor>();

    private string mCopiedItem;

    private SkillDescriptor mEditorItem;

    private List<string> mKnownFolders = new List<string>();

    public FrmSkill()
    {
        ApplyHooks();
        InitializeComponent();
        Icon = Program.Icon;
        _btnSave = btnSave;
        _btnCancel = btnCancel;

        lstGameObjects.Init(UpdateToolStripItems, AssignEditorItem, toolStripItemNew_Click, toolStripItemCopy_Click, toolStripItemUndo_Click, toolStripItemPaste_Click, toolStripItemDelete_Click);
    }

    private void AssignEditorItem(Guid id)
    {
        mEditorItem = SkillDescriptor.Get(id);
        UpdateEditor();
    }

    protected override void GameObjectUpdatedDelegate(GameObjectType type)
    {
        if (type == GameObjectType.Skill) // Ensure you have a GameObjectType.Skill
        {
            InitEditor();
            if (mEditorItem != null && !DatabaseObject<SkillDescriptor>.Lookup.Values.Contains(mEditorItem))
            {
                mEditorItem = null;
                UpdateEditor();
            }
        }
    }

    private void UpdateEditor()
    {
        if (mEditorItem != null)
        {
            pnlContainer.Show();

            txtName.Text = mEditorItem.Name;
            cmbFolder.Text = mEditorItem.Folder;

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

    private void txtName_TextChanged(object sender, EventArgs e)
    {
        mEditorItem.Name = txtName.Text;
        lstGameObjects.UpdateText(txtName.Text);
    }

    private void FrmSkill_FormClosed(object sender, FormClosedEventArgs e)
    {
        btnCancel_Click(null, null);
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
        foreach (var item in mChanged)
        {
            PacketSender.SendSaveObject(item);
            item.DeleteBackup();
        }

        Hide();
        Globals.CurrentEditor = -1;
        Dispose();
    }

    private void toolStripItemNew_Click(object sender, EventArgs e)
    {
        PacketSender.SendCreateObject(GameObjectType.Skill); // Ensure you have a GameObjectType.Skill
    }

    private void toolStripItemDelete_Click(object sender, EventArgs e)
    {
        if (mEditorItem != null && lstGameObjects.Focused)
        {
            if (DarkMessageBox.ShowWarning(
                    Strings.SkillEditor.deleteprompt, Strings.SkillEditor.delete,
                    DarkDialogButton.YesNo, Icon
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
                    Strings.SkillEditor.undoprompt, Strings.SkillEditor.undotitle,
                    DarkDialogButton.YesNo, Icon
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

    private void lstGameObjects_FocusChanged(object sender, EventArgs e)
    {
        UpdateToolStripItems();
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

    private void FrmSkill_Load(object sender, EventArgs e)
    {
        InitLocalization();
        UpdateEditor();
    }

    private void InitLocalization()
    {
        Text = Strings.SkillEditor.title;
        toolStripItemNew.Text = Strings.SkillEditor.New;
        toolStripItemDelete.Text = Strings.SkillEditor.delete;
        toolStripItemCopy.Text = Strings.SkillEditor.copy;
        toolStripItemPaste.Text = Strings.SkillEditor.paste;
        toolStripItemUndo.Text = Strings.SkillEditor.undo;

        grpGeneral.Text = Strings.SkillEditor.general;
        lblName.Text = Strings.SkillEditor.name;

        btnAlphabetical.ToolTipText = Strings.SkillEditor.sortalphabetically;
        txtSearch.Text = Strings.SkillEditor.searchplaceholder;
        lblFolder.Text = Strings.SkillEditor.folderlabel;

        btnSave.Text = Strings.SkillEditor.save;
        btnCancel.Text = Strings.SkillEditor.cancel;

        grpLeveling.Text = Strings.ClassEditor.leveling;
        lblBaseExp.Text = Strings.ClassEditor.levelexp;
        lblExpIncrease.Text = Strings.ClassEditor.levelexpscale;

        //Exp Grid
        btnExpGrid.Text = Strings.ClassEditor.expgrid;
        grpExpGrid.Text = Strings.ClassEditor.experiencegrid;
        btnResetExpGrid.Text = Strings.ClassEditor.gridreset;
        btnCloseExpGrid.Text = Strings.ClassEditor.gridclose;
        btnExpPaste.Text = Strings.ClassEditor.gridpaste;

        //Create EXP Grid...
        var levelCol = new DataGridViewTextBoxColumn();
        levelCol.HeaderText = Strings.ClassEditor.gridlevel;
        levelCol.ReadOnly = true;
        levelCol.SortMode = DataGridViewColumnSortMode.NotSortable;

        var tnlCol = new DataGridViewTextBoxColumn();
        tnlCol.HeaderText = Strings.ClassEditor.gridtnl;
        tnlCol.SortMode = DataGridViewColumnSortMode.NotSortable;

        var totalCol = new DataGridViewTextBoxColumn();
        totalCol.HeaderText = Strings.ClassEditor.gridtotalexp;
        totalCol.ReadOnly = true;
        totalCol.SortMode = DataGridViewColumnSortMode.NotSortable;

        expGrid.Columns.Clear();
        expGrid.Columns.Add(levelCol);
        expGrid.Columns.Add(tnlCol);
        expGrid.Columns.Add(totalCol);
    }

    #region "Item List - Folders, Searching, Sorting, Etc"

    public void InitEditor()
    {
        var mFolders = new List<string>();
        foreach (var itm in SkillDescriptor.Lookup)
        {
            if (!string.IsNullOrEmpty(((SkillDescriptor)itm.Value).Folder) &&
                !mFolders.Contains(((SkillDescriptor)itm.Value).Folder))
            {
                mFolders.Add(((SkillDescriptor)itm.Value).Folder);
                if (!mKnownFolders.Contains(((SkillDescriptor)itm.Value).Folder))
                {
                    mKnownFolders.Add(((SkillDescriptor)itm.Value).Folder);
                }
            }
        }

        mFolders.Sort();
        mKnownFolders.Sort();
        cmbFolder.Items.Clear();
        cmbFolder.Items.Add("");
        cmbFolder.Items.AddRange(mKnownFolders.ToArray());

        var items = SkillDescriptor.Lookup.OrderBy(p => p.Value?.Name).Select(pair => new KeyValuePair<Guid, KeyValuePair<string, string>>(pair.Key,
            new KeyValuePair<string, string>(((SkillDescriptor)pair.Value)?.Name ?? Models.DatabaseObject<SkillDescriptor>.Deleted, ((SkillDescriptor)pair.Value)?.Folder ?? ""))).ToArray();
        lstGameObjects.Repopulate(items, mFolders, btnAlphabetical.Checked, CustomSearch(), txtSearch.Text);
    }

    private void btnCloseExpGrid_Click(object sender, EventArgs e)
    {
        grpExpGrid.Hide();
    }

    private void expGrid_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
    {
        if (e.Button == MouseButtons.Right &&
            expGrid.CurrentCell != null &&
            expGrid.CurrentCell.IsInEditMode == false)
        {
            var cell = expGrid.CurrentCell;
            if (cell != null)
            {
                var r = cell.DataGridView.GetCellDisplayRectangle(cell.ColumnIndex, cell.RowIndex, false);
                var p = new System.Drawing.Point(r.X + r.Width, r.Y + r.Height);
                mnuExpGrid.Show((DataGridView)sender, p);
            }
        }
    }

    private void expGrid_SelectionChanged(object sender, EventArgs e)
    {
        if (expGrid.Rows.Count <= 0)
        {
            return;
        }

        var sel = expGrid.SelectedCells;
        if (sel.Count == 0)
        {
            expGrid.Rows[0].Cells[1].Selected = true;
        }
        else
        {
            var selection = sel[0];
            if (selection.ColumnIndex != 1)
            {
                expGrid.Rows[selection.RowIndex].Cells[1].Selected = true;
            }
        }
    }

    private void btnPaste_Click(object sender, EventArgs e)
    {
        try
        {
            var s = Clipboard.GetText();
            var lines = s.Split('\n');
            int iFail = 0, iRow = expGrid.CurrentCell.RowIndex;
            var iCol = expGrid.CurrentCell.ColumnIndex;
            DataGridViewCell oCell;
            foreach (var line in lines)
            {
                if (iRow < expGrid.RowCount && line.Length > 0)
                {
                    var sCells = line.Split('\t');
                    for (var i = 0; i < 1; ++i)
                    {
                        if (iCol + i < this.expGrid.ColumnCount)
                        {
                            oCell = expGrid[iCol + i, iRow];
                            if (!oCell.ReadOnly)
                            {
                                if (oCell.Value.ToString() != sCells[i])
                                {
                                    var val = 0;
                                    if (int.TryParse(sCells[i], out val))
                                    {
                                        if (val > 0)
                                        {
                                            oCell.Value = Convert.ChangeType(val.ToString(), oCell.ValueType);
                                        }
                                    }
                                }
                                else
                                {
                                    iFail++;
                                }

                                //only traps a fail if the data has changed
                                //and you are pasting into a read only cell
                            }
                        }
                        else
                        {
                            break;
                        }
                    }

                    iRow++;
                }
                else
                {
                    break;
                }
            }
        }
        catch (Exception)
        {
            return;
        }
    }

    private void expGrid_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
    {
        e.Control.KeyPress -= new KeyPressEventHandler(expGrid_KeyPress);
        var tb = e.Control as TextBox;
        if (tb != null)
        {
            tb.KeyPress += new KeyPressEventHandler(expGrid_KeyPress);
        }
    }

    private void expGrid_KeyPress(object sender, KeyPressEventArgs e)
    {
        if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
        {
            e.Handled = true;
        }
    }

    private void expGrid_CellEndEdit(object sender, DataGridViewCellEventArgs e)
    {
        var cell = expGrid.Rows[e.RowIndex].Cells[e.ColumnIndex];
        long val = 0;
        if (long.TryParse(cell.Value.ToString(), out val))
        {
            if (val == 0 || val == mEditorItem.ExperienceCurve.Calculate(e.RowIndex + 1))
            {
                if (mEditorItem.ExperienceOverrides.ContainsKey(e.RowIndex + 1))
                {
                    mEditorItem.ExperienceOverrides.Remove(e.RowIndex + 1);
                }
            }
            else
            {
                if (!mEditorItem.ExperienceOverrides.ContainsKey(e.RowIndex + 1))
                {
                    mEditorItem.ExperienceOverrides.Add(e.RowIndex + 1, val);
                }
                else
                {
                    mEditorItem.ExperienceOverrides[e.RowIndex + 1] = val;
                }
            }

            UpdateExpGridValues(e.RowIndex + 1);
        }
        else
        {
            UpdateExpGridValues(e.RowIndex + 1, e.RowIndex + 2);
        }
    }

    private void btnResetExpGrid_Click(object sender, EventArgs e)
    {
        mEditorItem.ExperienceOverrides.Clear();
        UpdateExpGridValues(1);
    }

    private void expGrid_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Delete)
        {
            if (expGrid.CurrentCell != null)
            {
                if (!expGrid.CurrentCell.IsInEditMode && expGrid.CurrentCell.ReadOnly == false)
                {
                    var level = expGrid.CurrentCell.RowIndex + 1;
                    if (mEditorItem.ExperienceOverrides.ContainsKey(level))
                    {
                        mEditorItem.ExperienceOverrides.Remove(level);
                    }

                    UpdateExpGridValues(level);
                }
            }
        }
    }

    private void btnAddFolder_Click(object sender, EventArgs e)
    {
        var folderName = string.Empty;
        var result = DarkInputBox.ShowInformation(
            Strings.SkillEditor.folderprompt, Strings.SkillEditor.foldertitle, ref folderName,
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
            txtSearch.Text = Strings.SkillEditor.searchplaceholder;
        }
    }

    private void txtSearch_Enter(object sender, EventArgs e)
    {
        txtSearch.SelectAll();
        txtSearch.Focus();
    }

    private void btnClearSearch_Click(object sender, EventArgs e)
    {
        txtSearch.Text = Strings.SkillEditor.searchplaceholder;
    }

    private bool CustomSearch()
    {
        return !string.IsNullOrWhiteSpace(txtSearch.Text) &&
               txtSearch.Text != Strings.SkillEditor.searchplaceholder;
    }

    private void txtSearch_Click(object sender, EventArgs e)
    {
        if (txtSearch.Text == Strings.SkillEditor.searchplaceholder)
        {
            txtSearch.SelectAll();
        }
    }

    private void nudBaseExp_ValueChanged(object sender, EventArgs e)
    {
        if (mEditorItem != null)
        {
            mEditorItem.BaseExp = (long)nudBaseExp.Value;
        }
    }

    private void nudExpIncrease_ValueChanged(object sender, EventArgs e)
    {
        if (mEditorItem != null)
        {
            mEditorItem.ExpIncrease = (long)nudExpIncrease.Value;
        }
    }

    private void btnExpGrid_Click(object sender, EventArgs e)
    {
        
    }
   
    private void UpdateExpGridValues(int start, int end = -1)
    {
        if (end == -1)
        {
            end = Options.Instance.Player.MaxLevel;
        }

        if (start > end)
        {
            return;
        }

        if (start < 1)
        {
            start = 1;
        }

        for (var i = start; i <= end; i++)
        {
            if (i < Options.Instance.Player.MaxLevel)
            {
                if (mEditorItem.ExperienceOverrides.ContainsKey(i))
                {
                    expGrid.Rows[i - 1].Cells[1].Value = Convert.ChangeType(
                        mEditorItem.ExperienceOverrides[i], expGrid.Rows[i - 1].Cells[1].ValueType
                    );

                    var style = expGrid.Rows[i - 1].Cells[1].InheritedStyle;
                    style.Font = new Font(style.Font, FontStyle.Bold);
                    expGrid.Rows[i - 1].Cells[1].Style.ApplyStyle(style);
                }
                else
                {
                    expGrid.Rows[i - 1].Cells[1].Value = Convert.ChangeType(
                        mEditorItem.ExperienceCurve.Calculate(i), expGrid.Rows[i - 1].Cells[1].ValueType
                    );

                    expGrid.Rows[i - 1].Cells[1].Style.ApplyStyle(expGrid.Rows[i - 1].Cells[0].InheritedStyle);
                }
            }
            else
            {
                expGrid.Rows[i - 1].Cells[1].Value = Convert.ChangeType(0, expGrid.Rows[i - 1].Cells[1].ValueType);
                expGrid.Rows[i - 1].Cells[1].ReadOnly = true;
            }

            if (i == 1)
            {
                expGrid.Rows[i - 1].Cells[2].Value = Convert.ChangeType(0, expGrid.Rows[i - 1].Cells[1].ValueType);
            }
            else
            {
                expGrid.Rows[i - 1].Cells[2].Value = Convert.ChangeType(
                    long.Parse(expGrid.Rows[i - 2].Cells[2].Value.ToString()) +
                    long.Parse(expGrid.Rows[i - 2].Cells[1].Value.ToString()),
                    expGrid.Rows[i - 1].Cells[2].ValueType
                );
            }
        }
    }

    #endregion
}