using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using BoostrapperChecker;

namespace BootstrapperCheckerTests
{
    [TestClass]
    public class BuildProjectTests
    {
        [TestMethod]
        public void DuplicateDependencies()
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
            Assert.AreEqual(true, testProject.DuplicateDependencies);

        }
    }
}
