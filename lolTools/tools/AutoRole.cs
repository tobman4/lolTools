using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LCU;
using LCU.Helper;

namespace lolTools.tools {
    class AutoRole : toolBase {

        public Roles first = Roles.UTILITY;
        public Roles second = Roles.JUNGLE;

        public AutoRole() : base() {
            /*
            if(Save.hasKey("PreferdRole")) {
                first = (Roles)Enum.Parse(typeof(Roles), Save.readData("PreferdRole"));
                second = (Roles)Enum.Parse(typeof(Roles), Save.readData("SecondRole"));
            }
            */
        }

        protected override void init() {
            base.init();

            mode = ToolMode.disabled;

            addToStrip("Auto role: off", toggle);
        }

        protected override void _run(ref bool didRun,toolStepArg arg) {
            if(mode == ToolMode.disabled) {
                return;
            }


            Lobby lobby = clientLCU.GetLobby();

            if(lobby?.gameConfig.showPositionSelector == false) {
                return;
            }

            if(lobby?.localMember.firstPositionPreference == Roles.UNSELECTED && 
               lobby?.localMember.secondPositionPreference == Roles.UNSELECTED) {
                clientLCU.setRoles(first,second);
                didRun = true;
            }
        }
    }
}
