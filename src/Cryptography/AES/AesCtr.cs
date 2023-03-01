using JSInterop.Browser.WebCryptoAPI.Interfaces;
using JSInterop.Browser.WebCryptoAPI.Interfaces.CryptoKeys;
using JSInterop.Browser.WebCryptoAPI.Interfaces.Subtle.AES;
using JSInterop.Browser.WebCryptoAPI.JSHelpers;

using Microsoft.Extensions.Logging;

namespace JSInterop.Browser.WebCryptoAPI.Cryptography.AES;

/// <summary>
/// AES-CTR.
/// <br />
/// Must be disposed (typically using await using)
/// to ensure unmanaged resources cleanup.
/// </summary>
public sealed class AesCtr : AesBase
{
    /// <summary>
    /// Logger.
    /// </summary>
    private readonly ILogger<AesCtr> _logger;

    /// <summary>
    /// Constructor used internally by <see cref="AesFactory"/>.
    /// </summary>
    /// <param name="crypto">Web Crypto API interop.</param>
    /// <param name="key">Key.</param>
    /// <param name="logger">Logger.</param>
    internal AesCtr(Crypto crypto,
                    CryptoKeyDescriptor key,
                    ILogger<AesCtr> logger)
        : base(crypto, key)
    {
        this._logger = logger;
    }

    /// <summary>
    /// Encrypts given plaintext using AES-CTR mode.
    /// </summary>
    /// <param name="plaintext">Plaintext to be encrypted.</param>
    /// <param name="counter">Initialial counter value.</param>
    /// <param name="length">The number of bits in the counter block that are used for the actual counter.</param>
    /// <returns>Ciphertext on success, null otherwise.</returns>
    public async Task<byte[]?>
    Encrypt(byte[] plaintext, byte[] counter, int length = 64)
    {
        AesCtrParams prs = new(counter, length);

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
    /// Decrypts given ciphertext using AES-CTR mode.
    /// </summary>
    /// <param name="ciphertext">Ciphertext to decrypt.</param>
    /// <param name="counter">Initialial counter value.</param>
    /// <param name="length">The number of bits in the counter block that are used for the actual counter.</param>
    /// <returns>Plaintext on success, null otherwise.</returns>
    public async Task<byte[]?>
    Decrypt(byte[] ciphertext, byte[] counter, int length = 64)
    {
        AesCtrParams prs = new(counter, length);

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
