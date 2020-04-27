using Azure.Functions.Contracts;
using Azure.Functions.Utilities;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Azure.Functions.App
{
    /// <summary>
    /// Blob Trigger
    /// </summary>
    public class BlobFunction
    {
        private readonly IScanImage _scanImageService;
        private readonly ILogger<BlobFunction> _logger;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="scanImage">Scan Image Service</param>
        /// <param name="logger">Logger</param>
        public BlobFunction(IScanImage scanImage, ILogger<BlobFunction> logger )
        {
            _scanImageService = scanImage ?? throw new ArgumentNullException(nameof(scanImage)) ;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Run the Trigger as soon as Blob has been pushed to the container
        /// </summary>
        /// <param name="imageBlob">Blob Stream</param>
        /// <param name="name">Name of the Blob pushed to the container</param>
        /// <param name="extension">Extension Of the Blob</param>
        /// <returns>None</returns>

        [FunctionName("ImageBlobFunction")]
        public async Task Run([BlobTrigger("images/{name}.{extension}", Connection = "AzureWebJobsStorage")]Stream imageBlob, string name, string extension )
        {
            imageBlob.ThrowIfNull(nameof(imageBlob));
            name.ThrowIfIsNullOrWhitespace(nameof(name));
            extension.ThrowIfIsNullOrWhitespace(nameof(extension));

            if (!extension.Equals("jpg",StringComparison.InvariantCultureIgnoreCase) &&
                !extension.Equals("jpeg", StringComparison.InvariantCultureIgnoreCase) &&
                !extension.Equals("png", StringComparison.InvariantCultureIgnoreCase) &&
                !extension.Equals("gif", StringComparison.InvariantCultureIgnoreCase))
                throw new ArgumentNullException(nameof(extension));

            var results = await _scanImageService.DetectShapes(imageBlob);
        }
    }
}
