namespace BND2Master
{
    partial class EntryEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EntryEditor));
            this.tabList = new System.Windows.Forms.TabControl();
            this.tabData = new System.Windows.Forms.TabPage();
            this.hexData = new HexEditor.HexView();
            this.tabExtraData = new System.Windows.Forms.TabPage();
            this.hexExtraData = new HexEditor.HexView();
            this.tabImage = new System.Windows.Forms.TabPage();
            this.pboImage = new System.Windows.Forms.PictureBox();
            this.lblLoading = new System.Windows.Forms.Label();
            this.mnuBinary = new System.Windows.Forms.MenuStrip();
            this.binaryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.importExtraToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportExtraToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.imageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stsMain = new System.Windows.Forms.StatusStrip();
            this.pbMain = new System.Windows.Forms.ProgressBar();
            this.tabList.SuspendLayout();
            this.tabData.SuspendLayout();
            this.tabExtraData.SuspendLayout();
            this.tabImage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pboImage)).BeginInit();
            this.mnuBinary.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabList
            // 
            this.tabList.Controls.Add(this.tabData);
            this.tabList.Controls.Add(this.tabExtraData);
            this.tabList.Controls.Add(this.tabImage);
            this.tabList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabList.Location = new System.Drawing.Point(0, 24);
            this.tabList.Name = "tabList";
            this.tabList.SelectedIndex = 0;
            this.tabList.Size = new System.Drawing.Size(624, 395);
            this.tabList.TabIndex = 0;
            this.tabList.SelectedIndexChanged += new System.EventHandler(this.tabList_SelectedIndexChanged);
            // 
            // tabData
            // 
            this.tabData.Controls.Add(this.hexData);
            this.tabData.Location = new System.Drawing.Point(4, 22);
            this.tabData.Name = "tabData";
            this.tabData.Padding = new System.Windows.Forms.Padding(3);
            this.tabData.Size = new System.Drawing.Size(616, 369);
            this.tabData.TabIndex = 0;
            this.tabData.Text = "Data";
            this.tabData.UseVisualStyleBackColor = true;
            // 
            // hexData
            // 
            this.hexData.AutoScroll = true;
            this.hexData.BackColor = System.Drawing.Color.White;
            this.hexData.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.hexData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hexData.Font = new System.Drawing.Font("Courier New", 10F);
            this.hexData.HexData = null;
            this.hexData.Location = new System.Drawing.Point(3, 3);
            this.hexData.Name = "hexData";
            this.hexData.Size = new System.Drawing.Size(610, 363);
            this.hexData.TabIndex = 0;
            // 
            // tabExtraData
            // 
            this.tabExtraData.Controls.Add(this.hexExtraData);
            this.tabExtraData.Location = new System.Drawing.Point(4, 22);
            this.tabExtraData.Name = "tabExtraData";
            this.tabExtraData.Padding = new System.Windows.Forms.Padding(3);
            this.tabExtraData.Size = new System.Drawing.Size(616, 369);
            this.tabExtraData.TabIndex = 2;
            this.tabExtraData.Text = "Extra Data";
            this.tabExtraData.UseVisualStyleBackColor = true;
            // 
            // hexExtraData
            // 
            this.hexExtraData.AutoScroll = true;
            this.hexExtraData.BackColor = System.Drawing.Color.White;
            this.hexExtraData.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.hexExtraData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hexExtraData.Font = new System.Drawing.Font("Courier New", 10F);
            this.hexExtraData.HexData = null;
            this.hexExtraData.Location = new System.Drawing.Point(3, 3);
            this.hexExtraData.Name = "hexExtraData";
            this.hexExtraData.Size = new System.Drawing.Size(610, 363);
            this.hexExtraData.TabIndex = 0;
            // 
            // tabImage
            // 
            this.tabImage.Controls.Add(this.pboImage);
            this.tabImage.Location = new System.Drawing.Point(4, 22);
            this.tabImage.Name = "tabImage";
            this.tabImage.Size = new System.Drawing.Size(616, 369);
            this.tabImage.TabIndex = 3;
            this.tabImage.Text = "Image";
            this.tabImage.UseVisualStyleBackColor = true;
            // 
            // pboImage
            // 
            this.pboImage.BackColor = System.Drawing.Color.DarkGray;
            this.pboImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pboImage.Location = new System.Drawing.Point(0, 0);
            this.pboImage.Name = "pboImage";
            this.pboImage.Size = new System.Drawing.Size(616, 369);
            this.pboImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pboImage.TabIndex = 3;
            this.pboImage.TabStop = false;
            // 
            // lblLoading
            // 
            this.lblLoading.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblLoading.Font = new System.Drawing.Font("Courier New", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLoading.Location = new System.Drawing.Point(0, 0);
            this.lblLoading.Name = "lblLoading";
            this.lblLoading.Size = new System.Drawing.Size(624, 419);
            this.lblLoading.TabIndex = 4;
            this.lblLoading.Text = "Loading";
            this.lblLoading.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // mnuBinary
            // 
            this.mnuBinary.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.binaryToolStripMenuItem,
            this.imageToolStripMenuItem});
            this.mnuBinary.Location = new System.Drawing.Point(0, 0);
            this.mnuBinary.Name = "mnuBinary";
            this.mnuBinary.Size = new System.Drawing.Size(624, 24);
            this.mnuBinary.TabIndex = 6;
            this.mnuBinary.Text = "menuStrip1";
            // 
            // binaryToolStripMenuItem
            // 
            this.binaryToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.importDataToolStripMenuItem,
            this.exportDataToolStripMenuItem,
            this.toolStripMenuItem1,
            this.importExtraToolStripMenuItem,
            this.exportExtraToolStripMenuItem});
            this.binaryToolStripMenuItem.Name = "binaryToolStripMenuItem";
            this.binaryToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
            this.binaryToolStripMenuItem.Text = "Binary";
            // 
            // importDataToolStripMenuItem
            // 
            this.importDataToolStripMenuItem.Name = "importDataToolStripMenuItem";
            this.importDataToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
            this.importDataToolStripMenuItem.Text = "Import Data";
            this.importDataToolStripMenuItem.Click += new System.EventHandler(this.importDataToolStripMenuItem_Click);
            // 
            // exportDataToolStripMenuItem
            // 
            this.exportDataToolStripMenuItem.Name = "exportDataToolStripMenuItem";
            this.exportDataToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
            this.exportDataToolStripMenuItem.Text = "Export Data";
            this.exportDataToolStripMenuItem.Click += new System.EventHandler(this.exportDataToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(135, 6);
            // 
            // importExtraToolStripMenuItem
            // 
            this.importExtraToolStripMenuItem.Name = "importExtraToolStripMenuItem";
            this.importExtraToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
            this.importExtraToolStripMenuItem.Text = "Import Extra";
            this.importExtraToolStripMenuItem.Click += new System.EventHandler(this.importExtraToolStripMenuItem_Click);
            // 
            // exportExtraToolStripMenuItem
            // 
            this.exportExtraToolStripMenuItem.Name = "exportExtraToolStripMenuItem";
            this.exportExtraToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
            this.exportExtraToolStripMenuItem.Text = "Export Extra";
            this.exportExtraToolStripMenuItem.Click += new System.EventHandler(this.exportExtraToolStripMenuItem_Click);
            // 
            // imageToolStripMenuItem
            // 
            this.imageToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exportToolStripMenuItem,
            this.importToolStripMenuItem});
            this.imageToolStripMenuItem.Name = "imageToolStripMenuItem";
            this.imageToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
            this.imageToolStripMenuItem.Text = "Image";
            // 
            // exportToolStripMenuItem
            // 
            this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
            this.exportToolStripMenuItem.Size = new System.Drawing.Size(110, 22);
            this.exportToolStripMenuItem.Text = "Export";
            this.exportToolStripMenuItem.Click += new System.EventHandler(this.exportToolStripMenuItem_Click);
            // 
            // importToolStripMenuItem
            // 
            this.importToolStripMenuItem.Name = "importToolStripMenuItem";
            this.importToolStripMenuItem.Size = new System.Drawing.Size(110, 22);
            this.importToolStripMenuItem.Text = "Import";
            this.importToolStripMenuItem.Click += new System.EventHandler(this.importToolStripMenuItem_Click);
            // 
            // stsMain
            // 
            this.stsMain.Location = new System.Drawing.Point(0, 419);
            this.stsMain.Name = "stsMain";
            this.stsMain.Size = new System.Drawing.Size(624, 22);
            this.stsMain.TabIndex = 7;
            // 
            // pbMain
            // 
            this.pbMain.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pbMain.Location = new System.Drawing.Point(12, 384);
            this.pbMain.MarqueeAnimationSpeed = 10;
            this.pbMain.Name = "pbMain";
            this.pbMain.Size = new System.Drawing.Size(600, 23);
            this.pbMain.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.pbMain.TabIndex = 8;
            // 
            // EntryEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(624, 441);
            this.Controls.Add(this.tabList);
            this.Controls.Add(this.pbMain);
            this.Controls.Add(this.mnuBinary);
            this.Controls.Add(this.lblLoading);
            this.Controls.Add(this.stsMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.mnuBinary;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(300, 200);
            this.Name = "EntryEditor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "EntryEditor";
            this.Shown += new System.EventHandler(this.EntryEditor_Shown);
            this.tabList.ResumeLayout(false);
            this.tabData.ResumeLayout(false);
            this.tabExtraData.ResumeLayout(false);
            this.tabImage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pboImage)).EndInit();
            this.mnuBinary.ResumeLayout(false);
            this.mnuBinary.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabList;
        private System.Windows.Forms.TabPage tabExtraData;
        private System.Windows.Forms.Label lblLoading;
        private System.Windows.Forms.MenuStrip mnuBinary;
        private System.Windows.Forms.ToolStripMenuItem binaryToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importDataToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportDataToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem importExtraToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportExtraToolStripMenuItem;
        private System.Windows.Forms.StatusStrip stsMain;
        private System.Windows.Forms.ProgressBar pbMain;
        private System.Windows.Forms.TabPage tabData;
        private HexEditor.HexView hexData;
        private HexEditor.HexView hexExtraData;
        private System.Windows.Forms.TabPage tabImage;
        private System.Windows.Forms.PictureBox pboImage;
        private System.Windows.Forms.ToolStripMenuItem imageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importToolStripMenuItem;
    }
}