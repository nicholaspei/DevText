using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Web;
using DevText.Framework.Script.Constants;

namespace DevText.Framework.Script
{
    internal class ScriptDependencyLoader
    {
        private ScriptDependencyContainer _dependencyContainer = new ScriptDependencyContainer();
        public ScriptDependencyContainer DependencyContainer { get { return _dependencyContainer; } }

        private const string FILENAME = "ScriptDependencies.xml";
        private readonly string[] FILEPATHS = new string[]
        {
            ".",
            "..",
            ".\\Dependencies",
            "..\\Dependencies",
            AppDomain.CurrentDomain.BaseDirectory,
            HttpContext.Current!=null?HttpContext.Current.Request.PhysicalApplicationPath:string.Empty,
            Environment.GetFolderPath(Environment.SpecialFolder.Resources),
            Environment.GetFolderPath(Environment.SpecialFolder.System),
            Environment.GetFolderPath(Environment.SpecialFolder.SystemX86),
            Environment.GetFolderPath(Environment.SpecialFolder.Windows)
        };

        public string DependencyResourceFile { get; set; }

        internal void FindDependencyFile()
        {
            // look in well known locations for a script dependency file
            foreach (var dirPath in FILEPATHS)
            {
                string tryPath = string.Format("{0}\\{1}", dirPath, FILENAME);
                if (Directory.Exists(dirPath) && System.IO.File.Exists(tryPath))
                {
                    DependencyResourceFile = tryPath;
                    break;
                }
            }

            if (string.IsNullOrWhiteSpace(DependencyResourceFile))
                throw new FileNotFoundException(string.Format("{0} Dependency file not found.", FILENAME));

        }

        internal void LoadDependencies()
        {
            LoadDependencies(DependencyResourceFile);
        }

        internal void LoadDependencies(string filename)
        {
            // If we did not find a file, or the dependency file load ended up loading nonthing,
            // then at least load up some known dependencies.
            if (string.IsNullOrWhiteSpace(filename))
                return;

            var xmlFile = XDocument.Load(filename);
            var rootElement = xmlFile.Element(XmlConstants.DependenciesNode);
            var dependencies = from e in rootElement.Elements(XmlConstants.DependencyNode)
                               select e;
            var releaseSuffixEl = rootElement.Attribute(XmlConstants.ReleaseSuffixAttribute);
            var debugSuffixEl = rootElement.Attribute(XmlConstants.DebugSuffixAttribute);

            _dependencyContainer.ReleaseSuffix = releaseSuffixEl != null ? releaseSuffixEl.Value : string.Empty;
            _dependencyContainer.DebugSuffix = debugSuffixEl != null ? debugSuffixEl.Value : string.Empty;

            ExtractDependencies(dependencies);
        }

        private void ExtractDependencies(IEnumerable<XElement> dependencies)
        {
            foreach (var dependency in dependencies)
            {
                var scriptDependency = new ScriptDependency();
                var nameAttrib = dependency.Attribute(XmlConstants.NameAttribute);
                if (nameAttrib != null)
                {
                    scriptDependency.ScriptName = nameAttrib.Value.ToLowerInvariant();
                }

                var typeAttrib = dependency.Attribute(XmlConstants.TypeAttribute);
                if (typeAttrib != null)
                {
                    scriptDependency.TypeOfScript = ScriptType.Javascript;
                    if (typeAttrib.Value.ToLowerInvariant() == XmlConstants.CSSTypeValue)
                    {
                        scriptDependency.TypeOfScript = ScriptType.CSS;
                    }
                }

                var file = dependency.Element(XmlConstants.ScriptFileElement);
                if (file != null)
                {
                    scriptDependency.ScriptPath = file.Value;
                }

                var requiredDependencies = dependency.Element(XmlConstants.RequirdDependenciesNode);
                if (requiredDependencies != null)
                {
                    var names = from rd in requiredDependencies.Elements(XmlConstants.NameElement)
                                select rd;
                    if (names.Count() > 0)
                    {
                        scriptDependency.RequiredDependencies = new List<string>();
                        names.ToList().ForEach(n => scriptDependency.RequiredDependencies.Add(n.Value.ToLowerInvariant()));
                    }
                }
                _dependencyContainer.Dependencies.Add(scriptDependency);
            }
        }

        internal void Initialise()
        {
            FindDependencyFile();
            LoadDependencies();
        }
    }
}
