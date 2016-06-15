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
        public BuildProject(String name)
        {
            m_name = name;
        }

        public override string ToString()
        {
            return m_name;
        }

        // Assumption that filename exists
        public void Load(String filename)
        {
            String xml = File.ReadAllText(filename);
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
                String allDependencies = readProperties["dependencies"];
                Dependencies = allDependencies.Split(',').ToList();
                var sortedDependencies = Dependencies.OrderBy(d => d);
                DuplicateDependencies = (Dependencies.Distinct().Count() != Dependencies.Count);
                DependenciesCorrectOrder = Dependencies.SequenceEqual(sortedDependencies);
            }
        }

        public bool IsOutputArtifactValid
        {
            get
            {
                String expectedOutputArtifact = $"{m_name}Output.txt";
                return OutputArtifact.Equals(expectedOutputArtifact);
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
                return (Dependencies.Count > 0);
            }
        }

        String m_name;

        // Artifacts
        public String OutputArtifact { get; set; }

        // Dependencies
        public List<String> Dependencies { get; set; }

        // Keeping dependencies in correct order makes it easier not to add duplicates
        public bool DependenciesCorrectOrder { get; set; }
        public bool DuplicateDependencies { get; set; }
    }
}
