using System;
using System.Configuration;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

using LCU;
using LCU.Helper;
using LCU.Champs;

using lolTools.tools;

namespace lolTools {
    class Program {
        static void Main(string[] args) {
            #region SETUP
            Console.WriteLine("<SETUP>");

            //trayFrom form = new trayFrom();

            #if DEBUG
            string path = @"G:\lel";
            #else
            throw new NotImplementedException();
            string path = ConfigurationManager.AppSettings.Get("LOL_FILE_PATH");
            #endif

            LCU.clientLCU.init(path);
            Champions.loadChamps();

            NotifyIcon icon = new NotifyIcon();

            icon.Icon = Properties.Resources.lolTools;
            icon.Text = "LOL tools";
            icon.Visible = true;

            icon.ContextMenuStrip = new ContextMenuStrip();
            icon.ContextMenuStrip.Items.Add("Auto accept: on", null, togelAutoAccept);
            icon.ContextMenuStrip.Items.Add("Auto ban: on", null, togelAutoBan);
            icon.ContextMenuStrip.Items.Add("Set ban order", null, setAutoBan);
            icon.ContextMenuStrip.Items.Add("Exit", null, exit);

            toolManeger.toolTickEvent += AutoAccept.run;
            toolManeger.toolTickEvent += AutoBan.run;


            //Thread appThread = new Thread(Application.Run);
            //appThread.Priority = ThreadPriority.BelowNormal;
            //appThread.Start();
            
            #endregion

            #region MAIN LOOP
            ThreadPool.QueueUserWorkItem(toolManeger.loopFunc);

            Thread.CurrentThread.Priority = ThreadPriority.BelowNormal;

            //toolManeger.loopFunc();

            Application.Run();

            #endregion
        }

        #region Menu strip function
        private static void togelAutoAccept(object sender, EventArgs e) {
            AutoAccept.mode = (AutoAccept.mode == ToolMode.disabled) ? ToolMode.enabled : ToolMode.disabled;
            if(sender.GetType() == typeof(ToolStripMenuItem)) {
                ((ToolStripMenuItem)sender).Text = "Auto accept: " + ((AutoAccept.mode == ToolMode.disabled) ? "off" : "on");
            }
        }

        private static void togelAutoBan(object sender, EventArgs e) {
            AutoBan.mode = (AutoBan.mode == ToolMode.disabled) ? ToolMode.enabled : ToolMode.disabled;
            if (sender.GetType() == typeof(ToolStripMenuItem)) {
                ((ToolStripMenuItem)sender).Text = "Auto ban: " + ((AutoBan.mode == ToolMode.disabled) ? "off" : "on");
            }
        }

        private static void setAutoBan(object sender, EventArgs e) {
            Champion[] newList = Champions.UIslectChamps();
            if(newList != null) {
                AutoBan.setBans(newList);
            }
            
        }

        private static void exit(object sender, EventArgs e) {
            toolManeger.stop();
            Application.Exit();
        }
        #endregion
    }
}
