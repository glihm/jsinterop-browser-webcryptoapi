using Microsoft.JSInterop;

namespace JSInterop.Browser.WebCryptoAPI.JSHelpers;

/// <summary>
/// Base class that represents a result from JS call.
/// </summary>
public abstract class JSResult : IJSResult
{
    /// <summary>
    /// JS errors that may occur dugin JS call.
    /// </summary>
    public JSException? Error { get; protected set; }

    /// <summary>
    /// Default ctor.
    /// </summary>
    public JSResult()
    {

    }

    /// <summary>
    /// Ctor from exception.
    /// </summary>
    /// <param name="error"></param>
    public JSResult(JSException? error)
    {
        this.Error = error;
    }

    /// <summary>
    /// Ctor from a custom exception reason.
    /// </summary>
    /// <param name="functionThrower"></param>
    /// <param name="reason"></param>
    public JSResult(string functionThrower, string reason)
    {
        this.Error = new JSException($"JS call to {functionThrower} has thrown: {reason}.");
    }

}
