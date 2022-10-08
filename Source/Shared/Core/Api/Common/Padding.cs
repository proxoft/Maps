namespace Proxoft.Maps.Core.Api
{
    public record Padding
    {
        public static readonly Padding Zero = new ();

        public int Top { get; init; }
        public int Left { get; init; }
        public int Bottom { get; init; }
        public int Right { get; init; }
    }
}
