using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.Extensions.FileProviders;

namespace Proxoft.Maps.OpenStreetMap.StaticResources.FileProviders;

internal static class ResourceReader
{
    private static readonly Assembly _assembly;
    private static readonly string _version;

    private static readonly DateTimeOffset _leafletDate = new DateTime(2024, 12, 09);
    private static readonly DateTimeOffset _mapScriptsCreated;

    static ResourceReader()
    {
        _assembly = typeof(ResourceFileProvider).Assembly;

        _mapScriptsCreated = _assembly.BuildTime();
        string? version = _assembly.GetName().Version?.ToString();
        if (version is not null)
        {
            int i = version.LastIndexOf('.');
            version = version[..i];
        }
        else
        {
            version = "0.0.1";
        }

        _version = version;
    }

    public static IEnumerable<IFileInfo> ReadAssets()
    {
        string[] resources = _assembly.GetManifestResourceNames();
        return resources
            .Select(resourceName =>
            {
                if (resourceName.IsLeaflet())
                {
                    return resourceName.CreateLeafletAsset();
                }
                else
                {
                    return resourceName.CreateMapScriptAsset();
                }
            });
    }

    private static bool IsLeaflet(this string resourceName)
    {
        return resourceName.Contains("Leaflet");
    }

    private static AssetFileInfo CreateLeafletAsset(this string resourceName)
    {
        byte[] content = resourceName.ReadContentFromDll();
        string fileName = resourceName.ExtractFileName();
        return new AssetFileInfo(fileName, content, _leafletDate);
    }

    private static AssetFileInfo CreateMapScriptAsset(this string resourceName)
    {
        string content = resourceName.ReadContentFromDllAsString();

        string fileName = resourceName.ExtractFileName(version: _version);
        string versionedContent = content.Replace("{version}", _version);

        return new AssetFileInfo(fileName, Encoding.UTF8.GetBytes(versionedContent), _mapScriptsCreated);
    }

    private static byte[] ReadContentFromDll(this string assetName)
    {
        using Stream? stream = _assembly.GetManifestResourceStream(assetName);
        if (stream is null)
        {
            return [];
        }

        byte[] content = new byte[stream.Length];
        _ = stream.Read(content, 0, (int)stream.Length);

        return content;
    }

    private static string ReadContentFromDllAsString(this string assetName)
    {
        using Stream? stream = _assembly.GetManifestResourceStream(assetName);
        if (stream is null)
        {
            return "";
        }

        using StreamReader reader = new(stream);
        string content = reader.ReadToEnd();
        return content;
    }

    private static string ExtractFileName(this string resourceName, string version = "")
    {
        string[] split = resourceName.Split('.');

        string[] nameElements = [
            split[^2],
            version,
            split[^1]
        ];

        string name = nameElements
            .Where(e => !string.IsNullOrEmpty(e))
            .JoinToString(".");

        return name;
    }

    private static string JoinToString<T>(this IEnumerable<T> values, string separator)
    {
        return string.Join(separator, values);
    }
}
