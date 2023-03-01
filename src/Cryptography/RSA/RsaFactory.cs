using JSInterop.Browser.WebCryptoAPI.Interfaces;
using JSInterop.Browser.WebCryptoAPI.Interfaces.CryptoKeys;
using JSInterop.Browser.WebCryptoAPI.Interfaces.Subtle.RSA;
using JSInterop.Browser.WebCryptoAPI.Interfaces.Subtle.SHA;

using Microsoft.Extensions.Logging;

namespace JSInterop.Browser.WebCryptoAPI.Cryptography.RSA;

/// <summary>
/// RSA factory.
/// </summary>
public class RsaFactory : FactoryBase
{
    /// <summary>
    /// Loggers.
    /// </summary>
    private readonly ILogger<RsaFactory> _logger;

    /// <summary>
    /// Constructor dependency injection.
    /// </summary>
    /// <param name="crypto">Web Crypto API interop.</param>
    /// <param name="loggerFactory">Logger factory.</param>
    public RsaFactory(Crypto crypto, ILoggerFactory loggerFactory)
        : base(crypto, loggerFactory)
    {
        this._logger = loggerFactory.CreateLogger<RsaFactory>();
    }

    /// <summary>
    /// Creates RSA instance.
    /// </summary>
    /// <param name="modulusLength">Length in bits of the RSA modulus.</param>
    /// <param name="hash">Digest function to use.</param>
    /// <param name="publicExponent">Public exponent.</param>
    /// <param name="kpImport">Crypto key pair import parameters.</param>
    /// <param name="extractable">If not kpImport is provided, defines if the genertated keys can be exported later.</param>
    /// <returns>RSA instance on success, null otherwise.</returns>
    public async Task<T?>
    Create<T>(int modulusLength,
              ShaAlgorithm hash,
              byte[]? publicExponent = null,
              CryptoKeyPairImport? kpImport = null,
              bool extractable = false)
        where T : RsaBase
    {
        RsaAlgorithm algo = this.RsaAlgorithmFromType(typeof(T));

        CryptoKeyPairDescriptor? ckp;

        if (kpImport is null || kpImport.IsEmpty())
        {
            // No key to import -> generate key on the fly.

            CryptoKeyUsage usages = algo == RsaAlgorithm.OAEP ?
                CryptoKeyUsage.Encrypt | CryptoKeyUsage.Decrypt :
                CryptoKeyUsage.Sign | CryptoKeyUsage.Verify;

            RsaHashedKeyGenParams gen = new(algo, modulusLength, hash, publicExponent);

            ckp = await this.GenerateCryptoKeyPair(gen, usages, extractable)
                            .ConfigureAwait(false);
        }
        else
        {
            RsaHashedImportParams imp = new(algo, hash);
            ckp = await this.ImportKeyPair(imp, kpImport)
                            .ConfigureAwait(false);
        }

        if (ckp is null)
        {
            return null;
        }

        return this.CreateRsaInstanceFromAlgo<T>(algo, ckp);
    }

    /// <summary>
    /// Creates an instance from the given descriptor.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="keyPair"></param>
    /// <returns></returns>
    public T?
    Create<T>(CryptoKeyPairDescriptor keyPair)
        where T : RsaBase
    {
        RsaAlgorithm algo = this.RsaAlgorithmFromType(typeof(T));
        return this.CreateRsaInstanceFromAlgo<T>(algo, keyPair);
    }

    /// <summary>
    /// Converts a type to the corresponding RsaAlgorithm.
    /// </summary>
    /// <param name="t"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException">Type is not a supported RsaAlgorithm.</exception>
    private RsaAlgorithm
    RsaAlgorithmFromType(Type t)
    {
        RsaAlgorithm algo;

        if (t == typeof(RsaOaep))
        {
            algo = RsaAlgorithm.OAEP;
        }
        else if (t == typeof(RsaPss))
        {
            algo = RsaAlgorithm.PSS;
        }
        else if (t == typeof(RsaSsa))
        {
            algo = RsaAlgorithm.SSA_PKCS1_v1_5;
        }
        else
        {
            throw new ArgumentException($"Create on type {t} is not supported.");
        }

        return algo;
    }

    /// <summary>
    /// Creates an RSA implementation for the given crypto key,
    /// using the given algorithm.
    /// </summary>
    /// <typeparam name="T">RSA instance type.</typeparam>
    /// <param name="algo">RSA algorithm to use.</param>
    /// <param name="keyPair">Key pair to use.</param>
    /// <returns>RSA instance on success, null otherwise.</returns>
    /// <exception cref="ArgumentException">Type of T is not supported.</exception>
    private T?
    CreateRsaInstanceFromAlgo<T>(RsaAlgorithm algo, CryptoKeyPairDescriptor keyPair)
        where T : RsaBase
    {
        return algo switch
        {
            RsaAlgorithm.OAEP => new RsaOaep(
                this._crypto,
                keyPair,
                this._loggerFactory.CreateLogger<RsaOaep>()) as T,

            RsaAlgorithm.PSS => new RsaPss(
                this._crypto,
                keyPair,
                this._loggerFactory.CreateLogger<RsaPss>()) as T,

            RsaAlgorithm.SSA_PKCS1_v1_5 => new RsaSsa(
                this._crypto,
                keyPair,
                this._loggerFactory.CreateLogger<RsaSsa>()) as T,

            _ => throw new ArgumentException($"Create on type {typeof(T)} is not supported."),
        };
    }
}
