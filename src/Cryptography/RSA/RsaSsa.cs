using Glihm.JSInterop.Browser.WebCryptoAPI.Interfaces;
using Glihm.JSInterop.Browser.WebCryptoAPI.Interfaces.CryptoKeys;
using Glihm.JSInterop.Browser.WebCryptoAPI.Interfaces.Subtle.RSA;
using Glihm.JSInterop.Browser.WebCryptoAPI.JSHelpers;

using Microsoft.Extensions.Logging;

namespace Glihm.JSInterop.Browser.WebCryptoAPI.Cryptography.RSA;

/// <summary>
/// RSA Ssa.
/// </summary>
public sealed class RsaSsa : RsaBase
{
    /// <summary>
    /// Logger.
    /// </summary>
    private readonly ILogger<RsaSsa> _logger;

    /// <summary>
    /// Constructor used internally by <see cref="RsaFactory"/>.
    /// </summary>
    /// <param name="crypto">Web Crypto API interop.</param>
    /// <param name="keyPair">Key pair.</param>
    /// <param name="logger">Logger.</param>
    internal RsaSsa(Crypto crypto,
                    CryptoKeyPairDescriptor keyPair,
                    ILogger<RsaSsa> logger)
        : base(crypto, keyPair)
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
        CryptoKeyDescriptor privateKey = this.KeyPair.GetPrivateKeyOrThrow("RSA SSA sign");

        JSResultValue<byte[]> res = await this._crypto.Subtle.Sign(
            new RsaSsaPkcs1Params(),
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
    /// <returns>True if the signature is valid, false otherwise.</returns>
    public async Task<bool>
    Verify(byte[] data, byte[] signature)
    {
        CryptoKeyDescriptor publicKey = this.KeyPair.GetPublicKeyOrThrow("RSA SSA verify");

        JSResultValue<bool> resVerify = await this._crypto.Subtle.Verify(
            new RsaSsaPkcs1Params(),
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
