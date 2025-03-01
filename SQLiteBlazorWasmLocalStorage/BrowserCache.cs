// <copyright file="BrowserCache.cs" company="VeriNova Technologies Inc.">
// Copyright (c) VeriNova Technologies Inc.. All rights reserved.
// </copyright>

using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace SQLiteBlazorWasmLocalStorage
{
    /// <summary>
    /// Wrapper for JavaScript code to sychronize the database.
    /// </summary>
    public sealed class BrowserCache : IAsyncDisposable, IBrowserCache
    {
        private readonly Lazy<Task<IJSObjectReference>> moduleTask;

        /// <summary>
        /// Initializes a new instance of the <see cref="BrowserCache"/> class.
        /// </summary>
        /// <param name="jsRuntime">The <see cref="IJSRuntime"/> instance.</param>
        public BrowserCache(IJSRuntime jsRuntime)
        {
            moduleTask = new(() => jsRuntime.InvokeAsync<IJSObjectReference>(
                "import", "./_content/SQLiteBlazorWasmLocalStorage/browserCache.js").AsTask()!);
        }

        /// <summary>
        /// Disposes of the task that references the JavaScript module.
        /// </summary>
        /// <returns>A task to await.</returns>
        public async ValueTask DisposeAsync()
        {
            if (moduleTask.IsValueCreated)
            {
                var module = await moduleTask.Value;
                await module.DisposeAsync();
            }
        }

        /// <summary>
        /// Calls the code to check save to/restore from cache.
        /// </summary>
        /// <param name="filename">The name of the file to process.</param>
        /// <returns>Either -1 (no cache), 0 (restored), or 1 (cached).</returns>
        public async Task<int> SyncDbWithCacheAsync(string filename)
        {
            var module = await moduleTask.Value;
            return await module.InvokeAsync<int>("synchronizeDbWithCache", filename);
        }

        /// <summary>
        /// Calls the code to restore the database from a given ArrayBuffer.
        /// </summary>
        /// <param name="arrayBuffer">The ArrayBuffer containing the database file.</param>
        /// <param name="filename">The name of the file to process.</param>
        /// <returns>0 if successful, -1 otherwise.</returns>
        public async Task<int> ManualRestore(byte[] arrayBuffer, string filename)
        {
            var module = await moduleTask.Value;
            return await module.InvokeAsync<int>("manualRestore", arrayBuffer, filename);
        }

        /// <summary>
        /// Creates an anchor tag to download the database and injects it into the parent.
        /// </summary>
        /// <param name="parent">The host for the tag.</param>
        /// <param name="filename">The database filename.</param>
        /// <returns>A value indicating whether the operation was successful.</returns>
        public async Task<bool> GenerateDownloadLinkAsync(ElementReference parent, string filename)
        {
            var module = await moduleTask.Value;
            return await module.InvokeAsync<bool>("generateDownloadLink", parent, filename);
        }
    }
}
