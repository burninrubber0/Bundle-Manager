using System;
using System.Windows.Forms;

namespace BundleManager
{
    public partial class LoadingDialog : Form
    {
        public delegate void OnDone(bool cancelled, object value);
        public event OnDone Done;

        private delegate object GetObject();
        private delegate void SetObject(object obj);
        private object _value;
        public object Value
        {
            get
            {
                if (InvokeRequired)
                {
                    GetObject method = () =>
                    {
                        return _value;
                    };
                    return Invoke(method);
                }
                else
                {
                    return _value;
                }
            }
            set
            {
                if (InvokeRequired)
                {
                    SetObject method = (object obj) =>
                    {
                        _value = obj;
                    };
                    Invoke(method, value);
                }
                else
                {
                    _value = value;
                }
            }
        }
        
        private bool _isDone;
        public bool IsDone
        {
            get
            {
                return _isDone;
            }
            set
            {
                _isDone = value;
            }
        }

        public string Status
        {
            get
            {
                return lblStatus.Text;
            }
            set
            {
                lblStatus.Text = value;
            }
        }

        public LoadingDialog()
        {
            InitializeComponent();

            IsDone = false;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            /*if (Done != null)
            {
                Done(shouldCancel);
            }*/
            Close();
        }

        private void LoadingDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Done != null)
            {
                Done(!IsDone, Value);
            }
        }

        private void tmrDoneCheck_Tick(object sender, EventArgs e)
        {
            if (IsDone)
            {
                tmrDoneCheck.Enabled = false;
                Close();
            }
        }
    }
}
