﻿using BoostrapperChecker;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BootstrapperCheckerApp
{
    public partial class frmChecker : Form
    {
        public frmChecker()
        {
            InitializeComponent();
            txtFolder.Text = @"D:\CodeSandbox\CruiseControlConfig_trunk";
            txtWorkingCopiesFolder.Text = @"D:\CodeSandbox";
            lblInformation.Text = "";
        }

        private void btnReadProjects_Click(object sender, EventArgs e)
        {
            txtFolder.Enabled = false;
            m_ccConfig = new CruiseControlConfig(txtFolder.Text);

            String summaryText = "";
            List<BuildProject> projects = m_ccConfig.ReadProjects(ref summaryText);
            List<VSSolution> solutions = new List<VSSolution>();

            lblInformation.Text = summaryText;
            Color nodeColour = Color.Black;
            foreach (BuildProject project in projects)
            {
                // Add the solution - should be located in <Working copies dir>\<project name>_trunk
                String predictedWorkingCopyDir = System.IO.Path.Combine(txtWorkingCopiesFolder.Text, $"{ project.ToString()}_trunk");
                VSSolution tempSolution = new VSSolution(project.ToString(), predictedWorkingCopyDir);
                tempSolution.ParseSolution();

                solutions.Add(tempSolution);

                // TODO Think we'll add the solution onto the actual project (AttachSolution method or property) but for now just build a list
                
                TreeNode projectNode = new TreeNode(project.ToString());

                // Artifact sub tree node
                nodeColour = Color.Black;
                if (!project.IsArtifactPresent)
                {
                    projectNode.Nodes.Add("No artifacts present");
                    nodeColour = Color.Red;
                }
                else
                {
                    if (!project.IsOutputArtifactValid)
                    {
                        projectNode.Nodes.Add($"Output artifact invalid");
                        nodeColour = Color.Red;
                    }
                    else
                    {
                        projectNode.Nodes.Add("Output artifact valid");
                    }
                }

                // Dependencies sub tree node
                if (!project.IsDependenciesPresent)
                {
                    projectNode.Nodes.Add("No dependencies present");
                    nodeColour = Color.Red;
                }
                else
                {
                    if (!project.DependenciesCorrectOrder)
                    {
                        projectNode.Nodes.Add("Dependencies not in alpabetical order");
                        nodeColour = Color.Red;
                    }
                    else
                    {
                        if (project.DuplicateDependencies)
                        {
                            projectNode.Nodes.Add("Duplicate dependency present");
                            nodeColour = Color.Red;
                        }
                        else
                        {
                            if (!project.DependenciesCorrectSpacing)
                            {
                                projectNode.Nodes.Add("Incorrect spacing in dependencies");
                                nodeColour = Color.Red;
                            }
                            else
                            {
                                projectNode.Nodes.Add($"Dependencies present ({String.Join(",", project.Dependencies)})");
                            }
                        }
                    }
                }

                projectNode.ForeColor = nodeColour;
                treeProjects.Nodes.Add(projectNode);
            }
        }

        CruiseControlConfig m_ccConfig;
    }
}
