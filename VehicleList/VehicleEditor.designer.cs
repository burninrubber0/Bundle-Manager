namespace VehicleList
{
    partial class VehicleEditor
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
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.txtID = new System.Windows.Forms.TextBox();
            this.lblID = new System.Windows.Forms.Label();
            this.lblBrand = new System.Windows.Forms.Label();
            this.txtBrand = new System.Windows.Forms.TextBox();
            this.lblName = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.lblWheels = new System.Windows.Forms.Label();
            this.txtWheels = new System.Windows.Forms.TextBox();
            this.lblCategory = new System.Windows.Forms.Label();
            this.txtCategory = new System.Windows.Forms.TextBox();
            this.lblFlags = new System.Windows.Forms.Label();
            this.txtFlags = new System.Windows.Forms.TextBox();
            this.lblUnknown26 = new System.Windows.Forms.Label();
            this.txtUnknown26 = new System.Windows.Forms.TextBox();
            this.txtGroupID1 = new System.Windows.Forms.TextBox();
            this.lblGroupID1 = new System.Windows.Forms.Label();
            this.lblGroupID2 = new System.Windows.Forms.Label();
            this.txtGroupID2 = new System.Windows.Forms.TextBox();
            this.lblBoost = new System.Windows.Forms.Label();
            this.cboBoost = new System.Windows.Forms.ComboBox();
            this.lblLockByte = new System.Windows.Forms.Label();
            this.cboLockByte = new System.Windows.Forms.ComboBox();
            this.lblEngineID1 = new System.Windows.Forms.Label();
            this.txtEngineID1 = new System.Windows.Forms.TextBox();
            this.lblEngineID2 = new System.Windows.Forms.Label();
            this.txtEngineID2 = new System.Windows.Forms.TextBox();
            this.lblSortOrder = new System.Windows.Forms.Label();
            this.txtSortOrder = new System.Windows.Forms.TextBox();
            this.txtDisplaySpeed = new System.Windows.Forms.TextBox();
            this.lblDisplaySpeed = new System.Windows.Forms.Label();
            this.lblDisplayBoost = new System.Windows.Forms.Label();
            this.txtDisplayBoost = new System.Windows.Forms.TextBox();
            this.lblDisplayStrength = new System.Windows.Forms.Label();
            this.txtDisplayStrength = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.Location = new System.Drawing.Point(372, 379);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 0;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(291, 379);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // txtID
            // 
            this.txtID.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtID.Location = new System.Drawing.Point(89, 12);
            this.txtID.Name = "txtID";
            this.txtID.Size = new System.Drawing.Size(358, 20);
            this.txtID.TabIndex = 3;
            // 
            // lblID
            // 
            this.lblID.AutoSize = true;
            this.lblID.Location = new System.Drawing.Point(12, 15);
            this.lblID.Name = "lblID";
            this.lblID.Size = new System.Drawing.Size(21, 13);
            this.lblID.TabIndex = 4;
            this.lblID.Text = "ID:";
            // 
            // lblBrand
            // 
            this.lblBrand.AutoSize = true;
            this.lblBrand.Location = new System.Drawing.Point(12, 41);
            this.lblBrand.Name = "lblBrand";
            this.lblBrand.Size = new System.Drawing.Size(38, 13);
            this.lblBrand.TabIndex = 6;
            this.lblBrand.Text = "Brand:";
            // 
            // txtBrand
            // 
            this.txtBrand.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtBrand.Location = new System.Drawing.Point(89, 38);
            this.txtBrand.Name = "txtBrand";
            this.txtBrand.Size = new System.Drawing.Size(358, 20);
            this.txtBrand.TabIndex = 5;
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(12, 67);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(38, 13);
            this.lblName.TabIndex = 8;
            this.lblName.Text = "Name:";
            // 
            // txtName
            // 
            this.txtName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtName.Location = new System.Drawing.Point(89, 64);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(358, 20);
            this.txtName.TabIndex = 7;
            // 
            // lblWheels
            // 
            this.lblWheels.AutoSize = true;
            this.lblWheels.Location = new System.Drawing.Point(12, 93);
            this.lblWheels.Name = "lblWheels";
            this.lblWheels.Size = new System.Drawing.Size(46, 13);
            this.lblWheels.TabIndex = 10;
            this.lblWheels.Text = "Wheels:";
            // 
            // txtWheels
            // 
            this.txtWheels.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtWheels.Location = new System.Drawing.Point(89, 90);
            this.txtWheels.Name = "txtWheels";
            this.txtWheels.Size = new System.Drawing.Size(358, 20);
            this.txtWheels.TabIndex = 9;
            // 
            // lblCategory
            // 
            this.lblCategory.AutoSize = true;
            this.lblCategory.Location = new System.Drawing.Point(12, 119);
            this.lblCategory.Name = "lblCategory";
            this.lblCategory.Size = new System.Drawing.Size(52, 13);
            this.lblCategory.TabIndex = 12;
            this.lblCategory.Text = "Category:";
            // 
            // txtCategory
            // 
            this.txtCategory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCategory.Location = new System.Drawing.Point(89, 116);
            this.txtCategory.Name = "txtCategory";
            this.txtCategory.Size = new System.Drawing.Size(358, 20);
            this.txtCategory.TabIndex = 11;
            // 
            // lblFlags
            // 
            this.lblFlags.AutoSize = true;
            this.lblFlags.Location = new System.Drawing.Point(12, 145);
            this.lblFlags.Name = "lblFlags";
            this.lblFlags.Size = new System.Drawing.Size(35, 13);
            this.lblFlags.TabIndex = 14;
            this.lblFlags.Text = "Flags:";
            // 
            // txtFlags
            // 
            this.txtFlags.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFlags.Location = new System.Drawing.Point(89, 142);
            this.txtFlags.Name = "txtFlags";
            this.txtFlags.Size = new System.Drawing.Size(358, 20);
            this.txtFlags.TabIndex = 13;
            // 
            // lblUnknown26
            // 
            this.lblUnknown26.AutoSize = true;
            this.lblUnknown26.Location = new System.Drawing.Point(12, 171);
            this.lblUnknown26.Name = "lblUnknown26";
            this.lblUnknown26.Size = new System.Drawing.Size(64, 13);
            this.lblUnknown26.TabIndex = 16;
            this.lblUnknown26.Text = "Max Speed:";
            // 
            // txtUnknown26
            // 
            this.txtUnknown26.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtUnknown26.Location = new System.Drawing.Point(89, 168);
            this.txtUnknown26.Name = "txtUnknown26";
            this.txtUnknown26.Size = new System.Drawing.Size(121, 20);
            this.txtUnknown26.TabIndex = 15;
            // 
            // txtGroupID1
            // 
            this.txtGroupID1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtGroupID1.Location = new System.Drawing.Point(89, 194);
            this.txtGroupID1.Name = "txtGroupID1";
            this.txtGroupID1.Size = new System.Drawing.Size(358, 20);
            this.txtGroupID1.TabIndex = 17;
            // 
            // lblGroupID1
            // 
            this.lblGroupID1.AutoSize = true;
            this.lblGroupID1.Location = new System.Drawing.Point(12, 197);
            this.lblGroupID1.Name = "lblGroupID1";
            this.lblGroupID1.Size = new System.Drawing.Size(59, 13);
            this.lblGroupID1.TabIndex = 18;
            this.lblGroupID1.Text = "Group ID1:";
            // 
            // lblGroupID2
            // 
            this.lblGroupID2.AutoSize = true;
            this.lblGroupID2.Location = new System.Drawing.Point(12, 223);
            this.lblGroupID2.Name = "lblGroupID2";
            this.lblGroupID2.Size = new System.Drawing.Size(59, 13);
            this.lblGroupID2.TabIndex = 20;
            this.lblGroupID2.Text = "Group ID2:";
            // 
            // txtGroupID2
            // 
            this.txtGroupID2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtGroupID2.Location = new System.Drawing.Point(89, 220);
            this.txtGroupID2.Name = "txtGroupID2";
            this.txtGroupID2.Size = new System.Drawing.Size(358, 20);
            this.txtGroupID2.TabIndex = 19;
            // 
            // lblBoost
            // 
            this.lblBoost.AutoSize = true;
            this.lblBoost.Location = new System.Drawing.Point(13, 249);
            this.lblBoost.Name = "lblBoost";
            this.lblBoost.Size = new System.Drawing.Size(64, 13);
            this.lblBoost.TabIndex = 21;
            this.lblBoost.Text = "Boost Type:";
            // 
            // cboBoost
            // 
            this.cboBoost.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboBoost.FormattingEnabled = true;
            this.cboBoost.Items.AddRange(new object[] {
            "Speed",
            "Aggression",
            "Stunt",
            "None",
            "Locked",
            "Bike"});
            this.cboBoost.Location = new System.Drawing.Point(89, 246);
            this.cboBoost.Name = "cboBoost";
            this.cboBoost.Size = new System.Drawing.Size(121, 21);
            this.cboBoost.TabIndex = 22;
            // 
            // lblLockByte
            // 
            this.lblLockByte.AutoSize = true;
            this.lblLockByte.Location = new System.Drawing.Point(13, 276);
            this.lblLockByte.Name = "lblLockByte";
            this.lblLockByte.Size = new System.Drawing.Size(37, 13);
            this.lblLockByte.TabIndex = 23;
            this.lblLockByte.Text = "Finish:";
            // 
            // cboLockByte
            // 
            this.cboLockByte.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboLockByte.FormattingEnabled = true;
            this.cboLockByte.Items.AddRange(new object[] {
            "Default",
            "Unknown2",
            "Upgrade",
            "Unknown3",
            "Unknown4",
            "Unknown5"});
            this.cboLockByte.Location = new System.Drawing.Point(89, 273);
            this.cboLockByte.Name = "cboLockByte";
            this.cboLockByte.Size = new System.Drawing.Size(121, 21);
            this.cboLockByte.TabIndex = 24;
            // 
            // lblEngineID1
            // 
            this.lblEngineID1.AutoSize = true;
            this.lblEngineID1.Location = new System.Drawing.Point(12, 303);
            this.lblEngineID1.Name = "lblEngineID1";
            this.lblEngineID1.Size = new System.Drawing.Size(63, 13);
            this.lblEngineID1.TabIndex = 26;
            this.lblEngineID1.Text = "Engine ID1:";
            // 
            // txtEngineID1
            // 
            this.txtEngineID1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtEngineID1.Location = new System.Drawing.Point(89, 300);
            this.txtEngineID1.Name = "txtEngineID1";
            this.txtEngineID1.Size = new System.Drawing.Size(358, 20);
            this.txtEngineID1.TabIndex = 25;
            // 
            // lblEngineID2
            // 
            this.lblEngineID2.AutoSize = true;
            this.lblEngineID2.Location = new System.Drawing.Point(12, 329);
            this.lblEngineID2.Name = "lblEngineID2";
            this.lblEngineID2.Size = new System.Drawing.Size(63, 13);
            this.lblEngineID2.TabIndex = 28;
            this.lblEngineID2.Text = "Engine ID2:";
            // 
            // txtEngineID2
            // 
            this.txtEngineID2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtEngineID2.Location = new System.Drawing.Point(89, 326);
            this.txtEngineID2.Name = "txtEngineID2";
            this.txtEngineID2.Size = new System.Drawing.Size(358, 20);
            this.txtEngineID2.TabIndex = 27;
            // 
            // lblSortOrder
            // 
            this.lblSortOrder.AutoSize = true;
            this.lblSortOrder.Location = new System.Drawing.Point(226, 171);
            this.lblSortOrder.Name = "lblSortOrder";
            this.lblSortOrder.Size = new System.Drawing.Size(94, 13);
            this.lblSortOrder.TabIndex = 30;
            this.lblSortOrder.Text = "Max Boost Speed:";
            // 
            // txtSortOrder
            // 
            this.txtSortOrder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSortOrder.Location = new System.Drawing.Point(326, 168);
            this.txtSortOrder.Name = "txtSortOrder";
            this.txtSortOrder.Size = new System.Drawing.Size(121, 20);
            this.txtSortOrder.TabIndex = 29;
            // 
            // txtDisplaySpeed
            // 
            this.txtDisplaySpeed.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDisplaySpeed.Location = new System.Drawing.Point(138, 352);
            this.txtDisplaySpeed.Name = "txtDisplaySpeed";
            this.txtDisplaySpeed.Size = new System.Drawing.Size(66, 20);
            this.txtDisplaySpeed.TabIndex = 31;
            // 
            // lblDisplaySpeed
            // 
            this.lblDisplaySpeed.AutoSize = true;
            this.lblDisplaySpeed.Location = new System.Drawing.Point(91, 355);
            this.lblDisplaySpeed.Name = "lblDisplaySpeed";
            this.lblDisplaySpeed.Size = new System.Drawing.Size(41, 13);
            this.lblDisplaySpeed.TabIndex = 32;
            this.lblDisplaySpeed.Text = "Speed:";
            // 
            // lblDisplayBoost
            // 
            this.lblDisplayBoost.AutoSize = true;
            this.lblDisplayBoost.Location = new System.Drawing.Point(210, 355);
            this.lblDisplayBoost.Name = "lblDisplayBoost";
            this.lblDisplayBoost.Size = new System.Drawing.Size(37, 13);
            this.lblDisplayBoost.TabIndex = 34;
            this.lblDisplayBoost.Text = "Boost:";
            // 
            // txtDisplayBoost
            // 
            this.txtDisplayBoost.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDisplayBoost.Location = new System.Drawing.Point(253, 352);
            this.txtDisplayBoost.Name = "txtDisplayBoost";
            this.txtDisplayBoost.Size = new System.Drawing.Size(66, 20);
            this.txtDisplayBoost.TabIndex = 33;
            // 
            // lblDisplayStrength
            // 
            this.lblDisplayStrength.AutoSize = true;
            this.lblDisplayStrength.Location = new System.Drawing.Point(325, 355);
            this.lblDisplayStrength.Name = "lblDisplayStrength";
            this.lblDisplayStrength.Size = new System.Drawing.Size(50, 13);
            this.lblDisplayStrength.TabIndex = 35;
            this.lblDisplayStrength.Text = "Strength:";
            // 
            // txtDisplayStrength
            // 
            this.txtDisplayStrength.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDisplayStrength.Location = new System.Drawing.Point(381, 352);
            this.txtDisplayStrength.Name = "txtDisplayStrength";
            this.txtDisplayStrength.Size = new System.Drawing.Size(66, 20);
            this.txtDisplayStrength.TabIndex = 36;
            // 
            // VehicleEditor
            // 
            this.AcceptButton = this.btnOk;
            this.AccessibleName = "";
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(459, 414);
            this.Controls.Add(this.txtDisplayStrength);
            this.Controls.Add(this.lblDisplayStrength);
            this.Controls.Add(this.lblDisplayBoost);
            this.Controls.Add(this.txtDisplayBoost);
            this.Controls.Add(this.lblDisplaySpeed);
            this.Controls.Add(this.txtDisplaySpeed);
            this.Controls.Add(this.lblSortOrder);
            this.Controls.Add(this.txtSortOrder);
            this.Controls.Add(this.lblEngineID2);
            this.Controls.Add(this.txtEngineID2);
            this.Controls.Add(this.lblEngineID1);
            this.Controls.Add(this.txtEngineID1);
            this.Controls.Add(this.cboLockByte);
            this.Controls.Add(this.lblLockByte);
            this.Controls.Add(this.cboBoost);
            this.Controls.Add(this.lblBoost);
            this.Controls.Add(this.lblGroupID2);
            this.Controls.Add(this.txtGroupID2);
            this.Controls.Add(this.lblGroupID1);
            this.Controls.Add(this.txtGroupID1);
            this.Controls.Add(this.lblUnknown26);
            this.Controls.Add(this.txtUnknown26);
            this.Controls.Add(this.lblFlags);
            this.Controls.Add(this.txtFlags);
            this.Controls.Add(this.lblCategory);
            this.Controls.Add(this.txtCategory);
            this.Controls.Add(this.lblWheels);
            this.Controls.Add(this.txtWheels);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.lblBrand);
            this.Controls.Add(this.txtBrand);
            this.Controls.Add(this.lblID);
            this.Controls.Add(this.txtID);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "VehicleEditor";
            this.Text = "Vehicle Editor";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TextBox txtID;
        private System.Windows.Forms.Label lblID;
        private System.Windows.Forms.Label lblBrand;
        private System.Windows.Forms.TextBox txtBrand;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label lblWheels;
        private System.Windows.Forms.TextBox txtWheels;
        private System.Windows.Forms.Label lblCategory;
        private System.Windows.Forms.TextBox txtCategory;
        private System.Windows.Forms.Label lblFlags;
        private System.Windows.Forms.TextBox txtFlags;
        private System.Windows.Forms.Label lblUnknown26;
        private System.Windows.Forms.TextBox txtUnknown26;
        private System.Windows.Forms.TextBox txtGroupID1;
        private System.Windows.Forms.Label lblGroupID1;
        private System.Windows.Forms.Label lblGroupID2;
        private System.Windows.Forms.TextBox txtGroupID2;
        private System.Windows.Forms.Label lblBoost;
        private System.Windows.Forms.ComboBox cboBoost;
        private System.Windows.Forms.Label lblLockByte;
        private System.Windows.Forms.ComboBox cboLockByte;
        private System.Windows.Forms.Label lblEngineID1;
        private System.Windows.Forms.TextBox txtEngineID1;
        private System.Windows.Forms.Label lblEngineID2;
        private System.Windows.Forms.TextBox txtEngineID2;
        private System.Windows.Forms.Label lblSortOrder;
        private System.Windows.Forms.TextBox txtSortOrder;
        private System.Windows.Forms.TextBox txtDisplaySpeed;
        private System.Windows.Forms.Label lblDisplaySpeed;
        private System.Windows.Forms.Label lblDisplayBoost;
        private System.Windows.Forms.TextBox txtDisplayBoost;
        private System.Windows.Forms.Label lblDisplayStrength;
        private System.Windows.Forms.TextBox txtDisplayStrength;
    }
}