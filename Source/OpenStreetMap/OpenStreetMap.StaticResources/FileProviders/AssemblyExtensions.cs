using System;
using System.IO;
using System.Reflection;

namespace Proxoft.Maps.OpenStreetMap.StaticResources.FileProviders;

internal static class AssemblyExtensions
{
    public static DateTimeOffset BuildTime(this Assembly assembly)
    {
        return File.GetLastWriteTime(assembly.Location);
    }
}
