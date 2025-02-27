using ChatEx.Models.ReqModels;
using Refit;

namespace ChatEx.Interfaces
{
    public interface IChatApi
    {
        [Post("/v1/chat/completions")]
        Task<HttpResponseMessage> CreateChatCompletionAsync([Body] ChatRequest request,[Header("Authorization")] string authorization);
    }
}
