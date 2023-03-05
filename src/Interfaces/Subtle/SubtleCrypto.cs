using Glihm.JSInterop.Browser.WebCryptoAPI.Interfaces.CryptoKeys;
using Glihm.JSInterop.Browser.WebCryptoAPI.Interfaces.Subtle.Params;
using Glihm.JSInterop.Browser.WebCryptoAPI.Interfaces.Subtle.SHA;
using Glihm.JSInterop.Browser.WebCryptoAPI.JSHelpers;

using Microsoft.JSInterop;

namespace Glihm.JSInterop.Browser.WebCryptoAPI.Interfaces.Subtle;

/// <summary>
/// Implements Web Crypto API Subtle.
/// <see href="https://developer.mozilla.org/en-US/docs/Web/API/SubtleCrypto" />
/// <br />
/// The key managment is done exclusively on JS runtime, and C# uses
/// it's own internal CryptoKey representation to work with Subtle,
/// without the need to export the CryptoKey from the JS runtime.
/// </summary>
public class SubtleCrypto : JSModule
{
    /// <summary>
    /// Ctor DI.
    /// </summary>
    /// <param name="jsRuntime"></param>
    public SubtleCrypto(IJSRuntime jsRuntime)
        : base(jsRuntime, "./_content/Glihm.JSInterop.Browser.WebCryptoAPI/SubtleCrypto.js")
    {
    }

    /// <summary>
    /// Generates a new crytpo key (symmetric algorithms).
    /// <br />
    /// <see href="https://developer.mozilla.org/en-US/docs/Web/API/SubtleCrypto/generateKey" />
    /// </summary>
    /// <param name="algorithm">Algorithm specific parameters.</param>
    /// <param name="extractable">A boolean value indicating whether it will be possible to export the key using.</param>
    /// <param name="usages">What can be done with the newly generated key.</param>
    /// <returns>Generated key's descriptor on success.</returns>
    public async ValueTask<JSResultValue<CryptoKeyDescriptor>>
    GenerateKey(IGenParams algorithm,
                bool extractable,
                CryptoKeyUsage usages)
    {
        JSResultValue<CryptoKeyDescriptor[]> res = await this.ModuleInvokeAsync<CryptoKeyDescriptor[]>(
            "generateKey",
            algorithm,
            extractable,
            usages)
            .ConfigureAwait(false);

        if (!res)
        {
            return new JSResultValue<CryptoKeyDescriptor>(res);
        }

        CryptoKeyDescriptor d = res.GetValueOrThrow()[0]
                                   .AttachSubtleCryptoRef(this);

        return new JSResultValue<CryptoKeyDescriptor>(d);
    }

    /// <summary>
    /// Generates a new crytpo key pair (public key algorithms).
    /// <br />
    /// <see href="https://developer.mozilla.org/en-US/docs/Web/API/SubtleCrypto/generateKey" />
    /// </summary>
    /// <param name="algorithm">Algorithm specific parameters.</param>
    /// <param name="extractable">A boolean value indicating whether it will be possible to export the key.</param>
    /// <param name="usages">What can be done with the newly generated key.</param>
    /// <returns>Generated key's descriptor on success.</returns>
    public async ValueTask<JSResultValue<CryptoKeyPairDescriptor>>
    GenerateKeyPair(IGenParams algorithm, bool extractable, CryptoKeyUsage usages)
    {
        JSResultValue<CryptoKeyDescriptor[]> res = await this.ModuleInvokeAsync<CryptoKeyDescriptor[]>(
            "generateKey",
            algorithm,
            extractable,
            usages)
            .ConfigureAwait(false);

        if (!res)
        {
            return new JSResultValue<CryptoKeyPairDescriptor>(res);
        }

        CryptoKeyDescriptor[] keys = res.GetValueOrThrow();
        CryptoKeyPairDescriptor kp = new(
            keys[0].AttachSubtleCryptoRef(this),
            keys[1].AttachSubtleCryptoRef(this));

        return new JSResultValue<CryptoKeyPairDescriptor>(kp);
    }

