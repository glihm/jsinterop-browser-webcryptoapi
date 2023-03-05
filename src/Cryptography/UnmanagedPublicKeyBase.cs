using System.Text;

using Glihm.JSInterop.Browser.WebCryptoAPI.Interfaces;
using Glihm.JSInterop.Browser.WebCryptoAPI.Interfaces.CryptoKeys;

namespace Glihm.JSInterop.Browser.WebCryptoAPI.Cryptography;

/// <summary>
/// Base for cryptography classes that will handle
/// unmanages resources from JS runtime for public-key
/// algorithms, which involve a crypto key pair.
/// </summary>
public abstract class UnmanagedPublicKeyBase : IAsyncDisposable
{
    /// <summary>
    /// Crypto WebAPI interop.
    /// </summary>
    protected readonly Crypto _crypto;

    /// <summary>
    /// JS interop  key pair.
    /// </summary>
    public CryptoKeyPairDescriptor KeyPair { get; }

    /// <summary>
    /// Constructor used internally..
    /// </summary>
    /// <param name="crypto">Crypto Web API instance.</param>
    /// <param name="keyPair">KeyPair.</param>
    protected UnmanagedPublicKeyBase(Crypto crypto, CryptoKeyPairDescriptor keyPair)
    {
        this._crypto = crypto;
        this.KeyPair = keyPair;
    }

    /// <summary>
    /// Gets the public key, or throw.
    /// </summary>
    /// <returns></returns>
    public CryptoKeyDescriptor
    GetPublicKeyOrThrow()
    {
        return this.KeyPair.GetPublicKeyOrThrow();
    }

    /// <summary>
    /// Gets the private key, or throw.
    /// </summary>
    /// <returns></returns>
    public CryptoKeyDescriptor
    GetPrivateKeyOrThrow()
    {
        return this.KeyPair.GetPrivateKeyOrThrow();
    }

    /// <summary>
    /// Exports the public key in the given format.
    /// </summary>
    /// <param name="format">Format of the returned key.</param>
    /// <param name="convertToPem">If true, the returned buffer are bytes of PEM formatted string.</param>
    /// <returns>Key buffer.</returns>
    public async Task<byte[]?>
    ExportPublicKey(CryptoKeyFormat format = CryptoKeyFormat.SubjectPublicKeyInfo,
                    bool convertToPem = false)
    {
        if (this.KeyPair.PublicKey is null)
        {
            return null;
        }

        byte[]? key = await this.KeyPair.PublicKey.Export(format)
                                                  .ConfigureAwait(false);
        if (key is null)
        {
            return null;
        }

        if (convertToPem)
        {
            string pem = PemHelper.PublicKeyPem(key);
            return Encoding.ASCII.GetBytes(pem);
        }
        else
        {
            return key;
        }
    }

    /// <summary>
    /// Exports the private key in the given format.
    /// </summary>
    /// <param name="format">Format of the returned key.</param>
    /// <param name="convertToPem">If true, the returned buffer are bytes of PEM formatted string.</param>
    /// <returns>Key buffer.</returns>
    public async Task<byte[]?>
    ExportPrivateKey(CryptoKeyFormat format = CryptoKeyFormat.PKCS8,
                     bool convertToPem = false)
    {
        if (this.KeyPair.PrivateKey is null)
        {
            return null;
        }

        byte[]? key = await this.KeyPair.PrivateKey.Export(format)
                                                   .ConfigureAwait(false);
        if (key is null)
        {
            return null;
        }

        if (convertToPem)
        {
            string pem = PemHelper.PrivateKeyPem(key);
            return Encoding.ASCII.GetBytes(pem);
        }
        else
        {
            return key;
        }
    }

    /// <summary>
    /// Exports the current public key to JSONWebKey format.
    /// </summary>
    /// <returns>String containing the JSON key.</returns>
    public async Task<string?>
    ExportPublicJSONWebKey()
    {
        if (this.KeyPair.PublicKey is null)
        {
            return null;
        }

        return await this.KeyPair.PublicKey.ExportJSONWebKey()
                                           .ConfigureAwait(false);
    }

    /// <summary>
    /// Exports the current private key to JSONWebKey format.
    /// </summary>
    /// <returns>String containing the JSON key.</returns>
    public async Task<string?>
    ExportPrivateJSONWebKey()
    {
        if (this.KeyPair.PrivateKey is null)
        {
            return null;
        }

        return await this.KeyPair.PrivateKey.ExportJSONWebKey()
                                            .ConfigureAwait(false);
    }

    /// <summary>
    /// Disposes unmanaged resources allocated for this
    /// crypto key in the JS runtime.
    /// </summary>
    /// <returns></returns>
    public async ValueTask
    DisposeAsync()
    {
        if (this.KeyPair.PublicKey is not null)
        {
            await this.KeyPair.PublicKey.DisposeAsync()
                                        .ConfigureAwait(false);
        }

        if (this.KeyPair.PrivateKey is not null)
        {
            await this.KeyPair.PrivateKey.DisposeAsync()
                                         .ConfigureAwait(false);
        }
    }
}
