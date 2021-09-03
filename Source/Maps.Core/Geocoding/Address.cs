namespace Proxoft.Maps.Core.Geocoding
{
    public record Address
    {
        public string Location { get; set; }

        public string Country { get; set; }

        public string City { get; set; }

        public string Street { get; set; }

        public string RegisterNumber { get; set; }

        public string StreetNumber { get; set; }

        public string Zip { get; set; }

        public LatLng LatLng { get; set; }
    }
}
