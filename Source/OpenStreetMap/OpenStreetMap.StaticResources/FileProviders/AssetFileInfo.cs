using System;
using System.IO;
using System.Text;
using Microsoft.Extensions.FileProviders;

namespace Proxoft.Maps.OpenStreetMap.StaticResources.FileProviders;

internal class AssetFileInfo : IFileInfo
{
    private readonly string _content;

    public AssetFileInfo(
        string name,
        string content,
        DateTimeOffset lastModified)
    {
        _content = content;
        this.Name = name;
        this.LastModified = lastModified;
    }

    public bool Exists => true;

    public long Length => _content.Length;

    public string? PhysicalPath => null;

    public string Name { get; }

    public DateTimeOffset LastModified { get; }

    public bool IsDirectory => false;

    public Stream CreateReadStream()
    {
        return new MemoryStream(Encoding.UTF8.GetBytes(_content));
    }
}
