namespace Proxoft.Maps.Google.Geocoding
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>", Scope = "member")]
    internal class GeocoderResponse
    {
        public string status { get; set; }

        public GeocoderResult[] results { get; set; }
    }
}
