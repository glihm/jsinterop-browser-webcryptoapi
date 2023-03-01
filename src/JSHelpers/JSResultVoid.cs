using Microsoft.JSInterop;

namespace JSInterop.Browser.WebCryptoAPI.JSHelpers;

/// <summary>
/// Represents a result without any value attached.
/// </summary>
public class JSResultVoid : JSResult
{
    /// <summary>
    /// Default ctor.
    /// </summary>
    public JSResultVoid()
    {

    }

    /// <summary>
    /// Ctor from custom exception.
    /// </summary>
    /// <param name="functionThrower"></param>
    /// <param name="reason"></param>
    public JSResultVoid(string functionThrower, string reason)
        : base(functionThrower, reason)
    {

    }

    /// <summary>
    /// Ctor from error.
    /// </summary>
    /// <param name="error"></param>
    public JSResultVoid(JSException error)
        : base(error)
    {
        this.Error = error;
    }

    /// <summary>
    /// Ctor from any IJSResult.
    /// </summary>
    /// <param name="res"></param>
    public JSResultVoid(IJSResult res)
    {
        this.Error = res.Error;
    }

    public static bool operator true(JSResultVoid t)
    {
        return t.Error is null;
    }

    public static bool operator false(JSResultVoid t)
    {
        return t.Error is not null;
    }

    public static bool operator !(JSResultVoid t)
    {
        return t.Error is not null;
    }
}
