using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using System.IO;
using System.Threading.Tasks;

namespace Azure.Functions.Contracts
{
    public interface IScanImage
    {
        Task<DetectResult> DetectShapes(Stream imageStream);
    }
}
