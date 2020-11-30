namespace Proxoft.Maps.OpenStreetMap.Geocoding.Models
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>", Scope = "member")]
    public class Result
    {
        public AddressDetail address { get; set; }
        public string lat { get; set; }
        public string lon { get; set; }
    }
}
