using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows.Forms;

using lolTools;

namespace lolTools.tools {
    public abstract class toolBase {

        public ToolMode mode;
        public DateTime lastRun;

        public toolBase() {
            toolManeger.toolTickEvent += run;
            toolManeger.controll += command;
        }

        public void toggle(object sender, EventArgs e) {
            mode = (mode == ToolMode.disabled ? ToolMode.enabled : ToolMode.disabled);
            string status = (mode == ToolMode.disabled ? "Off" : "On");
            flipStatus(sender);
        }

        protected virtual void addToStrip(string txt, EventHandler cb) {
            toolManeger.strip.Items.Add(txt,null,cb);
        }

        protected virtual void init() {
            Debug.WriteLine($"{this.GetType().Name} initialized");
        }

        public void flipStatus(object obj) {
            if(obj.GetType() == typeof(ToolStripMenuItem)) {
                ToolStripMenuItem item = (ToolStripMenuItem)obj;

                string[] split = item.Text.Split(": ");

                item.Text = $"{split[0]}: {((mode == ToolMode.disabled) ? "off" : "on")}";

            }
        }

        public bool includesMe(string l) {
            return l.Contains(this.GetType().Name) || l.Contains("ALL");
        }

        public virtual void command(object sendr, ActionArg arg) {
            if(includesMe(arg.target)) {
                switch(arg.action) {
                    case ControllAction.SetMode:
                        mode = arg.mode;
                        break;
                    case ControllAction.INIT:
                        init();
                        break;
                } 
            }
        }

        public void run(object sender, toolStepArg arg) {
            if(mode == ToolMode.disabled) {
                return;
            }

            bool res = false;

            try {
                _run(ref res,arg);
            } catch(Exception e) {
                Debug.WriteLine(e);
            }

            if(res) {
                mode = (res && mode == ToolMode.runOnce) ? ToolMode.disabled : mode;
                lastRun = DateTime.Now;
            }

        }
        protected abstract void _run(ref bool didRun ,toolStepArg arg);

        /*
        public virtual void run(object sender, toolStepArg arg) {
            // run after tools run
            // if tool run dont complete actions dont to base.run()

            if(mode == ToolMode.runOnce && arg.didRun) {
                mode = ToolMode.disabled;
            }
            lastRun = DateTime.Now;
        }
        */
    }
}
