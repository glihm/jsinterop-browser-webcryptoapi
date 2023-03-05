using Microsoft.JSInterop;

namespace Glihm.JSInterop.Browser.WebCryptoAPI.JSHelpers;

/// <summary>
/// Represents a result with an attached value.
/// </summary>
/// <typeparam name="TValue">Type of the returned value.</typeparam>
public class JSResultValue<TValue> : JSResult
{
    /// <summary>
    /// Value expected when JS call exits successfully.
    /// </summary>
    public TValue? Value { get; }

    /// <summary>
    /// Ctor from custom exception.
    /// </summary>
    /// <param name="functionThrower"></param>
    /// <param name="reason"></param>
    public JSResultValue(string functionThrower, string reason)
        : base(functionThrower, reason)
    {
        this.Value = default;
    }

    /// <summary>
    /// Ctor from error.
    /// </summary>
    /// <param name="error"></param>
    public JSResultValue(JSException error)
        : base(error)
    {
        this.Value = default;
        this.Error = error;
    }

    /// <summary>
    /// Ctor from any IJSResult.
    /// </summary>
    /// <param name="res"></param>
    public JSResultValue(IJSResult res)
    {
        this.Error = res.Error;
    }

    /// <summary>
    /// Ctor from value.
    /// </summary>
    public JSResultValue(TValue value)
    {
        this.Value = value;
    }

    /// <summary>
    /// Returns the underlying value.
    /// This must be call when you know that the value is not
    /// supposed to be null.
    /// </summary>
    /// <returns>Underlying value on success.</returns>
    /// <exception cref="InvalidDataException">Underlying value is null.</exception>
    public TValue
    GetValueOrThrow()
    {
        if (this.Value is null)
        {
            throw new InvalidDataException($"Value is not expected to be null for type {typeof(TValue)}.");
        }

        return this.Value;
    }

    public static bool operator true(JSResultValue<TValue> t)
    {
        return t.Error is null;
    }

    public static bool operator false(JSResultValue<TValue> t)
    {
        return t.Error is not null;
    }

    public static bool operator !(JSResultValue<TValue> t)
    {
        return t.Error is not null;
    }
}
