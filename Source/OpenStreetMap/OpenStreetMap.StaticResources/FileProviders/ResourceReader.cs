using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.FileProviders;

namespace Proxoft.Maps.OpenStreetMap.StaticResources.FileProviders;

internal static class ResourceReader
{
    private static readonly Assembly _assembly;
    private static readonly string _version;

    static ResourceReader()
    {
        _assembly = typeof(ResourceFileProvider).Assembly;

        string? version = _assembly.GetName().Version?.ToString();
        if(version is not null)
        {
            int i = version.LastIndexOf('.');
            version = version.Substring(0, i);
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
        DateTimeOffset created = _assembly.BuildTime();

        return resources
            .Select(resourceName =>
            {
                string content = resourceName.ReadContentFromDll();

                int nameStartIndex = resourceName.IndexOf("Assets") + "Assets".Length + 1;
                string name = resourceName[nameStartIndex..].Versioned(_version);

                string versionedContent = content.Replace("{version}", _version);
                return new AssetFileInfo(name, versionedContent, created);
            });
    }

    private static string ReadContentFromDll(this string assetName)
    {
        using Stream? stream = _assembly.GetManifestResourceStream(assetName);
        if(stream is null)
        {
            return "";
        }

        using StreamReader reader = new(stream);
        string content = reader.ReadToEnd();
        return content;
    }

    private static string Versioned(this string assetName, string version)
    {
        int index = assetName.LastIndexOf('.');

        return $"{assetName[0..index]}_{version}.{assetName[(index + 1)..]}";
    }
}
