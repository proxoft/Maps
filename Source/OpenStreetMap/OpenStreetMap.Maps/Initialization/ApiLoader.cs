﻿using System;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace Proxoft.Maps.OpenStreetMap.Maps.Initialization;

internal class ApiLoader : IAsyncDisposable
{
    private readonly Lazy<Task<IJSObjectReference>> _moduleTask;

    private readonly DotNetObjectReference<ApiLoader> _netObjRef;
    private TaskCompletionSource<LoadResponse>? _taskCompletionSource;

    public ApiLoader(IJSRuntime jsRuntime)
    {
        _netObjRef = DotNetObjectReference.Create(this);
        _moduleTask = new(() => jsRuntime.InvokeAsync<IJSObjectReference>(
           "import", "./_content/Proxoft.Maps.OpenStreetMap.Maps/apiLoader_0.1.2.js").AsTask());
    }

    public async Task<LoadResponse> LoadMapScripts()
    {
        if (_taskCompletionSource is null)
        {
            _taskCompletionSource = new TaskCompletionSource<LoadResponse>();

            IJSObjectReference v = await _moduleTask.Value;
            await v.InvokeVoidAsync("addOpenStreetMapScripts", new object[] { _netObjRef });
        }
        else
        {
            Console.WriteLine("tcs is not null: skipping call");
        }

        LoadResponse response = await _taskCompletionSource.Task;
        _taskCompletionSource = null;

        return response;
    }

    [JSInvokable]
    public void NotifyLoadScriptStatus(string status)
    {
        if(!Enum.TryParse<LoadResponse>(status, out var apiStatus))
        {
            apiStatus = LoadResponse.FatalError;
        }

        _taskCompletionSource?.SetResult(apiStatus);
    }

    public async ValueTask DisposeAsync()
    {
        if (_taskCompletionSource is not null)
        {
            _taskCompletionSource.SetCanceled();
            _taskCompletionSource = null;
        }

        if (_moduleTask.IsValueCreated)
        {
            var module = await _moduleTask.Value;
            await module.DisposeAsync();
        }

        _netObjRef.Dispose();
    }
}
