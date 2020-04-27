using AutoFixture;
using Azure.Functions.Contracts;
using FluentAssertions;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Azure.Functions.App.Unit.Tests
{
    [TestFixture]
    public class BlobFunctionTests:TestBase
    {

        private BlobFunction _sut;
        private Mock<IScanImage> _mockScanImageService;
        private Mock<ILogger<BlobFunction>> _mockLogger;

        [SetUp]
        public void Setup()
        {
            _mockScanImageService = _mockRepository.Create<IScanImage>();
            _mockLogger = _mockRepository.Create<ILogger<BlobFunction>>();
            _sut = new BlobFunction(_mockScanImageService.Object, _mockLogger.Object);
        }

        [Test]
        public void Run_With_Null_Image_Throws_Exception()
        {
            // Arrange
            var name = _fixture.Create<string>();
            var extension = _fixture.Create<string>();

            // Act
            Func<Task> act = async () => { await _sut.Run(null, name, extension); };

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void Run_With_Null_Name_Throws_Exception()
        {
            // Arrange
            var stream = new MemoryStream();
            var extension = _fixture.Create<string>();

            // Act
            Func<Task> act = async () => { await _sut.Run(stream, null, extension); };

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void Run_With_Null_Extension_Throws_Exception()
        {
            // Arrange
            var stream = new MemoryStream();
            var name = _fixture.Create<string>();

            // Act
            Func<Task> act = async () => { await _sut.Run(stream, name, null); };

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void Run_With_InValid_Extension_Throws_Exception()
        {
            // Arrange
            var stream = new MemoryStream();
            var name = _fixture.Create<string>();
            var extension = "test";

            // Act
            Func<Task> act = async () => { await _sut.Run(stream, name, extension); };

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void Run_With_Exception_While_Scanning_Image_Throws_Exception()
        {
            // Arrange
            var stream = new MemoryStream();
            var name = _fixture.Create<string>();
            var extension = "gif";
            _mockScanImageService.Setup(x => x.DetectShapes(It.IsAny<Stream>())).Throws(new SystemException());

            // Act
            Func<Task> act = async () => { await _sut.Run(stream, name, extension); };

            // Assert
            act.Should().Throw<SystemException>();
        }

        [Test]
        public async Task Run_With_Valid_Request_Receives_Valid_Response()
        {
            // Arrange
            var stream = new MemoryStream();
            var name = _fixture.Create<string>();
            var extension = "jpeg";
            var detectResult = _fixture.Create<DetectResult>();
            _mockScanImageService.Setup(x => x.DetectShapes(It.IsAny<Stream>())).ReturnsAsync(detectResult);

            // Act
            await _sut.Run(stream, name, extension);

            // Assert
        }
    }
}
