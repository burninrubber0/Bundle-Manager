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
            this.colVehicleID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colParentID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colWheel = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colVehicleName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colMfrName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colDamageLimit = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colFlags = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colBoostLength = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colVehicleRank = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colBoostCapacity = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colStrengthStat = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colAttribKey = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colExhaustName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colExhaustID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colEngineID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colEngineName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colClassUnlockStr = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colCarWonStrID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colCarReleasedStrID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colAIMusicHash = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colAIExStr1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colAIExStr2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colAIExStr3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colCategory = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colVehicleType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colBoostType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colFinishType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colTopSpeed = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colTopSpeedBoost = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colTopSpeedStat = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colTopSpeedBoostStat = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colColor = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colColorPalette = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
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
            this.colVehicleID,
            this.colParentID,
            this.colWheel,
            this.colVehicleName,
            this.colMfrName,
            this.colDamageLimit,
            this.colFlags,
            this.colBoostLength,
            this.colVehicleRank,
            this.colBoostCapacity,
            this.colStrengthStat,
            this.colAttribKey,
            this.colExhaustName,
            this.colExhaustID,
            this.colEngineID,
            this.colEngineName,
            this.colClassUnlockStr,
            this.colCarWonStrID,
            this.colCarReleasedStrID,
            this.colAIMusicHash,
            this.colAIExStr1,
            this.colAIExStr2,
            this.colAIExStr3,
            this.colCategory,
            this.colVehicleType,
            this.colBoostType,
            this.colFinishType,
            this.colTopSpeed,
            this.colTopSpeedBoost,
            this.colTopSpeedStat,
            this.colTopSpeedBoostStat,
            this.colColor,
            this.colColorPalette});
            this.lstVehicles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstVehicles.FullRowSelect = true;
            this.lstVehicles.GridLines = true;
            this.lstVehicles.HideSelection = false;
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
            // colVehicleID
            // 
            this.colVehicleID.Text = "ID";
            this.colVehicleID.Width = 80;
            // 
            // colParentID
            // 
            this.colParentID.Text = "Parent ID";
            // 
            // colWheel
            // 
            this.colWheel.Text = "Wheel";
            // 
            // colVehicleName
            // 
            this.colVehicleName.Text = "Vehicle";
            this.colVehicleName.Width = 180;
            // 
            // colMfrName
            // 
            this.colMfrName.Text = "Manufacturer";
            this.colMfrName.Width = 100;
            // 
            // colDamageLimit
            // 
            this.colDamageLimit.Text = "Damage Limit";
            // 
            // colFlags
            // 
            this.colFlags.Text = "Flags";
            this.colFlags.Width = 80;
            // 
            // colBoostLength
            // 
            this.colBoostLength.Text = "Boost Length";
            // 
            // colVehicleRank
            // 
            this.colVehicleRank.Text = "Rank";
            // 
            // colBoostCapacity
            // 
            this.colBoostCapacity.Text = "Boost Capacity";
            // 
            // colStrengthStat
            // 
            this.colStrengthStat.Text = "Strength Stat";
            // 
            // colAttribKey
            // 
            this.colAttribKey.Text = "AttribSys ID";
            // 
            // colExhaustName
            // 
            this.colExhaustName.Text = "Exhaust Name";
            this.colExhaustName.Width = 120;
            // 
            // colExhaustID
            // 
            this.colExhaustID.Text = "Exhuast ID";
            this.colExhaustID.Width = 80;
            // 
            // colEngineID
            // 
            this.colEngineID.Text = "Engine ID";
            this.colEngineID.Width = 80;
            // 
            // colEngineName
            // 
            this.colEngineName.Text = "Engine Name";
            this.colEngineName.Width = 120;
            // 
            // colClassUnlockStr
            // 
            this.colClassUnlockStr.Text = "Class Unlock Stream ID";
            // 
            // colCarWonStrID
            // 
            this.colCarWonStrID.Text = "Car Won Stream ID";
            // 
            // colCarReleasedStrID
            // 
            this.colCarReleasedStrID.Text = "Car Released Stream ID";
            // 
            // colAIMusicHash
            // 
            this.colAIMusicHash.Text = "AI Music";
            // 
            // colAIExStr1
            // 
            this.colAIExStr1.Text = "AI Exhaust 1";
            // 
            // colAIExStr2
            // 
            this.colAIExStr2.Text = "AI Exhaust 2";
            // 
            // colAIExStr3
            // 
            this.colAIExStr3.Text = "AI Exhaust 3";
            // 
            // colCategory
            // 
            this.colCategory.Text = "Category";
            this.colCategory.Width = 80;
            // 
            // colVehicleType
            // 
            this.colVehicleType.Text = "Vehicle Type";
            // 
            // colBoostType
            // 
            this.colBoostType.Text = "Boost Type";
            this.colBoostType.Width = 90;
            // 
            // colFinishType
            // 
            this.colFinishType.Text = "Finish Type";
            this.colFinishType.Width = 80;
            // 
            // colTopSpeed
            // 
            this.colTopSpeed.Text = "Top Speed";
            // 
            // colTopSpeedBoost
            // 
            this.colTopSpeedBoost.Text = "Top Boost Speed";
            // 
            // colTopSpeedStat
            // 
            this.colTopSpeedStat.Text = "Speed Stat";
            // 
            // colTopSpeedBoostStat
            // 
            this.colTopSpeedBoostStat.Text = "Boost Stat";
            // 
            // colColor
            // 
            this.colColor.Text = "Default Color";
            // 
            // colColorPalette
            // 
            this.colColorPalette.Text = "Default Color Type";
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
        private System.Windows.Forms.ColumnHeader colVehicleName;
        private System.Windows.Forms.ColumnHeader colMfrName;
        private System.Windows.Forms.ColumnHeader colCategory;
        private System.Windows.Forms.ColumnHeader colFlags;
        private System.Windows.Forms.ColumnHeader colExhaustID;
        private System.Windows.Forms.ColumnHeader colEngineID;
        private System.Windows.Forms.StatusStrip stsMain;
        private System.Windows.Forms.ToolStripStatusLabel stlStatusLabel;
        private System.Windows.Forms.ColumnHeader colVehicleID;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tsbAddItem;
        private System.Windows.Forms.ToolStripMenuItem itemsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addItemToolStripMenuItem;
        private System.Windows.Forms.ColumnHeader colExhaustName;
        private System.Windows.Forms.ColumnHeader colEngineName;
        private System.Windows.Forms.ColumnHeader colBoostType;
        private System.Windows.Forms.ColumnHeader colFinishType;
        private System.Windows.Forms.ColumnHeader colColor;
        private System.Windows.Forms.ColumnHeader colTopSpeedStat;
        private System.Windows.Forms.ColumnHeader colTopSpeedBoostStat;
        private System.Windows.Forms.ColumnHeader colStrengthStat;
        private System.Windows.Forms.ColumnHeader colWheel;
        private System.Windows.Forms.ColumnHeader colParentID;
        private System.Windows.Forms.ColumnHeader colBoostLength;
        private System.Windows.Forms.ColumnHeader colVehicleRank;
        private System.Windows.Forms.ColumnHeader colBoostCapacity;
        private System.Windows.Forms.ColumnHeader colAttribKey;
        private System.Windows.Forms.ColumnHeader colClassUnlockStr;
        private System.Windows.Forms.ColumnHeader colCarWonStrID;
        private System.Windows.Forms.ColumnHeader colCarReleasedStrID;
        private System.Windows.Forms.ColumnHeader colAIMusicHash;
        private System.Windows.Forms.ColumnHeader colAIExStr1;
        private System.Windows.Forms.ColumnHeader colAIExStr2;
        private System.Windows.Forms.ColumnHeader colAIExStr3;
        private System.Windows.Forms.ColumnHeader colVehicleType;
        private System.Windows.Forms.ColumnHeader colTopSpeed;
        private System.Windows.Forms.ColumnHeader colTopSpeedBoost;
        private System.Windows.Forms.ColumnHeader colDamageLimit;
        private System.Windows.Forms.ColumnHeader colColorPalette;
    }
}

