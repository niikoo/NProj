using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OSERV_BASE.Classes;

namespace OSERV_BASE
{
    public partial class EndDebug : Form
    {
        public EndDebug()
        {
            InitializeComponent();
        }

        private void endDebugBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void EndDebug_FormClosing(object sender, FormClosingEventArgs e)
        {
            NamedPipeCommunication.sendMessageToServer("debug::disable");
        }

        private void startGLOWscriptBtn_Click(object sender, EventArgs e)
        {
            NamedPipeCommunication.sendMessageToServer("glow::" + startGLOWscript.Text);
        }

        private void runEventBtn_Click(object sender, EventArgs e)
        {
            NamedPipeCommunication.sendMessageToServer("event::" + runEvent.Text);
        }

        private void ics_setup_Click(object sender, EventArgs e)
        {
            NamedPipeCommunication.sendMessageToServer("ics::setup");
        }

        private void ddwrt_radio_on_Click(object sender, EventArgs e)
        {
            NamedPipeCommunication.sendMessageToServer("ddwrt::radio-on");
        }

        private void ddwrt_radio_off_Click(object sender, EventArgs e)
        {
            NamedPipeCommunication.sendMessageToServer("ddwrt::radio-off");
        }
    }
}
