using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace BoostrapperChecker
{
    public class ConfigFile
    {
        public ConfigFile(IXmlLoader xmlLoader)
        {
            m_xmlLoader = xmlLoader;
            Includes = new List<String>();
            ConfigProjects = new List<ConfigProject>();
        }

        public void Read(String filename)
        {
            String xmlText = m_xmlLoader.LoadFromFile(filename);
            String parentDirectory = Path.GetDirectoryName(filename);

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlText);

            XmlNodeList includeNodes = xmlDoc.SelectNodes("cruisecontrol/include.projects");
            XmlNodeList configProjects = xmlDoc.SelectNodes("cruisecontrol/project");
            foreach(XmlNode includeNode in includeNodes)
            {
                Includes.Add(Path.Combine(parentDirectory, includeNode.Attributes["file"].Value));
            }

            foreach(XmlNode configProject in configProjects)
            {
                ConfigProject newConfigProject = new ConfigProject(new XmlLoader(configProject.OuterXml));
                newConfigProject.Load();
                ConfigProjects.Add(newConfigProject);
            }
        }

        IXmlLoader m_xmlLoader;
        public List<String> Includes { get; set; }
        List<ConfigProject> ConfigProjects;
    }
}
