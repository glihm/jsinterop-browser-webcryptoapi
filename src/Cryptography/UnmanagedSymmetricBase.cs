using JSInterop.Browser.WebCryptoAPI.Interfaces;
using JSInterop.Browser.WebCryptoAPI.Interfaces.CryptoKeys;

namespace JSInterop.Browser.WebCryptoAPI.Cryptography;

/// <summary>
/// Base for cryptography classes that will handle
/// unmanages resources from JS runtime for symmetric
/// algorithms, which involve one crypto key.
/// </summary>
public abstract class UnmanagedSymmetricBase : IAsyncDisposable
{
    /// <summary>
    /// Crypto WebAPI interop.
    /// </summary>
    protected readonly Crypto _crypto;

    /// <summary>
    /// Key to use.
    /// </summary>
    public CryptoKeyDescriptor Key { get; }

    /// <summary>
    /// Constructor used internally.
    /// </summary>
    /// <param name="crypto">Crypto Web API instance.</param>
    /// <param name="key">CryptoKey.</param>
    protected UnmanagedSymmetricBase(Crypto crypto, CryptoKeyDescriptor key)
    {
        this._crypto = crypto;
        this.Key = key;
    }

    /// <summary>
    /// Exports the current key to raw format.
    /// </summary>
    /// <returns>Key buffer.</returns>
    public async Task<byte[]?>
    ExportKeyRaw()
    {
        return await this.Key.Export(CryptoKeyFormat.Raw)
                             .ConfigureAwait(false);
    }

    /// <summary>
    /// Exports the current key to JSONWebKey format.
    /// </summary>
    /// <returns>String containing the JSON key.</returns>
    public async Task<string?>
    ExportKeyJSONWebKey()
    {
        return await this.Key.ExportJSONWebKey()
                             .ConfigureAwait(false);
    }

    /// <summary>
    /// Disposes unmanaged resources allocated for this
    /// key in the JS runtime.
    /// </summary>
    /// <returns></returns>
    public async ValueTask
    DisposeAsync()
    {
        await this.Key.DisposeAsync()
                      .ConfigureAwait(false);
    }
}
