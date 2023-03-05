using Glihm.JSInterop.Browser.WebCryptoAPI.Interfaces;
using Glihm.JSInterop.Browser.WebCryptoAPI.Interfaces.CryptoKeys;
using Glihm.JSInterop.Browser.WebCryptoAPI.Interfaces.Subtle.AES;
using Glihm.JSInterop.Browser.WebCryptoAPI.Interfaces.Subtle.Params;
using Glihm.JSInterop.Browser.WebCryptoAPI.JSHelpers;

using Microsoft.Extensions.Logging;

namespace Glihm.JSInterop.Browser.WebCryptoAPI.Cryptography.AES;

/// <summary>
/// AES-KW.
/// </summary>
public sealed class AesKw : AesBase
{
    /// <summary>
    /// Logger.
    /// </summary>
    private readonly ILogger<AesKw> _logger;

    /// <summary>
    /// Constructor used internally by <see cref="AesFactory"/>.
    /// </summary>
    /// <param name="crypto">Web Crypto API interop.</param>
    /// <param name="key">Key.</param>
    /// <param name="logger">Logger.</param>
    internal AesKw(Crypto crypto,
                   CryptoKeyDescriptor key,
                   ILogger<AesKw> logger)
        : base(crypto, key)
    {
        this._logger = logger;
    }

    /// <summary>
    /// Wraps the given key.
    /// </summary>
    /// <param name="keyToWrap">Key to be wrapped.</param>
    /// <param name="format">Format is which the key is exported before being encrypted.</param>
    /// <returns>Buffer with the encrypted wrapped key in the given format.</returns>
    public async Task<byte[]?>
    WrapKey(CryptoKeyDescriptor keyToWrap, CryptoKeyFormat format)
    {

        JSResultValue<byte[]> res = await this._crypto.Subtle.WrapKey(
            format,
            keyToWrap,
            this.Key,
            new AesKwParams())
            .ConfigureAwait(false);

        if (!res)
        {
            this._logger.LogError(res.Error?.Message);
            return null;
        }

        return res.GetValueOrThrow();
    }

    /// <summary>
    /// Unwraps the given key.
    /// </summary>
    /// <param name="wrappedKey">Wrapped key.</param>
    /// <param name="format">Format of the wrapped key.</param>
    /// <param name="unwrappedKeyParams">Parameters to initialize the wrapped key.</param>
    /// <param name="extractable">If the wrapped key will be exportable.</param>
    /// <param name="usages">Wrapped key usages.</param>
    /// <returns></returns>
    public async Task<CryptoKeyDescriptor?>
    UnwrapKey(byte[] wrappedKey,
              CryptoKeyFormat format,
              IUnwrappedKeyParams unwrappedKeyParams,
              bool extractable,
              CryptoKeyUsage usages)
    {
        JSResultValue<CryptoKeyDescriptor> res = await this._crypto.Subtle.UnwrapKey(
            format,
            wrappedKey,
            this.Key,
            new AesKwParams(),
            unwrappedKeyParams,
            extractable,
            usages)
            .ConfigureAwait(false);

        if (!res)
        {
            this._logger.LogError(res.Error?.Message);
            return null;
        }

        return res.GetValueOrThrow();
    }

}
