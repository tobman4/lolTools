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

        public static NotifyIcon icon;

        private static AutoBan autoBan;// <---- TODO: remove

        static void Main(string[] args) {
            
            #if DEBUG
            string path = @"G:\lel";
            #else
            throw new NotImplementedException();
            string path = ConfigurationManager.AppSettings.Get("LOL_FILE_PATH");
            #endif

            #region INIT
            Save.init();
            clientLCU.init(path);
            Champions.loadChamps();// <--- make init

            #endregion

            #region ADD TOOLS
            new AutoAccept();
            autoBan = new AutoBan();
            new AutoRole();
            #endregion

            #region ICON/UI
            icon = new NotifyIcon();

            icon.Icon = Properties.Resources.lolTools;
            icon.Text = "LOL tools";
            icon.Visible = true;

            icon.ContextMenuStrip = new ContextMenuStrip();
            toolManeger.strip = icon.ContextMenuStrip;
            #endregion

            /*
            icon.BalloonTipText = "Change sums?";
            icon.BalloonTipTitle = "Sums watcher";
            icon.BalloonTipIcon = ToolTipIcon.Warning;
            icon.ShowBalloonTip(500);
            */

            toolManeger.init();

            icon.ContextMenuStrip.Items.Add("Set ban order", null, setAutoBan);
            icon.ContextMenuStrip.Items.Add("Exit", null, exit);

            ThreadPool.QueueUserWorkItem(toolManeger.loopFunc);
            Application.Run();

        }

        #region Menu strip function
        private static void togelAutoAccept(object sender, EventArgs e) {
            /*
            AutoAccept.mode = (AutoAccept.mode == ToolMode.disabled) ? ToolMode.enabled : ToolMode.disabled;
            if(sender.GetType() == typeof(ToolStripMenuItem)) {
                ((ToolStripMenuItem)sender).Text = "Auto accept: " + ((AutoAccept.mode == ToolMode.disabled) ? "off" : "on");
            }
            */
        }

        private static void togelAutoBan(object sender, EventArgs e) {
            autoBan.mode = (autoBan.mode == ToolMode.disabled) ? ToolMode.enabled : ToolMode.disabled;
            if (sender.GetType() == typeof(ToolStripMenuItem)) {
                ((ToolStripMenuItem)sender).Text = "Auto ban: " + ((autoBan.mode == ToolMode.disabled) ? "off" : "on");
            }
        }

        private static void setAutoBan(object sender, EventArgs e) {
            Champion[] newList = Champions.UIslectChamps(-99, autoBan.getCurrentList());
            if(newList != null) {
                autoBan.setBans(newList);
            }
            
        }

        private static void exit(object sender, EventArgs e) {
            toolManeger.stop();
            Application.Exit();
        }
        #endregion
    }
}
