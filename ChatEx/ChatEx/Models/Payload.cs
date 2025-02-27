namespace ChatEx.Models
{
    // 定义请求负载类 
    public class Payload
    {
        public string model { get; set; }
        public List<Message> messages { get; set; }
    }

    // 定义消息项类 
    public class Message
    {
        public string role { get; set; }
        public string content { get; set; }
    }
}
