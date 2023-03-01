using JSInterop.Browser.WebCryptoAPI.Interfaces.Subtle;
using JSInterop.Browser.WebCryptoAPI.JSHelpers;

using Microsoft.JSInterop;

namespace JSInterop.Browser.WebCryptoAPI.Interfaces;

/// <summary>
/// Crypto interface.
/// <br />
/// <see href="https://developer.mozilla.org/en-US/docs/Web/API/Crypto"/>
/// </summary>
public class Crypto : JSModule
{
    /// <summary>
    /// SubtleCrypto instance.
    /// </summary>
    public SubtleCrypto Subtle { get; }

    /// <summary>
    /// Constructor (DI).
    /// </summary>
    /// <param name="jsRuntime"></param>
    public Crypto(IJSRuntime jsRuntime)
        : base(jsRuntime, "./_content/JSInterop.Browser.WebCryptoAPI/Crypto.js")
    {
        this.Subtle = new(jsRuntime);
    }

    /// <summary>
    /// Implements <see href="https://developer.mozilla.org/en-US/docs/Web/API/Crypto/getRandomValues">getRandomValues</see> interop.
    /// </summary>
    /// <param name="buffer"> Buffer to be filled with random values. </param>
    public async ValueTask<JSResultVoid>
    GetRandomValues(byte[] buffer)
    {
        string fn = "getRandomValuesFromByteCount";

        JSResultValue<byte[]> res = await this.ModuleInvokeAsync<byte[]>(fn, buffer.Length)
                                              .ConfigureAwait(false);

        if (!res)
        {
            return new JSResultVoid(res.Error!);
        }

        Array.Copy(res.GetValueOrThrow(), buffer, buffer.Length);
        return new JSResultVoid();
    }

    /// <summary>
    /// Generates a buffer of random values from given byte count.
    /// </summary>
    /// <param name="byteCount">Size of the buffer to be generated.</param>
    /// <returns>Buffer with random values on success.</returns>
    public async ValueTask<JSResultValue<byte[]>>
    GetRandomValues(int byteCount)
    {
        string fn = "getRandomValuesFromByteCount";

        JSResultValue<byte[]> res = await this.ModuleInvokeAsync<byte[]>(fn, byteCount)
                                              .ConfigureAwait(false);

        if (!res)
        {
            return new JSResultValue<byte[]>(res.Error!);
        }

        byte[] buffer = new byte[byteCount];
        Array.Copy(res.GetValueOrThrow(), buffer, buffer.Length);
        return new JSResultValue<byte[]>(buffer);
    }

    /// <summary>
    /// Implements <see href="https://developer.mozilla.org/en-US/docs/Web/API/Crypto/randomUUID">randomUUID</see> interop.
    /// </summary>
    /// <returns>Newly generated UUID.</returns>
    public async ValueTask<JSResultValue<Guid>>
    RandomUUID()
    {
        string fn = this.GetWindowProperty("crypto.randomUUID");
        return await this.RuntimeInvokeAsync<Guid>(fn)
                         .ConfigureAwait(false);
    }

    /// <summary>
    /// Verifies that the JS runtime supports WebCrypto API.
    /// </summary>
    /// <returns></returns>
    public async ValueTask<JSResultVoid>
    IsWebCryptoAPISupported()
    {
        string fn = "verifyCryptoSupport";
        return await this.ModuleInvokeVoidAsync(fn)
                         .ConfigureAwait(false);
    }

}
