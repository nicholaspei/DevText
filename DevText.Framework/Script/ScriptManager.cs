using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web;
using System.Threading;
using System.IO;
using DevText.Framework.Script.Constants;
using DevText.Framework.Script.Hppt;

namespace DevText.Framework.Script
{
    public static class ScriptManager
    {
        private static ScriptDependencyLoader _scriptLoader = new ScriptDependencyLoader();

        static ScriptManager()
        {
            _scriptLoader.Initialise();
            WebHttpContext = new HttpContextAdapter(HttpContext.Current);
        }

        public static IHttpContext WebHttpContext { get; set; }

        public static MvcHtmlString RequiresScript (string scriptName)
        {
            if (string.IsNullOrWhiteSpace(scriptName))
                return MvcHtmlString.Empty;

            StringBuilder emittedScript = new StringBuilder();

            // If all scripts are specified, then make a call to the RequiresScripts passing in an array
            // containing all the scripts we read in from depnedency file.
            if (scriptName.ToLowerInvariant() == ScriptName.AllScripts)
            {
                List<string> allScripts = new List<string>();
                _scriptLoader.DependencyContainer.Dependencies.ForEach(s => allScripts.Add(s.ScriptName));

                return RequiresScripts(allScripts.ToArray());
            }
            GenerateDependencyScript(scriptName, emittedScript);

            return MvcHtmlString.Create(emittedScript.ToString());
        }
        public static MvcHtmlString RequiresScripts(params string[] scriptNames)
        {
            // Use this to register multiple scripts and prevent multiple entries of base script like
            // jQuery core.
            StringBuilder emittedScript = new StringBuilder();

            // First lets see if the required script has any dependencies and include them first
            if (scriptNames != null && scriptNames.Length > 0)
            {
                foreach (var scriptName in scriptNames)
                {
                    if (!string.IsNullOrWhiteSpace(scriptName))
                        GenerateDependencyScript(scriptName, emittedScript);
                }
            }

            return MvcHtmlString.Create(emittedScript.ToString());
        }

        private static void GenerateDependencyScript(string scriptName, StringBuilder emittedScript)
        {
            var dependency = _scriptLoader.DependencyContainer.Dependencies.SingleOrDefault(s => s.ScriptName.ToLowerInvariant() == scriptName.ToLowerInvariant());
            if (dependency != null)
            {
                if (dependency.RequiredDependencies != null && dependency.RequiredDependencies.Count > 0)
                {
                    dependency.RequiredDependencies.ForEach(dependencyName =>
                    {
                        var requiredDependency = _scriptLoader.DependencyContainer.Dependencies.Single(d => d.ScriptName == dependencyName.ToLowerInvariant());
                        AddScriptToOutputBuffer(requiredDependency, emittedScript);
                        // Recursively hunt for script dependencies
                        GenerateDependencyScript(requiredDependency.ScriptName, emittedScript);
                    });
                }
                if (!string.IsNullOrWhiteSpace(dependency.ScriptPath))
                {
                    AddScriptToOutputBuffer(dependency, emittedScript);
                }
            }
        }

        private static void AddScriptToOutputBuffer(ScriptDependency dependency, StringBuilder buffer)
        {
            string fullScriptInclude;
            var resolvedPath = WebHttpContext.ResolveScriptRelativePath(dependency.ScriptPath);
            if (dependency.TypeOfScript == ScriptType.CSS)
            {
                fullScriptInclude = string.Format(ScriptHelperConstants.CSSInclude, resolvedPath);
            }
            else
            {
                var scriptNameBasedOnMode = DetermineScriptNameBasedOnDebugOrRelease(resolvedPath);
                fullScriptInclude = string.Format(ScriptHelperConstants.ScriptInclude, scriptNameBasedOnMode);
            }
            if (!HasScriptAlreadyBeenAdded(fullScriptInclude, buffer))
            {
                buffer.Append(fullScriptInclude);
            }
        }

        private static object DetermineScriptNameBasedOnDebugOrRelease(string resolvedScriptPath)
        {
            if (WebHttpContext.HasValidWebContext)
            {
                ScriptState scriptState = ScriptState.Original;

                var debugSuffix = string.Format("{0}.js", _scriptLoader.DependencyContainer.DebugSuffix);
                if (resolvedScriptPath.Length > debugSuffix.Length)
                {
                    var scriptSuffix = resolvedScriptPath.Substring(resolvedScriptPath.Length - debugSuffix.Length, debugSuffix.Length);
                    if (scriptSuffix == debugSuffix)
                        scriptState = ScriptState.Debug;
                }

                var releaseSuffix = string.Format("{0}.js", _scriptLoader.DependencyContainer.ReleaseSuffix);
                if (resolvedScriptPath.Length > releaseSuffix.Length)
                {
                    var scriptSuffix = resolvedScriptPath.Substring(resolvedScriptPath.Length - releaseSuffix.Length, releaseSuffix.Length);
                    if (scriptSuffix == releaseSuffix)
                        scriptState = ScriptState.Release;
                }

                string actualScriptPath = null;
                if (scriptState != ScriptState.Debug && WebHttpContext.IsDebuggingEnabled)
                    actualScriptPath = ChangeScriptNameToDebug(resolvedScriptPath);
                if (scriptState != ScriptState.Release && !WebHttpContext.IsDebuggingEnabled)
                    actualScriptPath = ChangeScriptNameToRelease(resolvedScriptPath);

                if (!string.IsNullOrWhiteSpace(actualScriptPath))
                    return actualScriptPath;

                return resolvedScriptPath;
            }
            else
                return resolvedScriptPath;
        }

        private static string ChangeScriptNameToRelease(string resolvedScriptPath)
        {
            var scriptPreffix = resolvedScriptPath.Substring(0, resolvedScriptPath.Length - ScriptHelperConstants.JSPrefix.Length);
            if (string.IsNullOrWhiteSpace(_scriptLoader.DependencyContainer.ReleaseSuffix))
                return resolvedScriptPath;
            return string.Format("{0}.{1}.js", scriptPreffix, _scriptLoader.DependencyContainer.ReleaseSuffix);
        }

        private static string ChangeScriptNameToDebug(string resolvedScriptPath)
        {
            var scriptPrefix = resolvedScriptPath.Substring(0, resolvedScriptPath.Length - ScriptHelperConstants.JSPrefix.Length);
            if (string.IsNullOrWhiteSpace(_scriptLoader.DependencyContainer.DebugSuffix))
                return resolvedScriptPath;

            return string.Format("{0}.{1}.js", scriptPrefix, _scriptLoader.DependencyContainer.DebugSuffix);
        }

        private static bool HasScriptAlreadyBeenAdded(string scriptToCheck, StringBuilder emittedScript)
        {
            var existingScript = emittedScript.ToString().ToLowerInvariant();
            return (existingScript.Contains(scriptToCheck.ToLowerInvariant()));
        }


    }
}
