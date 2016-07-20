using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            String xmlText = m_xmlLoader.LoadFromFile(filename);
            List<String> interestingProperties = new List<String>() { "dependencies", "finalOutputArtifact" };
            Dictionary<String, String> readProperties = new Dictionary<String, String>();

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlText);
            XmlNodeList dependenciesNodes = xmlDoc.SelectNodes("project/property[@name='dependencies']");
            XmlNodeList finalOutputArtifactNodes = xmlDoc.SelectNodes("project/property[@name='finalOutputArtifact']");

            // Should only find one of each property declaration for dependencies & finalOutputArtifact
            if (dependenciesNodes.Count == 1)
            {
                readProperties["dependencies"] = dependenciesNodes[0].Attributes["value"].Value;
            }
            if (finalOutputArtifactNodes.Count == 1)
            {
                readProperties["finalOutputArtifact"] = finalOutputArtifactNodes[0].Attributes["value"].Value;
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

                List<String> dependenciesText = allDependencies.Split(',').ToList();
                if (dependenciesText.All(d => d.EndsWith("Output.txt")))
                {
                    Dependencies = dependenciesText.ConvertAll(t => new ProjectTitle(t.Substring(0, t.Length - 10)));

                    // Sort dependencies into custom order
                    ProjectTitleComparer comp = new ProjectTitleComparer();
                    Dependencies.Sort(comp);

                    foreach (ProjectTitle dep in Dependencies)
                    {
                        DependenciesCorrectSpacing = DependenciesCorrectSpacing && dep.CorrectSpacing;
                    }

                    var sortedDependencies = Dependencies.OrderBy(d => d);
                    DuplicateDependencies = (Dependencies.Distinct().Count() != Dependencies.Count);
                    DependenciesCorrectOrder = Dependencies.SequenceEqual(sortedDependencies);
                }
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

        public ProjectTitle Title
        {
            get
            {
                return m_title;
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

    public class BuildProjectDependencyComparer : IComparer<BuildProject>
    {
        public BuildProjectDependencyComparer(List<ProjectTitle> preferredOrder)
        {
            int i = 0;
            m_preferredOrder = new Dictionary<ProjectTitle, int>();
            foreach ( ProjectTitle title in preferredOrder)
            {
                m_preferredOrder[title] = i++;
            }
        }

        public int Compare(BuildProject x, BuildProject y)
        {
            int result = CompareEx(x, y);

            if (result == 0)
            {
                Debug.WriteLine($"{x} == {y}");
            }
            else if (result < 0)
            {
                Debug.WriteLine($"{x} < {y}");
            }
            else // if (result < 0)
            {
                Debug.WriteLine($"{x} > {y}");
            }

            return result;
        }

        public int CompareEx(BuildProject x, BuildProject y)
        {
            int xPos = m_preferredOrder[x.Title];
            int yPos = m_preferredOrder[y.Title];

            return xPos.CompareTo(yPos);
        }

        Dictionary<ProjectTitle, int> m_preferredOrder;
    }
}
