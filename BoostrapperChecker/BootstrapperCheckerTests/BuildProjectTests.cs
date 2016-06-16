using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using BoostrapperChecker;
using System.Collections.Generic;

namespace BootstrapperCheckerTests
{
    [TestClass]
    public class BuildProjectTests
    {
        [TestMethod]
        public void MissingDependenciesAndArtifacts()
        {
            var mockXmlLoader = new Mock<IXmlLoader>();
            mockXmlLoader.Setup(x => x.LoadFromFile("test")).Returns(
                @"<project name=""DummyProject"" default=""all"">" +
                @"</project>");

            BuildProject testProject = new BuildProject("DummyProject", mockXmlLoader.Object);
            testProject.Load("test");

            Assert.AreEqual(false, testProject.IsDependenciesPresent);
            Assert.AreEqual(false, testProject.IsArtifactPresent);
            Assert.AreEqual(false, testProject.IsOutputArtifactValid);
        }

        [TestMethod]
        public void IncorrectFinalArtifactName()
        {
            var mockXmlLoader = new Mock<IXmlLoader>();
            mockXmlLoader.Setup(x => x.LoadFromFile("test")).Returns(
                @"<project name=""DummyProject"" default=""all"">" +
                @"<property name=""dependencies"" value=""Project1Output.txt,Project2Output.txt,Project2Output.txt""/>" +
                @"<property name=""finalOutputArtifact"" value=""WrongProjectOutput.txt""/>" +
                @"</project>");

            BuildProject testProject = new BuildProject("DummyProject", mockXmlLoader.Object);
            testProject.Load("test");

            Assert.AreEqual(false, testProject.IsOutputArtifactValid);
        }

        [TestMethod]
        public void IncorrectSpacingDependencies()
        {
            var mockXmlLoader = new Mock<IXmlLoader>();
            mockXmlLoader.Setup(x => x.LoadFromFile("test")).Returns(
                @"<project name=""DummyProject"" default=""all"">" +
                @"<property name=""dependencies"" value=""Project1Output.txt, Project2Output.txt, Project2Output.txt""/>" +
                @"<property name=""finalOutputArtifact"" value=""DummyProjectOutput.txt""/>" +
                @"</project>");

            BuildProject testProject = new BuildProject("TestProject", mockXmlLoader.Object);
            testProject.Load("test");

            Assert.AreEqual(true, testProject.IsDependenciesPresent);
            Assert.AreEqual(false, testProject.DependenciesCorrectSpacing);
        }

        [TestMethod]
        public void DuplicateDependencies()
        {
            var mockXmlLoader = new Mock<IXmlLoader>();
            mockXmlLoader.Setup(x => x.LoadFromFile("test")).Returns(
                @"<project name=""DummyProject"" default=""all"">" +
                @"<property name=""dependencies"" value=""Project1Output.txt,Project2Output.txt,Project2Output.txt""/>" +
                @"<property name=""finalOutputArtifact"" value=""DummyProjectOutput.txt""/>" +
                @"</project>");

            BuildProject testProject = new BuildProject("TestProject", mockXmlLoader.Object);
            testProject.Load("test");

            Assert.AreEqual(true, testProject.IsDependenciesPresent);
            Assert.AreEqual(true, testProject.DuplicateDependencies);
            Assert.AreEqual(true, testProject.DependenciesCorrectOrder);
        }

        [TestMethod]
        public void ValidArtifactsDependencies()
        {
            var mockXmlLoader = new Mock<IXmlLoader>();
            mockXmlLoader.Setup(x => x.LoadFromFile("test")).Returns(
                @"<project name=""DummyProject"" default=""all"">" +
                @"<property name=""dependencies"" value=""Project1Output.txt,Project2Output.txt,Project2Output.txt""/>" +
                @"<property name=""finalOutputArtifact"" value=""DummyProjectOutput.txt""/>" +
                @"</project>");

            BuildProject testProject = new BuildProject("DummyProject", mockXmlLoader.Object);
            testProject.Load("test");

            Assert.AreEqual(true, testProject.IsOutputArtifactValid);
            Assert.AreEqual(true, testProject.IsDependenciesPresent);
            Assert.AreEqual(true, testProject.DuplicateDependencies);
            Assert.AreEqual(true, testProject.DependenciesCorrectOrder);
        }

        [TestMethod]
        public void SortedDependencies()
        {
            var mockXmlLoader = new Mock<IXmlLoader>();
            mockXmlLoader.Setup(x => x.LoadFromFile("test")).Returns(
                @"<project name=""DummyProject"" default=""all"">" +
                @"<property name=""dependencies"" value=""Project1Output.txt,Project2Output.txt,Project3Output.txt,DependenciesOutput.txt,InstallsOutput.txt""/>" +
                @"<property name=""finalOutputArtifact"" value=""DummyProjectOutput.txt""/>" +
                @"</project>");

            BuildProject testProject = new BuildProject("TestProject", mockXmlLoader.Object);
            testProject.Load("test");

            List<ProjectTitle> dependenciesInCorrectOrder = new List<ProjectTitle>() { new ProjectTitle("Dependencies"),
                                                                                        new ProjectTitle("Installs"),
                                                                                        new ProjectTitle("Project1"),
                                                                                        new ProjectTitle("Project2"),
                                                                                        new ProjectTitle("Project3") };
            CollectionAssert.AreEquivalent(testProject.Dependencies, dependenciesInCorrectOrder);
        }
    }
}
