namespace chatbot_kernel_memory.Util
{
    public class AppSettings
    {
        public string Endpoint { get; set; } = string.Empty;
        public string ApiKey { get; set; } = string.Empty;
        public List<Model> Models { get; set; } = new List<Model>();
    }

    public class Model
    {
        public string ModelId { get; set; } = string.Empty;

    }
}
