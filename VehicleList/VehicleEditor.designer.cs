using System.Windows.Forms;

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
            this.lblIndex = new System.Windows.Forms.Label();
            this.txtIndex = new System.Windows.Forms.TextBox();
            this.lblID = new System.Windows.Forms.Label();
            this.txtID = new System.Windows.Forms.TextBox();
            this.lblParentID = new System.Windows.Forms.Label();
            this.txtParentID = new System.Windows.Forms.TextBox();
            this.lblWheels = new System.Windows.Forms.Label();
            this.txtWheels = new System.Windows.Forms.TextBox();
            this.lblName = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.lblBrand = new System.Windows.Forms.Label();
            this.txtBrand = new System.Windows.Forms.TextBox();
            this.lblDamageLimit = new System.Windows.Forms.Label();
            this.txtDamageLimit = new System.Windows.Forms.TextBox();
            this.lblFlags = new System.Windows.Forms.Label();
            this.chlFlags = new System.Windows.Forms.CheckedListBox();
            this.lblBoostLength = new System.Windows.Forms.Label();
            this.txtBoostLength = new System.Windows.Forms.TextBox();
            this.lblRank = new System.Windows.Forms.Label();
            this.cboRank = new System.Windows.Forms.ComboBox();
            this.lblBoostCapacity = new System.Windows.Forms.Label();
            this.txtBoostCapacity = new System.Windows.Forms.TextBox();
            this.lblStrengthStat = new System.Windows.Forms.Label();
            this.txtStrengthStat = new System.Windows.Forms.TextBox();
            this.lblAttribSysCollectionKey = new System.Windows.Forms.Label();
            this.txtAttribSysCollectionKey = new System.Windows.Forms.TextBox();
            this.lblExhaustName = new System.Windows.Forms.Label();
            this.txtExhaustName = new System.Windows.Forms.TextBox();
            this.lblExhaustID = new System.Windows.Forms.Label();
            this.txtExhaustID = new System.Windows.Forms.TextBox();
            this.lblEngineID = new System.Windows.Forms.Label();
            this.txtEngineID = new System.Windows.Forms.TextBox();
            this.lblEngineName = new System.Windows.Forms.Label();
            this.txtEngineName = new System.Windows.Forms.TextBox();
            this.lblClassUnlock = new System.Windows.Forms.Label();
            this.cboClassUnlock = new System.Windows.Forms.ComboBox();
            this.lblCarWon = new System.Windows.Forms.Label();
            this.txtCarWon = new System.Windows.Forms.TextBox();
            this.lblCarReleased = new System.Windows.Forms.Label();
            this.txtCarReleased = new System.Windows.Forms.TextBox();
            this.lblAIMusic = new System.Windows.Forms.Label();
            this.cboAIMusic = new System.Windows.Forms.ComboBox();
            this.lblAIExhaust1 = new System.Windows.Forms.Label();
            this.cboAIExhaust1 = new System.Windows.Forms.ComboBox();
            this.lblAIExhaust2 = new System.Windows.Forms.Label();
            this.cboAIExhaust2 = new System.Windows.Forms.ComboBox();
            this.lblAIExhaust3 = new System.Windows.Forms.Label();
            this.cboAIExhaust3 = new System.Windows.Forms.ComboBox();
            this.lblCategory = new System.Windows.Forms.Label();
            this.chlCategory = new System.Windows.Forms.CheckedListBox();
            this.lblVehicleType = new System.Windows.Forms.Label();
            this.cboVehicleType = new System.Windows.Forms.ComboBox();
            this.lblBoostType = new System.Windows.Forms.Label();
            this.cboBoostType = new System.Windows.Forms.ComboBox();
            this.lblFinishType = new System.Windows.Forms.Label();
            this.cboFinishType = new System.Windows.Forms.ComboBox();
            this.lblMaxSpeed = new System.Windows.Forms.Label();
            this.txtMaxSpeed = new System.Windows.Forms.TextBox();
            this.lblMaxBoostSpeed = new System.Windows.Forms.Label();
            this.txtMaxBoostSpeed = new System.Windows.Forms.TextBox();
            this.lblSpeedStat = new System.Windows.Forms.Label();
            this.txtSpeedStat = new System.Windows.Forms.TextBox();
            this.lblBoostStat = new System.Windows.Forms.Label();
            this.txtBoostStat = new System.Windows.Forms.TextBox();
            this.lblColor = new System.Windows.Forms.Label();
            this.txtColor = new System.Windows.Forms.TextBox();
            this.lblColorType = new System.Windows.Forms.Label();
            this.cboColorType = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            
            // btnOk
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.Location = new System.Drawing.Point(372, 785);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 0;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            
            // btnCancel
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(291, 785);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            
            // lblIndex
            this.lblIndex.AutoSize = true;
            this.lblIndex.Location = new System.Drawing.Point(2, 5);
            this.lblIndex.Name = "lblIndex";
            this.lblIndex.Size = new System.Drawing.Size(36, 13);
            this.lblIndex.TabIndex = 2;
            this.lblIndex.Text = "Index:";
            
            // txtIndex
            this.txtIndex.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtIndex.Location = new System.Drawing.Point(96, 2);
            this.txtIndex.Name = "txtIndex";
            this.txtIndex.Size = new System.Drawing.Size(358, 20);
            this.txtIndex.TabIndex = 3;
            
            // lblID
            this.lblID.AutoSize = true;
            this.lblID.Location = new System.Drawing.Point(2, 26);
            this.lblID.Name = "lblID";
            this.lblID.Size = new System.Drawing.Size(21, 13);
            this.lblID.TabIndex = 4;
            this.lblID.Text = "ID:";
            
            // txtID
            this.txtID.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtID.Location = new System.Drawing.Point(96, 23);
            this.txtID.Name = "txtID";
            this.txtID.Size = new System.Drawing.Size(358, 20);
            this.txtID.TabIndex = 5;
            
            // lblParentID
            this.lblParentID.AutoSize = true;
            this.lblParentID.Location = new System.Drawing.Point(2, 47);
            this.lblParentID.Name = "lblParentID";
            this.lblParentID.Size = new System.Drawing.Size(55, 13);
            this.lblParentID.TabIndex = 6;
            this.lblParentID.Text = "Parent ID:";
            
            // txtParentID
            this.txtParentID.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtParentID.Location = new System.Drawing.Point(96, 44);
            this.txtParentID.Name = "txtParentID";
            this.txtParentID.Size = new System.Drawing.Size(358, 20);
            this.txtParentID.TabIndex = 7;
            
            // lblWheels
            this.lblWheels.AutoSize = true;
            this.lblWheels.Location = new System.Drawing.Point(2, 68);
            this.lblWheels.Name = "lblWheels";
            this.lblWheels.Size = new System.Drawing.Size(46, 13);
            this.lblWheels.TabIndex = 8;
            this.lblWheels.Text = "Wheels:";
            
            // txtWheels
            this.txtWheels.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtWheels.Location = new System.Drawing.Point(96, 65);
            this.txtWheels.Name = "txtWheels";
            this.txtWheels.Size = new System.Drawing.Size(358, 20);
            this.txtWheels.TabIndex = 9;
            
            // lblName
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(2, 89);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(38, 13);
            this.lblName.TabIndex = 10;
            this.lblName.Text = "Name:";
            
            // txtName
            this.txtName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtName.Location = new System.Drawing.Point(96, 86);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(358, 20);
            this.txtName.TabIndex = 11;
            
            // lblBrand
            this.lblBrand.AutoSize = true;
            this.lblBrand.Location = new System.Drawing.Point(2, 110);
            this.lblBrand.Name = "lblBrand";
            this.lblBrand.Size = new System.Drawing.Size(38, 13);
            this.lblBrand.TabIndex = 12;
            this.lblBrand.Text = "Brand:";
            
            // txtBrand
            this.txtBrand.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtBrand.Location = new System.Drawing.Point(96, 107);
            this.txtBrand.Name = "txtBrand";
            this.txtBrand.Size = new System.Drawing.Size(358, 20);
            this.txtBrand.TabIndex = 13;
            
            // lblDamageLimit
            this.lblDamageLimit.AutoSize = true;
            this.lblDamageLimit.Location = new System.Drawing.Point(2, 131);
            this.lblDamageLimit.Name = "lblDamageLimit";
            this.lblDamageLimit.Size = new System.Drawing.Size(74, 13);
            this.lblDamageLimit.TabIndex = 14;
            this.lblDamageLimit.Text = "Damage Limit:";
            
            // txtDamageLimit
            this.txtDamageLimit.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDamageLimit.Location = new System.Drawing.Point(96, 128);
            this.txtDamageLimit.Name = "txtDamageLimit";
            this.txtDamageLimit.Size = new System.Drawing.Size(358, 20);
            this.txtDamageLimit.TabIndex = 15;
            
            // lblFlags
            this.lblFlags.AutoSize = true;
            this.lblFlags.Location = new System.Drawing.Point(2, 152);
            this.lblFlags.Name = "lblFlags";
            this.lblFlags.Size = new System.Drawing.Size(35, 13);
            this.lblFlags.TabIndex = 16;
            this.lblFlags.Text = "Flags:";
            
            // chlFlags
            this.chlFlags.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chlFlags.CheckOnClick = true;
            this.chlFlags.Items.AddRange(new object[] {
            "Is Race Vehicle",
            "Can Check Traffic",
            "Can Be Checked",
            "Is Trailer",
            "Can Tow Trailer",
            "Paintable",
            "Unknown0",
            "Is First Car in Speed Range",
            "Has Switchable Boost",
            "Unknown1",
            "Unknown2",
            "Is WIP/Dev",
            "Is from 1.0",
            "Is from 1.3",
            "Is from 1.4",
            "Is from 1.5",
            "Is from 1.6",
            "Is from 1.7",
            "Is from 1.8",
            "Is from 1.9"});
            this.chlFlags.Location = new System.Drawing.Point(96, 149);
            this.chlFlags.Name = "chlFlags";
            this.chlFlags.Size = new System.Drawing.Size(358, 49);
            this.chlFlags.TabIndex = 17;
            
            // lblBoostLength
            this.lblBoostLength.AutoSize = true;
            this.lblBoostLength.Location = new System.Drawing.Point(2, 202);
            this.lblBoostLength.Name = "lblBoostLength";
            this.lblBoostLength.Size = new System.Drawing.Size(73, 13);
            this.lblBoostLength.TabIndex = 18;
            this.lblBoostLength.Text = "Boost Length:";
            
            // txtBoostLength
            this.txtBoostLength.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtBoostLength.Location = new System.Drawing.Point(96, 199);
            this.txtBoostLength.Name = "txtBoostLength";
            this.txtBoostLength.Size = new System.Drawing.Size(358, 20);
            this.txtBoostLength.TabIndex = 19;
            
            // lblRank
            this.lblRank.AutoSize = true;
            this.lblRank.Location = new System.Drawing.Point(2, 223);
            this.lblRank.Name = "lblRank";
            this.lblRank.Size = new System.Drawing.Size(36, 13);
            this.lblRank.TabIndex = 20;
            this.lblRank.Text = "Rank:";
            
            // cboRank
            this.cboRank.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboRank.FormattingEnabled = true;
            this.cboRank.Items.AddRange(new object[] {
            "Learner",
            "D_Class",
            "C_Class",
            "B_Class",
            "A_Class",
            "Burnout"});
            this.cboRank.Location = new System.Drawing.Point(96, 220);
            this.cboRank.Name = "cboRank";
            this.cboRank.Size = new System.Drawing.Size(358, 21);
            this.cboRank.TabIndex = 21;
            
            // lblBoostCapacity
            this.lblBoostCapacity.AutoSize = true;
            this.lblBoostCapacity.Location = new System.Drawing.Point(2, 245);
            this.lblBoostCapacity.Name = "lblBoostCapacity";
            this.lblBoostCapacity.Size = new System.Drawing.Size(81, 13);
            this.lblBoostCapacity.TabIndex = 22;
            this.lblBoostCapacity.Text = "Boost Capacity:";
            // 
            // txtBoostCapacity
            // 
            this.txtBoostCapacity.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtBoostCapacity.Location = new System.Drawing.Point(96, 242);
            this.txtBoostCapacity.Name = "txtBoostCapacity";
            this.txtBoostCapacity.Size = new System.Drawing.Size(358, 20);
            this.txtBoostCapacity.TabIndex = 23;
            
            // lblStrengthStat
            this.lblStrengthStat.AutoSize = true;
            this.lblStrengthStat.Location = new System.Drawing.Point(2, 266);
            this.lblStrengthStat.Name = "lblStrengthStat";
            this.lblStrengthStat.Size = new System.Drawing.Size(72, 13);
            this.lblStrengthStat.TabIndex = 24;
            this.lblStrengthStat.Text = "Strength Stat:";
            
            // txtStrengthStat
            this.txtStrengthStat.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtStrengthStat.Location = new System.Drawing.Point(96, 263);
            this.txtStrengthStat.Name = "txtStrengthStat";
            this.txtStrengthStat.Size = new System.Drawing.Size(358, 20);
            this.txtStrengthStat.TabIndex = 25;
            
            // lblAttribSysCollectionKey
            this.lblAttribSysCollectionKey.AutoSize = true;
            this.lblAttribSysCollectionKey.Location = new System.Drawing.Point(2, 287);
            this.lblAttribSysCollectionKey.Name = "lblAttribSysCollectionKey";
            this.lblAttribSysCollectionKey.Size = new System.Drawing.Size(65, 13);
            this.lblAttribSysCollectionKey.TabIndex = 26;
            this.lblAttribSysCollectionKey.Text = "AttribSys ID:";
            
            // txtAttribSysCollectionKey
            this.txtAttribSysCollectionKey.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtAttribSysCollectionKey.Location = new System.Drawing.Point(96, 284);
            this.txtAttribSysCollectionKey.Name = "txtAttribSysCollectionKey";
            this.txtAttribSysCollectionKey.Size = new System.Drawing.Size(358, 20);
            this.txtAttribSysCollectionKey.TabIndex = 27;
            
            // lblExhaustName
            this.lblExhaustName.AutoSize = true;
            this.lblExhaustName.Location = new System.Drawing.Point(2, 308);
            this.lblExhaustName.Name = "lblExhaustName";
            this.lblExhaustName.Size = new System.Drawing.Size(79, 13);
            this.lblExhaustName.TabIndex = 28;
            this.lblExhaustName.Text = "Exhaust Name:";
            
            // txtExhaustName
            this.txtExhaustName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtExhaustName.Location = new System.Drawing.Point(96, 305);
            this.txtExhaustName.Name = "txtExhaustName";
            this.txtExhaustName.Size = new System.Drawing.Size(358, 20);
            this.txtExhaustName.TabIndex = 29;
            
            // lblExhaustID
            this.lblExhaustID.AutoSize = true;
            this.lblExhaustID.Location = new System.Drawing.Point(2, 329);
            this.lblExhaustID.Name = "lblExhaustID";
            this.lblExhaustID.Size = new System.Drawing.Size(62, 13);
            this.lblExhaustID.TabIndex = 30;
            this.lblExhaustID.Text = "Exhaust ID:";
            
            // txtExhaustID
            this.txtExhaustID.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtExhaustID.Location = new System.Drawing.Point(96, 326);
            this.txtExhaustID.Name = "txtExhaustID";
            this.txtExhaustID.Size = new System.Drawing.Size(358, 20);
            this.txtExhaustID.TabIndex = 31;
            
            // lblEngineID
            this.lblEngineID.AutoSize = true;
            this.lblEngineID.Location = new System.Drawing.Point(2, 350);
            this.lblEngineID.Name = "lblEngineID";
            this.lblEngineID.Size = new System.Drawing.Size(57, 13);
            this.lblEngineID.TabIndex = 32;
            this.lblEngineID.Text = "Engine ID:";
            
            // txtEngineID
            this.txtEngineID.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtEngineID.Location = new System.Drawing.Point(96, 347);
            this.txtEngineID.Name = "txtEngineID";
            this.txtEngineID.Size = new System.Drawing.Size(358, 20);
            this.txtEngineID.TabIndex = 33;
            
            // lblEngineName
            this.lblEngineName.AutoSize = true;
            this.lblEngineName.Location = new System.Drawing.Point(2, 371);
            this.lblEngineName.Name = "lblEngineName";
            this.lblEngineName.Size = new System.Drawing.Size(74, 13);
            this.lblEngineName.TabIndex = 34;
            this.lblEngineName.Text = "Engine Name:";
            
            // txtEngineName
            this.txtEngineName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtEngineName.Location = new System.Drawing.Point(96, 368);
            this.txtEngineName.Name = "txtEngineName";
            this.txtEngineName.Size = new System.Drawing.Size(358, 20);
            this.txtEngineName.TabIndex = 35;
            
            // lblClassUnlock
            this.lblClassUnlock.AutoSize = true;
            this.lblClassUnlock.Location = new System.Drawing.Point(2, 392);
            this.lblClassUnlock.Name = "lblClassUnlock";
            this.lblClassUnlock.Size = new System.Drawing.Size(72, 13);
            this.lblClassUnlock.TabIndex = 36;
            this.lblClassUnlock.Text = "Class Unlock:";
            
            // cboClassUnlock
            this.cboClassUnlock.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboClassUnlock.FormattingEnabled = true;
            this.cboClassUnlock.Items.AddRange(new object[] {
            "SuperClassUnlock",
            "MuscleClassUnlock",
            "F1ClassUnlock",
            "TunerClassUnlock",
            "HotRodClassUnlock",
            "RivalGen"});
            this.cboClassUnlock.Location = new System.Drawing.Point(96, 389);
            this.cboClassUnlock.Name = "cboClassUnlock";
            this.cboClassUnlock.Size = new System.Drawing.Size(358, 21);
            this.cboClassUnlock.TabIndex = 37;
            
            // lblCarWon
            this.lblCarWon.AutoSize = true;
            this.lblCarWon.Location = new System.Drawing.Point(2, 414);
            this.lblCarWon.Name = "lblCarWon";
            this.lblCarWon.Size = new System.Drawing.Size(52, 13);
            this.lblCarWon.TabIndex = 38;
            this.lblCarWon.Text = "Car Won:";
            
            // txtCarWon
            this.txtCarWon.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCarWon.Location = new System.Drawing.Point(96, 411);
            this.txtCarWon.Name = "txtCarWon";
            this.txtCarWon.Size = new System.Drawing.Size(358, 20);
            this.txtCarWon.TabIndex = 39;
            
            // lblCarReleased
            this.lblCarReleased.AutoSize = true;
            this.lblCarReleased.Location = new System.Drawing.Point(2, 435);
            this.lblCarReleased.Name = "lblCarReleased";
            this.lblCarReleased.Size = new System.Drawing.Size(74, 13);
            this.lblCarReleased.TabIndex = 40;
            this.lblCarReleased.Text = "Car Released:";
            
            // txtCarReleased
            this.txtCarReleased.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCarReleased.Location = new System.Drawing.Point(96, 432);
            this.txtCarReleased.Name = "txtCarReleased";
            this.txtCarReleased.Size = new System.Drawing.Size(358, 20);
            this.txtCarReleased.TabIndex = 41;
            
            // lblAIMusic
            this.lblAIMusic.AutoSize = true;
            this.lblAIMusic.Location = new System.Drawing.Point(2, 456);
            this.lblAIMusic.Name = "lblAIMusic";
            this.lblAIMusic.Size = new System.Drawing.Size(51, 13);
            this.lblAIMusic.TabIndex = 42;
            this.lblAIMusic.Text = "AI Music:";
            
            // cboAIMusic
            this.cboAIMusic.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboAIMusic.FormattingEnabled = true;
            this.cboAIMusic.Items.AddRange(new object[] {
            "None",
            "Muscle",
            "Truck",
            "Tuner",
            "Sedan",
            "Exotic",
            "Super"});
            this.cboAIMusic.Location = new System.Drawing.Point(96, 453);
            this.cboAIMusic.Name = "cboAIMusic";
            this.cboAIMusic.Size = new System.Drawing.Size(358, 21);
            this.cboAIMusic.TabIndex = 43;
            
            // lblAIExhaust1
            this.lblAIExhaust1.AutoSize = true;
            this.lblAIExhaust1.Location = new System.Drawing.Point(2, 478);
            this.lblAIExhaust1.Name = "lblAIExhaust1";
            this.lblAIExhaust1.Size = new System.Drawing.Size(61, 13);
            this.lblAIExhaust1.TabIndex = 44;
            this.lblAIExhaust1.Text = "AI Exhaust:";
            
            // cboAIExhaust1
            this.cboAIExhaust1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboAIExhaust1.FormattingEnabled = true;
            this.cboAIExhaust1.Items.AddRange(new object[] {
            "None",
            "AIROD_EX",
            "AI_CIVIC_EX",
            "AI_GT_ENG",
            "AI_MUST_EX",
            "AI_F1_EX"});
            this.cboAIExhaust1.Location = new System.Drawing.Point(96, 475);
            this.cboAIExhaust1.Name = "cboAIExhaust1";
            this.cboAIExhaust1.Size = new System.Drawing.Size(358, 21);
            this.cboAIExhaust1.TabIndex = 45;
            
            // lblAIExhaust2
            this.lblAIExhaust2.AutoSize = true;
            this.lblAIExhaust2.Location = new System.Drawing.Point(2, 500);
            this.lblAIExhaust2.Name = "lblAIExhaust2";
            this.lblAIExhaust2.Size = new System.Drawing.Size(70, 13);
            this.lblAIExhaust2.TabIndex = 46;
            this.lblAIExhaust2.Text = "AI Exhaust 2:";
            
            // cboAIExhaust2
            this.cboAIExhaust2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboAIExhaust2.FormattingEnabled = true;
            this.cboAIExhaust2.Items.AddRange(new object[] {
            "None",
            "AIROD_EX",
            "AI_CIVIC_EX",
            "AI_GT_ENG",
            "AI_MUST_EX",
            "AI_F1_EX"});
            this.cboAIExhaust2.Location = new System.Drawing.Point(96, 497);
            this.cboAIExhaust2.Name = "cboAIExhaust2";
            this.cboAIExhaust2.Size = new System.Drawing.Size(358, 21);
            this.cboAIExhaust2.TabIndex = 47;
            
            // lblAIExhaust3
            this.lblAIExhaust3.AutoSize = true;
            this.lblAIExhaust3.Location = new System.Drawing.Point(2, 522);
            this.lblAIExhaust3.Name = "lblAIExhaust3";
            this.lblAIExhaust3.Size = new System.Drawing.Size(70, 13);
            this.lblAIExhaust3.TabIndex = 48;
            this.lblAIExhaust3.Text = "AI Exhaust 3:";
            
            // cboAIExhaust3
            this.cboAIExhaust3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboAIExhaust3.FormattingEnabled = true;
            this.cboAIExhaust3.Items.AddRange(new object[] {
            "None",
            "AIROD_EX",
            "AI_CIVIC_EX",
            "AI_GT_ENG",
            "AI_MUST_EX",
            "AI_F1_EX"});
            this.cboAIExhaust3.Location = new System.Drawing.Point(96, 519);
            this.cboAIExhaust3.Name = "cboAIExhaust3";
            this.cboAIExhaust3.Size = new System.Drawing.Size(358, 21);
            this.cboAIExhaust3.TabIndex = 49;
            
            // lblCategory
            this.lblCategory.AutoSize = true;
            this.lblCategory.Location = new System.Drawing.Point(2, 544);
            this.lblCategory.Name = "lblCategory";
            this.lblCategory.Size = new System.Drawing.Size(52, 13);
            this.lblCategory.TabIndex = 50;
            this.lblCategory.Text = "Category:";
            
            // chlCategory
            this.chlCategory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chlCategory.CheckOnClick = true;
            this.chlCategory.Items.AddRange(new object[] {
            "(1) Paradise Cars",
            "(2) Paradise Bikes",
            "(3) Online Cars",
            "(4) Toy Cars",
            "(5) Legendary Cars",
            "(6) Boost Specials",
            "(7) Cop Cars",
            "(8) Island Cars"});
            this.chlCategory.Location = new System.Drawing.Point(96, 541);
            this.chlCategory.Name = "chlCategory";
            this.chlCategory.Size = new System.Drawing.Size(358, 49);
            this.chlCategory.TabIndex = 51;
            
            // lblVehicleType
            this.lblVehicleType.AutoSize = true;
            this.lblVehicleType.Location = new System.Drawing.Point(2, 594);
            this.lblVehicleType.Name = "lblVehicleType";
            this.lblVehicleType.Size = new System.Drawing.Size(72, 13);
            this.lblVehicleType.TabIndex = 52;
            this.lblVehicleType.Text = "Vehicle Type:";
            
            // cboVehicleType
            this.cboVehicleType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboVehicleType.FormattingEnabled = true;
            this.cboVehicleType.Items.AddRange(new object[] {
            "Car",
            "Bike",
            "Plane"});
            this.cboVehicleType.Location = new System.Drawing.Point(96, 591);
            this.cboVehicleType.Name = "cboVehicleType";
            this.cboVehicleType.Size = new System.Drawing.Size(358, 21);
            this.cboVehicleType.TabIndex = 53;
            
            // lblBoostType
            this.lblBoostType.AutoSize = true;
            this.lblBoostType.Location = new System.Drawing.Point(2, 616);
            this.lblBoostType.Name = "lblBoostType";
            this.lblBoostType.Size = new System.Drawing.Size(64, 13);
            this.lblBoostType.TabIndex = 54;
            this.lblBoostType.Text = "Boost Type:";
            
            // cboBoostType
            this.cboBoostType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboBoostType.FormattingEnabled = true;
            this.cboBoostType.Items.AddRange(new object[] {
            "Speed",
            "Aggression",
            "Stunt",
            "None",
            "Locked"});
            this.cboBoostType.Location = new System.Drawing.Point(96, 613);
            this.cboBoostType.Name = "cboBoostType";
            this.cboBoostType.Size = new System.Drawing.Size(358, 21);
            this.cboBoostType.TabIndex = 55;
            
            // lblFinishType
            this.lblFinishType.AutoSize = true;
            this.lblFinishType.Location = new System.Drawing.Point(2, 638);
            this.lblFinishType.Name = "lblFinishType";
            this.lblFinishType.Size = new System.Drawing.Size(64, 13);
            this.lblFinishType.TabIndex = 56;
            this.lblFinishType.Text = "Finish Type:";
            
            // cboFinishType
            this.cboFinishType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboFinishType.FormattingEnabled = true;
            this.cboFinishType.Items.AddRange(new object[] {
            "Default",
            "Colour",
            "Pattern",
            "Platinum",
            "Gold",
            "Community"});
            this.cboFinishType.Location = new System.Drawing.Point(96, 635);
            this.cboFinishType.Name = "cboFinishType";
            this.cboFinishType.Size = new System.Drawing.Size(358, 21);
            this.cboFinishType.TabIndex = 57;
            
            // lblMaxSpeed
            this.lblMaxSpeed.AutoSize = true;
            this.lblMaxSpeed.Location = new System.Drawing.Point(2, 660);
            this.lblMaxSpeed.Name = "lblMaxSpeed";
            this.lblMaxSpeed.Size = new System.Drawing.Size(64, 13);
            this.lblMaxSpeed.TabIndex = 58;
            this.lblMaxSpeed.Text = "Max Speed:";
            
            // txtMaxSpeed
            this.txtMaxSpeed.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtMaxSpeed.Location = new System.Drawing.Point(96, 657);
            this.txtMaxSpeed.Name = "txtMaxSpeed";
            this.txtMaxSpeed.Size = new System.Drawing.Size(358, 20);
            this.txtMaxSpeed.TabIndex = 59;
            
            // lblMaxBoostSpeed
            this.lblMaxBoostSpeed.AutoSize = true;
            this.lblMaxBoostSpeed.Location = new System.Drawing.Point(2, 681);
            this.lblMaxBoostSpeed.Name = "lblMaxBoostSpeed";
            this.lblMaxBoostSpeed.Size = new System.Drawing.Size(94, 13);
            this.lblMaxBoostSpeed.TabIndex = 60;
            this.lblMaxBoostSpeed.Text = "Max Boost Speed:";
            
            // txtMaxBoostSpeed
            this.txtMaxBoostSpeed.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtMaxBoostSpeed.Location = new System.Drawing.Point(96, 678);
            this.txtMaxBoostSpeed.Name = "txtMaxBoostSpeed";
            this.txtMaxBoostSpeed.Size = new System.Drawing.Size(358, 20);
            this.txtMaxBoostSpeed.TabIndex = 61;
            
            // lblSpeedStat
            this.lblSpeedStat.AutoSize = true;
            this.lblSpeedStat.Location = new System.Drawing.Point(2, 702);
            this.lblSpeedStat.Name = "lblSpeedStat";
            this.lblSpeedStat.Size = new System.Drawing.Size(63, 13);
            this.lblSpeedStat.TabIndex = 62;
            this.lblSpeedStat.Text = "Speed Stat:";
            
            // txtSpeedStat
            this.txtSpeedStat.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSpeedStat.Location = new System.Drawing.Point(96, 699);
            this.txtSpeedStat.Name = "txtSpeedStat";
            this.txtSpeedStat.Size = new System.Drawing.Size(358, 20);
            this.txtSpeedStat.TabIndex = 63;
            
            // lblBoostStat
            this.lblBoostStat.AutoSize = true;
            this.lblBoostStat.Location = new System.Drawing.Point(2, 723);
            this.lblBoostStat.Name = "lblBoostStat";
            this.lblBoostStat.Size = new System.Drawing.Size(59, 13);
            this.lblBoostStat.TabIndex = 64;
            this.lblBoostStat.Text = "Boost Stat:";
            
            // txtBoostStat
            this.txtBoostStat.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtBoostStat.Location = new System.Drawing.Point(96, 720);
            this.txtBoostStat.Name = "txtBoostStat";
            this.txtBoostStat.Size = new System.Drawing.Size(358, 20);
            this.txtBoostStat.TabIndex = 65;
            
            // lblColor
            this.lblColor.AutoSize = true;
            this.lblColor.Location = new System.Drawing.Point(2, 744);
            this.lblColor.Name = "lblColor";
            this.lblColor.Size = new System.Drawing.Size(34, 13);
            this.lblColor.TabIndex = 66;
            this.lblColor.Text = "Color:";
            
            // txtColor
            this.txtColor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtColor.Location = new System.Drawing.Point(96, 741);
            this.txtColor.Name = "txtColor";
            this.txtColor.Size = new System.Drawing.Size(358, 20);
            this.txtColor.TabIndex = 67;
            
            // lblColorType
            this.lblColorType.AutoSize = true;
            this.lblColorType.Location = new System.Drawing.Point(2, 765);
            this.lblColorType.Name = "lblColorType";
            this.lblColorType.Size = new System.Drawing.Size(61, 13);
            this.lblColorType.TabIndex = 68;
            this.lblColorType.Text = "Color Type:";
            
            // cboColorType
            this.cboColorType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboColorType.FormattingEnabled = true;
            this.cboColorType.Items.AddRange(new object[] {
            "Gloss",
            "Metallic",
            "Pearlescent",
            "Special",
            "Unknown"});
            this.cboColorType.Location = new System.Drawing.Point(96, 762);
            this.cboColorType.Name = "cboColorType";
            this.cboColorType.Size = new System.Drawing.Size(358, 21);
            this.cboColorType.TabIndex = 69;
            
            // VehicleEditor
            this.AcceptButton = this.btnOk;
            this.AccessibleName = "";
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(470, 811);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.lblIndex);
            this.Controls.Add(this.txtIndex);
            this.Controls.Add(this.lblID);
            this.Controls.Add(this.txtID);
            this.Controls.Add(this.lblParentID);
            this.Controls.Add(this.txtParentID);
            this.Controls.Add(this.lblWheels);
            this.Controls.Add(this.txtWheels);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.lblBrand);
            this.Controls.Add(this.txtBrand);
            this.Controls.Add(this.lblDamageLimit);
            this.Controls.Add(this.txtDamageLimit);
            this.Controls.Add(this.lblFlags);
            this.Controls.Add(this.chlFlags);
            this.Controls.Add(this.lblBoostLength);
            this.Controls.Add(this.txtBoostLength);
            this.Controls.Add(this.lblRank);
            this.Controls.Add(this.cboRank);
            this.Controls.Add(this.lblBoostCapacity);
            this.Controls.Add(this.txtBoostCapacity);
            this.Controls.Add(this.lblStrengthStat);
            this.Controls.Add(this.txtStrengthStat);
            this.Controls.Add(this.lblAttribSysCollectionKey);
            this.Controls.Add(this.txtAttribSysCollectionKey);
            this.Controls.Add(this.lblExhaustName);
            this.Controls.Add(this.txtExhaustName);
            this.Controls.Add(this.lblExhaustID);
            this.Controls.Add(this.txtExhaustID);
            this.Controls.Add(this.lblEngineID);
            this.Controls.Add(this.txtEngineID);
            this.Controls.Add(this.lblEngineName);
            this.Controls.Add(this.txtEngineName);
            this.Controls.Add(this.lblClassUnlock);
            this.Controls.Add(this.cboClassUnlock);
            this.Controls.Add(this.lblCarWon);
            this.Controls.Add(this.txtCarWon);
            this.Controls.Add(this.lblCarReleased);
            this.Controls.Add(this.txtCarReleased);
            this.Controls.Add(this.lblAIMusic);
            this.Controls.Add(this.cboAIMusic);
            this.Controls.Add(this.lblAIExhaust1);
            this.Controls.Add(this.cboAIExhaust1);
            this.Controls.Add(this.lblAIExhaust2);
            this.Controls.Add(this.cboAIExhaust2);
            this.Controls.Add(this.lblAIExhaust3);
            this.Controls.Add(this.cboAIExhaust3);
            this.Controls.Add(this.lblCategory);
            this.Controls.Add(this.chlCategory);
            this.Controls.Add(this.lblVehicleType);
            this.Controls.Add(this.cboVehicleType);
            this.Controls.Add(this.lblBoostType);
            this.Controls.Add(this.cboBoostType);
            this.Controls.Add(this.lblFinishType);
            this.Controls.Add(this.cboFinishType);
            this.Controls.Add(this.lblMaxSpeed);
            this.Controls.Add(this.txtMaxSpeed);
            this.Controls.Add(this.lblMaxBoostSpeed);
            this.Controls.Add(this.txtMaxBoostSpeed);
            this.Controls.Add(this.lblSpeedStat);
            this.Controls.Add(this.txtSpeedStat);
            this.Controls.Add(this.lblBoostStat);
            this.Controls.Add(this.txtBoostStat);
            this.Controls.Add(this.lblColor);
            this.Controls.Add(this.txtColor);
            this.Controls.Add(this.lblColorType);
            this.Controls.Add(this.cboColorType);
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
        private System.Windows.Forms.Label lblIndex;
        private System.Windows.Forms.TextBox txtIndex;
        private System.Windows.Forms.Label lblID;
        private System.Windows.Forms.TextBox txtID;
        private System.Windows.Forms.Label lblParentID;
        private System.Windows.Forms.TextBox txtParentID;
        private System.Windows.Forms.Label lblWheels;
        private System.Windows.Forms.TextBox txtWheels;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label lblBrand;
        private System.Windows.Forms.TextBox txtBrand;
        private System.Windows.Forms.Label lblDamageLimit;
        private System.Windows.Forms.TextBox txtDamageLimit;
        private System.Windows.Forms.Label lblFlags;
        public System.Windows.Forms.CheckedListBox chlFlags;
        private System.Windows.Forms.Label lblBoostLength;
        private System.Windows.Forms.TextBox txtBoostLength;
        private System.Windows.Forms.Label lblRank;
        private System.Windows.Forms.ComboBox cboRank;
        private System.Windows.Forms.Label lblBoostCapacity;
        private System.Windows.Forms.TextBox txtBoostCapacity;
        private System.Windows.Forms.Label lblStrengthStat;
        private System.Windows.Forms.TextBox txtStrengthStat;
        private System.Windows.Forms.Label lblAttribSysCollectionKey;
        private System.Windows.Forms.TextBox txtAttribSysCollectionKey;
        private System.Windows.Forms.Label lblExhaustName;
        private System.Windows.Forms.TextBox txtExhaustName;
        private System.Windows.Forms.Label lblExhaustID;
        private System.Windows.Forms.TextBox txtExhaustID;
        private System.Windows.Forms.Label lblEngineID;
        private System.Windows.Forms.TextBox txtEngineID;
        private System.Windows.Forms.Label lblEngineName;
        private System.Windows.Forms.TextBox txtEngineName;
        private System.Windows.Forms.Label lblClassUnlock;
        private System.Windows.Forms.ComboBox cboClassUnlock;
        private System.Windows.Forms.Label lblCarWon;
        private System.Windows.Forms.TextBox txtCarWon;
        private System.Windows.Forms.Label lblCarReleased;
        private System.Windows.Forms.TextBox txtCarReleased;
        private System.Windows.Forms.Label lblAIMusic;
        private System.Windows.Forms.ComboBox cboAIMusic;
        private System.Windows.Forms.Label lblAIExhaust1;
        private System.Windows.Forms.ComboBox cboAIExhaust1;
        private System.Windows.Forms.Label lblAIExhaust2;
        private System.Windows.Forms.ComboBox cboAIExhaust2;
        private System.Windows.Forms.Label lblAIExhaust3;
        private System.Windows.Forms.ComboBox cboAIExhaust3;
        private System.Windows.Forms.Label lblCategory;
        private System.Windows.Forms.CheckedListBox chlCategory;
        private System.Windows.Forms.Label lblVehicleType;
        private System.Windows.Forms.ComboBox cboVehicleType;
        private System.Windows.Forms.Label lblBoostType;
        private System.Windows.Forms.ComboBox cboBoostType;
        private System.Windows.Forms.Label lblFinishType;
        private System.Windows.Forms.ComboBox cboFinishType;
        private System.Windows.Forms.Label lblMaxSpeed;
        private System.Windows.Forms.TextBox txtMaxSpeed;
        private System.Windows.Forms.Label lblMaxBoostSpeed;
        private System.Windows.Forms.TextBox txtMaxBoostSpeed;
        private System.Windows.Forms.Label lblSpeedStat;
        private System.Windows.Forms.TextBox txtSpeedStat;
        private System.Windows.Forms.Label lblBoostStat;
        private System.Windows.Forms.TextBox txtBoostStat;
        private System.Windows.Forms.Label lblColor;
        private System.Windows.Forms.TextBox txtColor;
        private System.Windows.Forms.Label lblColorType;
        private System.Windows.Forms.ComboBox cboColorType;
    }
}
