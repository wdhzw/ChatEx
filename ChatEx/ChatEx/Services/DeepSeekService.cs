using System.IO;
using ChatEx.Interfaces;
using ChatEx.Models;
using ChatEx.Models.ReqModels;
using ChatEx.WebApi;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using static MudBlazor.Colors;

namespace ChatEx.Services
{
    public class DeepSeekService
    {
        private readonly ApiInfo _apiInfo;
        private readonly IChatApi _chatApi;
        private readonly IConfiguration _config;
        private readonly StreamParserService _streamParser;
        public DeepSeekService(
            IOptions<ApiInfo> apiInfo,
            IChatApi chatApi,
            IConfiguration config,
            StreamParserService streamParser)
        {
            _config = config;
            _apiInfo = apiInfo.Value;
            _chatApi = chatApi;
            _streamParser = streamParser;
        }

        public ApiInfo GetModels()
        {
            var apiKey = _config["Bear"];
            var info = _apiInfo;
            return info;
        }

        /// <summary>
        /// 组装请求参数
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        private ChatRequest CreateDsReq(QuesReq req)
        {
            var request = new ChatRequest
            {
                //model = "Pro/deepseek-ai/DeepSeek-R1",
                model = req.Mode,
                max_tokens = 8000,
                stop = new[] { "20" },
                temperature = 0.3f,
                top_p = 0.85f,
                messages = new List<ChatMessage>
                {
                    new ChatMessage()
                    {
                        role = "user",
                        content = req.Ques
                    }
                }
            };

            if (!req.IsFirst)
            {
                request.messages.Add(new ChatMessage
                {
                    role = "assistant",
                    content = req.Ques
                });
            }
            return request;
        }

        /// <summary>
        /// 请求DeepSeek接口
        /// </summary>
        /// <param name="req"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        public async Task PostToDeepSeekAsync(QuesReq req, Stream body)
        {
            string ques = req.Ques;
            string thinkProgress = string.Empty;
            string result = string.Empty;
            
            var request = CreateDsReq(req);

            var apiKey = _config["Bear"];
            if (string.IsNullOrEmpty(apiKey))
            {
                Console.WriteLine("获取配置文件出错！！！！！");
                return;
            }

            var response = await _chatApi.CreateChatCompletionAsync(request, apiKey);
            await using var stream = await response.Content.ReadAsStreamAsync();
            await ReadAndWriteStreamAsync(stream, body);

            Console.WriteLine($"问题：{ques}");
            Console.WriteLine($"过程：{thinkProgress}");
            Console.WriteLine($"答案：{result}");
        }

        /// <summary>
        /// 同时读写流式传输
        /// </summary>
        /// <param name="readStream"></param>
        /// <param name="writeStream"></param>
        /// <returns></returns>
        public async Task ReadAndWriteStreamAsync(Stream readStream, Stream writeStream)
        {

            using (var writer = new StreamWriter(writeStream))
            {
                await foreach (var chunk in _streamParser.ParseEventStreamAsync(readStream))
                {
                    var content = chunk?.choices?.FirstOrDefault()?.delta?.ReasoningContent;
                    var contentResult = chunk?.choices?.FirstOrDefault()?.delta?.content;

                    string packet = string.Empty;
                    var data = new ResponseMessageStream();

                    if (!string.IsNullOrEmpty(content))
                    {
                        data.ThinkProgress = content;
                    }

                    if (!string.IsNullOrEmpty(contentResult))
                    {
                        data.ResultContent = contentResult;
                    }
                    var json = JsonConvert.SerializeObject(data); // 将对象序列化为 JSON
                    packet = $"data: {json}\n"; // 包装为 "data: JSON" 格式

                    await writer.WriteAsync(packet);
                    await writer.FlushAsync(); // 刷新缓冲区，确保数据立即发送到客户端
                }
            }

        }
    }
}
