﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BundleManager
{
    public partial class InputDialog : Form
    {
        public delegate void OnAccepted(string value);
        public event OnAccepted Accepted;

        public InputDialog()
        {
            InitializeComponent();
        }

        private void btnAccept_Click(object sender, EventArgs e)
        {
            Accepted?.Invoke(txtInput.Text);
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        public static string ShowInput(IWin32Window window, string question)
        {
            string response = null;

            InputDialog dialog = new InputDialog();
            dialog.lblQuestion.Text = question;
            dialog.Accepted += value => response = value;
            dialog.ShowDialog(window);

            return response;
        }

        public static string ShowInput(string question)
        {
            string response = null;

            InputDialog dialog = new InputDialog();
            dialog.lblQuestion.Text = question;
            dialog.Accepted += value => response = value;
            dialog.ShowDialog();

            return response;
        }
    }
}
