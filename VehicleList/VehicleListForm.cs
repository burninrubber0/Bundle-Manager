using System;
using System.Collections;
using System.Globalization;
using System.Windows.Forms;
using BundleUtilities;
using PluginAPI;

namespace VehicleList
{
    public partial class VehicleListForm : Form, IEntryEditor
    {
        public delegate void OnEdit();
        public event OnEdit Edit;

        private VehicleListData _list;
        public VehicleListData List
        {
            get => _list;
            set
            {
                _list = value;
                UpdateDisplay();
            }
        }

        public VehicleListForm()
        {
            InitializeComponent();
        }

        private void UpdateDisplay()
        {
            lstVehicles.Items.Clear();

            if (List == null)
                return;

            for (int i = 0; i < List.Entries.Count; i++)
            {
                Vehicle vehicle = List.Entries[i];

                string[] value = {
                    vehicle.Index.ToString(),
                    vehicle.ID.Value,
                    vehicle.ParentID.Value,
                    vehicle.WheelType,
                    vehicle.CarName,
                    vehicle.CarBrand,
                    vehicle.DamageLimit.ToString(),
                    vehicle.Flags.ToString(),
                    vehicle.BoostLength.ToString(),
                    vehicle.VehicleRank.ToString(),
                    vehicle.BoostCapacity.ToString(),
                    vehicle.DisplayStrength.ToString(),
                    vehicle.AttribSysCollectionKey.ToString(),
                    vehicle.ExhaustName.Value,
                    vehicle.ExhaustID.ToString(),
                    vehicle.EngineID.ToString(),
                    vehicle.EngineName.Value,
                    vehicle.ClassUnlockStreamHash.ToString(),
                    vehicle.CarShutdownStreamID.ToString(),
                    vehicle.CarReleasedStreamID.ToString(),
                    vehicle.AIMusicHash.ToString(),
                    vehicle.AIExhaustIndex.ToString(),
                    vehicle.AIExhaustIndex2.ToString(),
                    vehicle.AIExhaustIndex3.ToString(),
                    vehicle.Category.ToString(),
                    vehicle.VehicleType.ToString(),
                    vehicle.BoostType.ToString(),
                    vehicle.FinishType.ToString(),
                    vehicle.MaxSpeedNoBoost.ToString(),
                    vehicle.MaxSpeedBoost.ToString(),
                    vehicle.DisplaySpeed.ToString(),
                    vehicle.DisplayBoost.ToString(),
                    vehicle.Color.ToString(),
                    vehicle.ColorType.ToString()
                };
                lstVehicles.Items.Add(new ListViewItem(value));
            }

            lstVehicles.ListViewItemSorter = new VehicleSorter(0);
            lstVehicles.Sort();
        }

        private void EditSelectedEntry()
        {
            if (lstVehicles.SelectedItems.Count > 1)
                return;
            if (List == null || lstVehicles.SelectedIndices.Count <= 0)
                return;

            if (!int.TryParse(lstVehicles.SelectedItems[0].Text, out int index))
                return;
            Vehicle vehicle = List.Entries[index];

            VehicleEditor editor = new VehicleEditor(List.Entries.Count - 1);
            editor.Vehicle = vehicle;
            editor.OnDone += Editor_OnDone;
            editor.ShowDialog(this);
        }

        private void AddItem()
        {
            if (List == null)
                return;
            Vehicle vehicle = new Vehicle();
            vehicle.Index = List.Entries.Count;
            vehicle.ID = new EncryptedString("");
            vehicle.ParentID = new EncryptedString("");
            vehicle.ExhaustName = new EncryptedString("");
            vehicle.EngineName = new EncryptedString("");

            VehicleEditor editor = new VehicleEditor(List.Entries.Count);
            editor.Vehicle = vehicle;
            editor.OnDone += Editor_OnDone1; ;
            editor.ShowDialog(this);
        }

