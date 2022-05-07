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
                UpdateDisplay();
            }
        }

        private void UpdateDisplay()
        {
          
        }

        public LuaListEditor()
        {
            InitializeComponent();
        }
    }
}
