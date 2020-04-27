using Azure.Functions.Contracts;
using Azure.Functions.Utilities;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Azure.Functions.Services
{
    /// <summary>
    /// Scan the Image for Objects
    /// </summary>
    public class ScanImage : IScanImage
    {
        private readonly ILogger<ScanImage> _logger;
        private readonly ComputerVisionClient _client;
        private readonly IConfiguration _config;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="config">Configuration</param>
        /// <param name="logger">Logger</param>
        public ScanImage(IConfiguration config, ILogger<ScanImage> logger)
        {
            _logger = logger?? throw new ArgumentNullException(nameof(logger));
            _config = config ?? throw new ArgumentNullException(nameof(config));
            var endpoint = _config["VisionEndpoint"]?? throw new ArgumentNullException("VisionEndpoint");
            var accessToken = _config["VisionAccessKey"]?? throw new ArgumentNullException("VisionAccessKey");
            _client = new ComputerVisionClient(new ApiKeyServiceClientCredentials(accessToken)){ Endpoint = endpoint };
        }

        /// <summary>
        /// Detect Shapes
        /// </summary>
        /// <param name="imageStream">Image Stream</param>
        /// <returns>Results Received from the Computer Vision Service</returns>
        public async Task<DetectResult> DetectShapes(Stream imageStream)
        {
            imageStream.ThrowIfNull(nameof(imageStream));
            var result = await _client.DetectObjectsInStreamAsync(imageStream);
            return result;
        }
    }
}
