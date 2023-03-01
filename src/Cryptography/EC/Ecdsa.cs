using JSInterop.Browser.WebCryptoAPI.Interfaces;
using JSInterop.Browser.WebCryptoAPI.Interfaces.CryptoKeys;
using JSInterop.Browser.WebCryptoAPI.Interfaces.Subtle.EC;
using JSInterop.Browser.WebCryptoAPI.Interfaces.Subtle.SHA;
using JSInterop.Browser.WebCryptoAPI.JSHelpers;

using Microsoft.Extensions.Logging;

namespace JSInterop.Browser.WebCryptoAPI.Cryptography.EC;

/// <summary>
/// ECDSA.
/// </summary>
public sealed class Ecdsa : EcBase
{
    /// <summary>
    /// Logger.
    /// </summary>
    private readonly ILogger<Ecdsa> _logger;

    /// <summary>
    /// Constructor used internally by <see cref="EcFactory"/>.
    /// </summary>
    /// <param name="crypto">Web Crypto API interop.</param>
    /// <param name="keyPair">Key pair..</param>
    /// <param name="logger">Logger.</param>
    internal Ecdsa(Crypto crypto,
                   CryptoKeyPairDescriptor keyPair,
                   ILogger<Ecdsa> logger)
        : base(crypto, keyPair)
    {
        this._logger = logger;
    }

    /// <summary>
    /// Signs the given data.
    /// </summary>
    /// <param name="data">Data to be signed.</param>
    /// <param name="hash">Digest function to use.</param>
    /// <returns>Signature on success, null otherwise.</returns>
    public async Task<byte[]?>
    Sign(byte[] data, ShaAlgorithm hash)
    {
        CryptoKeyDescriptor privateKey = this.KeyPair.GetPrivateKeyOrThrow("ECDSA sign");

        JSResultValue<byte[]> res = await this._crypto.Subtle.Sign(
            new EcdsaParams(hash),
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
    /// <param name="hash">Digest function to use.</param>
    /// <param name="signature">Signature to verify.</param>
    /// <returns>True if the signature is valid, false otherwise.</returns>
    public async Task<bool>
    Verify(byte[] data, ShaAlgorithm hash, byte[] signature)
    {
        CryptoKeyDescriptor publicKey = this.KeyPair.GetPublicKeyOrThrow("ECDSA verify");

        JSResultValue<bool> resVerify = await this._crypto.Subtle.Verify(
            new EcdsaParams(hash),
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
