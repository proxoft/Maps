namespace Proxoft.Maps.OpenStreetMap.Hosting.Builders
{
    public interface IOpenStreetMapGeocodingBuilder
    {
        IOpenStreetMapApiBuilder AddGeocoder(string language);
    }

    public interface IOpenStreetMapApiBuilder
    {
        void Register();
    }
}
