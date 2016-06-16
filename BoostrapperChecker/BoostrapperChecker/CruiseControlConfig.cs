using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            // TODO Because Clone() is a bit of a nightmare I'm going to build 2 lists, one for the final return object and another which is the same but we can modify
            // We should really make one list and then clone it before we try and work out the dependency order
            List<BuildProject> modifiableProjects = new List<BuildProject>();
            List<BuildProject> finalProjects = new List<BuildProject>();
            if (!Directory.Exists(m_configDirectory))
            {
                summaryText = $"Cruise Control config folder {m_configDirectory} does not exist";
            }
            else
            {
                String bootstrapperDirectory = Path.Combine(m_configDirectory, "ProjectBootstrappers");
                String[] configFiles = Directory.GetFiles(m_configDirectory, "config*.xml");
                String[] boostrapperFiles = Directory.GetFiles(bootstrapperDirectory, "*.xml", SearchOption.AllDirectories);

                // TODO This guarantees alphabetical order when dependencies are equal later (kind of - it will use the folder name as well)
                Array.Sort(boostrapperFiles);

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
                                    BuildProject newProject = new BuildProject(projectName, new XmlLoader());
                                    newProject.Load(boostrapperFile);
                                    finalProjects.Add(newProject);

                                    BuildProject newProject2 = new BuildProject(projectName, new XmlLoader());
                                    newProject2.Load(boostrapperFile);
                                    modifiableProjects.Add(newProject2);
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

            // Now work out order based on dependencies
            // This will be the final order - list of project titles
            List<ProjectTitle> finalProjectOrder = new List<ProjectTitle>();

            // Projects with no dependents that we build up each time round a loop
            List<BuildProject> projectsWithNoDependents = new List<BuildProject>();

            // First we need to clone our projects list (can't use existing reference as we remove dependencies each time round the loop)
            List<BuildProject> projectsWithDependencies = new List<BuildProject>();
            projectsWithDependencies.AddRange(modifiableProjects);

            bool dirty = true;

            // TODO I'm really confident this won't end up in an infinite loop
            while (dirty)
            {
                dirty = false;
                // Find projects with no dependencies
                foreach (BuildProject project in projectsWithDependencies)
                {
                    if (project.Dependencies != null)
                    {
                        if (project.Dependencies.Count == 0)
                        {
                            projectsWithNoDependents.Add(project);
                            dirty = true;
                        }
                    }
                    else
                    {
                        projectsWithNoDependents.Add(project);
                        dirty = true;
                    }
                }

                // Remove these projects (with no dependents) from other projects Dependencies
                foreach (BuildProject project in projectsWithDependencies)
                {
                    if (project.Dependencies != null)
                    {
                        foreach (BuildProject projectToRemove in projectsWithNoDependents)
                        {
                            if (project.Dependencies.Contains(projectToRemove.Title))
                            {
                                dirty = true;
                                project.Dependencies.Remove(projectToRemove.Title);

                            }
                        }
                    }
                }

                // Now remove these projects (with no dependents) from the projects list
                foreach (BuildProject projectToRemove in projectsWithNoDependents)
                {
                    if (projectsWithDependencies.Contains(projectToRemove))
                    {
                        dirty = true;
                        projectsWithDependencies.Remove(projectToRemove);
                    }
                }

                Debug.WriteLine("---");
                projectsWithNoDependents.ForEach(x => Debug.WriteLine($"{x}"));

                finalProjectOrder.AddRange(projectsWithNoDependents.Select(x => x.Title).ToList<ProjectTitle>());
                projectsWithNoDependents.Clear();
            }

            if (projectsWithDependencies.Count > 0)
            {
                throw new Exception("Algorithm to calculate project order based on dependencies is broken");
            }

            if (finalProjectOrder.Count != finalProjectOrder.Count)
            {
                throw new Exception("Algorithm to calculate project order based on dependencies is broken");
            }
            finalProjects.Sort(new BuildProjectDependencyComparer(finalProjectOrder));

            return finalProjects;
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
