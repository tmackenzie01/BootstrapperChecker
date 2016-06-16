using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace BoostrapperChecker
{
    public class BuildProject
    {
        public BuildProject(String name, IXmlLoader xmlLoader)
        {
            m_title = new ProjectTitle(name);
            m_xmlLoader = xmlLoader;
        }

        public override string ToString()
        {
            return m_title.Name;
        }

        // Assumption that filename exists
        public void Load(String filename)
        {
            String xml = m_xmlLoader.LoadFromFile(filename);
            List<String> interestingProperties = new List<String>() { "dependencies", "finalOutputArtifact" };
            Dictionary<String, String> readProperties = new Dictionary<String, String>();

            using (XmlReader reader = XmlReader.Create(new StringReader(xml)))
            {
                reader.ReadToDescendant("property");

                do
                {
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element:
                            // Check for dependencies, artifacts, externals
                            while (reader.MoveToNextAttribute())
                            {
                                if (interestingProperties.Contains(reader.Value))
                                {
                                    String thisProperty = reader.Value;
                                    reader.MoveToNextAttribute();
                                    readProperties[thisProperty] = reader.Value;
                                }
                            }
                            break;
                        case XmlNodeType.Text:
                        case XmlNodeType.EndElement:
                            // Don't care about these
                            break;
                    }
                } while (reader.Read());
            }

            // All properties read?
            if (readProperties.Count == interestingProperties.Count)
            {
                // Artifact
                OutputArtifact = readProperties["finalOutputArtifact"];

                // Dependencies
                // Should all end with Output.txt, should refer to valid projects
                DependenciesCorrectSpacing = true;
                String allDependencies = readProperties["dependencies"];
                Dependencies = allDependencies.Split(',').ToList().ConvertAll(t => new ProjectTitle(t));

                foreach(ProjectTitle dep in Dependencies)
                {
                    DependenciesCorrectSpacing = DependenciesCorrectSpacing && dep.CorrectSpacing;
                }

                var sortedDependencies = Dependencies.OrderBy(d => d);
                DuplicateDependencies = (Dependencies.Distinct().Count() != Dependencies.Count);
                DependenciesCorrectOrder = Dependencies.SequenceEqual(sortedDependencies);
            }
        }

        public bool IsOutputArtifactValid
        {
            get
            {
                String expectedOutputArtifact = $"{m_title.Name}Output.txt";
                if (!String.IsNullOrEmpty(OutputArtifact))
                {
                    return OutputArtifact.Equals(expectedOutputArtifact);
                }

                return false;
            }
        }

        public bool IsArtifactPresent
        {
            get
            {
                return !String.IsNullOrEmpty(OutputArtifact);
            }
        }

        public bool IsDependenciesPresent
        {
            get
            {
                if (Dependencies != null)
                {
                    return (Dependencies.Count > 0);
                }

                return false;
            }
        }

        ProjectTitle m_title;
        IXmlLoader m_xmlLoader;

        // Artifacts
        public String OutputArtifact { get; set; }

        // Dependencies
        public List<ProjectTitle> Dependencies { get; set; }

        // Keeping dependencies in correct order makes it easier not to add duplicates
        public bool DependenciesCorrectSpacing { get; set; }
        public bool DependenciesCorrectOrder { get; set; }
        public bool DuplicateDependencies { get; set; }
    }
}
