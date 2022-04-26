
namespace VaultFormat
{
    partial class AttribSysVaultForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.menu = new System.Windows.Forms.MenuStrip();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.lstDataChunks = new System.Windows.Forms.ListView();
            this.colClassName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colClassHash = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colCollectionHash = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.changeCollectionHashToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.propertyGrid2 = new System.Windows.Forms.PropertyGrid();
            this.menu.SuspendLayout();
            this.contextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // menu
            // 
            this.menu.GripMargin = new System.Windows.Forms.Padding(2, 2, 0, 2);
            this.menu.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.menu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1});
            this.menu.Location = new System.Drawing.Point(0, 0);
            this.menu.Name = "menu";
            this.menu.Padding = new System.Windows.Forms.Padding(4, 2, 0, 2);
            this.menu.Size = new System.Drawing.Size(1312, 33);
            this.menu.TabIndex = 1;
            this.menu.Text = "menuStrip1";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(54, 29);
            this.toolStripMenuItem1.Text = "File";
            // 
            // lstDataChunks
            // 
            this.lstDataChunks.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colClassName,
            this.colClassHash,
            this.colCollectionHash});
            this.lstDataChunks.ContextMenuStrip = this.contextMenu;
            this.lstDataChunks.Dock = System.Windows.Forms.DockStyle.Top;
            this.lstDataChunks.FullRowSelect = true;
            this.lstDataChunks.GridLines = true;
            this.lstDataChunks.HideSelection = false;
            this.lstDataChunks.Location = new System.Drawing.Point(0, 33);
            this.lstDataChunks.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.lstDataChunks.Name = "lstDataChunks";
            this.lstDataChunks.Size = new System.Drawing.Size(1312, 301);
            this.lstDataChunks.TabIndex = 4;
            this.lstDataChunks.UseCompatibleStateImageBehavior = false;
            this.lstDataChunks.View = System.Windows.Forms.View.Details;
            this.lstDataChunks.DoubleClick += new System.EventHandler(this.lstDataChunks_DoubleClick);
            // 
            // colClassName
            // 
            this.colClassName.Text = "ClassName";
            this.colClassName.Width = 250;
            // 
            // colClassHash
            // 
            this.colClassHash.Text = "ClassHash";
            this.colClassHash.Width = 250;
            // 
            // colCollectionHash
            // 
            this.colCollectionHash.Text = "CollectionHash";
            this.colCollectionHash.Width = 250;
            // 
            // contextMenu
            // 
            this.contextMenu.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.changeCollectionHashToolStripMenuItem});
            this.contextMenu.Name = "contextMenu";
            this.contextMenu.Size = new System.Drawing.Size(268, 36);
            // 
            // changeCollectionHashToolStripMenuItem
            // 
            this.changeCollectionHashToolStripMenuItem.Name = "changeCollectionHashToolStripMenuItem";
            this.changeCollectionHashToolStripMenuItem.Size = new System.Drawing.Size(267, 32);
            this.changeCollectionHashToolStripMenuItem.Text = "Change CollectionHash";
            this.changeCollectionHashToolStripMenuItem.Click += new System.EventHandler(this.changeCollectionHashToolStripMenuItem_Click);
            // 
            // propertyGrid2
            // 
            this.propertyGrid2.Location = new System.Drawing.Point(0, 346);
            this.propertyGrid2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.propertyGrid2.Name = "propertyGrid2";
            this.propertyGrid2.Size = new System.Drawing.Size(1312, 752);
            this.propertyGrid2.TabIndex = 6;
            this.propertyGrid2.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.propertyGrid2_PropertyValueChanged);
            // 
            // AttribSysVaultForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1312, 1097);
            this.Controls.Add(this.propertyGrid2);
            this.Controls.Add(this.lstDataChunks);
            this.Controls.Add(this.menu);
            this.MainMenuStrip = this.menu;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "AttribSysVaultForm";
            this.Text = "AttribSysVault Editor";
            this.menu.ResumeLayout(false);
            this.menu.PerformLayout();
            this.contextMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.MenuStrip menu;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ListView lstDataChunks;
        private System.Windows.Forms.ColumnHeader colClassName;
        private System.Windows.Forms.ColumnHeader colClassHash;
        private System.Windows.Forms.ColumnHeader colCollectionHash;
        private System.Windows.Forms.ContextMenuStrip contextMenu;
        private System.Windows.Forms.ToolStripMenuItem changeCollectionHashToolStripMenuItem;
        private System.Windows.Forms.PropertyGrid propertyGrid2;
    }
}

