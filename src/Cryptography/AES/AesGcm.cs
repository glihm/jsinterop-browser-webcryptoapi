using JSInterop.Browser.WebCryptoAPI.Interfaces;
using JSInterop.Browser.WebCryptoAPI.Interfaces.CryptoKeys;
using JSInterop.Browser.WebCryptoAPI.Interfaces.Subtle.AES;
using JSInterop.Browser.WebCryptoAPI.JSHelpers;

using Microsoft.Extensions.Logging;

namespace JSInterop.Browser.WebCryptoAPI.Cryptography.AES;

/// <summary>
/// AES-GCM.
/// <br />
/// AES-GCM in Web Crypto API specification
/// works with a ciphertext with the tag appended.
/// So, ciphertext = C|T where '|' is concatenation.
/// Other implementations, like in .NET, are generating
/// the tag as a separate entity.
/// <br />
/// Must be disposed (typically using await using)
/// to ensure unmanaged resources cleanup.
/// </summary>
public sealed class AesGcm : AesBase
{
    /// <summary>
    /// Logger.
    /// </summary>
    private readonly ILogger<AesGcm> _logger;

    /// <summary>
    /// Constructor used internally by <see cref="AesFactory"/>.
    /// </summary>
    /// <param name="crypto">Web Crypto API interop.</param>
    /// <param name="key">Key.</param>
    /// <param name="logger">Logger.</param>
    internal AesGcm(Crypto crypto,
                    CryptoKeyDescriptor key,
                    ILogger<AesGcm> logger)
        : base(crypto, key)
    {
        this._logger = logger;
    }

    /// <summary>
    /// Encrypts given plaintext using AES-GCM mode.
    /// </summary>
    /// <param name="plaintext">Plaintext to be encrypted.</param>
    /// <param name="iv">Initialization vector.</param>
    /// <param name="additionalData">Additional data, not encrypted but authenticated.</param>
    /// <param name="tagLength">Length of the tag in bits.</param>
    /// <returns>Ciphertext on success, null otherwise.</returns>
    public async Task<byte[]?>
    Encrypt(byte[] plaintext, byte[] iv, byte[]? additionalData = null, int? tagLength = null)
    {
        AesGcmParams prs = new(iv, additionalData, tagLength);

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
    /// Decrypts given ciphertext with an explicit tag.
    /// This overload will fail if called on a ciphertext
    /// with the tag already appended. Consider using overload
    /// with tagLength if the ciphertext comes from Web Crypto API.
    /// </summary>
    /// <param name="ciphertext">Ciphertext to decrypt.</param>
    /// <param name="iv">Initialization vector.</param>
    /// <param name="tag">Authentication tag.</param>
    /// <returns></returns>
    public async Task<byte[]?>
    Decrypt(byte[] ciphertext, byte[] iv, byte[] tag)
    {
        byte[] ciphertextAndTag = ciphertext.Concat(tag).ToArray();
        return await this.Decrypt(ciphertextAndTag, iv, tag.Length * 8)
                         .ConfigureAwait(false);
    }

    /// <summary>
    /// Decrypts given ciphertext using AES-GCM mode.
    /// </summary>
    /// <param name="ciphertext">Ciphertext to decrypt.</param>
    /// <param name="iv">Initialization vector.</param>
    /// <param name="tagLength">Length of the tag in bits.</param>
    /// <returns>Plaintext on success, null otherwise.</returns>
    public async Task<byte[]?>
    Decrypt(byte[] ciphertext, byte[] iv, int? tagLength = null)
    {
        AesGcmParams prs = new(iv, tagLength: tagLength);

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
