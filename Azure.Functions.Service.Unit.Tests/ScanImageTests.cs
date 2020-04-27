using FluentAssertions;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Azure.Functions.Services.Unit.Tests
{
    [TestFixture]
    public class ScanImageTests:TestBase
    {
        private Mock<IConfiguration> _mockConfiguration;
        private Mock<ILogger<ScanImage>> _mockLogger;
        private ScanImage _sut;
        private readonly string endpoint = "https://scanimage.cognitiveservices.azure.com/";
        private readonly string accessKey = Guid.NewGuid().ToString();

        [SetUp]
        public void Setup()
        {
            _mockConfiguration = _mockRepository.Create<IConfiguration>();
            _mockLogger = _mockRepository.Create<ILogger<ScanImage>>();
        }

        [Test]
        public void ScanImage_With_Null_Configuration_Raises_Exception()
        {
            Action action = () => {
                var scanImage = new ScanImage(null, _mockLogger.Object);
            };

            action.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void ScanImage_With_Null_Logger_Raises_Exception()
        {
            Action action = () => {
                var scanImage = new ScanImage(_mockConfiguration.Object, null);
            };

            action.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void ScanImage_With_Null_Vision_Endpoint_Raises_Exception()
        {
           var stream = new MemoryStream();
           var endpoint = default(string);
           _mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "VisionEndpoint")]).Returns(endpoint);

            Action action = () => { _sut = new ScanImage(_mockConfiguration.Object, _mockLogger.Object); };

            action.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void ScanImage_With_Null_AccessKey_Raises_Exception()
        {
            var accessKey = default(string);
            var stream = new MemoryStream();

            _mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "VisionEndpoint")]).Returns(endpoint);
            _mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "VisionAccessKey")]).Returns(accessKey);
            Action action = () => { _sut = new ScanImage(_mockConfiguration.Object, _mockLogger.Object); };

            action.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void DetectShapes_With_Null_Stream_Raises_Exception()
        {
            _mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "VisionEndpoint")]).Returns(endpoint);
            _mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "VisionAccessKey")]).Returns(accessKey);
            _sut = new ScanImage(_mockConfiguration.Object, _mockLogger.Object);
            Func<Task<DetectResult>> method = async () => await _sut.DetectShapes(null);
            method.Should().Throw<ArgumentNullException>();
        }
    }
}
