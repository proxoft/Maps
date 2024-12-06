﻿using System.Collections;
using System.Collections.Generic;
using Microsoft.Extensions.FileProviders;

namespace Proxoft.Maps.OpenStreetMap.StaticResources.FileProviders;

internal class AssetDirectoryContent : IDirectoryContents
{
    private readonly IFileInfo[] _fileInfos;

    public AssetDirectoryContent(IFileInfo[] fileInfos)
    {
        _fileInfos = fileInfos;
    }

    public bool Exists => true;

    public IEnumerator<IFileInfo> GetEnumerator()
    {
        return ((IEnumerable<IFileInfo>)_fileInfos).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return _fileInfos.GetEnumerator();
    }
}