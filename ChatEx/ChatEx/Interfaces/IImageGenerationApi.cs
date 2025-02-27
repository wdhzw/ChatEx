using ChatEx.Models.ReqModels;
using Refit;

namespace ChatEx.Interfaces
{
    public interface IImageGenerationApi
    {
        [Post("/v1/images/generations")]
        Task<ImageGenerationResponse> GenerateImageAsync([Body] ImageGenerationRequest request, [Header("Authorization")] string authorization);
    }
}
