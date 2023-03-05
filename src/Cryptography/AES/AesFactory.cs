using Glihm.JSInterop.Browser.WebCryptoAPI.Interfaces;
using Glihm.JSInterop.Browser.WebCryptoAPI.Interfaces.CryptoKeys;
using Glihm.JSInterop.Browser.WebCryptoAPI.Interfaces.Subtle.AES;
using Glihm.JSInterop.Browser.WebCryptoAPI.JSHelpers;

using Microsoft.Extensions.Logging;

namespace Glihm.JSInterop.Browser.WebCryptoAPI.Cryptography.AES;

/// <summary>
/// AES factory.
/// <br />
/// This class aims at being injected in a project,
/// to then create any AES mode implementation
/// that is available using Crypto Web API.
/// </summary>
public class AesFactory : FactoryBase
{
    /// <summary>
    /// Loggers.
    /// </summary>
    private readonly ILogger<AesFactory> _logger;

    /// <summary>
    /// Constructor dependency injection.
    /// </summary>
    /// <param name="crypto">Web Crypto API interop.</param>
    /// <param name="loggerFactory">Logger factory.</param>
    public AesFactory(Crypto crypto, ILoggerFactory loggerFactory)
        : base(crypto, loggerFactory)
    {
        this._logger = loggerFactory.CreateLogger<AesFactory>();
    }

    /// <summary>
    /// Generates a random initialization vector of the given length.
    /// </summary>
    /// <param name="byteCount">Length in byte of the initialization vector.</param>
    /// <returns>Buffer with random values on success, null otherwise.</returns>
    public async Task<byte[]?>
    GenerateRandomIv(int byteCount)
    {
        JSResultValue<byte[]> randRes = await this._crypto.GetRandomValues(byteCount)
                                                          .ConfigureAwait(false);
        if (!randRes)
        {
            return null;
        }

        return randRes.GetValueOrThrow();
    }

    /// <summary>
    /// Creates an instance of the required AES implementation.
    /// This method constructs the AES implementation with most often
    /// used parameters:
    /// 1. Usages are <see cref="CryptoKeyUsage.Encrypt"/> and <see cref="CryptoKeyUsage.Decrypt"/>.
    /// 2. Format of the key is <see cref="CryptoKeyFormat.Raw"/>.
    /// </summary>
    /// <param name="key">AES key.</param>
    /// <returns>AES instance on success, null otherwise.</returns>
    public async Task<T?>
    Create<T>(byte[] key, bool extractable = false)
        where T : AesBase
    {
        CryptoKeyImport cki = new(
            key,
            CryptoKeyFormat.Raw,
            CryptoKeyUsage.Encrypt | CryptoKeyUsage.Decrypt,
            extractable);

        if (typeof(T) == typeof(AesGcm))
        {
            return await this.Create<AesGcm>(cki)
                             .ConfigureAwait(false) as T;
        }
        else if (typeof(T) == typeof(AesCtr))
        {
            return await this.Create<AesCtr>(cki)
                             .ConfigureAwait(false) as T;
        }
        else if (typeof(T) == typeof(AesCbc))
        {
            return await this.Create<AesCbc>(cki)
                             .ConfigureAwait(false) as T;
        }
        else
        {
            throw new ArgumentException($"Create on type {typeof(T)} is not supported.");
        }
    }

    /// <summary>
    /// Creates an instance of the required AES implementation by generating the key.
    /// </summary>
    /// <param name="keyLength">Length of the key in bits.</param>
    /// <param name="usages">CryptoKey usages.</param>
    /// <param name="extractable">If the key can later be exported.</param>
    /// <returns>AES instance on success, null otherwise.</returns>
    public async Task<T?>
    Create<T>(int keyLength = 256,
              CryptoKeyUsage usages = CryptoKeyUsage.Encrypt | CryptoKeyUsage.Decrypt,
              bool extractable = false)
        where T : AesBase
    {
        AesAlgorithm algo = this.AesAlgorithmFromType(typeof(T));

        AesKeyGenParams gen = new(algo, keyLength);
        CryptoKeyDescriptor? ck = await this.GenerateCryptoKey(gen, usages, extractable)
                                            .ConfigureAwait(false);
        if (ck is null)
        {
            return null;
        }

        return this.CreateAesInstanceFromAlgo<T>(algo, ck);
    }

    /// <summary>
    /// Creates an AES instance from the existing key and it's
    /// parameters.
    /// </summary>
    /// <typeparam name="T">AES instance to create.</typeparam>
    /// <param name="keyImport">Key import parameters.</param>
    /// <returns>AES instance on success, null otherwise.</returns>
    /// <exception cref="ArgumentException">Type of T is not supported.</exception>
    public async Task<T?>
    Create<T>(CryptoKeyImport keyImport)
        where T : AesBase
    {
        AesAlgorithm algo = this.AesAlgorithmFromType(typeof(T));

        CryptoKeyDescriptor? ck = await this.ImportKey(keyImport, new AesImportParams(algo))
                                            .ConfigureAwait(false);
        if (ck is null)
        {
            return null;
        }

        return this.CreateAesInstanceFromAlgo<T>(algo, ck);
    }

    /// <summary>
    /// Creates an instance from the given descriptor.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <returns></returns>
    public T?
    Create<T>(CryptoKeyDescriptor key)
        where T : AesBase
    {
        AesAlgorithm algo = this.AesAlgorithmFromType(typeof(T));
        return this.CreateAesInstanceFromAlgo<T>(algo, key);
    }

    /// <summary>
    /// Converts a type to the corresponding AesAlgorithm.
    /// </summary>
    /// <param name="t"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException">Type is not a supported AesAlgorithm.</exception>
    private AesAlgorithm
    AesAlgorithmFromType(Type t)
    {
        AesAlgorithm algo;

        if (t == typeof(AesGcm))
        {
            algo = AesAlgorithm.GCM;
        }
        else if (t == typeof(AesCtr))
        {
            algo = AesAlgorithm.CTR;
        }
        else if (t == typeof(AesCbc))
        {
            algo = AesAlgorithm.CBC;
        }
        else
        {
            throw new ArgumentException($"Create on type {t} is not supported.");
        }

        return algo;
    }

    /// <summary>
    /// Creates an AES implementation for the given crypto key,
    /// using the given algorithm.
    /// </summary>
    /// <typeparam name="T">AES instance type.</typeparam>
    /// <param name="algo">AES algorithm to use.</param>
    /// <param name="key">Key to use.</param>
    /// <returns>Aes instance on success, null otherwise.</returns>
    /// <exception cref="ArgumentException">Type of T is not supported.</exception>
    private T?
    CreateAesInstanceFromAlgo<T>(AesAlgorithm algo, CryptoKeyDescriptor key)
        where T : AesBase
    {
        return algo switch
        {
            AesAlgorithm.GCM => new AesGcm(
                this._crypto,
                key,
                this._loggerFactory.CreateLogger<AesGcm>()) as T,

            AesAlgorithm.CBC => new AesCbc(
                this._crypto,
                key,
                this._loggerFactory.CreateLogger<AesCbc>()) as T,

            AesAlgorithm.CTR => new AesCtr(
                this._crypto,
                key,
                this._loggerFactory.CreateLogger<AesCtr>()) as T,

            _ => throw new ArgumentException($"Create on type {typeof(T)} is not supported."),
        };
    }

}
