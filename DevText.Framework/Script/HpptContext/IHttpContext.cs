using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DevText.Framework.Script.Hppt
{
    public interface IHttpContext
    {
        bool IsDebuggingEnabled { get; }
        bool HasValidWebContext { get; }
        string ResolveScriptRelativePath(string relativePath);
    }
}
