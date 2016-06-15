using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BoostrapperChecker
{
    public class CruiseControlConfig
    {
        public CruiseControlConfig(String folder)
        {
            m_configDirectory = folder;
        }

        public List<BuildProject> ReadProjects(ref String summaryText)
        {
            List<BuildProject> projects = new List<BuildProject>();
            if (!Directory.Exists(m_configDirectory))
            {
                summaryText = $"Cruise Control config folder {m_configDirectory} does not exist";
            }
            else
            {
                String bootstrapperDirectory = Path.Combine(m_configDirectory, "ProjectBootstrappers");
                String[] configFiles = Directory.GetFiles(m_configDirectory, "config*.xml");
                String[] boostrapperFiles = Directory.GetFiles(bootstrapperDirectory, "*.xml", SearchOption.AllDirectories);

                if (configFiles.Length > 0)
                {
                    if (boostrapperFiles.Length > 0)
                    {
                        // Parse config for projects xml

                        // To start create project object for each bootstrapper found (could do it for the config projects - doesn't really matter)
                        Regex bootstrapperRegex = new Regex("(?<projectName>[A-Z,a-z,0-9]+)Bootstrapper.xml$", RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.Multiline);

                        foreach (String boostrapperFile in boostrapperFiles)
                        {
                            Match match = bootstrapperRegex.Match(Path.GetFileName(boostrapperFile));
                            if (match.Success)
                            {
                                String projectName = match.Groups["projectName"].Value;
                                if (!IsExemptBootstrapper(projectName))
                                {
                                    BuildProject newProject = new BuildProject(projectName);
                                    newProject.Load(boostrapperFile);
                                    projects.Add(newProject);
                                }
                            }
                        }

                        // Add any projects that exist in the config but don't have a bootstrapper
                    }
                    else
                    {
                        summaryText = $"No bootstrapper files found in {bootstrapperDirectory}";
                    }
                }
                else
                {
                    summaryText = $"No config files found in {m_configDirectory}";
                }
            }

            return projects;
        }

        public bool IsExemptBootstrapper(String bootstrapperName)
        {
            return (m_exemptBootstrappers.Contains(bootstrapperName) || (bootstrapperName.EndsWith("ReleaseBuild")));
        }

        String m_configDirectory;

        String[] m_exemptBootstrappers = new String[] { "Base", "BaseRelease", "FullInstallTest",
            "DropboxUploaderHistory", "DropboxUploaderHistoryTags", "DropboxUploaderHistoryTestTags" };
    }
}
