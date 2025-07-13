using DarkUI.Controls;

namespace Intersect.Editor.Forms.Editors
{
    partial class FrmSkill
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmSkill));
            btnCancel = new DarkButton();
            btnSave = new DarkButton();
            grpSkills = new DarkGroupBox();
            btnClearSearch = new DarkButton();
            txtSearch = new DarkTextBox();
            lstGameObjects = new Intersect.Editor.Forms.Controls.GameObjectList();
            pnlContainer = new Panel();
            grpGeneral = new DarkGroupBox();
            btnAddFolder = new DarkButton();
            lblFolder = new Label();
            cmbFolder = new DarkComboBox();
            lblName = new Label();
            txtName = new DarkTextBox();
            toolStrip = new DarkToolStrip();
            toolStripItemNew = new ToolStripButton();
            toolStripSeparator1 = new ToolStripSeparator();
            toolStripItemDelete = new ToolStripButton();
            toolStripSeparator2 = new ToolStripSeparator();
            btnAlphabetical = new ToolStripButton();
            toolStripSeparator4 = new ToolStripSeparator();
            toolStripItemCopy = new ToolStripButton();
            toolStripItemPaste = new ToolStripButton();
            toolStripSeparator3 = new ToolStripSeparator();
            toolStripItemUndo = new ToolStripButton();
            grpLeveling = new DarkGroupBox();
            grpSkills.SuspendLayout();
            pnlContainer.SuspendLayout();
            grpGeneral.SuspendLayout();
            toolStrip.SuspendLayout();
            SuspendLayout();
            // 
            // btnCancel
            // 
            btnCancel.DialogResult = DialogResult.Cancel;
            btnCancel.Location = new System.Drawing.Point(358, 540);
            btnCancel.Margin = new Padding(4, 3, 4, 3);
            btnCancel.Name = "btnCancel";
            btnCancel.Padding = new Padding(6);
            btnCancel.Size = new Size(201, 31);
            btnCancel.TabIndex = 24;
            btnCancel.Text = "Cancel";
            btnCancel.Click += btnCancel_Click;
            // 
            // btnSave
            // 
            btnSave.Location = new System.Drawing.Point(154, 540);
            btnSave.Margin = new Padding(4, 3, 4, 3);
            btnSave.Name = "btnSave";
            btnSave.Padding = new Padding(6);
            btnSave.Size = new Size(197, 31);
            btnSave.TabIndex = 23;
            btnSave.Text = "Save";
            btnSave.Click += btnSave_Click;
            // 
            // grpSkills
            // 
            grpSkills.BackColor = System.Drawing.Color.FromArgb(45, 45, 48);
            grpSkills.BorderColor = System.Drawing.Color.FromArgb(90, 90, 90);
            grpSkills.Controls.Add(btnClearSearch);
            grpSkills.Controls.Add(txtSearch);
            grpSkills.Controls.Add(lstGameObjects);
            grpSkills.ForeColor = System.Drawing.Color.Gainsboro;
            grpSkills.Location = new System.Drawing.Point(14, 42);
            grpSkills.Margin = new Padding(4, 3, 4, 3);
            grpSkills.Name = "grpSkills";
            grpSkills.Padding = new Padding(4, 3, 4, 3);
            grpSkills.Size = new Size(237, 492);
            grpSkills.TabIndex = 22;
            grpSkills.TabStop = false;
            grpSkills.Text = "Skills";
            // 
            // btnClearSearch
            // 
            btnClearSearch.Location = new System.Drawing.Point(209, 18);
            btnClearSearch.Margin = new Padding(4, 3, 4, 3);
            btnClearSearch.Name = "btnClearSearch";
            btnClearSearch.Padding = new Padding(6);
            btnClearSearch.Size = new Size(21, 23);
            btnClearSearch.TabIndex = 25;
            btnClearSearch.Text = "X";
            btnClearSearch.Click += btnClearSearch_Click;
            // 
            // txtSearch
            // 
            txtSearch.BackColor = System.Drawing.Color.FromArgb(69, 73, 74);
            txtSearch.BorderStyle = BorderStyle.FixedSingle;
            txtSearch.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            txtSearch.Location = new System.Drawing.Point(7, 18);
            txtSearch.Margin = new Padding(4, 3, 4, 3);
            txtSearch.Name = "txtSearch";
            txtSearch.Size = new Size(194, 23);
            txtSearch.TabIndex = 24;
            txtSearch.Text = "Search...";
            txtSearch.Click += txtSearch_Click;
            txtSearch.TextChanged += txtSearch_TextChanged;
            txtSearch.Enter += txtSearch_Enter;
            txtSearch.Leave += txtSearch_Leave;
            // 
            // lstGameObjects
            // 
            lstGameObjects.AllowDrop = true;
            lstGameObjects.BackColor = System.Drawing.Color.FromArgb(60, 63, 65);
            lstGameObjects.BorderStyle = BorderStyle.None;
            lstGameObjects.ForeColor = System.Drawing.Color.Gainsboro;
            lstGameObjects.HideSelection = false;
            lstGameObjects.ImageIndex = 0;
            lstGameObjects.LineColor = System.Drawing.Color.FromArgb(150, 150, 150);
            lstGameObjects.Location = new System.Drawing.Point(7, 48);
            lstGameObjects.Margin = new Padding(4, 3, 4, 3);
            lstGameObjects.Name = "lstGameObjects";
            lstGameObjects.SelectedImageIndex = 0;
            lstGameObjects.Size = new Size(223, 436);
            lstGameObjects.TabIndex = 23;
            // 
            // pnlContainer
            // 
            pnlContainer.Controls.Add(grpGeneral);
            pnlContainer.Controls.Add(grpLeveling);
            pnlContainer.Location = new System.Drawing.Point(258, 42);
            pnlContainer.Margin = new Padding(4, 3, 4, 3);
            pnlContainer.Name = "pnlContainer";
            pnlContainer.Size = new Size(309, 492);
            pnlContainer.TabIndex = 31;
            pnlContainer.Visible = false;
            // 
            // grpGeneral
            // 
            grpGeneral.BackColor = System.Drawing.Color.FromArgb(45, 45, 48);
            grpGeneral.BorderColor = System.Drawing.Color.FromArgb(90, 90, 90);
            grpGeneral.Controls.Add(btnAddFolder);
            grpGeneral.Controls.Add(lblFolder);
            grpGeneral.Controls.Add(cmbFolder);
            grpGeneral.Controls.Add(lblName);
            grpGeneral.Controls.Add(txtName);
            grpGeneral.ForeColor = System.Drawing.Color.Gainsboro;
            grpGeneral.Location = new System.Drawing.Point(8, 3);
            grpGeneral.Margin = new Padding(4, 3, 4, 3);
            grpGeneral.Name = "grpGeneral";
            grpGeneral.Padding = new Padding(4, 3, 4, 3);
            grpGeneral.Size = new Size(292, 88);
            grpGeneral.TabIndex = 34;
            grpGeneral.TabStop = false;
            grpGeneral.Text = "General";
            // 
            // btnAddFolder
            // 
            btnAddFolder.Location = new System.Drawing.Point(264, 51);
            btnAddFolder.Margin = new Padding(4, 3, 4, 3);
            btnAddFolder.Name = "btnAddFolder";
            btnAddFolder.Padding = new Padding(6);
            btnAddFolder.Size = new Size(21, 24);
            btnAddFolder.TabIndex = 23;
            btnAddFolder.Text = "+";
            btnAddFolder.Click += btnAddFolder_Click;
            // 
            // lblFolder
            // 
            lblFolder.AutoSize = true;
            lblFolder.Location = new System.Drawing.Point(5, 55);
            lblFolder.Margin = new Padding(4, 0, 4, 0);
            lblFolder.Name = "lblFolder";
            lblFolder.Size = new Size(43, 15);
            lblFolder.TabIndex = 22;
            lblFolder.Text = "Folder:";
            // 
            // cmbFolder
            // 
            cmbFolder.BackColor = System.Drawing.Color.FromArgb(69, 73, 74);
            cmbFolder.BorderColor = System.Drawing.Color.FromArgb(90, 90, 90);
            cmbFolder.BorderStyle = ButtonBorderStyle.Solid;
            cmbFolder.ButtonColor = System.Drawing.Color.FromArgb(43, 43, 43);
            cmbFolder.DrawDropdownHoverOutline = false;
            cmbFolder.DrawFocusRectangle = false;
            cmbFolder.DrawMode = DrawMode.OwnerDrawFixed;
            cmbFolder.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbFolder.FlatStyle = FlatStyle.Flat;
            cmbFolder.ForeColor = System.Drawing.Color.Gainsboro;
            cmbFolder.FormattingEnabled = true;
            cmbFolder.Location = new System.Drawing.Point(66, 51);
            cmbFolder.Margin = new Padding(4, 3, 4, 3);
            cmbFolder.Name = "cmbFolder";
            cmbFolder.Size = new Size(185, 24);
            cmbFolder.TabIndex = 21;
            cmbFolder.Text = null;
            cmbFolder.TextPadding = new Padding(2);
            cmbFolder.SelectedIndexChanged += cmbFolder_SelectedIndexChanged;
            // 
            // lblName
            // 
            lblName.AutoSize = true;
            lblName.Location = new System.Drawing.Point(5, 23);
            lblName.Margin = new Padding(4, 0, 4, 0);
            lblName.Name = "lblName";
            lblName.Size = new Size(42, 15);
            lblName.TabIndex = 19;
            lblName.Text = "Name:";
            // 
            // txtName
            // 
            txtName.BackColor = System.Drawing.Color.FromArgb(69, 73, 74);
            txtName.BorderStyle = BorderStyle.FixedSingle;
            txtName.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            txtName.Location = new System.Drawing.Point(66, 21);
            txtName.Margin = new Padding(4, 3, 4, 3);
            txtName.Name = "txtName";
            txtName.Size = new Size(218, 23);
            txtName.TabIndex = 18;
            txtName.TextChanged += txtName_TextChanged;
            // 
            // toolStrip
            // 
            toolStrip.AutoSize = false;
            toolStrip.BackColor = System.Drawing.Color.FromArgb(45, 45, 48);
            toolStrip.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            toolStrip.Items.AddRange(new ToolStripItem[] { toolStripItemNew, toolStripSeparator1, toolStripItemDelete, toolStripSeparator2, btnAlphabetical, toolStripSeparator4, toolStripItemCopy, toolStripItemPaste, toolStripSeparator3, toolStripItemUndo });
            toolStrip.Location = new System.Drawing.Point(0, 0);
            toolStrip.Name = "toolStrip";
            toolStrip.Padding = new Padding(6, 0, 1, 0);
            toolStrip.Size = new Size(573, 29);
            toolStrip.TabIndex = 43;
            toolStrip.Text = "toolStrip1";
            // 
            // toolStripItemNew
            // 
            toolStripItemNew.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolStripItemNew.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            toolStripItemNew.Image = (Image)resources.GetObject("toolStripItemNew.Image");
            toolStripItemNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripItemNew.Name = "toolStripItemNew";
            toolStripItemNew.Size = new Size(23, 26);
            toolStripItemNew.Text = "New";
            toolStripItemNew.Click += toolStripItemNew_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            toolStripSeparator1.Margin = new Padding(0, 0, 2, 0);
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(6, 29);
            // 
            // toolStripItemDelete
            // 
            toolStripItemDelete.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolStripItemDelete.Enabled = false;
            toolStripItemDelete.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            toolStripItemDelete.Image = (Image)resources.GetObject("toolStripItemDelete.Image");
            toolStripItemDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripItemDelete.Name = "toolStripItemDelete";
            toolStripItemDelete.Size = new Size(23, 26);
            toolStripItemDelete.Text = "Delete";
            toolStripItemDelete.Click += toolStripItemDelete_Click;
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            toolStripSeparator2.Margin = new Padding(0, 0, 2, 0);
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new Size(6, 29);
            // 
            // btnAlphabetical
            // 
            btnAlphabetical.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btnAlphabetical.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            btnAlphabetical.Image = (Image)resources.GetObject("btnAlphabetical.Image");
            btnAlphabetical.ImageTransparentColor = System.Drawing.Color.Magenta;
            btnAlphabetical.Name = "btnAlphabetical";
            btnAlphabetical.Size = new Size(23, 26);
            btnAlphabetical.Text = "Order Chronologically";
            btnAlphabetical.Click += btnAlphabetical_Click;
            // 
            // toolStripSeparator4
            // 
            toolStripSeparator4.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            toolStripSeparator4.Margin = new Padding(0, 0, 2, 0);
            toolStripSeparator4.Name = "toolStripSeparator4";
            toolStripSeparator4.Size = new Size(6, 29);
            // 
            // toolStripItemCopy
            // 
            toolStripItemCopy.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolStripItemCopy.Enabled = false;
            toolStripItemCopy.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            toolStripItemCopy.Image = (Image)resources.GetObject("toolStripItemCopy.Image");
            toolStripItemCopy.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripItemCopy.Name = "toolStripItemCopy";
            toolStripItemCopy.Size = new Size(23, 26);
            toolStripItemCopy.Text = "Copy";
            toolStripItemCopy.Click += toolStripItemCopy_Click;
            // 
            // toolStripItemPaste
            // 
            toolStripItemPaste.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolStripItemPaste.Enabled = false;
            toolStripItemPaste.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            toolStripItemPaste.Image = (Image)resources.GetObject("toolStripItemPaste.Image");
            toolStripItemPaste.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripItemPaste.Name = "toolStripItemPaste";
            toolStripItemPaste.Size = new Size(23, 26);
            toolStripItemPaste.Text = "Paste";
            toolStripItemPaste.Click += toolStripItemPaste_Click;
            // 
            // toolStripSeparator3
            // 
            toolStripSeparator3.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            toolStripSeparator3.Margin = new Padding(0, 0, 2, 0);
            toolStripSeparator3.Name = "toolStripSeparator3";
            toolStripSeparator3.Size = new Size(6, 29);
            // 
            // toolStripItemUndo
            // 
            toolStripItemUndo.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolStripItemUndo.Enabled = false;
            toolStripItemUndo.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            toolStripItemUndo.Image = (Image)resources.GetObject("toolStripItemUndo.Image");
            toolStripItemUndo.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripItemUndo.Name = "toolStripItemUndo";
            toolStripItemUndo.Size = new Size(23, 26);
            toolStripItemUndo.Text = "Undo";
            toolStripItemUndo.Click += toolStripItemUndo_Click;
            // 
            // grpLeveling
            // 
            grpLeveling.BackColor = System.Drawing.Color.FromArgb(45, 45, 48);
            grpLeveling.BorderColor = System.Drawing.Color.FromArgb(90, 90, 90);
            grpLeveling.ForeColor = System.Drawing.Color.Gainsboro;
            grpLeveling.Location = new System.Drawing.Point(8, 97);
            grpLeveling.Name = "grpLeveling";
            grpLeveling.Size = new Size(292, 124);
            grpLeveling.TabIndex = 29;
            grpLeveling.TabStop = false;
            grpLeveling.Text = "Experience Scaling";
            //   
            // nudBaseExp  
            //   
            this.nudBaseExp.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(73)))), ((int)(((byte)(74)))));
            this.nudBaseExp.ForeColor = System.Drawing.Color.Gainsboro;
            this.nudBaseExp.Location = new System.Drawing.Point(7, 31);
            this.nudBaseExp.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
            this.nudBaseExp.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            this.nudBaseExp.Name = "nudBaseExp";
            this.nudBaseExp.Size = new System.Drawing.Size(99, 20);
            this.nudBaseExp.TabIndex = 36;
            this.nudBaseExp.Value = new decimal(new int[] { 100, 0, 0, 0 });
            this.nudBaseExp.ValueChanged += new System.EventHandler(this.nudBaseExp_ValueChanged);
            //   
            // nudExpIncrease  
            //   
            this.nudExpIncrease.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(73)))), ((int)(((byte)(74)))));
            this.nudExpIncrease.ForeColor = System.Drawing.Color.Gainsboro;
            this.nudExpIncrease.Location = new System.Drawing.Point(115, 31);
            this.nudExpIncrease.Name = "nudExpIncrease";
            this.nudExpIncrease.Size = new System.Drawing.Size(122, 20);
            this.nudExpIncrease.TabIndex = 31;
            this.nudExpIncrease.Value = new decimal(new int[] { 50, 0, 0, 0 });
            this.nudExpIncrease.ValueChanged += new System.EventHandler(this.nudExpIncrease_ValueChanged);
            //   
            // lblBaseExp  
            //   
            this.lblBaseExp.AutoSize = true;
            this.lblBaseExp.Location = new System.Drawing.Point(6, 16);
            this.lblBaseExp.Name = "lblBaseExp";
            this.lblBaseExp.Size = new System.Drawing.Size(100, 13);
            this.lblBaseExp.TabIndex = 19;
            this.lblBaseExp.Text = "Base Exp To Level:";
            //   
            // lblExpIncrease  
            //   
            this.lblExpIncrease.AutoSize = true;
            this.lblExpIncrease.Location = new System.Drawing.Point(112, 15);
            this.lblExpIncrease.Name = "lblExpIncrease";
            this.lblExpIncrease.Size = new System.Drawing.Size(125, 13);
            this.lblExpIncrease.TabIndex = 21;
            this.lblExpIncrease.Text = "Exp Increase (Per Lvl %):";
            //   
            // btnExpGrid  
            //   
            this.btnExpGrid.Location = new System.Drawing.Point(333, 30);
            this.btnExpGrid.Name = "btnExpGrid";
            this.btnExpGrid.Size = new System.Drawing.Size(111, 21);
            this.btnExpGrid.TabIndex = 37;
            this.btnExpGrid.Text = "Experience Grid";
            this.btnExpGrid.Click += new System.EventHandler(this.btnExpGrid_Click);
            // 
            // FrmSkill
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSize = true;
            BackColor = System.Drawing.Color.FromArgb(45, 45, 48);
            ClientSize = new Size(573, 585);
            Controls.Add(toolStrip);
            Controls.Add(pnlContainer);
            Controls.Add(btnCancel);
            Controls.Add(btnSave);
            Controls.Add(grpSkills);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            KeyPreview = true;
            Margin = new Padding(4, 3, 4, 3);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmSkill";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Skill Editor";
            FormClosed += FrmSkill_FormClosed;
            Load += FrmSkill_Load;
            KeyDown += form_KeyDown;
            grpSkills.ResumeLayout(false);
            grpSkills.PerformLayout();
            pnlContainer.ResumeLayout(false);
            grpGeneral.ResumeLayout(false);
            grpGeneral.PerformLayout();
            toolStrip.ResumeLayout(false);
            toolStrip.PerformLayout();
            ResumeLayout(false);

        }

        #endregion

        private DarkButton btnCancel;
        private DarkButton btnSave;
        private DarkGroupBox grpSkills;
        private System.Windows.Forms.Panel pnlContainer;
        private DarkToolStrip toolStrip;
        private System.Windows.Forms.ToolStripButton toolStripItemNew;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolStripItemDelete;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        public System.Windows.Forms.ToolStripButton toolStripItemCopy;
        public System.Windows.Forms.ToolStripButton toolStripItemPaste;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        public System.Windows.Forms.ToolStripButton toolStripItemUndo;
        private DarkGroupBox grpGeneral;
        private System.Windows.Forms.Label lblName;
        private DarkTextBox txtName;
        private DarkButton btnClearSearch;
        private DarkTextBox txtSearch;
        private DarkButton btnAddFolder;
        private System.Windows.Forms.Label lblFolder;
        private DarkComboBox cmbFolder;
        private System.Windows.Forms.ToolStripButton btnAlphabetical;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private Controls.GameObjectList lstGameObjects;
        private DarkGroupBox grpLeveling;
        private DarkNumericUpDown nudBaseExp;
        private DarkNumericUpDown nudExpIncrease;
        private System.Windows.Forms.Label lblBaseExp;
        private System.Windows.Forms.Label lblExpIncrease;
        private DarkButton btnExpGrid;
        private System.Windows.Forms.DataGridView expGrid;
        private System.Windows.Forms.ContextMenuStrip mnuExpGrid;
    }
}