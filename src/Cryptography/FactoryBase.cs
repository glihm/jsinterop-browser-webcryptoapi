using Glihm.JSInterop.Browser.WebCryptoAPI.Interfaces;
using Glihm.JSInterop.Browser.WebCryptoAPI.Interfaces.CryptoKeys;
using Glihm.JSInterop.Browser.WebCryptoAPI.Interfaces.Subtle.Params;
using Glihm.JSInterop.Browser.WebCryptoAPI.JSHelpers;

using Microsoft.Extensions.Logging;

namespace Glihm.JSInterop.Browser.WebCryptoAPI.Cryptography;

public class FactoryBase
{
    /// <summary>
    /// Loggers.
    /// </summary>
    protected readonly ILoggerFactory _loggerFactory;
    private readonly ILogger<FactoryBase> _logger;

    /// <summary>
    /// Crypto Web API interop.
    /// </summary>
    protected readonly Crypto _crypto;

    /// <summary>
    /// Constructor dependency injection.
    /// </summary>
    /// <param name="crypto">Web Crypto API interop.</param>
    /// <param name="loggerFactory">Logger factory.</param>
    public FactoryBase(Crypto crypto, ILoggerFactory loggerFactory)
    {
        this._crypto = crypto;
        this._loggerFactory = loggerFactory;
        this._logger = this._loggerFactory.CreateLogger<FactoryBase>();
    }

    /// <summary>
    /// Generates a key for given parameters.
    /// </summary>
    /// <param name="genParams">Generation parameters.</param>
    /// <returns>Key on success, null otherwise.</returns>
    protected async Task<CryptoKeyDescriptor?>
    GenerateCryptoKey(IGenParams genParams,
                      CryptoKeyUsage usages,
                      bool extractable = false)
    {
        JSResultValue<CryptoKeyDescriptor> res = await this._crypto.Subtle.GenerateKey(
            genParams,
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

    /// <summary>
    /// Generates a key pair for given parameters.
    /// </summary>
    /// <param name="genParams">Generation parameters.</param>
    /// <returns>Key pair on success, null otherwise.</returns>
    protected async Task<CryptoKeyPairDescriptor?>
    GenerateCryptoKeyPair(IGenParams genParams,
                          CryptoKeyUsage usages,
                          bool extractable = false)
    {
        JSResultValue<CryptoKeyPairDescriptor> res = await this._crypto.Subtle.GenerateKeyPair(
            genParams,
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

    /// <summary>
    /// Import public and private key to form a new CryptoKeyPair.
    /// </summary>
    /// <param name="importParams">Algorithm import parameters.</param>
    /// <param name="kpImport">Crypto key pair description to be imported.</param>
    /// <returns>Newly formed crypto key pair on success, null otherwise.</returns>
    protected async Task<CryptoKeyPairDescriptor?>
    ImportKeyPair(IImportParams importParams, CryptoKeyPairImport kpImport)
    {
        CryptoKeyDescriptor? pub = null;
        CryptoKeyDescriptor? prv = null;

        if (kpImport.PublicKey is not null)
        {
            pub = await this.ImportKey(kpImport.PublicKey, importParams)
                            .ConfigureAwait(false);

            if (pub is null)
            {
                return null;
            }
        }

        if (kpImport.PrivateKey is not null)
        {
            prv = await this.ImportKey(kpImport.PrivateKey, importParams)
                            .ConfigureAwait(false);

            if (prv is null)
            {
                return null;
            }
        }

        return new CryptoKeyPairDescriptor(publicKey: pub, privateKey: prv);
    }

    /// <summary>
    /// Imports a crypto key from it's description.
    /// </summary>
    /// <param name="keyImport">Key and it's parameters.</param>
    /// <param name="importParams">Algorithm params to import the key.</param>
    /// <returns>CryptoKey on success, null otherwise.</returns>
    protected async Task<CryptoKeyDescriptor?>
    ImportKey(CryptoKeyImport keyImport, IImportParams importParams)
    {
        JSResultValue<CryptoKeyDescriptor> importRes = await this._crypto.Subtle.ImportKey(
            keyImport.Format,
            keyImport.Key,
            importParams,
            keyImport.Extractable,
            keyImport.Usages)
            .ConfigureAwait(false);

        if (!importRes)
        {
            this._logger.LogError(importRes.Error?.Message);
            return null;
        }

        return importRes.GetValueOrThrow();
    }
}
