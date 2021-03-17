using System;
using System.Threading;
using System.Diagnostics;

using LCU;
using LCU.Helper;
using LCU.Champs;


namespace lolTools.tools {

    public enum ToolMode {
        runOnce,
        enabled,
        disabled,
    }

    public class toolStepArg : EventArgs {

        public toolStepArg(TimeSpan dt) {
            deltaTime = dt;
            phase = clientLCU.GetGamePhase();
        }

        public TimeSpan deltaTime;
        public gameFlowPhase phase;
        public bool clientAPI = clientLCU.IsApiReady();
        //public bool gameAPI = gameLCU.IsApiReady();
        public Session champSession = clientLCU.getChampSelectSession();
    }
    //########################################################################

    //########################################################################
    public static class toolManeger {

        public static event EventHandler<toolStepArg> toolTickEvent;

        private static DateTime lastStep = DateTime.Now; // <--- start at now?
        private static bool runing = true;


        public static void loopFunc(object state = null) {
            while(runing) {
                invokeTools();
                Thread.Sleep(250);
            }
        }

        public static void stop() {
            runing = false;
        }

        public static void invokeTools(object requestor = null) {
            toolStepArg data = new toolStepArg(DateTime.Now-lastStep);

            if(!data.clientAPI) {
                try {
                    data.clientAPI = clientLCU.init();
                } catch {
                    Debug.WriteLine($"Cant re init client lcu");
                }
            }

            toolTickEvent.Invoke(requestor, data);
        }

    }
}
