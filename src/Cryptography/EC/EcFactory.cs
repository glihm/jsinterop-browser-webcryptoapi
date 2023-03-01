using JSInterop.Browser.WebCryptoAPI.Interfaces;
using JSInterop.Browser.WebCryptoAPI.Interfaces.CryptoKeys;
using JSInterop.Browser.WebCryptoAPI.Interfaces.Subtle.EC;

using Microsoft.Extensions.Logging;

namespace JSInterop.Browser.WebCryptoAPI.Cryptography.EC;

/// <summary>
/// EC factory.
/// </summary>
public class EcFactory : FactoryBase
{
    /// <summary>
    /// Loggers.
    /// </summary>
    private readonly ILogger<EcFactory> _logger;

    /// <summary>
    /// Constructor dependency injection.
    /// </summary>
    /// <param name="crypto">Web Crypto API interop.</param>
    /// <param name="loggerFactory">Logger factory.</param>
    public EcFactory(Crypto crypto, ILoggerFactory loggerFactory)
        : base(crypto, loggerFactory)
    {
        this._logger = loggerFactory.CreateLogger<EcFactory>();
    }

    /// <summary>
    /// Creates EC instance.
    /// </summary>
    /// <param name="namedCurve">Curve to use.</param>
    /// <param name="kpImport">Crypto key pair import parameters.</param>
    /// <param name="extractable">If not kpImport is provided, defines if the genertated keys can be exported later.</param>
    /// <returns>EC instance on success, null otherwise.</returns>
    public async Task<T?>
    Create<T>(EcNamedCurve namedCurve,
              CryptoKeyPairImport? kpImport = null,
              bool extractable = false)
        where T : EcBase
    {
        EcAlgorithm algo = this.EcAlgorithmFromType(typeof(T));

        CryptoKeyPairDescriptor? ckp;

        if (kpImport is null || kpImport.IsEmpty())
        {
            // No key to import -> generate key on the fly.

            CryptoKeyUsage usages = algo == EcAlgorithm.ECDSA ?
                CryptoKeyUsage.Sign | CryptoKeyUsage.Verify :
                CryptoKeyUsage.DeriveKey | CryptoKeyUsage.DeriveBits;

            EcKeyGenParams gen = new(algo, namedCurve);

            ckp = await this.GenerateCryptoKeyPair(gen, usages, extractable)
                            .ConfigureAwait(false);
        }
        else
        {
            EcKeyImportParams imp = new(algo, namedCurve);
            ckp = await this.ImportKeyPair(imp, kpImport)
                            .ConfigureAwait(false);
        }

        if (ckp is null)
        {
            return null;
        }

        return this.CreateEcdsaInstanceFromAlgo<T>(algo, ckp);
    }

    /// <summary>
    /// Creates an instance from the given descriptor.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="keyPair"></param>
    /// <returns></returns>
    public T?
    Create<T>(CryptoKeyPairDescriptor keyPair)
        where T : EcBase
    {
        EcAlgorithm algo = this.EcAlgorithmFromType(typeof(T));
        return this.CreateEcdsaInstanceFromAlgo<T>(algo, keyPair);
    }

    /// <summary>
    /// Converts a type to the corresponding EcAlgorithm.
    /// </summary>
    /// <param name="t"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException">Type is not a supported EcAlgorithm.</exception>
    private EcAlgorithm
    EcAlgorithmFromType(Type t)
    {
        EcAlgorithm algo;

        if (t == typeof(Ecdsa))
        {
            algo = EcAlgorithm.ECDSA;
        }
        else if (t == typeof(Ecdh))
        {
            algo = EcAlgorithm.ECDH;
        }
        else
        {
            throw new ArgumentException($"Create on type {t} is not supported.");
        }

        return algo;
    }

    /// <summary>
    /// Creates an EC implementation for the given crypto key,
    /// using the given algorithm.
    /// </summary>
    /// <typeparam name="T">EC instance type.</typeparam>
    /// <param name="algo">EC algorithm to use.</param>
    /// <param name="keyPair">Key pair to use.</param>
    /// <returns>EC instance on success, null otherwise.</returns>
    /// <exception cref="ArgumentException">Type of T is not supported.</exception>
    private T?
    CreateEcdsaInstanceFromAlgo<T>(EcAlgorithm algo, CryptoKeyPairDescriptor keyPair)
        where T : EcBase
    {
        return algo switch
        {
            EcAlgorithm.ECDSA => new Ecdsa(
                this._crypto,
                keyPair,
                this._loggerFactory.CreateLogger<Ecdsa>()) as T,

            EcAlgorithm.ECDH => new Ecdh(
                this._crypto,
                keyPair,
                this._loggerFactory.CreateLogger<Ecdh>()) as T,

            _ => throw new ArgumentException($"Create on type {typeof(T)} is not supported."),
        };
    }
}
