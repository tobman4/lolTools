using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lolTools.tools {

    
    public enum ControllAction {  
        INIT,
        SetMode,
        GetLast,
        None,
    }

    public class ActionArg : EventArgs {
        public ControllAction action;
        public ToolMode mode;
        public string target = "ALL";
    }
}
