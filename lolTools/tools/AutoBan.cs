using System;
using System.Diagnostics;

using LCU;
using LCU.Helper;
using LCU.Champs;


namespace lolTools.tools {
    public static class AutoBan {

        public static ToolMode mode = ToolMode.enabled;
        public static DateTime lastCall = DateTime.Now;

        private static Champion[] banList;

        private static int getBan() {
            
            if(banList == null) {
                return 63;
            }
            
            string canBan = string.Join(",",clientLCU.getBannableChamps());
            canBan += ","; // <---- fat brain:)

            foreach(Champion c in banList) {
                if(canBan.Contains($"{c.iKey},")) {
                    return c.iKey;
                }
            }
            return 63; // <--- Brand
        }

        public static void setBans(Champion[] newList) {
            if(newList.Length == 0) {
                Debug.WriteLine("Cant set ban list to a list with zero champs. chane mode to turn it off");
                return;
            } else {
                banList = newList;
            }
        }

        public static void run(object sender, toolStepArg data) {
            if (data.champSession == null || mode == ToolMode.disabled) {
                return;
            } else if((DateTime.Now-lastCall).TotalMilliseconds < 250) {
                return;
            }

            lastCall = DateTime.Now;

            Session s = data.champSession;
            LCU.Helper.Action a = s.getCurrent(true);

            if(a == null) {
                return;
            }

            if(a.type == "ban") {
                ActionPatch patch = new ActionPatch(a);
                patch.championId = getBan();
                clientLCU.patchAction(patch);

                if(mode == ToolMode.runOnce) {
                    mode = ToolMode.disabled;
                }

            }

        }
    }
}
