namespace ChatEx.Models
{
    public class AnwserContent
    {
        public string id { get; set; }
        public string _object { get; set; }
        public int created { get; set; }
        public string model { get; set; }
        public Choice[] choices { get; set; }
        public Usage usage { get; set; }
        public string system_fingerprint { get; set; }
    }

    public class Usage
    {
        public int prompt_tokens { get; set; }
        public int completion_tokens { get; set; }
        public int total_tokens { get; set; }
    }

    public class Choice
    {
        public int index { get; set; }
        public Messages message { get; set; }
        public string finish_reason { get; set; }
    }

    public class Messages
    {
        public string role { get; set; }
        public string content { get; set; }
        public string reasoning_content { get; set; }
    }
}
