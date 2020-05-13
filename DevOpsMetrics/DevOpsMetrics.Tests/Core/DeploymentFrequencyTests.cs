using DevOpsMetrics.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace DevOpsMetrics.Tests.Core
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    [TestCategory("UnitTest")]
    [TestClass]
    public class DeploymentFrequencyTests
    {
        [TestMethod]
        public void DeploymentFrequencySingleOneDayTest()
        {
            //Arrange
            DeploymentFrequency metrics = new DeploymentFrequency();
            string pipelineName = "TestPipeline.CI";
            int numberOfDays = 1;
            List<KeyValuePair<DateTime, DateTime>> deploymentFrequencyList = new List<KeyValuePair<DateTime, DateTime>>
            {
                new KeyValuePair<DateTime, DateTime>(DateTime.Now, DateTime.Now)
            };

            //Act
            float result = metrics.ProcessDeploymentFrequency(deploymentFrequencyList, pipelineName, numberOfDays);

            //Assert
            Assert.AreEqual(1f, result);
        }

        [TestMethod]
        public void DeploymentFrequencyNullOneDayTest()
        {
            //Arrange
            DeploymentFrequency metrics = new DeploymentFrequency();
            string pipelineName = "TestPipeline.CI";
            int numberOfDays = 1;
            List<KeyValuePair<DateTime, DateTime>> deploymentFrequencyList = null;

            //Act
            float result = metrics.ProcessDeploymentFrequency(deploymentFrequencyList, pipelineName, numberOfDays);

            //Assert
            Assert.AreEqual(0f, result);
        }

        [TestMethod]
        public void DeploymentFrequencyFiveSevenDaysTest()
        {
            //Arrange
            DeploymentFrequency metrics = new DeploymentFrequency();
            string pipelineName = "TestPipeline.CI";
            int numberOfDays = 7;
            List<KeyValuePair<DateTime, DateTime>> deploymentFrequencyList = new List<KeyValuePair<DateTime, DateTime>>
            {
                new KeyValuePair<DateTime, DateTime>(DateTime.Now, DateTime.Now),
                new KeyValuePair<DateTime, DateTime>(DateTime.Now.AddDays(-1), DateTime.Now.AddDays(-1)),
                new KeyValuePair<DateTime, DateTime>(DateTime.Now.AddDays(-2), DateTime.Now.AddDays(-2)),
                new KeyValuePair<DateTime, DateTime>(DateTime.Now.AddDays(-3), DateTime.Now.AddDays(-3)),
                new KeyValuePair<DateTime, DateTime>(DateTime.Now.AddDays(-4), DateTime.Now.AddDays(-4)),
                new KeyValuePair<DateTime, DateTime>(DateTime.Now.AddDays(-14), DateTime.Now.AddDays(-14)) //this record should be out of range
            };

            //Act
            float result = metrics.ProcessDeploymentFrequency(deploymentFrequencyList, pipelineName, numberOfDays);

            //Assert
            Assert.AreEqual(0.7143f, result);
        }

        [TestMethod]
        public void DeploymentFrequencyRatingEliteTest()
        {
            //Arrange
            DeploymentFrequency metrics = new DeploymentFrequency();
            float dailyDeployment = 1f;
            //float weeklyDeployment = 1f / 7f;
            //float monthlyDeployment = 1f / 30f;

            //Act
            string EliteResult = metrics.GetDeploymentFrequencyRating(dailyDeployment + dailyDeployment);
            string EliteResult2 = metrics.GetDeploymentFrequencyRating(dailyDeployment + 0.001f);

            //Assert
            Assert.AreEqual("Elite", EliteResult);
            Assert.AreEqual("Elite", EliteResult2);
        }

        [TestMethod]
        public void DeploymentFrequencyRatingHighTest()
        {
            //Arrange
            DeploymentFrequency metrics = new DeploymentFrequency();
            //float dailyDeployment = 1f;
            float weeklyDeployment = 1f / 7f;
            //float monthlyDeployment = 1f / 30f;

            //Act
            string HighResult = metrics.GetDeploymentFrequencyRating(weeklyDeployment);

            //Assert
            Assert.AreEqual("High", HighResult);
        }

        [TestMethod]
        public void DeploymentFrequencyRatingMediumTest()
        {
            //Arrange
            DeploymentFrequency metrics = new DeploymentFrequency();
            //float dailyDeployment = 1f;
            //float weeklyDeployment = 1f / 7f;
            float monthlyDeployment = 1f / 30f;

            //Act
            string MediumResult = metrics.GetDeploymentFrequencyRating(monthlyDeployment);

            //Assert
            Assert.AreEqual("Medium", MediumResult);

        }

        [TestMethod]
        public void DeploymentFrequencyRatingLowTest()
        {
            //Arrange
            DeploymentFrequency metrics = new DeploymentFrequency();
            //float dailyDeployment = 1f;
            //float weeklyDeployment = 1f / 7f;
            float monthlyDeployment = 1f / 30f;

            //Act
            string LowResult = metrics.GetDeploymentFrequencyRating(monthlyDeployment - 0.01f);

            //Assert
            Assert.AreEqual("Low", LowResult);
        }

        [TestMethod]
        public void DeploymentFrequencyRatingZeroLowTest()
        {
            //Arrange
            DeploymentFrequency metrics = new DeploymentFrequency();
            //float dailyDeployment = 1f;
            //float weeklyDeployment = 1f / 7f;
            //float monthlyDeployment = 1f / 30f;

            //Act
            string LowResult = metrics.GetDeploymentFrequencyRating(0f);

            //Assert
            Assert.AreEqual("Low", LowResult);
        }

    }
}