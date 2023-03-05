using Glihm.JSInterop.Browser.WebCryptoAPI.Interfaces;
using Glihm.JSInterop.Browser.WebCryptoAPI.Interfaces.CryptoKeys;
using Glihm.JSInterop.Browser.WebCryptoAPI.Interfaces.Subtle.HMAC;
using Glihm.JSInterop.Browser.WebCryptoAPI.Interfaces.Subtle.SHA;

using Microsoft.Extensions.Logging;

namespace Glihm.JSInterop.Browser.WebCryptoAPI.Cryptography.HMAC;

/// <summary>
/// HMAC factory.
/// </summary>
public class HmacFactory : FactoryBase
{
    /// <summary>
    /// Loggers.
    /// </summary>
    private readonly ILogger<HmacFactory> _logger;

    /// <summary>
    /// Constructor dependency injection.
    /// </summary>
    /// <param name="crypto">Web Crypto API interop.</param>
    /// <param name="loggerFactory">Logger factory.</param>
    public HmacFactory(Crypto crypto, ILoggerFactory loggerFactory)
        : base(crypto, loggerFactory)
    {
        this._logger = loggerFactory.CreateLogger<HmacFactory>();
    }

    /// <summary>
    /// Creates HMAC instance from given key.
    /// Assumption is made that the key is in Raw format, for
    /// sign and verify operations.
    /// </summary>
    /// <param name="key">HMAC key.</param>
    /// <param name="hash">Digest function to use.</param>
    /// <param name="length">Length in bits of the key.</param>
    /// <param name="extractable">If the imported key can be extracted.</param>
    /// <returns></returns>
    public async Task<Hmac?>
    Create(byte[] key,
           ShaAlgorithm hash,
           int? length = null,
           bool extractable = false)
    {
        CryptoKeyImport ki = new(
            key,
            CryptoKeyFormat.Raw,
            CryptoKeyUsage.Sign | CryptoKeyUsage.Verify,
            extractable);

        return await this.Create(hash, length: length, keyImport: ki, extractable: extractable)
                         .ConfigureAwait(false);
    }

    /// <summary>
    /// Creates an instance from descriptor.
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public Hmac?
    Create(CryptoKeyDescriptor key)
    {
        return new Hmac(this._crypto, key, this._loggerFactory.CreateLogger<Hmac>());
    }

    /// <summary>
    /// Creates HMAC instance.
    /// </summary>
    /// <param name="hash">Digest function to use.</param>
    /// <param name="length">Length in bits of the key.</param>
    /// <param name="keyImport">Crypto key import parameters.</param>
    /// <param name="extractable">If keyImport is null, defines if the generated key can be exported.</param>
    /// <returns>HMAC instance on success, null otherwise.</returns>
    public async Task<Hmac?>
    Create(ShaAlgorithm hash,
           int? length = null,
           CryptoKeyImport? keyImport = null,
           bool extractable = false)
    {
        CryptoKeyDescriptor? ck;

        if (keyImport is null)
        {
            CryptoKeyUsage usages = CryptoKeyUsage.Sign | CryptoKeyUsage.Verify;
            HmacKeyGenParams gen = new(hash, length);
            ck = await this.GenerateCryptoKey(gen, usages, extractable)
                           .ConfigureAwait(false);
        }
        else
        {
            HmacImportParams imp = new HmacImportParams(hash, length);
            ck = await this.ImportKey(keyImport, imp)
                           .ConfigureAwait(false);
        }

        if (ck is null)
        {
            return null;
        }

        return new Hmac(this._crypto, ck, this._loggerFactory.CreateLogger<Hmac>());
    }

}
