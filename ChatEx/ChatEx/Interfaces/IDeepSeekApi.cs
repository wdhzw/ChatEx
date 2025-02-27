using ChatEx.Models;
using Refit;

namespace ChatEx.Interfaces
{
    public interface IDeepSeekApi
    {
        [Post("/v1/chat/completions")]
        Task<HttpResponseMessage> GetChatCompletions([Body] Payload payload);
    }
}
