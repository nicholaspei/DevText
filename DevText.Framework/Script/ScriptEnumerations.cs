using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DevText.Framework.Script
{
    internal enum ScriptState
    {
        Original,  // neither debug nor release, but its original naming
        Debug,    // the script is named with a debug extension eg. script.debug.js
        Release    // the script is named with a release extension eg. script.min.js
    }

    public enum ScriptType
    {
        Javascript,
        CSS
    }
}
