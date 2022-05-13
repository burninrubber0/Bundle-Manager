using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PluginAPI;

namespace BaseHandlers
{

    public delegate void Notify();  // delegate
    public partial class TriggerDataEditor : Form, IEntryEditor
    {
        public event Notify EditEvent;

        private TriggerData _trigger;
        public TriggerData trigger
        {
            get => _trigger;
            set
            {
                _trigger = value;
                UpdateComponent();
            }
        }

        public void UpdateComponent()
        {

            propertyGrid1.SelectedObject = trigger;
        }

        public TriggerDataEditor()
        {
            InitializeComponent();
            UpdateComponent();
        }

        private void propertyGrid1_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            EditEvent?.Invoke();
        }

        private void propertyChanged() {
            EditEvent?.Invoke();
        }
    }
}
