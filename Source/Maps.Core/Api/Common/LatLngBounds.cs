namespace Proxoft.Maps.Core.Api
{
    public record LatLngBounds
    {
        public static readonly LatLngBounds Empty = new();

        public LatLng SouthWest { get; init; } = LatLng.None;

        public LatLng NorthEast { get; init; } = LatLng.None;

        public static LatLngBounds FromPosition(LatLng position)
            => new()
            {
                SouthWest = position,
                NorthEast = position
            };
    }
}
