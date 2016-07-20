namespace BootstrapperCheckerApp
{
    partial class frmChecker
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.txtFolder = new System.Windows.Forms.TextBox();
            this.lblFolder = new System.Windows.Forms.Label();
            this.btnReadProjects = new System.Windows.Forms.Button();
            this.treeProjects = new System.Windows.Forms.TreeView();
            this.lblInformation = new System.Windows.Forms.Label();
            this.txtWorkingCopiesFolder = new System.Windows.Forms.TextBox();
            this.lblWorkingCopies = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txtFolder
            // 
            this.txtFolder.Location = new System.Drawing.Point(139, 12);
            this.txtFolder.Name = "txtFolder";
            this.txtFolder.Size = new System.Drawing.Size(284, 20);
            this.txtFolder.TabIndex = 0;
            // 
            // lblFolder
            // 
            this.lblFolder.AutoSize = true;
            this.lblFolder.Location = new System.Drawing.Point(3, 15);
            this.lblFolder.Name = "lblFolder";
            this.lblFolder.Size = new System.Drawing.Size(130, 13);
            this.lblFolder.TabIndex = 1;
            this.lblFolder.Text = "CruiseControl config folder";
            // 
            // btnReadProjects
            // 
            this.btnReadProjects.Location = new System.Drawing.Point(573, 54);
            this.btnReadProjects.Name = "btnReadProjects";
            this.btnReadProjects.Size = new System.Drawing.Size(90, 23);
            this.btnReadProjects.TabIndex = 2;
            this.btnReadProjects.Text = "Read projects";
            this.btnReadProjects.UseVisualStyleBackColor = true;
            this.btnReadProjects.Click += new System.EventHandler(this.btnReadProjects_Click);
            // 
            // treeProjects
            // 
            this.treeProjects.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.treeProjects.Location = new System.Drawing.Point(6, 99);
            this.treeProjects.Name = "treeProjects";
            this.treeProjects.Size = new System.Drawing.Size(657, 399);
            this.treeProjects.TabIndex = 3;
            // 
            // lblInformation
            // 
            this.lblInformation.AutoSize = true;
            this.lblInformation.Location = new System.Drawing.Point(136, 40);
            this.lblInformation.Name = "lblInformation";
            this.lblInformation.Size = new System.Drawing.Size(59, 13);
            this.lblInformation.TabIndex = 4;
            this.lblInformation.Text = "Information";
            // 
            // txtWorkingCopiesFolder
            // 
            this.txtWorkingCopiesFolder.Location = new System.Drawing.Point(139, 56);
            this.txtWorkingCopiesFolder.Name = "txtWorkingCopiesFolder";
            this.txtWorkingCopiesFolder.Size = new System.Drawing.Size(284, 20);
            this.txtWorkingCopiesFolder.TabIndex = 0;
            // 
            // lblWorkingCopies
            // 
            this.lblWorkingCopies.AutoSize = true;
            this.lblWorkingCopies.Location = new System.Drawing.Point(3, 59);
            this.lblWorkingCopies.Name = "lblWorkingCopies";
            this.lblWorkingCopies.Size = new System.Drawing.Size(114, 13);
            this.lblWorkingCopies.TabIndex = 1;
            this.lblWorkingCopies.Text = "Project working copies";
            // 
            // frmChecker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(675, 504);
            this.Controls.Add(this.lblInformation);
            this.Controls.Add(this.treeProjects);
            this.Controls.Add(this.btnReadProjects);
            this.Controls.Add(this.lblWorkingCopies);
            this.Controls.Add(this.lblFolder);
            this.Controls.Add(this.txtWorkingCopiesFolder);
            this.Controls.Add(this.txtFolder);
            this.Name = "frmChecker";
            this.Text = "frmChecker";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtFolder;
        private System.Windows.Forms.Label lblFolder;
        private System.Windows.Forms.Button btnReadProjects;
        private System.Windows.Forms.TreeView treeProjects;
        private System.Windows.Forms.Label lblInformation;
        private System.Windows.Forms.TextBox txtWorkingCopiesFolder;
        private System.Windows.Forms.Label lblWorkingCopies;
    }
}