        private void CopyItem()
        {
            if (List == null || lstVehicles.SelectedItems.Count > 1
                || lstVehicles.SelectedIndices.Count <= 0)
                return;

            if (!int.TryParse(lstVehicles.SelectedItems[0].Text, out int index))
                return;
            Vehicle vehicle = new Vehicle(List.Entries[index]);
            vehicle.Index = List.Entries.Count;

            VehicleEditor editor = new VehicleEditor(List.Entries.Count);
            editor.Vehicle = vehicle;
            editor.OnDone += Editor_OnDone1;
            editor.ShowDialog(this);
        }

        private void Editor_OnDone1(Vehicle vehicle)
        {
            // Insert if not at end, else add
            if (vehicle.Index != List.Entries.Count)
            {
                List.Entries.Insert(vehicle.Index, vehicle);

                for (int i = 0; i < List.Entries.Count; ++i)
                    List.Entries[i].Index = i;
            }
            else
            {
                List.Entries.Add(vehicle);
            }

            Edit?.Invoke();
            UpdateDisplay();
        }

        private void Editor_OnDone(Vehicle vehicle)
        {
            // If the index has changed, edit the list
            int oldIndex = int.Parse(lstVehicles.SelectedItems[0].Text); // Tried in EditSelectedEntry()
            if (oldIndex != vehicle.Index)
            {
                Vehicle old = List.Entries[oldIndex];
                List.Entries.RemoveAt(oldIndex);
                List.Entries.Insert(vehicle.Index, old);

                for (int i = 0; i < List.Entries.Count; ++i)
                    List.Entries[i].Index = i;
            }

            // Edit the vehicle
            List.Entries[vehicle.Index] = vehicle;
            Edit?.Invoke();
            UpdateDisplay();
        }

        private void lstVehicles_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            EditSelectedEntry();
        }
        
        private void lstVehicles_SelectedIndexChanged(object sender, EventArgs e)
        {
            stlStatusLabel.Text = lstVehicles.SelectedItems.Count + " Item(s) Selected";
            tsbCopyItem.Enabled = true;
        }

        private void lstVehicles_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            int column = e.Column;

            bool direction = false;

            if (lstVehicles.ListViewItemSorter is VehicleSorter sorter)
            {
                if (sorter.Column == column)
                {
                    sorter.Swap();
                    lstVehicles.Sort();
                    return;
                }
                direction = sorter.Direction;
            }

            VehicleSorter newSorter = new VehicleSorter(column)
            {
                Direction = !direction
            };
            lstVehicles.ListViewItemSorter = newSorter;
            lstVehicles.Sort();
        }

        private class VehicleSorter : IComparer
        {
            public readonly int Column;
            public bool Direction;

            public VehicleSorter(int column)
            {
                Column = column;
                Direction = false;
            }

            public int Compare(object x, object y)
            {
                ListViewItem itemX = (ListViewItem)x;
                ListViewItem itemY = (ListViewItem)y;

                if (Column > itemX.SubItems.Count || Column > itemY.SubItems.Count)
                {
                    if (Direction)
                        return -1;
                    return 1;
                }

                string iX = itemX.SubItems[Column].Text;
                string iY = itemY.SubItems[Column].Text;


                if (int.TryParse(iX, NumberStyles.HexNumber, CultureInfo.CurrentCulture, out int iXint))
                {
                    if (int.TryParse(iY, NumberStyles.HexNumber, CultureInfo.CurrentCulture, out int iYint))
                    {
                        int val2 = iXint.CompareTo(iYint);
                        if (this.Direction)
                            return val2 * -1;
                        return val2;
                    }
                }

                int val = string.CompareOrdinal(iX, iY);
                if (Direction)
                    return val * -1;
                return val;
            }

            public void Swap()
            {
                Direction = !Direction;
            }
        }

        private void addItemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddItem();
        }

        private void tsbAddItem_Click(object sender, EventArgs e)
        {
            AddItem();
        }

        private void copyItemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CopyItem();
        }

        private void tsbCopyItem_Click(object sender, EventArgs e)
        {
            CopyItem();
        }
    }
}
