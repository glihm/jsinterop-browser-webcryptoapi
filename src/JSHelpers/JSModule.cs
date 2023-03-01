using Microsoft.JSInterop;

namespace JSInterop.Browser.WebCryptoAPI.JSHelpers;

/// <summary>
/// Base class to bind JS module to a C# class.
/// 
/// This class factorizes the way each module may call JSRuntime
/// in a generic manner, or the scoped functions from the imported module.
/// </summary>
public abstract class JSModule : IAsyncDisposable
{
    /// <summary>
    /// Reference to the JS runtime.
    /// In Blazor, this JSRuntime that is injected by the framework
    /// represents the browser runtime, which typically includes the
    /// `window` object.
    /// </summary>
    private IJSRuntime _jsRuntime { get; }

    /// <summary>
    /// Reference to the lazy loaded module.
    /// </summary>
    protected Lazy<Task<IJSObjectReference>> JSModuleTask { get; }

    /// <summary>
    /// Constructor (usually DI).
    /// </summary>
    /// <param name="jsRuntime"> JS runtime. </param>
    public JSModule(IJSRuntime jsRuntime, string modulePath)
    {
        this._jsRuntime = jsRuntime;
        this.JSModuleTask = new(
            () => this._jsRuntime.InvokeAsync<IJSObjectReference>("import", modulePath)
                                 .AsTask());
    }

    /// <summary>
    /// Disposes allocated resources, mainly the lazy loaded module.
    /// </summary>
    /// <returns></returns>
    public async ValueTask
    DisposeAsync()
    {
        await this.DisposeInDerivedAsync()
                  .ConfigureAwait(false);

        if (this.JSModuleTask.IsValueCreated)
        {
            IJSObjectReference m = await this.JSModuleTask.Value;
            await m.DisposeAsync()
                   .ConfigureAwait(false);
        }
    }

    /// <summary>
    /// Disposes allocated resources in the derived class.
    /// This method is always called by DisposeAsync.
    /// 
    /// Override this method in the derived class if there
    /// is a need to free additional resources. The module
    /// itself is freed by the DisposeAsync call.
    /// </summary>
    /// <returns></returns>
    protected virtual ValueTask
    DisposeInDerivedAsync()
    {
        return ValueTask.CompletedTask;
    }

    /// <summary>
    /// Invokes a JS function with no return value, in the scope of the module.
    /// </summary>
    /// <param name="identifier">Identifier of the function to invoke.</param>
    /// <param name="args">List of arguments that are JSON-Serializable.</param>
    /// <returns></returns>
    protected virtual async ValueTask<JSResultVoid>
    ModuleInvokeVoidAsync(string identifier, params object?[]? args)
    {
        IJSObjectReference m = await this.JSModuleTask.Value.ConfigureAwait(false);

        try
        {
            await m.InvokeVoidAsync(identifier, args)
                   .ConfigureAwait(false);

            return new JSResultVoid();
        }
        catch (JSException e)
        {
            return new JSResultVoid(e);
        }
    }

    /// <summary>
    /// Invokes a JS function returning a JSON-Serializable result,
    /// in the scope of the module.
    /// </summary>
    /// <typeparam name="TValue">Type of the returned value.</typeparam>
    /// <param name="identifier">Identifier of the function to invoke.</param>
    /// <param name="args">List of arguments that are JSON-Serializable.</param>
    /// <returns></returns>
    protected virtual async ValueTask<JSResultValue<TValue>>
    ModuleInvokeAsync<TValue>(string identifier, params object?[]? args)
    {
        IJSObjectReference m = await this.JSModuleTask.Value.ConfigureAwait(false);

        try
        {
            TValue v = await m.InvokeAsync<TValue>(identifier, args)
                              .ConfigureAwait(false);

            return new JSResultValue<TValue>(v);
        }
        catch (JSException e)
        {
            return new JSResultValue<TValue>(e);
        }
    }

    /// <summary>
    /// Overload of <see cref="ModuleInvokeAsync">ModuleInvokeAsync</see> with a cancellation token.
    /// Called in the scope of the module.
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="identifier">Identifier of the function to invoke.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <param name="args">List of arguments that are JSON-Serializable.</param>
    /// <returns></returns>
    protected virtual async ValueTask<JSResultValue<TValue>>
    ModuleInvokeAsync<TValue>(string identifier, CancellationToken cancellationToken, params object?[]? args)
    {
        IJSObjectReference m = await this.JSModuleTask.Value.ConfigureAwait(false);

        try
        {
            TValue v = await m.InvokeAsync<TValue>(identifier, cancellationToken, args)
                              .ConfigureAwait(false);

            return new JSResultValue<TValue>(v);
        }
        catch (JSException e)
        {
            return new JSResultValue<TValue>(e);
        }
    }

    /// <summary>
    /// Invokes a JS function with no return value, directly on the runtime.
    /// </summary>
    /// <param name="identifier">Identifier of the function to invoke.</param>
    /// <param name="args">List of arguments that are JSON-Serializable.</param>
    /// <returns></returns>
    protected virtual async ValueTask<JSResultVoid>
    RuntimeInvokeVoidAsync(string identifier, params object?[]? args)
    {
        try
        {
            await this._jsRuntime.InvokeVoidAsync(identifier, args)
                                 .ConfigureAwait(false);

            return new JSResultVoid();
        }
        catch (JSException e)
        {
            return new JSResultVoid(e);
        }
    }

    /// <summary>
    /// Invokes a JS function returning a JSON-Serializable result,
    /// directly on the runtime.
    /// </summary>
    /// <typeparam name="TValue">Type of the returned value.</typeparam>
    /// <param name="identifier">Identifier of the function to invoke.</param>
    /// <param name="args">List of arguments that are JSON-Serializable.</param>
    /// <returns></returns>
    protected virtual async ValueTask<JSResultValue<TValue>>
    RuntimeInvokeAsync<TValue>(string identifier, params object?[]? args)
    {
        try
        {
            TValue v = await this._jsRuntime.InvokeAsync<TValue>(identifier, args)
                                            .ConfigureAwait(false);

            return new JSResultValue<TValue>(v);
        }
        catch (JSException e)
        {
            return new JSResultValue<TValue>(e);
        }
    }

    /// <summary>
    /// Overload of <see cref="ModuleInvokeAsync">ModuleInvokeAsync</see> with a cancellation token.
    /// Called directly on the runtime.
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="identifier">Identifier of the function to invoke.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <param name="args">List of arguments that are JSON-Serializable.</param>
    /// <returns></returns>
    protected virtual async ValueTask<JSResultValue<TValue>>
    RuntimeInvokeAsync<TValue>(string identifier, CancellationToken cancellationToken, params object?[]? args)
    {
        try
        {
            TValue v = await this._jsRuntime.InvokeAsync<TValue>(identifier, cancellationToken, args)
                                            .ConfigureAwait(false);

            return new JSResultValue<TValue>(v);
        }
        catch (JSException e)
        {
            return new JSResultValue<TValue>(e);
        }
    }

    /// <summary>
    /// Returns a string corresponding to the JS
    /// identifier to retrieve the web API object in JS.
    /// </summary>
    /// <returns>A string refering to the web API identifier in JS.</returns>
    protected virtual string
    GetWindowProperty(string propertyName)
    {
        string fmt = "self.{0}";
        return string.Format(fmt, propertyName);
    }

}
