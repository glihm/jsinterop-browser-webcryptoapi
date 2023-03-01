using Microsoft.JSInterop;

namespace JSInterop.Browser.WebCryptoAPI.JSHelpers;

/// <summary>
/// Basic contract for a IJSResult to be fullfilled.
/// </summary>
public interface IJSResult
{
    /// <summary>
    /// JS errors that may occur dugin JS call.
    /// </summary>
    public JSException? Error { get; }

}
