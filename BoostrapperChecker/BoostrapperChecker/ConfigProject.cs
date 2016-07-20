using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace BoostrapperChecker
{
    public class ConfigProject
    {
        public ConfigProject(IXmlLoader xmlLoader)
        {
            m_xmlLoader = xmlLoader;
        }

        public void Load()
        {
            // Assume valid till proven otherwise
            IsValid = true;

            String xmlText = m_xmlLoader.Load();

            XmlDocument xmlDoc = new XmlDocument();
            //Debug.WriteLine(xmlText.Replace(">", ">\r\n"));
            xmlDoc.LoadXml(xmlText);

            var projectNodes = xmlDoc.SelectNodes("project");
            if (projectNodes.Count == 1)
            {
                m_title = new ProjectTitle(projectNodes[0].Attributes["name"].Value);
            }

            bool validDependencies = true;
            var fileSystemNodes = xmlDoc.SelectNodes("project/modificationset/filesystem");
            foreach (XmlNode fileSystemNode in fileSystemNodes)
            {
                // dependencyFile should be located in C:\CruiseControl\artifacts\trunk\ and be <projectTitle>Output.txt
                // modName should be mod-<projectTitle> and should match one another

                String dependencyFile = fileSystemNode.Attributes["folder"].Value;
                String fileProjectName = "";
                Regex dependencyRegex = new Regex(@"c..cruisecontrol.artifacts.trunk.(?<fileProjectName>[a-z0-9]*)output.txt$");

                Match match = dependencyRegex.Match(dependencyFile.ToLower());
                if (match.Success)
                {
                    fileProjectName = match.Groups["fileProjectName"].Value;
                }

                String modName = fileSystemNode.Attributes["property"].Value;
                String modProjectName = "";
                Regex modRegex = new Regex(@"mod.(?<modProjectName>[a-z0-9]*)$");
                match = modRegex.Match(modName.ToLower());
                if (match.Success)
                {
                    modProjectName = match.Groups["modProjectName"].Value;
                }

                validDependencies = validDependencies && (fileProjectName.Equals(modProjectName));
            }

            if (!validDependencies)
            {
                Debug.WriteLine($"*** THIS ONE DOES NOT MATCH {m_title.Name}");
            }
        }

        public String Name
        {
            get
            {
                return m_title.Name;
            }
        }

        IXmlLoader m_xmlLoader;

        bool IsValid { get; set; }
        ProjectTitle m_title;
        public List<ProjectTitle> Dependencies { get; set; }
    }
}
