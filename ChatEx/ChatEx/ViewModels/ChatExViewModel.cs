﻿using ChatEx.Interfaces;
using ChatEx.Models.ReqModels;
using ChatEx.Services;
using System.Text.RegularExpressions;

namespace ChatEx.ViewModels
{
    public class ChatExViewModel
    {
        private readonly IChatApi _chatApi;
        private readonly StreamParserService _streamParser;
        public ChatExViewModel(IChatApi chatApi,
            StreamParserService streamParser)
        {
            _chatApi = chatApi;
            _streamParser = streamParser;
            Declare();
        }

        public string Ques { get; set; } = string.Empty;

        public List<HistMessage> HistoryMeesageList { get; set; } = new List<HistMessage>();

        public string ThinkingProcess { get; set; }  = string.Empty;
        public string OutPutContent {  get; set; } = string.Empty;

        public bool isStreaming {  get; set; } = false;

        public event EventHandler UpdateUIEventHandler;

        public void Declare()
        {
            Ques = string.Empty;
            HistoryMeesageList.Clear();
            ThinkingProcess = string.Empty;
            OutPutContent = string.Empty;
            isStreaming = false;
        }


        public void TestList()
        {
            for (int i = 0; i < 20; i++)
            {
                HistoryMeesageList.Add(new HistMessage()
                {
                    Title = $"问题{i}",
                    ThinkingProcess = $"过程{i}",
                    OutPutContent = $"结果{i}"
                });
            }
            ThinkingProcess = "这是过程，";
            OutPutContent = "这是答案";
            UpdateUIEventHandler?.Invoke(this, EventArgs.Empty);
        }

        public async Task SendQuestion()
        {
            if (isStreaming || string.IsNullOrWhiteSpace(Ques)) return;

            try
            {
                isStreaming = true;
                
                // 保存当前问题
                string currentQuestion = Ques;
                
                // 清空思考过程和输出内容
                ThinkingProcess = "思考中...";
                OutPutContent = string.Empty;
                
                // 通知 UI 更新
                UpdateUIEventHandler?.Invoke(this, EventArgs.Empty);
                
                // 调用 API 获取回答
                var response = await _chatApi.GetChatResponseAsync(new ChatRequest 
                { 
                    Question = currentQuestion 
                });
                
                // 处理回答
                if (response != null)
                {
                    // 更新思考过程和输出内容
                    ThinkingProcess = response.ThinkingProcess ?? string.Empty;
                    OutPutContent = response.Answer ?? string.Empty;
                    
                    // 将问题和回答添加到历史消息列表
                    HistoryMeesageList.Add(new HistMessage
                    {
                        Title = currentQuestion,
                        ThinkingProcess = ThinkingProcess,
                        OutPutContent = OutPutContent
                    });
                    
                    // 清空当前问题
                    Ques = string.Empty;
                }
            }
            catch (Exception ex)
            {
                // 处理异常
                ThinkingProcess = string.Empty;
                OutPutContent = $"发生错误: {ex.Message}";
            }
            finally
            {
                isStreaming = false;
                
                // 通知 UI 更新
                UpdateUIEventHandler?.Invoke(this, EventArgs.Empty);
            }
        }


        public async Task ApiTest(string ques)
        {
            var request = new ChatRequest
            {
                model = "Pro/deepseek-ai/DeepSeek-R1",
                max_tokens = 8000,
                stop = new[] { "20" },
                temperature = 0.3f,
                top_p = 0.85f,
                messages = new List<ChatMessage>
                 {
                     new ChatMessage()
                     {
                         role = "user",
                         content = ques
                     }
                 }
            };

            var response = await _chatApi.CreateChatCompletionAsync(
                request,
                "Bearer sk-bsjtdimuhfcbyyqhxkqlandpkjbmrdkluycjfygbjgjccqpf");

            await using var stream = await response.Content.ReadAsStreamAsync();

            await foreach (var chunk in _streamParser.ParseEventStreamAsync(stream))
            {
                var content = chunk?.choices?.FirstOrDefault()?.delta?.ReasoningContent;
                var contentResult = chunk?.choices?.FirstOrDefault()?.delta?.content;

                if (!string.IsNullOrEmpty(content))
                {
                    //Console.Write(content);
                    //ThinkingProcess += content;
                    //UpdateUIEventHandler?.Invoke(this, EventArgs.Empty);
                    //await Task.Delay(50); // 控制更新频率
                }
                if (!string.IsNullOrEmpty(contentResult))
                {
                    Console.WriteLine(contentResult);
                    //var cleanedText = CleanText(content);
                    //OutPutContent += contentResult;
                    //UpdateUIEventHandler?.Invoke(this, EventArgs.Empty);
                    //await Task.Delay(50); // 控制更新频率
                }
            }

            //isStreaming = false;
            //HistoryMeesageList.Add(new HistMessage()
            //{
            //    Title = Ques,
            //    ThinkingProcess = ThinkingProcess,
            //    OutPutContent = OutPutContent,

            //});
            //Ques = string.Empty;
            //ThinkingProcess = string.Empty;
            //OutPutContent = string.Empty;
        }
    }

    public class HistMessage
    {
        public string Title { get; set; }

        public string ThinkingProcess { get; set; }
        public string OutPutContent { get; set; }
    }
}
