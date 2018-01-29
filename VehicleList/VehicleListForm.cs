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

        public void Open(BundleEntry entry)
        {
            Entry = entry;
            byte[] data = entry.Header;
            MemoryStream ms = new MemoryStream(data);
            BinaryReader2 mbr = new BinaryReader2(ms);
            mbr.BigEndian = entry.Console;
            currentList = mbr.ReadVehicleList();
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
