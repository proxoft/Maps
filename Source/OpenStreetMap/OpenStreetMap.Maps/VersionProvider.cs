namespace Proxoft.Maps.OpenStreetMap.Maps;

internal static class VersionProvider
{
    public static string GetScriptsVersion()
    {
        string v = typeof(VersionProvider).Assembly.GetName().Version?.ToString() ?? "0.0.0.0";
        int i = v.LastIndexOf('.');
        return v[..i];
    }
}
