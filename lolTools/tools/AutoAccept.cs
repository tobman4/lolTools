using System;
using System.Diagnostics;

using LCU;

namespace lolTools.tools {
    public class AutoAccept : toolBase {

        public AutoAccept() : base() {
        }

        //public ToolMode mode = ToolMode.enabled;
        //public DateTime lastCall = DateTime.Now;
        
        /*
        public void toggle(object sender, EventArgs e) {
            mode = (mode == ToolMode.disabled ? ToolMode.enabled : ToolMode.disabled);
            string status = (mode == ToolMode.disabled ? "Off" : "On");
            flipStatus(sender);
        }
        */
        protected override void init() {
            base.init();

            mode = ToolMode.disabled;

            addToStrip("Auto accept: off", toggle);
        }

        protected override void _run(ref bool didRun, toolStepArg arg) {

            if (!arg.clientAPI ||
                (DateTime.Now - lastRun).TotalSeconds < 5) {
                return;
            }

            //gameFlowPhase phase = clientLCU.GetGamePhase();
            if(arg.phase == gameFlowPhase.ReadyCheck) {
                clientLCU.AcceptMatch();
                didRun = true;
            }
        }
    }
}