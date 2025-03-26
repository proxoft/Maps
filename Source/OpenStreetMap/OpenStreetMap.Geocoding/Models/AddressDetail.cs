
namespace Proxoft.Maps.OpenStreetMap.Geocoding.Models;

[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>", Scope = "member")]
public class AddressDetail
{
    public string? country { get; set; }
    public string? country_code { get; set; }

    public string? state { get; set; }
    public string? region { get; set; }
    public string? state_district { get; set; }
    public string? county { get; set; }

    public string? municipality { get; set; }
    public string? city { get; set; }
    
    public string? town { get; set; }
    public string? village { get; set; }

    public string? city_district { get; set; }
    public string? district { get; set; }
    public string? borough { get; set; }
    public string? suburb { get; set; }
    public string? subdivision { get; set; }

    public string? road { get; set; }
    public string? house_number { get; set; }
    public string? house_name { get; set; }

    public string? postcode { get; set; }
}
