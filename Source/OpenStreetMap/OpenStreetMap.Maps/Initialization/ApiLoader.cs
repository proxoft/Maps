﻿using System;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace Proxoft.Maps.OpenStreetMap.Maps.Initialization
{
    internal class ApiLoader : IAsyncDisposable
    {
        private readonly Lazy<Task<IJSObjectReference>> _moduleTask;
       

        private readonly DotNetObjectReference<ApiLoader> _netObjRef;
        private TaskCompletionSource<ApiStatus> _taskCompletionSource;

        public ApiLoader(IJSRuntime jsRuntime)
        {
            _netObjRef = DotNetObjectReference.Create(this);
            _moduleTask = new(() => jsRuntime.InvokeAsync<IJSObjectReference>(
               "import", "./_content/Proxoft.Maps.OpenStreetMap.Maps/apiLoader.js").AsTask());

        }

        public async Task<ApiStatus> LoadGoogleScripts()
        {
            _taskCompletionSource = new TaskCompletionSource<ApiStatus>();

            var v = await _moduleTask.Value;
            await v.InvokeVoidAsync("addOpenStreetMapScripts", new object[] { _netObjRef });

            var status = await _taskCompletionSource.Task;
            return status;
        }

        [JSInvokable]
        public void NotifyLoadScriptStatus(string status)
        {
            if(!Enum.TryParse<ApiStatus>(status, out var apiStatus))
            {
                apiStatus = ApiStatus.FatalError;
            }

            _taskCompletionSource.SetResult(apiStatus);
        }

        public async ValueTask DisposeAsync()
        {
            if (_moduleTask.IsValueCreated)
            {
                var module = await _moduleTask.Value;
                await module.DisposeAsync();
            }

            _netObjRef.Dispose();
        }
    }
}