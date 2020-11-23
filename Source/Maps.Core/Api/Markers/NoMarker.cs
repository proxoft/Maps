namespace Proxoft.Maps.Core.Api.Markers
{
    public sealed class NoMarker : IMarker
    {
        public static readonly NoMarker Instance = new();

        public void Dispose()
        {
        }
    }
}
