namespace RmlEditorWeb
{
    public class AppSettings
    {
        public ApiUrls ApiUrls { get; set; }
        public PreloadedValues PreloadedValues { get; set; }
    }

    public class ApiUrls
    {
        public string BaseUrl { get; set; }
        public string AuthUrl { get; set; }
    }

    public class PreloadedValues
    {
        public string BuildVersion { get; set; }
        public string DefaultUsername { get; set; }
        public int MaxRetries { get; set; }
        public bool EnableFeatureX { get; set; }
    }

}