    /// <summary>
    /// Encrypts the given data buffer.
    /// <br />
    /// <see href="https://developer.mozilla.org/en-US/docs/Web/API/SubtleCrypto/encrypt" />
    /// </summary>
    /// <param name="algorithm">Algorithm specific parameters.</param>
    /// <param name="key">Key to be used.</param>
    /// <param name="data">Plaintext buffer to be encrypted.</param>
    /// <returns>The ciphertext on success.</returns>
    public async ValueTask<JSResultValue<byte[]>>
    Encrypt(IEncryptParams algorithm, CryptoKeyDescriptor key, byte[] data)
    {
        return await this.ModuleInvokeAsync<byte[]>(
            "encrypt",
            algorithm,
            key,
            data)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Decrypts the given data buffer with the key associated with
    /// the given identifier.
    /// <br />
    /// <see href="https://developer.mozilla.org/en-US/docs/Web/API/SubtleCrypto/decrypt" />
    /// </summary>
    /// <param name="algorithm">Algorithm specific parameters.</param>
    /// <param name="key">Key to be used.</param>
    /// <param name="data">Ciphertext buffer to be decrypted.</param>
    /// <returns>The plaintext on success.</returns>
    public async ValueTask<JSResultValue<byte[]>>
    Decrypt(IDecryptParams algorithm, CryptoKeyDescriptor key, byte[] data)
    {
        return await this.ModuleInvokeAsync<byte[]>(
            "decrypt",
            algorithm,
            key,
            data)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Signs (generated digital signature) for the given data, using the
    /// key associated with the key identifier.
    /// <br />
    /// <see href="https://developer.mozilla.org/en-US/docs/Web/API/SubtleCrypto/sign" />
    /// </summary>
    /// <param name="algorithm">Algorithm specific parameters.</param>
    /// <param name="key">Key to be used.</param>
    /// <param name="data">Data to be signed.</param>
    /// <returns>Signature buffer on success.</returns>
    public async ValueTask<JSResultValue<byte[]>>
    Sign(ISignParams algorithm, CryptoKeyDescriptor key, byte[] data)
    {
        return await this.ModuleInvokeAsync<byte[]>(
            "sign",
            algorithm,
            key,
            data)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Verifies a digital signature for the given data, using the
    /// key associated with the key identifier.
    /// <br />
    /// <see href="https://developer.mozilla.org/en-US/docs/Web/API/SubtleCrypto/verify" />
    /// </summary>
    /// <param name="algorithm">Algorithm specific parameters.</param>
    /// <param name="key">Key to be used.</param>
    /// <param name="signature">Signature to verify.</param>
    /// <param name="data">Data to be signed.</param>
    /// <returns>True if the signature is valid, false otherwise.</returns>
    public async ValueTask<JSResultValue<bool>>
    Verify(IVerifyParams algorithm,
           CryptoKeyDescriptor key,
           byte[] signature,
           byte[] data)
    {
        return await this.ModuleInvokeAsync<bool>(
            "verify",
            algorithm,
            key,
            signature,
            data)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Imports key from a buffer.
    /// <br />
    /// <see href="https://developer.mozilla.org/en-US/docs/Web/API/SubtleCrypto/importKey" />
    /// </summary>
    /// <param name="format">CrytoKey format.</param>
    /// <param name="keyData">Key data as a buffer.</param>
    /// <param name="algorithm">Algorithm specific parameters.</param>
    /// <param name="extractable">A boolean value indicating whether it will be possible to export the key.</param>
    /// <param name="usages">What can be done with the newly generated key.</param>
    /// <returns>Imported key's descriptor on success.</returns>
    /// <exception cref="ArgumentException">Format is JSONWebKey, which is not supported from a buffer.</exception>
    public async ValueTask<JSResultValue<CryptoKeyDescriptor>>
    ImportKey(CryptoKeyFormat format,
              byte[] keyData,
              IImportParams algorithm,
              bool extractable,
              CryptoKeyUsage usages)
    {
        if (format == CryptoKeyFormat.JSONWebKey)
        {
            throw new ArgumentException(
                "JSONWebKey format is not supported from a buffer. " +
                "Please use the ImportKey overload with keyData as a string instead.");
        }

        JSResultValue<CryptoKeyDescriptor> importRes = await this.ModuleInvokeAsync<CryptoKeyDescriptor>(
            "importKey",
            format,
            keyData,
            algorithm,
            extractable,
            usages)
            .ConfigureAwait(false);

        if (importRes)
        {
            importRes.Value?.AttachSubtleCryptoRef(this);
        }

        return importRes;
    }

    /// <summary>
    /// Imports key from a string.
    /// <br />
    /// <see href="https://developer.mozilla.org/en-US/docs/Web/API/SubtleCrypto/importKey" />
    /// </summary>
    /// <param name="format">CrytoKey format.</param>
    /// <param name="keyData">Key data as a buffer.</param>
    /// <param name="algorithm">Algorithm specific parameters.</param>
    /// <param name="extractable">A boolean value indicating whether it will be possible to export the key.</param>
    /// <param name="usages">What can be done with the newly generated key.</param>
    /// <returns>Imported key's descriptor on success.</returns>
    /// <exception cref="ArgumentException">Format is not JSONWebKey, which is the only format to be imported from a string.</exception>
    public async ValueTask<JSResultValue<CryptoKeyDescriptor>>
    ImportKey(CryptoKeyFormat format,
              string keyData,
              IImportParams algorithm,
              bool extractable,
              CryptoKeyUsage usages)
    {
        if (format != CryptoKeyFormat.JSONWebKey)
        {
            throw new ArgumentException(
                $"{format} format is not supported from a string, only JSONWebKey. " +
                "Please use the ImportKey overload with keyData as a buffer instead.");
        }

        JSResultValue<CryptoKeyDescriptor> importRes = await this.ModuleInvokeAsync<CryptoKeyDescriptor>(
            "importKey",
            format,
            keyData,
            algorithm,
            extractable,
            usages)
            .ConfigureAwait(false);

        if (importRes)
        {
            importRes.Value?.AttachSubtleCryptoRef(this);
        }

        return importRes;
    }

    /// <summary>
    /// Exports a key to JSONWebKey format.
    /// <br />
    /// <see href="https://developer.mozilla.org/en-US/docs/Web/API/SubtleCrypto/exportKey" />
    /// </summary>
    /// <param name="key">Key to be exported.</param>
    /// <returns>String containing JSONWebKey formatted key on success.</returns>
    public async ValueTask<JSResultValue<string>>
    ExportKeyJSONWebKey(CryptoKeyDescriptor key)
    {
        return await this.ModuleInvokeAsync<string>(
            "exportKey",
            "jwk",
            key)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Exports a key to the provided buffer format but <see cref="CryptoKeyFormat.JSONWebKey"/>.
    /// <br />
    /// <see href="https://developer.mozilla.org/en-US/docs/Web/API/SubtleCrypto/exportKey" />
    /// </summary>
    /// <param name="format">Buffer format in which the key must be exported (but JSONWebKey).</param>
    /// <param name="key">Key to be exported.</param>
    /// <returns>Buffer with the key in the specified format on success.</returns>
    /// <exception cref="InvalidOperationException">Provided format is <see cref="CryptoKeyFormat.JSONWebKey"/></exception>
    public async ValueTask<JSResultValue<byte[]>>
    ExportKey(CryptoKeyFormat format, CryptoKeyDescriptor key)
    {
        if (format == CryptoKeyFormat.JSONWebKey)
        {
            throw new InvalidOperationException("CryptoKey can't be exported in a buffer if the format is JSONWebKey.");
        }

        return await this.ModuleInvokeAsync<byte[]>(
            "exportKey",
            format,
            key)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Wraps a key.
    /// <br />
    /// <see href="https://developer.mozilla.org/en-US/docs/Web/API/SubtleCrypto/wrapKey" />
    /// </summary>
    /// <param name="format">Format of the exported key before it is encrypted.</param>
    /// <param name="key">Key to wrap.</param>
    /// <param name="wrappingKey">Key used to encrypt the wrap result.</param>
    /// <param name="wrapAlgo">Wrap algorithm.</param>
    /// <returns></returns>
    public async ValueTask<JSResultValue<byte[]>>
    WrapKey(CryptoKeyFormat format,
            CryptoKeyDescriptor key,
            CryptoKeyDescriptor wrappingKey,
            IWrapParams wrapAlgo)
    {
        if (!key.Extractable)
        {
            throw new InvalidOperationException("Key to wrap must be extractable.");
        }

        return await this.ModuleInvokeAsync<byte[]>(
            "wrapKey",
            format,
            key,
            wrappingKey,
            wrapAlgo)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Unwraps a key.
    /// <br />
    /// <see href="https://developer.mozilla.org/en-US/docs/Web/API/SubtleCrypto/unwrapKey" />
    /// </summary>
    /// <param name="format">Format of the encrypted key.</param>
    /// <param name="wrappedKey">Buffer containing the wrapped key in the given format.</param>
    /// <param name="unwrappingKey">Key to decrypt the wrapped key.</param>
    /// <param name="unwrapAlgo"></param>
    /// <param name="unwrappedKeyAlgo"></param>
    /// <param name="extractable"></param>
    /// <param name="usages"></param>
    /// <returns></returns>
    public async ValueTask<JSResultValue<CryptoKeyDescriptor>>
    UnwrapKey(CryptoKeyFormat format,
              byte[] wrappedKey,
              CryptoKeyDescriptor unwrappingKey,
              IUnwrapParams unwrapAlgo,
              IUnwrappedKeyParams unwrappedKeyAlgo,
              bool extractable,
              CryptoKeyUsage usages)
    {
        return await this.ModuleInvokeAsync<CryptoKeyDescriptor>(
            "unwrapKey",
            format,
            wrappedKey,
            unwrappingKey,
            unwrapAlgo,
            unwrappedKeyAlgo,
            extractable,
            usages)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Derives a secret key from the given master key.
    /// </summary>
    /// <param name="algorithm">Derivation algorithm to use.</param>
    /// <param name="baseKey">Master key to use.</param>
    /// <param name="derivedKeyAlgorithm">Algorithm the derive key will be used for.</param>
    /// <param name="extractable">Whether it will be possible to export the key.</param>
    /// <param name="usages">Key usages.</param>
    /// <returns>CryptoKey on success.</returns>
    public async ValueTask<JSResultValue<CryptoKeyDescriptor>>
    DeriveKey(IDerivationParams algorithm,
              CryptoKeyDescriptor baseKey,
              IDerivedKeyParams derivedKeyAlgorithm,
              bool extractable,
              CryptoKeyUsage usages)
    {
        return await this.ModuleInvokeAsync<CryptoKeyDescriptor>(
            "deriveKey",
            algorithm,
            baseKey,
            derivedKeyAlgorithm,
            extractable,
            usages)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Derives array of bits from a base key.
    /// </summary>
    /// <param name="algorithm">Derivation algorithm to use.</param>
    /// <param name="baseKey">Base key as input for the derivation.</param>
    /// <param name="length">Number of bits to derive. Should be multiple of 8.</param>
    /// <returns>Array of derived bits on success.</returns>
    public async ValueTask<JSResultValue<byte[]>>
    DeriveBits(IDerivationParams algorithm, CryptoKeyDescriptor baseKey, int length)
    {
        return await this.ModuleInvokeAsync<byte[]>(
            "deriveBits",
            algorithm,
            baseKey,
            length)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Generates a digest of the given data.
    /// <br />
    /// <see href="https://developer.mozilla.org/en-US/docs/Web/API/SubtleCrypto/digest" />
    /// </summary>
    /// <param name="algorithm">Hash function to be used.</param>
    /// <param name="data">Data to be digested.</param>
    /// <returns>Digest buffer on success.</returns>
    public async ValueTask<JSResultValue<byte[]>>
    Digest(ShaAlgorithm algorithm, byte[] data)
    {
        return await this.ModuleInvokeAsync<byte[]>(
            "digest",
            algorithm,
            data)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Deletes unmanaged resource in JS runtime
    /// related to the given crypto key.
    /// </summary>
    /// <param name="key">Key to delete.</param>
    /// <returns></returns>
    public async ValueTask<JSResultVoid>
    DeleteKey(CryptoKeyDescriptor key)
    {
        return await this.ModuleInvokeVoidAsync("deleteKey", key)
                         .ConfigureAwait(false);
    }

    /// <summary>
    /// Dumps keys present in the JS module, debug only.
    /// </summary>
    /// <returns></returns>
    public async ValueTask<JSResultVoid>
    DumpKeys()
    {
#if DEBUG
        string fn = "dumpKeys";
#else
        string fn = "";
#endif
        return await this.ModuleInvokeVoidAsync(fn)
                         .ConfigureAwait(false);
    }
}
