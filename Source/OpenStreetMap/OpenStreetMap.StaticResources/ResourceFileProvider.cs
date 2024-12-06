using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;

namespace Proxoft.Maps.OpenStreetMap.StaticResources;

//public class ResourceFileProvider : IFileProvider
//{
//    private const string _assemblyPath = "OpenStreetMap";
//    private readonly ManifestEmbeddedFileProvider _provider = new (typeof(ResourceFileProvider).Assembly);

//    public IDirectoryContents GetDirectoryContents(string subpath)
//    {
//        var result = _provider.GetDirectoryContents(_assemblyPath + subpath);
//        return result;
//    }

//    public IFileInfo GetFileInfo(string subpath)
//    {
//        var result = _provider.GetFileInfo(_assemblyPath + subpath);
//        return result;
//    }

//    public IChangeToken Watch(string filter)
//    {
//        var result = _provider.Watch(_assemblyPath + filter);
//        return result;
//    }
//}
