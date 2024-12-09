using System.Linq;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;

namespace Proxoft.Maps.OpenStreetMap.StaticResources.FileProviders;

internal class ResourceFileProvider : IFileProvider, IContentTypeProvider
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
        string name = subpath.StartsWith('/')
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

    public bool TryGetContentType(string subpath, out string contentType)
    {
        IFileInfo fileInfo = this.GetFileInfo(subpath);
        if(fileInfo is NotFoundFileInfo)
        {
            contentType = "";
            return false;
        }

        string extension = fileInfo.Name.Split('.').Last().ToLower();
        contentType = extension switch
        {
            "js" => "text/javascript",
            "css" => "text/css",
            "png" => "image/png",
            _ => "text/plain"
        };
        return true;
    }
}
