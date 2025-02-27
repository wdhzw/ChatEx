namespace ChatEx.Models
{
    public class ApiInfo
    {
        public List<DialogueModel> DialogueModel { get; set; }
        public List<ImgModel> ImgModel { get; set; }
    }
    public class DialogueModel
    {
        public string Name { get; set; }
        public double Price { get; set; }
        public string Url { get; set; }
    }

    public class ImgModel
    {
        public string Name { get; set; }
        public double Price { get; set; }
        public string Url { get; set; }
    }
}
