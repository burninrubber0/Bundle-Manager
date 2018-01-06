namespace BundleManager
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
            this.tabInfo = new System.Windows.Forms.TabPage();
            this.txtInfo = new System.Windows.Forms.TextBox();
            this.tabHeader = new System.Windows.Forms.TabPage();
            this.hexData = new HexEditor.HexView();
            this.tabBody = new System.Windows.Forms.TabPage();
            this.hexExtraData = new HexEditor.HexView();
            this.lblLoading = new System.Windows.Forms.Label();
            this.mnuBar = new System.Windows.Forms.MenuStrip();
            this.binaryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importHeaderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportHeaderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.importBodyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportBodyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.imageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.modelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.infoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportObjToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stsMain = new System.Windows.Forms.StatusStrip();
            this.pbMain = new System.Windows.Forms.ProgressBar();
            this.pboImage = new System.Windows.Forms.PictureBox();
            this.tabList.SuspendLayout();
            this.tabInfo.SuspendLayout();
            this.tabHeader.SuspendLayout();
            this.tabBody.SuspendLayout();
            this.mnuBar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pboImage)).BeginInit();
            this.SuspendLayout();
            // 
            // tabList
            // 
            this.tabList.Controls.Add(this.tabInfo);
            this.tabList.Controls.Add(this.tabHeader);
            this.tabList.Controls.Add(this.tabBody);
            this.tabList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabList.Location = new System.Drawing.Point(0, 0);
            this.tabList.Name = "tabList";
            this.tabList.SelectedIndex = 0;
            this.tabList.Size = new System.Drawing.Size(624, 419);
            this.tabList.TabIndex = 0;
            this.tabList.SelectedIndexChanged += new System.EventHandler(this.tabList_SelectedIndexChanged);
            // 
            // tabInfo
            // 
            this.tabInfo.Controls.Add(this.txtInfo);
            this.tabInfo.Location = new System.Drawing.Point(4, 22);
            this.tabInfo.Name = "tabInfo";
            this.tabInfo.Size = new System.Drawing.Size(616, 393);
            this.tabInfo.TabIndex = 3;
            this.tabInfo.Text = "Info";
            this.tabInfo.UseVisualStyleBackColor = true;
            // 
            // txtInfo
            // 
            this.txtInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtInfo.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.txtInfo.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtInfo.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtInfo.Location = new System.Drawing.Point(8, 3);
            this.txtInfo.Multiline = true;
            this.txtInfo.Name = "txtInfo";
            this.txtInfo.ReadOnly = true;
            this.txtInfo.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtInfo.Size = new System.Drawing.Size(600, 387);
            this.txtInfo.TabIndex = 1;
            // 
            // tabHeader
            // 
            this.tabHeader.Controls.Add(this.hexData);
            this.tabHeader.Location = new System.Drawing.Point(4, 22);
            this.tabHeader.Name = "tabHeader";
            this.tabHeader.Padding = new System.Windows.Forms.Padding(3);
            this.tabHeader.Size = new System.Drawing.Size(616, 393);
            this.tabHeader.TabIndex = 0;
            this.tabHeader.Text = "Header";
            this.tabHeader.UseVisualStyleBackColor = true;
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
            this.hexData.Size = new System.Drawing.Size(610, 387);
            this.hexData.TabIndex = 0;
            // 
            // tabBody
            // 
            this.tabBody.Controls.Add(this.hexExtraData);
            this.tabBody.Location = new System.Drawing.Point(4, 22);
            this.tabBody.Name = "tabBody";
            this.tabBody.Padding = new System.Windows.Forms.Padding(3);
            this.tabBody.Size = new System.Drawing.Size(616, 393);
            this.tabBody.TabIndex = 2;
            this.tabBody.Text = "Body";
            this.tabBody.UseVisualStyleBackColor = true;
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
            this.hexExtraData.Size = new System.Drawing.Size(610, 387);
            this.hexExtraData.TabIndex = 0;
            // 
            // lblLoading
            // 
            this.lblLoading.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblLoading.Font = new System.Drawing.Font("Courier New", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLoading.Location = new System.Drawing.Point(0, 0);
            this.lblLoading.Name = "lblLoading";
            this.lblLoading.Size = new System.Drawing.Size(624, 441);
            this.lblLoading.TabIndex = 4;
            this.lblLoading.Text = "Loading";
            this.lblLoading.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // mnuBar
            // 
            this.mnuBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.binaryToolStripMenuItem,
            this.imageToolStripMenuItem,
            this.modelToolStripMenuItem});
            this.mnuBar.Location = new System.Drawing.Point(0, 0);
            this.mnuBar.Name = "mnuBar";
            this.mnuBar.Size = new System.Drawing.Size(624, 24);
            this.mnuBar.TabIndex = 6;
            this.mnuBar.Text = "menuStrip1";
            // 
            // binaryToolStripMenuItem
            // 
            this.binaryToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.importHeaderToolStripMenuItem,
            this.exportHeaderToolStripMenuItem,
            this.toolStripMenuItem1,
            this.importBodyToolStripMenuItem,
            this.exportBodyToolStripMenuItem});
            this.binaryToolStripMenuItem.Name = "binaryToolStripMenuItem";
            this.binaryToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
            this.binaryToolStripMenuItem.Text = "Binary";
            // 
            // importHeaderToolStripMenuItem
            // 
            this.importHeaderToolStripMenuItem.Name = "importHeaderToolStripMenuItem";
            this.importHeaderToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.importHeaderToolStripMenuItem.Text = "Import Header";
            this.importHeaderToolStripMenuItem.Click += new System.EventHandler(this.importDataToolStripMenuItem_Click);
            // 
            // exportHeaderToolStripMenuItem
            // 
            this.exportHeaderToolStripMenuItem.Name = "exportHeaderToolStripMenuItem";
            this.exportHeaderToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.exportHeaderToolStripMenuItem.Text = "Export Header";
            this.exportHeaderToolStripMenuItem.Click += new System.EventHandler(this.exportDataToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(149, 6);
            // 
            // importBodyToolStripMenuItem
            // 
            this.importBodyToolStripMenuItem.Name = "importBodyToolStripMenuItem";
            this.importBodyToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.importBodyToolStripMenuItem.Text = "Import Body";
            this.importBodyToolStripMenuItem.Click += new System.EventHandler(this.importExtraToolStripMenuItem_Click);
            // 
            // exportBodyToolStripMenuItem
            // 
            this.exportBodyToolStripMenuItem.Name = "exportBodyToolStripMenuItem";
            this.exportBodyToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.exportBodyToolStripMenuItem.Text = "Export Body";
            this.exportBodyToolStripMenuItem.Click += new System.EventHandler(this.exportExtraToolStripMenuItem_Click);
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
            // modelToolStripMenuItem
            // 
            this.modelToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.infoToolStripMenuItem,
            this.exportObjToolStripMenuItem});
            this.modelToolStripMenuItem.Name = "modelToolStripMenuItem";
            this.modelToolStripMenuItem.Size = new System.Drawing.Size(53, 20);
            this.modelToolStripMenuItem.Text = "Model";
            // 
            // infoToolStripMenuItem
            // 
            this.infoToolStripMenuItem.Name = "infoToolStripMenuItem";
            this.infoToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
            this.infoToolStripMenuItem.Text = "Info";
            this.infoToolStripMenuItem.Click += new System.EventHandler(this.infoToolStripMenuItem_Click);
            // 
            // exportObjToolStripMenuItem
            // 
            this.exportObjToolStripMenuItem.Name = "exportObjToolStripMenuItem";
            this.exportObjToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
            this.exportObjToolStripMenuItem.Text = "Export Obj";
            this.exportObjToolStripMenuItem.Click += new System.EventHandler(this.exportObjToolStripMenuItem_Click);
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
            // pboImage
            // 
            this.pboImage.BackColor = System.Drawing.Color.DarkGray;
            this.pboImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pboImage.Location = new System.Drawing.Point(0, 24);
            this.pboImage.Name = "pboImage";
            this.pboImage.Size = new System.Drawing.Size(624, 395);
            this.pboImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pboImage.TabIndex = 9;
            this.pboImage.TabStop = false;
            // 
            // EntryEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(624, 441);
            this.Controls.Add(this.pboImage);
            this.Controls.Add(this.mnuBar);
            this.Controls.Add(this.tabList);
            this.Controls.Add(this.stsMain);
            this.Controls.Add(this.pbMain);
            this.Controls.Add(this.lblLoading);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.mnuBar;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(300, 200);
            this.Name = "EntryEditor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "EntryEditor";
            this.Shown += new System.EventHandler(this.EntryEditor_Shown);
            this.tabList.ResumeLayout(false);
            this.tabInfo.ResumeLayout(false);
            this.tabInfo.PerformLayout();
            this.tabHeader.ResumeLayout(false);
            this.tabBody.ResumeLayout(false);
            this.mnuBar.ResumeLayout(false);
            this.mnuBar.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pboImage)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabList;
        private System.Windows.Forms.TabPage tabBody;
        private System.Windows.Forms.Label lblLoading;
        private System.Windows.Forms.MenuStrip mnuBar;
        private System.Windows.Forms.ToolStripMenuItem binaryToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importHeaderToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportHeaderToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem importBodyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportBodyToolStripMenuItem;
        private System.Windows.Forms.StatusStrip stsMain;
        private System.Windows.Forms.ProgressBar pbMain;
        private System.Windows.Forms.TabPage tabHeader;
        private HexEditor.HexView hexData;
        private HexEditor.HexView hexExtraData;
        private System.Windows.Forms.ToolStripMenuItem imageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importToolStripMenuItem;
        private System.Windows.Forms.PictureBox pboImage;
        private System.Windows.Forms.ToolStripMenuItem modelToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem infoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportObjToolStripMenuItem;
        private System.Windows.Forms.TabPage tabInfo;
        private System.Windows.Forms.TextBox txtInfo;
    }
}