using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using DebugViewerLib.Util;

namespace DebugViewerLib
{
    /// <summary>
    /// Displays debug info for an object
    /// </summary>
    public class DebugViewer : Control
    {
        private object _selectedObject;

        /// <summary>
        /// The object for this DebugViewer to display.
        /// </summary>
        public object SelectedObject
        {
            get
            {
                return _selectedObject;
            }
            set
            {
                _selectedObject = value;
                UpdateDisplay();
            }
        }

        private Dictionary<string, object> Fields;
        private ListView list;

        /// <summary>
        /// Initialize a new instance of the DebugViewer class with default settings.
        /// </summary>
        public DebugViewer()
        {
            Fields = new Dictionary<string, object>();
            //DoubleBuffered = true;

            list = new ListView();
            list.View = View.Details;
            list.Dock = DockStyle.Fill;
            list.FullRowSelect = true;
            list.GridLines = true;
            list.MultiSelect = false;
            list.DoubleClick += List_DoubleClick;

            list.Columns.Add("Index", 60);
            list.Columns.Add("Name", 120);
            list.Columns.Add("Value", 420);

            list.ColumnWidthChanging += ListOnColumnWidthChanging;

            Controls.Add(list);
        }

        private void ListOnColumnWidthChanging(object sender, ColumnWidthChangingEventArgs e)
        {
            // Disabled as it's unnessecary
            /*ListView listView = sender as ListView;
            if (listView == null)
                return; // Should never happen
            e.Cancel = true;
            e.NewWidth = listView.Columns[e.ColumnIndex].Width;*/
        }

        private void List_DoubleClick(object sender, EventArgs e)
        {
            if (list.SelectedIndices.Count == 0)
                return;
            int index = list.SelectedIndices[0];
            object obj = Fields.Values.ToList()[index];

            ProcessObject(obj);
        }

        private void View_DoubleClick(object sender, EventArgs e)
        {
            SuperListView listView = (SuperListView)sender;
            if (listView.SelectedIndices.Count == 0)
                return;
            int index = listView.SelectedIndices[0];

            IList list = listView.List;
            object obj;
            if (list is Array && ((Array) list).Rank > 1)
            {
                SpecialListViewItem item = listView.SelectedItems[0] as SpecialListViewItem;
                if (item == null)
                    return;

                int[] indices = item.SpecialIndex;
                obj = ((Array) list).GetValue(indices);
            }
            else
            {
                if (index > list.Count)
                    return;
                obj = list[index];
            }
            ProcessObject(obj);
        }

        private string GetPrimitiveString(object obj)
        {
            string value;
            if (obj is byte)
            {
                value = "0x" + ((byte)obj).ToString("X2");
            }
            else if (obj is sbyte)
            {
                value = ((sbyte)obj).ToString();
            }
            else if (obj is char)
            {
                value = obj.ToString();
            }
            else if (obj is short)
            {
                value = ((short)obj).ToString();
            }
            else if (obj is ushort)
            {
                value = "0x" + ((ushort)obj).ToString("X4");
            }
            else if (obj is int)
            {
                value = ((int)obj).ToString();
            }
            else if (obj is uint)
            {
                value = "0x" + ((uint)obj).ToString("X8");
            }
            else if (obj is long)
            {
                value = ((long)obj).ToString();
            }
            else if (obj is ulong)
            {
                value = "0x" + ((ulong)obj).ToString("X16");
            }
            else if (obj is float)
            {
                value = ((float)obj).ToString("0.00");
            }
            else if (obj is double)
            {
                value = ((double)obj).ToString("0.0000");
            }
            else
            {
                value = obj.ToString();
            }
            return value;
        }

