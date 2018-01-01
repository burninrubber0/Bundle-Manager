using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VehicleList
{
    public partial class VehicleEditor : Form
    {
        public delegate void Done(Vehicle vehicle);
        public event Done OnDone;
        private Vehicle _vehicle;
        public Vehicle Vehicle
        {
            get
            {
                return _vehicle;
            }
            set
            {
                _vehicle = value;
                UpdateDisplay();
            }
        }

        public VehicleEditor()
        {
            InitializeComponent();
        }

        private void UpdateDisplay()
        {
            Text = "Edit Vehicle: " + Vehicle.Index;

            //txtID.Text = Vehicle.ID.Value.ToString("X16");
            txtID.Text = Vehicle.ID.Value;//Encoding.ASCII.GetString(Vehicle.ID.Decrypted);
            txtBrand.Text = Vehicle.CarBrand;
            txtName.Text = Vehicle.CarName;
            txtWheels.Text = Vehicle.WheelType;
            txtCategory.Text = Vehicle.Category.ToString();
            txtFlags.Text = Convert.ToString(Vehicle.Flags, 2).PadLeft(18, '0');//Vehicle.Flags.ToString();
            txtUnknown26.Text = Vehicle.MaxSpeedNoBoost.ToString("X2");
            txtSortOrder.Text = Vehicle.MaxSpeedBoost.ToString("D3");
            txtGroupID1.Text = Vehicle.GroupID.ToString();
            txtGroupID2.Text = Vehicle.GroupIDAlt.ToString();

            int boostType = (int) Vehicle.BoostType;
            if (boostType == 19)
                boostType = 5;
            cboBoost.SelectedIndex = boostType;

            cboLockByte.SelectedIndex = (int)Vehicle.FinishType;

            txtEngineID1.Text = Vehicle.ExhauseID.Value;
            txtEngineID2.Text = Vehicle.EngineID.Value;

            txtDisplaySpeed.Text = Vehicle.DisplaySpeed.ToString();
            txtDisplayBoost.Text = Vehicle.DisplayBoost.ToString();
            txtDisplayStrength.Text = Vehicle.DisplayStrength.ToString();
        }

        private Vehicle GetModifiedVehicle()
        {
            Vehicle result = new Vehicle(Vehicle);

            /*long id;
            if (long.TryParse(txtID.Text, NumberStyles.HexNumber, NumberFormatInfo.CurrentInfo, out id))
                result.ID.Value = id;
            else
                return null;*/

            if (txtID.Text.Trim().Length == 0)
                return null;
            
            try {
                EncryptedString ID = new EncryptedString(txtID.Text);//Encoding.ASCII.GetBytes(txtID.Text), false);
                result.ID = ID;
            } catch (ArgumentException e)
            {
                MessageBox.Show(this, e.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return null;
            }

            result.CarBrand = txtBrand.Text;
            result.CarName = txtName.Text;
            result.WheelType = txtWheels.Text;

            if (result.CarBrand.Trim().Length == 0)
                return null;
            if (result.CarName.Trim().Length == 0)
                return null;
            if (result.WheelType.Trim().Length == 0)
                return null;

            int cat;
            if (int.TryParse(txtCategory.Text, out cat))
                result.Category = cat;
            else
                return null;

            /*int flags;
            if (int.TryParse(txtFlags.Text, out flags))
                result.Flags = flags;
            else
                return null;*/

            try
            {
                result.Flags = Convert.ToInt32(txtFlags.Text, 2);
            }
            catch
            {
                return null;
            }

            byte u26;
            if (byte.TryParse(txtUnknown26.Text, NumberStyles.HexNumber, NumberFormatInfo.CurrentInfo, out u26))
                result.MaxSpeedNoBoost = u26;
            else
                return null;

            byte sortOrder;
            if (byte.TryParse(txtSortOrder.Text, out sortOrder))
                result.MaxSpeedBoost = sortOrder;
            else
                return null;

            long group1;
            if (long.TryParse(txtGroupID1.Text, out group1))
                result.GroupID = group1;
            else
                return null;

            long group2;
            if (long.TryParse(txtGroupID2.Text, out group2))
                result.GroupIDAlt = group2;
            else
                return null;

            int selectedBoostIndex = cboBoost.SelectedIndex;
            if (selectedBoostIndex == 5)
                selectedBoostIndex = 19;

            BoostType boostType = (BoostType)selectedBoostIndex;
            result.BoostType = boostType;

            result.FinishType = (FinishType)cboLockByte.SelectedIndex;

            if (txtEngineID1.Text.Trim().Length == 0)
                return null;

            try
            {
                EncryptedString OtherID1 = new EncryptedString(txtEngineID1.Text);
                result.ExhauseID = OtherID1;
            }
            catch (ArgumentException e)
            {
                MessageBox.Show(this, e.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return null;
            }

            if (txtEngineID2.Text.Trim().Length == 0)
                return null;

            try
            {
                EncryptedString OtherID2 = new EncryptedString(txtEngineID2.Text);
                result.EngineID = OtherID2;
            }
            catch (ArgumentException e)
            {
                MessageBox.Show(this, e.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return null;
            }

            byte displaySpeed;
            if (byte.TryParse(txtDisplaySpeed.Text, out displaySpeed))
                result.DisplaySpeed = displaySpeed;
            else
                return null;

            byte displayBoost;
            if (byte.TryParse(txtDisplayBoost.Text, out displayBoost))
                result.DisplayBoost = displayBoost;
            else
                return null;

            byte displayStrength;
            if (byte.TryParse(txtDisplayStrength.Text, out displayStrength))
                result.DisplayStrength = displayStrength;
            else
                return null;

            return result;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            Vehicle retVehicle = GetModifiedVehicle();
            if (retVehicle == null)
            {
                MessageBox.Show(this, "Some values are invalid!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (OnDone != null)
                OnDone(retVehicle);

            Close();
        }
    }
}
