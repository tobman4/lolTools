using System;
using System.Diagnostics;

using LCU;

namespace lolTools.tools {
    public static class AutoAccept {

        public static ToolMode mode = ToolMode.enabled;
        public static DateTime lastCall = DateTime.Now;


        public static void run(object sender,toolStepArg data) {

            if (!data.clientAPI ||
                (DateTime.Now - lastCall).TotalSeconds < 5 ||
                mode == ToolMode.disabled) {
                return;
            }

            lastCall = DateTime.Now;

            //gameFlowPhase phase = clientLCU.GetGamePhase();
            if(data.phase == gameFlowPhase.ReadyCheck) {
                clientLCU.AcceptMatch();
                if(mode == ToolMode.runOnce) {
                    mode = ToolMode.disabled;
                }
            }
        }
    }
}