        private List<ListViewItem> GetMultiItems(Array values, int[] indices, int dimensionNum)
        {
            // http://csharphelper.com/blog/2016/12/loop-over-an-array-of-unknown-dimension-in-c/
            // by Rod Stephens

            List<ListViewItem> result = new List<ListViewItem>();
            int maxIndex = values.GetUpperBound(dimensionNum);
            for (int i = 0; i <= maxIndex; i++)
            {
                indices[dimensionNum] = i;

                if (dimensionNum == values.Rank - 2)
                {
                    result.AddRange(GetMultiInner(values, indices));
                }
                else
                {
                    result.AddRange(GetMultiItems(values, indices, dimensionNum + 1));
                }
            }

            return result;
        }

        private List<ListViewItem> GetMultiInner(Array values, int[] indices)
        {
            // http://csharphelper.com/blog/2016/12/loop-over-an-array-of-unknown-dimension-in-c/
            // by Rod Stephens

            List<ListViewItem> result = new List<ListViewItem>();

            int dimensionNum = values.Rank - 1;
            int maxIndex = values.GetUpperBound(dimensionNum);
            for (int i = 0; i <= maxIndex; i++)
            {
                indices[dimensionNum] = i;

                string[] item =
                {
                    "[" + indices.AsArrayIndexString() + "]",
                    values.GetValue(indices).ToString()
                };
                result.Add(new SpecialListViewItem(indices, item));
            }

            return result;
        }

        private void ProcessObject(object obj)
        {
            if (obj == null)
                return;
            if (obj.GetType().IsPrimitive || obj is string)
            {
                string value = GetPrimitiveString(obj);

                Form editForm = new Form
                {
                    FormBorderStyle = FormBorderStyle.SizableToolWindow,
                    MaximizeBox = false,
                    MinimizeBox = false,
                    Size = new Size(287 + 16, 44 + 39), // 16 is the x border size and 39 is the y border size
                    StartPosition = FormStartPosition.CenterParent,
                    ShowInTaskbar = false,
                    Text = "View Value"
                };

                TextBox txtBox = new TextBox
                {
                    Location = new Point(12, 12),
                    Size = new Size(176, 20),
                    ReadOnly = true,
                    Text = value
                };

                SuperButton btnOk = new SuperButton(editForm, txtBox)
                {
                    Location = new Point(176 + 24, 11),
                    Size = new Size(75, 22),
                    Text = "OK"
                };
                btnOk.Click += BtnOk_Click;

                editForm.Controls.Add(txtBox);
                editForm.Controls.Add(btnOk);

                editForm.AcceptButton = btnOk;

                editForm.ShowDialog(this);
                
            }
            else if (obj is IList)
            {
                var array = obj as Array;
                if (array != null && array.Rank > 1)
                {
                    Array arr = array;

                    Form listForm = new Form
                    {
                        Text = "View List",
                        Size = new Size(640, 480),
                        StartPosition = FormStartPosition.CenterParent,
                        FormBorderStyle = FormBorderStyle.SizableToolWindow,
                        MaximizeBox = false,
                        MinimizeBox = false,
                        ShowInTaskbar = false
                    };

                    SuperListView view = new SuperListView(arr)
                    {
                        View = View.Details,
                        Dock = DockStyle.Fill,
                        FullRowSelect = true,
                        GridLines = true,
                        MultiSelect = false,
                    };

                    view.DoubleClick += View_DoubleClick;

                    view.Columns.Add("Index", 60);
                    view.Columns.Add("Name", 540);

                    view.ColumnWidthChanging += ListOnColumnWidthChanging;

                    int[] indices = new int[arr.Rank];
                    List<ListViewItem> items = GetMultiItems(arr, indices, 0);
                    view.Items.AddRange(items.ToArray());

                    listForm.Controls.Add(view);

                    listForm.ShowDialog(this);
                }
                else
                {
                    IList listObj = (IList) obj;

                    Form listForm = new Form
                    {
                        Text = "View List",
                        Size = new Size(640, 480),
                        StartPosition = FormStartPosition.CenterParent,
                        FormBorderStyle = FormBorderStyle.SizableToolWindow,
                        MaximizeBox = false,
                        MinimizeBox = false,
                        ShowInTaskbar = false
                    };

                    SuperListView view = new SuperListView(listObj)
                    {
                        View = View.Details,
                        Dock = DockStyle.Fill,
                        FullRowSelect = true,
                        GridLines = true,
                        MultiSelect = false
                    };

                    view.DoubleClick += View_DoubleClick;

                    view.Columns.Add("Index", 60);
                    view.Columns.Add("Name", 540);

                    view.ColumnWidthChanging += ListOnColumnWidthChanging;

                    for (int i = 0; i < listObj.Count; i++)
                    {
                        string[] listItem =
                        {
                            i.ToString(),
                            listObj[i].ToString()
                        };
                        view.Items.Add(new ListViewItem(listItem));
                    }

                    listForm.Controls.Add(view);

                    listForm.ShowDialog(this);
                }
            }
            else
            {
                Form dlg = BuildDebugDlg(obj);
                dlg.ShowDialog(this);
            }
        }

