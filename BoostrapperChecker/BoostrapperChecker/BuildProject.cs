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
            List<String> interestingProperties = new List<String>() { "dependencies", "artifacts" };
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
                String allArtifacts = readProperties["artifacts"];

                // Artifacts should end with <projectname>Output.txt
                String expectedOutputArtifact = $"{m_name}Output.txt";
                if (allArtifacts.EndsWith(expectedOutputArtifact))
                {
                    Artifacts = allArtifacts.Split(',').ToList();

                    // Remove expected artifact
                    Artifacts.Remove(expectedOutputArtifact);
                    OutputArtifact = expectedOutputArtifact;
                }
            }
        }

        public bool IsOutputArtifactValid
        {
            get
            {
                return !String.IsNullOrEmpty(OutputArtifact);
            }
        }

        public bool IsArtifactsPresent
        {
            get
            {
                return (Artifacts?.Count > 0);
            }
        }        

        String m_name;

        // Artifacts
        public String OutputArtifact { get; set; }
        public List<String> Artifacts { get; set; }
    }
}
