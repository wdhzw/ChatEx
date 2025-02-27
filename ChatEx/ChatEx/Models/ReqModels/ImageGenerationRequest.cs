namespace ChatEx.Models.ReqModels
{
    public class ImageGenerationRequest
    {
        public string Model { get; set; }
        public string Prompt { get; set; }
    }

    public class ImageGenerationResponse
    {
        public string ImageUrl { get; set; } // 假设 API 返回图片的 URL
        public byte[] ImageData { get; set; } // 用于存储二进制图片数据

        public List<ImageInfo> Images { get; set; }
        public TimingInfo Timings { get; set; }
        public long Seed { get; set; }
        public string SharedId { get; set; }
        public List<DataInfo> Data { get; set; }
        public long Created { get; set; }
    }

    public class ImageInfo
    {
        public string Url { get; set; }
    }

    public class TimingInfo
    {
        public double Inference { get; set; }
    }

    public class DataInfo
    {
        public string Url { get; set; }
    }
}
