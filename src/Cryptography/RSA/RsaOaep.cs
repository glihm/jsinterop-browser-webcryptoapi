using Glihm.JSInterop.Browser.WebCryptoAPI.Interfaces;
using Glihm.JSInterop.Browser.WebCryptoAPI.Interfaces.CryptoKeys;
using Glihm.JSInterop.Browser.WebCryptoAPI.Interfaces.Subtle.RSA;
using Glihm.JSInterop.Browser.WebCryptoAPI.JSHelpers;

using Microsoft.Extensions.Logging;

namespace Glihm.JSInterop.Browser.WebCryptoAPI.Cryptography.RSA;

/// <summary>
/// RSA OAEP.
/// </summary>
public sealed class RsaOaep : RsaBase
{
    /// <summary>
    /// Logger.
    /// </summary>
    private readonly ILogger<RsaOaep> _logger;

    /// <summary>
    /// Constructor used internally by <see cref="RsaFactory"/>.
    /// </summary>
    /// <param name="crypto">Web Crypto API interop.</param>
    /// <param name="keyPair">CryptoKeyPair.</param>
    /// <param name="logger">Logger.</param>
    internal RsaOaep(Crypto crypto,
                     CryptoKeyPairDescriptor keyPair,
                     ILogger<RsaOaep> logger)
        : base(crypto, keyPair)
    {
        this._logger = logger;
    }

    /// <summary>
    /// Encrypts given plaintext using RSA-OAEP.
    /// </summary>
    /// <param name="plaintext">Plaintext to be encrypted.</param>
    /// <param name="label">Array of bytes bound to the ciphertext, but not encrypted.</param>
    /// <returns>Ciphertext on success, null otherwise.</returns>
    public async Task<byte[]?>
    Encrypt(byte[] plaintext, byte[]? label = null)
    {
        CryptoKeyDescriptor publicKey = this.KeyPair.GetPublicKeyOrThrow("RSA OAEP encrypt");

        JSResultValue<byte[]> res = await this._crypto.Subtle.Encrypt(
            new RsaOaepParams(label),
            publicKey,
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
    /// Decrypts given ciphertext using RSA-OAEP.
    /// </summary>
    /// <param name="ciphertext">Ciphertext to decrypt.</param>
    /// <returns>Plaintext on success, null otherwise.</returns>
    public async Task<byte[]?>
    Decrypt(byte[] ciphertext)
    {
        CryptoKeyDescriptor privateKey = this.KeyPair.GetPrivateKeyOrThrow("RSA OAEP decrypt");

        // TODO: verify the label usage!
        JSResultValue<byte[]> res = await this._crypto.Subtle.Decrypt(
            new RsaOaepParams(),
            privateKey,
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
