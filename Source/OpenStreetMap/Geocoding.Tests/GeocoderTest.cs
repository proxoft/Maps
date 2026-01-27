using Proxoft.Maps.Core.Abstractions.Geocoding;
using Proxoft.Maps.OpenStreetMap.Common;
using Proxoft.Maps.OpenStreetMap.Geocoding.Parsing;
using Proxoft.Optional;

namespace Proxoft.Maps.OpenStreetMap.Geocoding.Tests;

public class GeocoderTest
{
    private readonly OsmGeocoder _geocoder;

    public GeocoderTest()
    {
        HttpClient http = new()
        {
            BaseAddress = new Uri("https://nominatim.openstreetmap.org/")
        };
        http.DefaultRequestHeaders.Referrer = new Uri("https://yourapplication.example/");
        http.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/135.0.0.0 Safari/537.36");

        _geocoder = new(
            http,
            new OpenStreetMapOptions(),
            new OsmDefaultResultParser()
        );
    }

    [Fact]
    public async Task GeocodeAddress1()
    {
        Either<ErrorStatus, Address> errorOrAddress = await _geocoder.Geocode("gercenova 5 bratislava");

        bool isSuccess = errorOrAddress
            .Map(a => a.Street == "Gercenova"
                    && a.City == "Bratislava"
                    && a.StreetNumber == "5")
            .Reduce(_ => false);

        Assert.True(isSuccess);
    }
}
