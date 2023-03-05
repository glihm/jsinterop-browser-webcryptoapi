using Glihm.JSInterop.Browser.WebCryptoAPI.Interfaces;
using Glihm.JSInterop.Browser.WebCryptoAPI.Interfaces.CryptoKeys;
using Glihm.JSInterop.Browser.WebCryptoAPI.Interfaces.Subtle.RSA;
using Glihm.JSInterop.Browser.WebCryptoAPI.JSHelpers;

using Microsoft.Extensions.Logging;

namespace Glihm.JSInterop.Browser.WebCryptoAPI.Cryptography.RSA;

/// <summary>
/// RSA PSS.
/// </summary>
public sealed class RsaPss : RsaBase
{
    /// <summary>
    /// Logger.
    /// </summary>
    private readonly ILogger<RsaPss> _logger;

    /// <summary>
    /// Constructor used internally by <see cref="RsaFactory"/>.
    /// </summary>
    /// <param name="crypto">Web Crypto API interop.</param>
    /// <param name="keyPair">Key pair.</param>
    /// <param name="logger">Logger.</param>
    internal RsaPss(Crypto crypto,
                     CryptoKeyPairDescriptor keyPair,
                     ILogger<RsaPss> logger)
        : base(crypto, keyPair)
    {
        this._logger = logger;
    }

    /// <summary>
    /// Signs the given data.
    /// </summary>
    /// <param name="data">Data to be signed.</param>
    /// <param name="saltLength">Length of the random salt to use.</param>
    /// <returns>Signature on success, null otherwise.</returns>
    public async Task<byte[]?>
    Sign(byte[] data, int saltLength = 0)
    {
        CryptoKeyDescriptor privateKey = this.KeyPair.GetPrivateKeyOrThrow("RSA PSS sign");

        RsaPssParams prs = new(saltLength);

        JSResultValue<byte[]> res = await this._crypto.Subtle.Sign(
            prs,
            privateKey,
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
    /// <param name="saltLength">Length of the random salt to use.</param>
    /// <returns>True if the signature is valid, false otherwise.</returns>
    public async Task<bool>
    Verify(byte[] data, byte[] signature, int saltLength = 0)
    {
        CryptoKeyDescriptor publicKey = this.KeyPair.GetPublicKeyOrThrow("RSA PSS verify");

        RsaPssParams prs = new(saltLength);

        JSResultValue<bool> resVerify = await this._crypto.Subtle.Verify(
            prs,
            publicKey,
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
