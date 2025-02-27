using ChatEx.Interfaces;
using ChatEx.Models;
using ChatEx.Models.ReqModels;
using ChatEx.Services;
using ChatEx.ViewModels;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ChatEx.WebApi
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatController : ControllerBase
    {
        private readonly IChatApi _chatApi;
        private readonly StreamParserService _streamParser;
        private readonly DeepSeekService _deepSeekService;

        public ChatController(
            IChatApi chatApi,
            StreamParserService streamParser,
            DeepSeekService deepSeekService)
        {
            _chatApi = chatApi;
            _streamParser = streamParser;
            _deepSeekService = deepSeekService;
        }

        [HttpGet("Test")]
        public ApiInfo Test()
        {
            var result = _deepSeekService.GetModels();

            return result;
        }

        /// <summary>
        /// 流式对话
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost("StreamDialogue")]
        public async Task StreamDialogue(QuesReq req)
        {
            // 开启同步 I/O stream
            SyncInOutStream();

            // 设置响应内容类型为文本
            Response.ContentType = "text/plain";
            try
            {
                var body = Response.Body;
                await _deepSeekService.PostToDeepSeekAsync(req, body);
            }
            catch (Exception ex)
            {


            }
        }

        /// <summary>
        /// 文件上传时同步读取Request.Body流
        /// </summary>
        private void SyncInOutStream()
        {
            var syncIOFeature = HttpContext.Features.Get<IHttpBodyControlFeature>();
            if (syncIOFeature != null)
            {
                syncIOFeature.AllowSynchronousIO = true;
            }
        }

        /// <summary>
        /// 流式翻译
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost("StreamTranslate")]
        public async Task Translate(QuesReq req)
        {

        }

        /// <summary>
        /// 流式内容优化
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost("StreamContentOptimize")]
        public async Task StreamContentOptimize(QuesReq req)
        {

        }
    }

    public class QuesReq
    {
        public string User { get; set; }
        public DateTime QuesTime { get; set; }
        public string Mode { get; set; }
        public string Ques { get; set; }
        public bool IsFirst { get; set; }
    }

    public class ResponseMessageStream
    {
        public int Id { get; set; }

        public string ThinkProgress { get; set; }
        
        public string ResultContent { get; set; }
    }
}
