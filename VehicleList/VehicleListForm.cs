using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BundleFormat;
using BundleUtilities;
using PluginAPI;
using Util = BundleFormat.Util;

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

            int index;
            if (!int.TryParse(lstVehicles.SelectedItems[0].Text, out index))
                return;
            Vehicle vehicle = List.Entries[index];

            VehicleEditor editor = new VehicleEditor();
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

            VehicleEditor editor = new VehicleEditor();
            editor.Vehicle = vehicle;
            editor.OnDone += Editor_OnDone1; ;
            editor.ShowDialog(this);
        }

        private void Editor_OnDone1(Vehicle vehicle)
        {
            List.Entries.Add(vehicle);
            Edit?.Invoke();
            UpdateDisplay();
        }

        private void Editor_OnDone(Vehicle vehicle)
        {
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
                this.Column = column;
                this.Direction = false;
            }

            public int Compare(object x, object y)
            {
                ListViewItem itemX = (ListViewItem)x;
                ListViewItem itemY = (ListViewItem)y;

                if (Column > itemX.SubItems.Count || Column > itemY.SubItems.Count)
                {
                    if (this.Direction)
                        return -1;
                    return 1;
                }

                string iX = itemX.SubItems[Column].Text;
                string iY = itemY.SubItems[Column].Text;

                int iXint;
                int iYint;

                if (int.TryParse(iX, NumberStyles.HexNumber, CultureInfo.CurrentCulture, out iXint))
                {
                    if (int.TryParse(iY, NumberStyles.HexNumber, CultureInfo.CurrentCulture, out iYint))
                    {
                        int val2 = iXint.CompareTo(iYint);
                        if (this.Direction)
                            return val2 * -1;
                        return val2;
                    }
                }

                int val = String.CompareOrdinal(iX, iY);
                if (this.Direction)
                    return val * -1;
                return val;
            }

            public void Swap()
            {
                this.Direction = !this.Direction;
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
    }
}
