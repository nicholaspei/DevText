using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace DevText.Framework.Script.Hppt
{
    public class HttpContextAdapter:IHttpContext
    {
        HttpContext _context;
        private bool _hasValidWebContext;
        private bool _isDebuggingEnabled;

        public bool HasValidWebContext
        {
            get { return _hasValidWebContext; }
        }

        public bool IsDebuggingEnabled
        {
            get { return _isDebuggingEnabled; }
        }

        #region Constructors 
        public HttpContextAdapter()
        {
        }

        public HttpContextAdapter(HttpContext context)
        {
            _hasValidWebContext = false;
            if (context != null)
            {
                _context = context;
                _isDebuggingEnabled = context.IsDebuggingEnabled;
                _hasValidWebContext = true;
            }
        }
        #endregion

    

        public string ResolveScriptRelativePath(string relativePath)
        {
            if (HasValidWebContext)
            {
                return relativePath.Replace("~", _context.Request.ApplicationPath).Replace("//", "/");
            }
            return ResolveRelativePathWithNoHttpContext(relativePath);
        }

        private string ResolveRelativePathWithNoHttpContext(string relativePath)
        {
            return relativePath.Replace("~", "");
        }
    }
}
