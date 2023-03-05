using Glihm.JSInterop.Browser.WebCryptoAPI.Interfaces;
using Glihm.JSInterop.Browser.WebCryptoAPI.Interfaces.CryptoKeys;
using Glihm.JSInterop.Browser.WebCryptoAPI.Interfaces.Subtle.AES;
using Glihm.JSInterop.Browser.WebCryptoAPI.JSHelpers;

using Microsoft.Extensions.Logging;

namespace Glihm.JSInterop.Browser.WebCryptoAPI.Cryptography.AES;

/// <summary>
/// AES-CBC.
/// <br />
/// Must be disposed (typically using await using)
/// to ensure unmanaged resources cleanup.
/// </summary>
public sealed class AesCbc : AesBase
{
    /// <summary>
    /// Logger.
    /// </summary>
    private readonly ILogger<AesCbc> _logger;

    /// <summary>
    /// Constructor used internally by <see cref="AesFactory"/>.
    /// </summary>
    /// <param name="crypto">Web Crypto API interop.</param>
    /// <param name="key">Key.</param>
    /// <param name="logger">Logger.</param>
    internal AesCbc(Crypto crypto,
                    CryptoKeyDescriptor key,
                    ILogger<AesCbc> logger)
        : base(crypto, key)
    {
        this._logger = logger;
    }

    /// <summary>
    /// Encrypts given plaintext using AES-CBC mode.
    /// </summary>
    /// <param name="plaintext">Plaintext to be encrypted.</param>
    /// <param name="iv">Initialization vector.</param>
    /// <returns>Ciphertext on success, null otherwise.</returns>
    public async Task<byte[]?>
    Encrypt(byte[] plaintext, byte[] iv)
    {
        AesCbcParams prs = new(iv);

        JSResultValue<byte[]> res = await this._crypto.Subtle.Encrypt(
            prs,
            this.Key,
            plaintext)
            .ConfigureAwait(false);

        if (!res)
        {
            // TODO: is a log fair enough to library user to debug?
            this._logger.LogError(res.Error?.Message);
            return null;
        }

        return res.GetValueOrThrow();
    }

    /// <summary>
    /// Decrypts given ciphertext using AES-CBC mode.
    /// </summary>
    /// <param name="ciphertext">Ciphertext to decrypt.</param>
    /// <param name="iv">Initialization vector.</param>
    /// <returns>Plaintext on success, null otherwise.</returns>
    public async Task<byte[]?>
    Decrypt(byte[] ciphertext, byte[] iv)
    {
        AesCbcParams prs = new(iv);

        JSResultValue<byte[]> res = await this._crypto.Subtle.Decrypt(
            prs,
            this.Key,
            ciphertext)
            .ConfigureAwait(false);

        if (!res)
        {
            this._logger.LogError(res.Error?.Message);
            return null;
        }

        return res.GetValueOrThrow();
    }

}
