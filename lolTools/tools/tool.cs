using System;
using System.Collections.Generic;
using System.Text;

namespace lolTools.tools {

    
    [Obsolete("use event in toolManeger.cs")]
    public abstract class tool {

        public static ToolMode mode = ToolMode.disabled;

        public void run(TimeSpan deltaTime) {
            bool res = _run(deltaTime);
            if(res && mode == ToolMode.runOnce) {
                mode = ToolMode.disabled;
            }

            // TODO: log run and res?

        }
        public abstract bool _run(TimeSpan deltaTime);
    }
}
