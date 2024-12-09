using System;
using System.IO;
using System.Text;
using Microsoft.Extensions.FileProviders;

namespace Proxoft.Maps.OpenStreetMap.StaticResources.FileProviders;

internal class AssetFileInfo(
    string name,
    byte[] content,
    DateTimeOffset lastModified) : IFileInfo
{
    private readonly byte[] _content = content;

    public bool Exists => true;

    public long Length => _content.Length;

    public string? PhysicalPath => null;

    public string Name { get; } = name;

    public DateTimeOffset LastModified { get; } = lastModified;

    public bool IsDirectory => false;

    public Stream CreateReadStream()
    {
        return new MemoryStream(_content);
    }
}
