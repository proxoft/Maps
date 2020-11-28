using System;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Proxoft.Maps.Google.Common;

namespace Proxoft.Maps.Google.Maps.Initialization
{
    internal class ApiLoader : IAsyncDisposable
    {
        private readonly Lazy<Task<IJSObjectReference>> _moduleTask;
       

        private readonly DotNetObjectReference<ApiLoader> _netObjRef;
        private TaskCompletionSource<LoadResponse> _taskCompletionSource;

        public ApiLoader(IJSRuntime jsRuntime)
        {
            _netObjRef = DotNetObjectReference.Create(this);
            _moduleTask = new(() => jsRuntime.InvokeAsync<IJSObjectReference>(
               "import", "./_content/Proxoft.Maps.Google.Maps/apiLoader.js").AsTask());

        }

        public async Task<LoadResponse> LoadGoogleScripts(GoogleApiConfiguration configuration)
        {
            Console.WriteLine(configuration.ApiKey);

            _taskCompletionSource = new TaskCompletionSource<LoadResponse>();

            var v = await _moduleTask.Value;
            await v.InvokeVoidAsync("addGoogleMapsScripts", new object[] { configuration.ApiKey, configuration.Language, _netObjRef });

            var status = await _taskCompletionSource.Task;
            return status;
        }

        [JSInvokable]
        public void NotifyLoadScriptStatus(string status)
        {
            if(!Enum.TryParse<LoadResponse>(status, out var apiStatus))
            {
                apiStatus = LoadResponse.FatalError;
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
