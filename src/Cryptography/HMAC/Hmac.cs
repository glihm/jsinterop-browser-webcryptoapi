using Glihm.JSInterop.Browser.WebCryptoAPI.Interfaces;
using Glihm.JSInterop.Browser.WebCryptoAPI.Interfaces.CryptoKeys;
using Glihm.JSInterop.Browser.WebCryptoAPI.Interfaces.Subtle.HMAC;
using Glihm.JSInterop.Browser.WebCryptoAPI.JSHelpers;

using Microsoft.Extensions.Logging;

namespace Glihm.JSInterop.Browser.WebCryptoAPI.Cryptography.HMAC;

/// <summary>
/// HMAC.
/// </summary>
public sealed class Hmac : UnmanagedSymmetricBase
{
    /// <summary>
    /// Logger.
    /// </summary>
    private readonly ILogger<Hmac> _logger;

    /// <summary>
    /// Constructor used internally by <see cref="HmacFactory"/>.
    /// </summary>
    /// <param name="crypto">Web Crypto API interop.</param>
    /// <param name="key">Key.</param>
    /// <param name="logger">Logger.</param>
    internal Hmac(Crypto crypto,
                  CryptoKeyDescriptor key,
                  ILogger<Hmac> logger)
        : base(crypto, key)
    {
        this._logger = logger;
    }

    /// <summary>
    /// Signs the given data.
    /// </summary>
    /// <param name="data">Data to be signed.</param>
    /// <returns>Signature on success, null otherwise.</returns>
    public async Task<byte[]?>
    Sign(byte[] data)
    {
        JSResultValue<byte[]> res = await this._crypto.Subtle.Sign(
            new HmacParams(),
            this.Key,
            data)
            .ConfigureAwait(false);

        if (!res)
        {
            this._logger.LogError(res.Error?.Message);
            return null;
        }

        return res.GetValueOrThrow();
    }

    /// <summary>
    /// Verify the signature on the given data.
    /// </summary>
    /// <param name="data">Data whose signature is to be verified.</param>
    /// <param name="signature">Signature to verify.</param>
    /// <returns>True if the signature is valid, false otherwise.</returns>
    public async Task<bool>
    Verify(byte[] data, byte[] signature)
    {
        JSResultValue<bool> resVerify = await this._crypto.Subtle.Verify(
            new HmacParams(),
            this.Key,
            signature,
            data)
            .ConfigureAwait(false);

        if (!resVerify)
        {
            this._logger.LogError(resVerify.Error?.Message);
            return false;
        }

        return resVerify.GetValueOrThrow();
    }

}
