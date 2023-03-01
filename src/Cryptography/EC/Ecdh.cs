using JSInterop.Browser.WebCryptoAPI.Interfaces;
using JSInterop.Browser.WebCryptoAPI.Interfaces.CryptoKeys;
using JSInterop.Browser.WebCryptoAPI.Interfaces.Subtle.AES;
using JSInterop.Browser.WebCryptoAPI.Interfaces.Subtle.EC;
using JSInterop.Browser.WebCryptoAPI.Interfaces.Subtle.Params;
using JSInterop.Browser.WebCryptoAPI.JSHelpers;

using Microsoft.Extensions.Logging;

namespace JSInterop.Browser.WebCryptoAPI.Cryptography.EC;

/// <summary>
/// ECDH.
/// </summary>
public sealed class Ecdh : EcBase
{
    /// <summary>
    /// Logger.
    /// </summary>
    private readonly ILogger<Ecdh> _logger;

    /// <summary>
    /// Constructor used internally by <see cref="EcFactory"/>.
    /// </summary>
    /// <param name="crypto">Web Crypto API interop.</param>
    /// <param name="keyPair">Key pair.</param>
    /// <param name="logger">Logger.</param>
    internal Ecdh(Crypto crypto,
                   CryptoKeyPairDescriptor keyPair,
                   ILogger<Ecdh> logger)
        : base(crypto, keyPair)
    {
        this._logger = logger;
    }

    /// <summary>
    /// Derives an AES key from the other party public key and
    /// this ECDH private key.
    /// </summary>
    /// <param name="publicKeyOther">Public key of the other party.</param>
    /// <param name="aesAlgo">AES algorithm to use.</param>
    /// <param name="aesKeyLength">AES key length.</param>
    /// <returns></returns>
    public async Task<CryptoKeyDescriptor?>
    DeriveAesKey(CryptoKeyDescriptor publicKeyOther,
                 AesAlgorithm aesAlgo,
                 int aesKeyLength = 256,
                 bool extractable = false)
    {
        AesKeyGenParams gen = new(aesAlgo, aesKeyLength);
        return await this.DeriveKey(
            publicKeyOther,
            gen,
            CryptoKeyUsage.Encrypt | CryptoKeyUsage.Decrypt,
            extractable);
    }

    /// <summary>
    /// Signs the given data.
    /// </summary>
    /// <param name="publicKeyOther">Public key of the other party.</param>
    /// <param name="derivedSecretParams">Parameters of the HMAC or AES derived secret.</param>
    /// <returns>CryptoKey for the derived secret on success, null otherwise.</returns>
    public async Task<CryptoKeyDescriptor?>
    DeriveKey(CryptoKeyDescriptor publicKeyOther,
              IDerivedKeyParams derivedSecretParams,
              CryptoKeyUsage usages,
              bool extractable = false)
    {
        CryptoKeyDescriptor privateKey = this.KeyPair.GetPrivateKeyOrThrow("ECDH derive key");

        JSResultValue<CryptoKeyDescriptor> res = await this._crypto.Subtle.DeriveKey(
            new EcdhKeyDeriveParams(publicKeyOther),
            privateKey,
            derivedSecretParams,
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
