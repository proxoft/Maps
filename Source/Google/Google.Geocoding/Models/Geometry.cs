namespace Proxoft.Maps.Google.Geocoding
{
    //location_type:
    //ROOFTOP indicates that the returned result reflects a precise geocode.
    //RANGE_INTERPOLATED indicates that the returned result reflects an approximation (usually on a road) interpolated between two precise points(such as intersections). Interpolated results are generally returned when rooftop geocodes are unavailable for a street address.
    //GEOMETRIC_CENTER indicates that the returned result is the geometric center of a result such as a polyline(for example, a street) or polygon(region).
    //APPROXIMATE
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>", Scope = "member")]
    internal class Geometry
    {
        public LatLng location { get; set; }
        public string location_type { get; set; } // GeocoderLocationType

        public LatLngBounds viewport { get; set; }

        public LatLngBounds bounds { get; set; }
    }
}
