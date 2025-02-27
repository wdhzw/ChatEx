using ChatEx.Helpers;
using ChatEx.Interfaces;
using ChatEx.Models.ReqModels;
using static System.Net.WebRequestMethods;

namespace ChatEx.ViewModels
{
    public class ImageViewModel
    {
        private readonly IImageGenerationApi _imageGenerationApi;
        public event EventHandler UpdateUIEventHandler;
        public ImageViewModel(IImageGenerationApi imageGenerationApi)
        {
            _imageGenerationApi = imageGenerationApi;
        }

        public byte[] ImageData { get; set; } = new byte[0];
        
        public string Describe {  get; set; } = string.Empty;

        public string NoticeInfo { get; set; } = string.Empty;

        public void Declare()
        {
            ImageData = new byte[0];
            Describe = string.Empty;
        }

        public bool IsCreating { get; set; } = false;

        public async Task GenerateImageAsync()
        {
            if (IsCreating)
                return;

            NoticeInfo = "正在创建，请稍后..";
            IsCreating = true;
            UpdateUIEventHandler?.Invoke(this, EventArgs.Empty);

            var request = new ImageGenerationRequest
            {
                Model = "stabilityai/stable-diffusion-3-5-large",
                Prompt = Describe
            };

            var response = await RefitClientHelper.WrapAsync(_imageGenerationApi.GenerateImageAsync(request, "Bearer sk-bsjtdimuhfcbyyqhxkqlandpkjbmrdkluycjfygbjgjccqpf"));

            var url = response.Data.Images[0].Url;

            ImageData = await (new HttpClient()).GetByteArrayAsync(url);
            IsCreating = false;
            NoticeInfo = string.Empty;
            UpdateUIEventHandler?.Invoke(this, EventArgs.Empty);
        }

        public byte[] ConvertToJpg(byte[] pngData)
        {
            using var ms = new System.IO.MemoryStream(pngData);
            using var image = System.Drawing.Image.FromStream(ms);
            using var jpgMs = new System.IO.MemoryStream();
            image.Save(jpgMs, System.Drawing.Imaging.ImageFormat.Jpeg);
            return jpgMs.ToArray();
        }

        public async Task DonwLoadImgAsync()
        {
            string imageUrl = $@"https://sc-maas.oss-cn-shanghai.aliyuncs.com/outputs/c2036d77-196f-4fd4-8b39-7365975955ca_0.jpeg?OSSAccessKeyId=LTAI5tQnPSzwAnR8NmMzoQq4&Expires=1740403789&Signature=BOQpGLVz2fcogB%2B7CgvJGb8xFL4%3D";
            HttpClient client = new HttpClient();

            // 下载图片数据
            ImageData = await client.GetByteArrayAsync(imageUrl);
        }
    }
}
