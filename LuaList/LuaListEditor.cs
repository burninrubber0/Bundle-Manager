using System.Windows.Forms;
using PluginAPI;

namespace LuaList
{
    public delegate void Notify();  // delegate
    public partial class LuaListEditor : Form, IEntryEditor
    {
        public event Notify EditEvent;

        private LuaList _luaList;
        public LuaList LuaList
        {
            get => _luaList;
            set
            {
                _luaList = value;
                UpdateComponent();
            }
        }

        public void UpdateComponent()
        {
           
            propertyGrid1.SelectedObject = LuaList;
        }

        public LuaListEditor()
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
