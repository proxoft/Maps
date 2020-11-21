namespace Proxoft.Maps.Google.Common
{
    public class GoogleApiConfiguration
    {
        public GoogleApiConfiguration(string apiKey, string language, string region)
        {
            this.ApiKey = apiKey;
            this.Language = language;
            this.Region = region;
        }

        public string ApiKey { get; }
        public string Language { get; }
        public string Region { get; }
    }
}
