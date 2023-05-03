using System;
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
                Console.WriteLine(value.muSize);
                Console.WriteLine(_trigger.muSize);
                Console.WriteLine(trigger.muSize);
                
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
    }
}
