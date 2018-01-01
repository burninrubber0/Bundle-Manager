namespace VehicleList
{
    partial class VehicleListForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.mnuMain = new System.Windows.Forms.MenuStrip();
            this.itemsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addItemToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lstVehicles = new System.Windows.Forms.ListView();
            this.colIndex = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colModel = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colBrand = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colWheelType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colCategory = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colFlags = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colBoost = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colMaxBoostlessSpeed = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colTopBoostSpeed = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colNewUnknown = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colFinish = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colColor = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colDisplaySpeed = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colDisplayBoost = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colDisplayStrength = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colExhaustID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colExhaustFile = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colGroupID1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colEngineID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colEngineFile = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colGroupID2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.stsMain = new System.Windows.Forms.StatusStrip();
            this.stlStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsbAddItem = new System.Windows.Forms.ToolStripButton();
            this.mnuMain.SuspendLayout();
            this.stsMain.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // mnuMain
            // 
            this.mnuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.itemsToolStripMenuItem});
            this.mnuMain.Location = new System.Drawing.Point(0, 0);
            this.mnuMain.Name = "mnuMain";
            this.mnuMain.Size = new System.Drawing.Size(1079, 24);
            this.mnuMain.TabIndex = 0;
            this.mnuMain.Text = "menuStrip1";
            // 
            // itemsToolStripMenuItem
            // 
            this.itemsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addItemToolStripMenuItem});
            this.itemsToolStripMenuItem.Name = "itemsToolStripMenuItem";
            this.itemsToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.itemsToolStripMenuItem.Text = "Items";
            // 
            // addItemToolStripMenuItem
            // 
            this.addItemToolStripMenuItem.Image = global::VehicleList.Properties.Resources.AddTableHS;
            this.addItemToolStripMenuItem.Name = "addItemToolStripMenuItem";
            this.addItemToolStripMenuItem.Size = new System.Drawing.Size(123, 22);
            this.addItemToolStripMenuItem.Text = "Add Item";
            this.addItemToolStripMenuItem.Click += new System.EventHandler(this.addItemToolStripMenuItem_Click);
            // 
            // lstVehicles
            // 
            this.lstVehicles.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colIndex,
            this.colModel,
            this.colBrand,
            this.colName,
            this.colWheelType,
            this.colCategory,
            this.colFlags,
            this.colBoost,
            this.colMaxBoostlessSpeed,
            this.colTopBoostSpeed,
            this.colNewUnknown,
            this.colFinish,
            this.colColor,
            this.colDisplaySpeed,
            this.colDisplayBoost,
            this.colDisplayStrength,
            this.colExhaustID,
            this.colExhaustFile,
            this.colGroupID1,
            this.colEngineID,
            this.colEngineFile,
            this.colGroupID2});
            this.lstVehicles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstVehicles.FullRowSelect = true;
            this.lstVehicles.GridLines = true;
            this.lstVehicles.Location = new System.Drawing.Point(0, 49);
            this.lstVehicles.Name = "lstVehicles";
            this.lstVehicles.Size = new System.Drawing.Size(1079, 416);
            this.lstVehicles.TabIndex = 1;
            this.lstVehicles.UseCompatibleStateImageBehavior = false;
            this.lstVehicles.View = System.Windows.Forms.View.Details;
            this.lstVehicles.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lstVehicles_ColumnClick);
            this.lstVehicles.SelectedIndexChanged += new System.EventHandler(this.lstVehicles_SelectedIndexChanged);
            this.lstVehicles.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lstVehicles_MouseDoubleClick);
            // 
            // colIndex
            // 
            this.colIndex.Text = "Index";
            // 
            // colModel
            // 
            this.colModel.Text = "Model";
            this.colModel.Width = 80;
            // 
            // colBrand
            // 
            this.colBrand.Text = "Brand";
            this.colBrand.Width = 100;
            // 
            // colName
            // 
            this.colName.Text = "Name";
            this.colName.Width = 180;
            // 
            // colWheelType
            // 
            this.colWheelType.Text = "Wheel Type";
            this.colWheelType.Width = 160;
            // 
            // colCategory
            // 
            this.colCategory.Text = "Category";
            this.colCategory.Width = 80;
            // 
            // colFlags
            // 
            this.colFlags.Text = "Flags";
            this.colFlags.Width = 80;
            // 
            // colBoost
            // 
            this.colBoost.Text = "Boost";
            this.colBoost.Width = 90;
            // 
            // colMaxBoostlessSpeed
            // 
            this.colMaxBoostlessSpeed.Text = "Max Boostless Speed";
            this.colMaxBoostlessSpeed.Width = 80;
            // 
            // colTopBoostSpeed
            // 
            this.colTopBoostSpeed.Text = "Max Boost Speed";
            // 
            // colNewUnknown
            // 
            this.colNewUnknown.Text = "New Unknown";
            // 
            // colFinish
            // 
            this.colFinish.Text = "Finish";
            this.colFinish.Width = 80;
            // 
            // colColor
            // 
            this.colColor.Text = "Color";
            // 
            // colDisplaySpeed
            // 
            this.colDisplaySpeed.Text = "Display Speed";
            // 
            // colDisplayBoost
            // 
            this.colDisplayBoost.Text = "Display Boost";
            // 
            // colDisplayStrength
            // 
            this.colDisplayStrength.Text = "Display Strength";
            // 
            // colExhaustID
            // 
            this.colExhaustID.Text = "Exhaust ID";
            this.colExhaustID.Width = 120;
            // 
            // colExhaustFile
            // 
            this.colExhaustFile.Text = "Exhaust File";
            // 
            // colGroupID1
            // 
            this.colGroupID1.Text = "Group ID 1";
            this.colGroupID1.Width = 80;
            // 
            // colEngineID
            // 
            this.colEngineID.Text = "Engine ID";
            this.colEngineID.Width = 120;
            // 
            // colEngineFile
            // 
            this.colEngineFile.Text = "Engine File";
            // 
            // colGroupID2
            // 
            this.colGroupID2.Text = "Group ID 2";
            this.colGroupID2.Width = 80;
            // 
            // stsMain
            // 
            this.stsMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.stlStatusLabel});
            this.stsMain.Location = new System.Drawing.Point(0, 465);
            this.stsMain.Name = "stsMain";
            this.stsMain.Size = new System.Drawing.Size(1079, 22);
            this.stsMain.TabIndex = 2;
            this.stsMain.Text = "statusStrip1";
            // 
            // stlStatusLabel
            // 
            this.stlStatusLabel.Name = "stlStatusLabel";
            this.stlStatusLabel.Size = new System.Drawing.Size(0, 17);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbAddItem});
            this.toolStrip1.Location = new System.Drawing.Point(0, 24);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1079, 25);
            this.toolStrip1.TabIndex = 3;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tsbAddItem
            // 
            this.tsbAddItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbAddItem.Image = global::VehicleList.Properties.Resources.AddTableHS;
            this.tsbAddItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbAddItem.Name = "tsbAddItem";
            this.tsbAddItem.Size = new System.Drawing.Size(23, 22);
            this.tsbAddItem.Text = "Add Item";
            this.tsbAddItem.Click += new System.EventHandler(this.tsbAddItem_Click);
            // 
            // VehicleListForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1079, 487);
            this.Controls.Add(this.lstVehicles);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.stsMain);
            this.Controls.Add(this.mnuMain);
            this.MainMenuStrip = this.mnuMain;
            this.Name = "VehicleListForm";
            this.Text = "Vehicle List Viewer";
            this.mnuMain.ResumeLayout(false);
            this.mnuMain.PerformLayout();
            this.stsMain.ResumeLayout(false);
            this.stsMain.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip mnuMain;
        private System.Windows.Forms.ListView lstVehicles;
        private System.Windows.Forms.ColumnHeader colIndex;
        private System.Windows.Forms.ColumnHeader colName;
        private System.Windows.Forms.ColumnHeader colBrand;
        private System.Windows.Forms.ColumnHeader colWheelType;
        private System.Windows.Forms.ColumnHeader colCategory;
        private System.Windows.Forms.ColumnHeader colFlags;
        private System.Windows.Forms.ColumnHeader colGroupID1;
        private System.Windows.Forms.ColumnHeader colGroupID2;
        private System.Windows.Forms.StatusStrip stsMain;
        private System.Windows.Forms.ColumnHeader colMaxBoostlessSpeed;
        private System.Windows.Forms.ToolStripStatusLabel stlStatusLabel;
        private System.Windows.Forms.ColumnHeader colModel;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tsbAddItem;
        private System.Windows.Forms.ToolStripMenuItem itemsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addItemToolStripMenuItem;
        private System.Windows.Forms.ColumnHeader colExhaustID;
        private System.Windows.Forms.ColumnHeader colEngineID;
        private System.Windows.Forms.ColumnHeader colBoost;
        private System.Windows.Forms.ColumnHeader colFinish;
        private System.Windows.Forms.ColumnHeader colTopBoostSpeed;
        private System.Windows.Forms.ColumnHeader colNewUnknown;
        private System.Windows.Forms.ColumnHeader colExhaustFile;
        private System.Windows.Forms.ColumnHeader colEngineFile;
        private System.Windows.Forms.ColumnHeader colColor;
        private System.Windows.Forms.ColumnHeader colDisplaySpeed;
        private System.Windows.Forms.ColumnHeader colDisplayBoost;
        private System.Windows.Forms.ColumnHeader colDisplayStrength;
    }
}

