using System.Linq;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;

namespace Proxoft.Maps.OpenStreetMap.StaticResources.FileProviders;

internal class ResourceFileProvider : IFileProvider
{
    private readonly IFileInfo[] _fileInfos;

    private ResourceFileProvider(IFileInfo[] fileInfos)
    {
        _fileInfos = fileInfos;
    }

    public static ResourceFileProvider Initialize()
    {
        IFileInfo[] fileInfos = ResourceReader.ReadAssets().ToArray();
        return new ResourceFileProvider(fileInfos);
    }

    public IFileInfo GetFileInfo(string subpath)
    {
        string name = subpath.StartsWith("/")
            ? subpath[1..]
            : subpath;

        IFileInfo fileInfo = _fileInfos.FirstOrDefault(f => f.Name == name) ?? new NotFoundFileInfo(subpath);
        return fileInfo;
    }

    public IDirectoryContents GetDirectoryContents(string subpath)
    {
        return string.IsNullOrWhiteSpace(subpath)
            ? new AssetDirectoryContent(_fileInfos)
            : new NotFoundDirectoryContents();
    }

    public IChangeToken Watch(string filter)
    {
        return NullChangeToken.Singleton;
    }
}
