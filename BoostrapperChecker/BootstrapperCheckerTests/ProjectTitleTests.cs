using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using BoostrapperChecker;
using System.Linq;

namespace BootstrapperCheckerTests
{
    [TestClass]
    public class ProjectTitleTests
    {
        [TestMethod]
        public void UniqueProjectTitles()
        {
            ProjectTitle project1 = new ProjectTitle("Project");
            ProjectTitle project2 = new ProjectTitle("Project");
            ProjectTitle project3 = new ProjectTitle("ProjectDifferent");
            
            Assert.AreEqual(true, project1.Equals(project2));
            Assert.AreEqual(false, project1.Equals(project3));
        }

        [TestMethod]
        public void DistinctProjectTitles()
        {
            List<ProjectTitle> differentProjects = new List<ProjectTitle>() { new ProjectTitle("1"), new ProjectTitle("2"), new ProjectTitle("3") };
            Assert.AreEqual(3, differentProjects.Distinct().Count());

            List<ProjectTitle> duplicateProjects = new List<ProjectTitle>() { new ProjectTitle("1"), new ProjectTitle("2"), new ProjectTitle("1") };
            Assert.AreEqual(2, duplicateProjects.Distinct().Count());
        }

        [TestMethod]
        public void CorrectSpacingProjectTitles()
        {
            ProjectTitle project1 = new ProjectTitle("Project1");
            ProjectTitle project2 = new ProjectTitle(" Project2");

            Assert.AreEqual(true, project1.CorrectSpacing);
            Assert.AreEqual(false, project2.CorrectSpacing);
        }
    }
}
