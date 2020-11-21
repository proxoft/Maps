namespace Proxoft.Maps.Google.Geocoding
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>", Scope = "member")]
    internal class GeocoderResult
    {
        public string[] types { get; set; }

        public string formatted_address {get; set; }

        public AddressComponent[] address_components { get; set; }

        public bool partial_match { get; set; }

        public string place_id { get; set; }

        public string[] postcode_localities { get; set; }

        public Geometry geometry { get; set; }
    }
}
