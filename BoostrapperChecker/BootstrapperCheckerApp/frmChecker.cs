using BoostrapperChecker;
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
            lblInformation.Text = "";
        }

        private void btnReadProjects_Click(object sender, EventArgs e)
        {
            txtFolder.Enabled = false;
            m_ccConfig = new CruiseControlConfig(txtFolder.Text);

            String summaryText = "";
            List<BuildProject> projects = m_ccConfig.ReadProjects(ref summaryText);
            
            lblInformation.Text = summaryText;
            Color nodeColour = Color.Black;
            foreach (BuildProject project in projects)
            {
                nodeColour = Color.Black;
                TreeNode projectNode = new TreeNode(project.ToString());
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

                projectNode.ForeColor = nodeColour;
                treeProjects.Nodes.Add(projectNode);
            }
        }

        CruiseControlConfig m_ccConfig;
    }
}
