using System;
using System.Diagnostics;

using LCU;
using LCU.Helper;
using LCU.Champs;

using lolTools;

namespace lolTools.tools {
    public class AutoBan : toolBase {

        //public static ToolMode mode = ToolMode.enabled;
        //public static DateTime lastCall = DateTime.Now;

        private static Champion[] banList;

        public AutoBan() : base() {
        }

        protected override void init() {
            base.init();

            if(Save.hasKey("BanList")) {
                Champion[] list = Save.readData<Champion[]>("BanList");
                this.setBans(list);
            }

            mode = ToolMode.disabled;

            addToStrip("Auto accept: off", toggle);

        }

        public Champion[] getCurrentList() {
            if(banList == null) {
                return null;
            }
            return (Champion[])banList.Clone();
        }

        private int getBan() {
            
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

        public void setBans(Champion[] newList) {
            Save.writeData("BanList",newList);
            if(newList.Length == 0) {
                Debug.WriteLine("Cant set ban list to a list with zero champs. chane mode to turn it off");
                return;
            } else {
                banList = newList;
            }
        }

        protected override void _run(ref bool didRun, toolStepArg arg) {
            if (arg.champSession == null || mode == ToolMode.disabled) {
                return;
            } else if((DateTime.Now-lastRun).TotalMilliseconds < 250) {
                return;
            }

            Session s = arg.champSession;
            LCU.Helper.Action a = s.getCurrent(true);

            if(a == null) {
                return;
            }

            if(a.type == "ban") {
                ActionPatch patch = new ActionPatch(a);
                patch.championId = getBan();
                clientLCU.patchAction(patch);

                didRun = true;
            }


        }
    }
}
