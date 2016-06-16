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

        [TestMethod]
        public void ProjectTitlesComparing()
        {
            ProjectTitle projectDependencies1 = new ProjectTitle("Dependencies");
            ProjectTitle projectDependencies2 = new ProjectTitle("Dependencies");
            ProjectTitle projectInstalls = new ProjectTitle("Installs");
            ProjectTitle project1 = new ProjectTitle("Project1");
            ProjectTitle project2 = new ProjectTitle("Project2");
            ProjectTitle project3 = new ProjectTitle("Project3");

            // Same projects (2 different instances of the dependencies one though)
            List<ProjectTitle> sameProjects1 = new List<ProjectTitle>() { project1, projectDependencies1, project2, projectInstalls, project3 };
            List<ProjectTitle> sameProjects2 = new List<ProjectTitle>() { project1, projectDependencies2, project2, projectInstalls, project3 };
            CollectionAssert.AreEqual(sameProjects1, sameProjects2);

            // Same projects - after sorting (2 different instances of the dependencies one though)
            List<ProjectTitle> allProjects = new List<ProjectTitle>() { project1, projectDependencies1, project2, projectInstalls, project3 };
            List<ProjectTitle> allProjectsInCorrectOrder = new List<ProjectTitle>() { projectDependencies2, projectInstalls, project1, project2, project3 };
            ProjectTitleComparer comp = new ProjectTitleComparer();
            allProjects.Sort(comp);

            CollectionAssert.AreEqual(allProjects, allProjectsInCorrectOrder);
        }
    }
}
