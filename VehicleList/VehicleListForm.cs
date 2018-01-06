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
using Util = BundleFormat.Util;

namespace VehicleList
{
    public partial class VehicleListForm : Form
    {
        public delegate void OnEdit();
        public event OnEdit Edit;

        private BundleEntry Entry;
        private VehicleListData currentList;

        public VehicleListForm()
        {
            InitializeComponent();
        }

        private void UpdateDisplay()
        {
            lstVehicles.Items.Clear();

            if (currentList == null)
                return;

            for (int i = 0; i < currentList.Entries.Count; i++)
            {
                Vehicle vehicle = currentList.Entries[i];

                string[] value = {
                    vehicle.Index.ToString("D3"),
                    //vehicle.ID.Encrypted.ToString("X16"),
                    vehicle.ID.Value,
                    vehicle.CarBrand,
                    vehicle.CarName,
                    vehicle.WheelType,
                    vehicle.Category.ToString(),
                    //Convert.ToString(vehicle.Category, 2).PadLeft(6, '0'),
                    Convert.ToString(vehicle.Flags, 2).PadLeft(18, '0'),
                    vehicle.BoostType.ToString(),
                    vehicle.MaxSpeedNoBoost.ToString("X2"),
                    vehicle.MaxSpeedBoost.ToString("D3"),
                    vehicle.NewUnknown.ToString(),
                    vehicle.FinishType.ToString(),
                    vehicle.Color.ToString(),
                    vehicle.DisplaySpeed.ToString(),
                    vehicle.DisplayBoost.ToString(),
                    vehicle.DisplayStrength.ToString(),
                    vehicle.ExhauseID.Value,
                    Util.GetEngineFilenameByID(vehicle.ExhauseID.Value),
                    vehicle.GroupID.ToString(),
                    vehicle.EngineID.Value,
                    Util.GetEngineFilenameByID(vehicle.EngineID.Value),
                    vehicle.GroupIDAlt.ToString()
                    //vehicle.Unknown15.ToString(),
                    //vehicle.Unknown20.ToString(),
                    //vehicle.Unknown27.ToString()
                };
                lstVehicles.Items.Add(new ListViewItem(value));
            }

            lstVehicles.ListViewItemSorter = new VehicleSorter(0);
            lstVehicles.Sort();
        }

        public void Open(BundleEntry entry, bool console = false)
        {
            Entry = entry;
            byte[] data = entry.Header;
            MemoryStream ms = new MemoryStream(data);
            BinaryReader mbr = new BinaryReader(ms);
            currentList = mbr.ReadVehicleList(console);
            mbr.Close();

            UpdateDisplay();
        }

        public BundleEntry Write(bool console)
        {
            MemoryStream ms = new MemoryStream();
            BinaryWriter mbw = new BinaryWriter(ms);
            mbw.WriteVehicleList(currentList, console);
            byte[] data = ms.ToArray();
            mbw.Close();

            Entry.Header = data;

            return Entry;
        }

        private void EditSelectedEntry()
        {
            if (lstVehicles.SelectedItems.Count > 1)
                return;
            if (currentList == null || lstVehicles.SelectedIndices.Count <= 0)
                return;

            int index;
            if (!int.TryParse(lstVehicles.SelectedItems[0].Text, out index))
                return;
            Vehicle vehicle = currentList.Entries[index];

            VehicleEditor editor = new VehicleEditor();
            editor.Vehicle = vehicle;
            editor.OnDone += Editor_OnDone;
            editor.ShowDialog(this);
        }

        private void AddItem()
        {
            if (currentList == null)
                return;
            Vehicle vehicle = new Vehicle();
            vehicle.Index = currentList.Entries.Count;
            vehicle.ID = new EncryptedString("");

            VehicleEditor editor = new VehicleEditor();
            editor.Vehicle = vehicle;
            editor.OnDone += Editor_OnDone1; ;
            editor.ShowDialog(this);
        }

        private void Editor_OnDone1(Vehicle vehicle)
        {
            currentList.Entries.Add(vehicle);
            Entry.Dirty = true;
            Edit?.Invoke();
            UpdateDisplay();
        }

        private void Editor_OnDone(Vehicle vehicle)
        {
            currentList.Entries[vehicle.Index] = vehicle;
            Entry.Dirty = true;
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

            if (lstVehicles.ListViewItemSorter is VehicleSorter)
            {
                VehicleSorter sorter = (VehicleSorter)lstVehicles.ListViewItemSorter;
                if (sorter.column == column)
                {
                    sorter.swap();
                    lstVehicles.Sort();
                    return;
                }
            }

            lstVehicles.ListViewItemSorter = new VehicleSorter(column);
            lstVehicles.Sort();
        }

        private class VehicleSorter : IComparer
        {
            public int column;
            private bool direction;

            public VehicleSorter(int column)
            {
                this.column = column;
                this.direction = false;
            }

            public int Compare(object x, object y)
            {
                ListViewItem itemX = (ListViewItem)x;
                ListViewItem itemY = (ListViewItem)y;

                if (column > itemX.SubItems.Count || column > itemY.SubItems.Count)
                {
                    if (this.direction)
                        return -1;
                    return 1;
                }

                string iX = itemX.SubItems[column].Text;
                string iY = itemY.SubItems[column].Text;

                int iXint;
                int iYint;

                if (int.TryParse(iX, NumberStyles.HexNumber, CultureInfo.CurrentCulture, out iXint))
                {
                    if (int.TryParse(iY, NumberStyles.HexNumber, CultureInfo.CurrentCulture, out iYint))
                    {
                        int val2 = iXint.CompareTo(iYint);
                        if (this.direction)
                            return val2 * -1;
                        return val2;
                    }
                }

                int val = String.CompareOrdinal(iX, iY);
                if (this.direction)
                    return val * -1;
                return val;
            }

            public void swap()
            {
                this.direction = !this.direction;
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
