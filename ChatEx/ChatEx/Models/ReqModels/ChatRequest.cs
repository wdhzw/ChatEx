using System.Text.Json.Serialization;

namespace ChatEx.Models.ReqModels
{
    public class ChatRequest
    {
        public bool stream { get; set; } = true;
        public string model { get; set; }
        public int max_tokens { get; set; }
        public string[] stop { get; set; }
        public float temperature { get; set; }
        public float top_p { get; set; }
        public List<ChatMessage> messages { get; set; }

        [JsonPropertyName("response_format")]
        public ResponseFormat response_format { get; set; } = new ResponseFormat { Type = "text" };
    }

    public class ResponseFormat
    {
        [JsonPropertyName("type")]
        public string Type { get; set; } = "text";
    }

    public class ChatMessage
    {
        public string role { get; set; }
        public string content { get; set; }
    }

    public class StreamingResponse
    {
        public string id { get; set; }
        public string @object { get; set; }
        public long created { get; set; }
        public string model { get; set; }
        public List<Choice> choices { get; set; }
    }

    public class Choice
    {
        public int index { get; set; }
        public Delta delta { get; set; }
        public string finish_reason { get; set; }
    }

    public class Delta
    {
        public string content { get; set; }

        [JsonPropertyName("reasoning_content")]
        public string ReasoningContent { get; set; }
        public string role { get; set; }
    }
}