        public static Form BuildDebugDlg(object obj)
        {
            Form form = new Form
            {
                Text = "View Object",
                Size = new Size(640, 480),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.SizableToolWindow,
                MaximizeBox = false,
                MinimizeBox = false,
                ShowInTaskbar = false
            };

            DebugViewer view = new DebugViewer
            {
                Dock = DockStyle.Fill,
                SelectedObject = obj
            };

            form.Controls.Add(view);

            return form;
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            SuperButton button = (SuperButton)sender;

            button.Form.Close();
        }

        private void UpdateDisplay()
        {
            Fields.Clear();
            list.Items.Clear();
            object obj = _selectedObject;
            if (obj == null)
                return;
            Type t = obj.GetType();
            FieldInfo[] fields = t.GetFields();
            foreach (FieldInfo field in fields)
            {
                Fields.Add(field.Name, field.GetValue(obj));
            }

            PropertyInfo[] properties = t.GetProperties();
            foreach (PropertyInfo property in properties)
            {
                try
                {
                    Fields.Add(property.Name, property.GetValue(obj));
                }
                catch (TargetParameterCountException e)
                {
                    Debug.WriteLine(e.Message);
                    Debug.WriteLine(e.StackTrace);
                }
            }

            List<string> keys = Fields.Keys.ToList();
            List<object> values = Fields.Values.ToList();
            for (int i = 0; i < Fields.Count; i++)
            {
                string value;
                if (values[i] == null)
                    value = "<null>";
                else if (values[i].GetType().IsPrimitive || values[i] is string)
                    value = GetPrimitiveString(values[i]);
                else if (values[i] is IList)
                {
                    if (values[i] is Array && ((Array) values[i]).Rank > 1)
                    {
                        Array arr = (Array) values[i];
                        int[] indices = new int[arr.Rank];

                        for (int k = 0; k < arr.Rank; k++)
                        {
                            indices[k] = arr.GetLength(k);
                        }

                        value = "Array[" + indices.AsArrayIndexString() + "]";
                    }
                    else
                    {
                        IList list = (IList) values[i];
                        value = "List[" + list.Count.ToString() + "]";
                    }
                }
                else
                    value = values[i].ToString();
                string[] listItem = 
                {
                    i.ToString(),
                    keys[i],
                    value
                };
                list.Items.Add(new ListViewItem(listItem));
            }
        }

        private class SuperListView : ListView
        {
            public IList List
            {
                get;
                private set;
            }

            public SuperListView(IList list) : base()
            {
                List = list;
            }
        }

        private class SuperButton : Button
        {
            public Form Form
            {
                get;
                private set;
            }

            public TextBox TextBox
            {
                get;
                private set;
            }

            public SuperButton(Form form, TextBox textBox) : base()
            {
                Form = form;
                TextBox = textBox;
            }
        }

        private class SpecialListViewItem : ListViewItem
        {
            public int[] SpecialIndex
            {
                get;
                private set;
            }

            public SpecialListViewItem(int[] specialIndex, string[] subItems) : base(subItems)
            {
                SpecialIndex = specialIndex;
            }
        }
    }
}
