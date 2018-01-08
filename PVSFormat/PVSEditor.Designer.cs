namespace PVSFormat
{
    partial class PVSEditor
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
            this.dbgMain = new DebugHelper.DebugViewer();
            this.stsMain = new System.Windows.Forms.StatusStrip();
            this.SuspendLayout();
            // 
            // dbgMain
            // 
            this.dbgMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dbgMain.Location = new System.Drawing.Point(12, 12);
            this.dbgMain.Name = "dbgMain";
            this.dbgMain.SelectedObject = null;
            this.dbgMain.Size = new System.Drawing.Size(500, 324);
            this.dbgMain.TabIndex = 0;
            this.dbgMain.Text = "debugViewer1";
            // 
            // stsMain
            // 
            this.stsMain.Location = new System.Drawing.Point(0, 339);
            this.stsMain.Name = "stsMain";
            this.stsMain.Size = new System.Drawing.Size(524, 22);
            this.stsMain.TabIndex = 1;
            this.stsMain.Text = "statusStrip1";
            // 
            // PVSEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(524, 361);
            this.Controls.Add(this.stsMain);
            this.Controls.Add(this.dbgMain);
            this.MinimizeBox = false;
            this.Name = "PVSEditor";
            this.Text = "PVSEditor";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DebugHelper.DebugViewer dbgMain;
        private System.Windows.Forms.StatusStrip stsMain;
    }
}