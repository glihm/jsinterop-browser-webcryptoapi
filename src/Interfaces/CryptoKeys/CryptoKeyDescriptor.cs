using System.Text.Json.Serialization;

using JSInterop.Browser.WebCryptoAPI.Interfaces.Subtle;
using JSInterop.Browser.WebCryptoAPI.JSHelpers;

namespace JSInterop.Browser.WebCryptoAPI.Interfaces.CryptoKeys;

/// <summary>
/// C# representation of a CryptoKey.
/// <br />
/// The whole JS CryptoKey structure is not required in the
/// library. Indeed, the CryptoKey live in JS runtime, and
/// the library only keeps the necessary information to
/// interact with the keys from .NET JSInterop.
/// <br />
/// CryptoKey are not aimed at being created by the library user.
/// Actually, only JSInterop is creating CryptoKeys to reflect
/// JS runtime generated/imported keys.
/// <br />
/// For this reason, the a crypto key must be manually disposed
/// to release unmanaged resources.
/// <see href="https://developer.mozilla.org/en-US/docs/Web/API/CryptoKey"/>
/// </summary>
public sealed class CryptoKeyDescriptor : IAsyncDisposable
{
    /// <summary>
    /// Identifier used to track the key in the JS runtime.
    /// Any call to JS via JSInterop use this identifier
    /// to correctly select the key to be used.
    /// </summary>
    public string Identifier { get; }

    /// <summary>
    /// Type of the key.
    /// </summary>
    public CryptoKeyType Type { get; }

    /// <summary>
    /// If the crytpo key can be exported.
    /// </summary>
    public bool Extractable { get; }

    /// <summary>
    /// SubtleCrypto instance that generated the crypto key.
    /// This reference is important to ensure unmanaged resources
    /// are completely released.
    /// </summary>
    [JsonIgnore]
    public SubtleCrypto? SubtleCryptoRef { get; private set; }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="identifier">CryptoKey JS runtime identifier.</param>
    /// <param name="type">Type.</param>
    /// <param name="extractable">Defines if the key may be exported later.</param>
    [JsonConstructor]
    public CryptoKeyDescriptor(string identifier, CryptoKeyType type, bool extractable)
    {
        this.Identifier = identifier;
        this.Type = type;
        this.Extractable = extractable;
    }

    /// <summary>
    /// Exports this crypto key to <see cref="CryptoKeyFormat.JSONWebKey"/> format.
    /// </summary>
    /// <returns>String representing the JSONWebKey.</returns>
    /// <exception cref="InvalidOperationException">Key is not extractable or no <see cref="SubtleCryptoRef"/> was set.</exception>
    public async Task<string?>
    ExportJSONWebKey()
    {
        if (this.SubtleCryptoRef is null || this.Extractable == false)
        {
            throw new InvalidOperationException(
                $"CryptoKey {this.Identifier} can't be exported." +
                $" Extractable is set to false, or no reference to subtle crypto was found.");
        }

        JSResultValue<string> res = await this.SubtleCryptoRef.ExportKeyJSONWebKey(this);
        if (!res)
        {
            // TODO: logging missing here...
            return null;
        }

        return res.GetValueOrThrow();

    }

    /// <summary>
    /// Exports this crypto key to the provided format but JSONWebKey.
    /// </summary>
    /// <param name="format">Crypto key format (except JSONWebKey).</param>
    /// <returns>Key in the provided format.</returns>
    /// <exception cref="InvalidOperationException">Key is not extractable or no <see cref="SubtleCryptoRef"/> was set.</exception>
    /// <exception cref="ArgumentException">Fromat was <see cref="CryptoKeyFormat.JSONWebKey"/>.</exception>
    public async Task<byte[]?>
    Export(CryptoKeyFormat format)
    {
        if (this.SubtleCryptoRef is null || this.Extractable == false)
        {
            throw new InvalidOperationException(
                $"CryptoKey {this.Identifier} can't be exported." +
                $" Extractable is set to false, or no reference to subtle crypto was found.");
        }

        if (format == CryptoKeyFormat.JSONWebKey)
        {
            throw new ArgumentException(
                "Wrong format. To export a crypto key to JSONWebKey format, " +
                "use ExportJSONWebKey overload instead.");
        }

        JSResultValue<byte[]> res = await this.SubtleCryptoRef.ExportKey(format, this);
        if (!res)
        {
            // TODO: logging missing here...
            return null;
        }

        return res.GetValueOrThrow();
    }

    /// <summary>
    /// Attaches a reference to a SubtleCrypto instance
    /// that generated they key to ensure correct
    /// disposal of the CryptoKey.
    /// </summary>
    /// <param name="subtleCrypto">SubtleCrypto instance that generated the CryptoKey.</param>
    /// <returns>This instance.</returns>
    public CryptoKeyDescriptor
    AttachSubtleCryptoRef(SubtleCrypto subtleCrypto)
    {
        this.SubtleCryptoRef = subtleCrypto;
        return this;
    }

    /// <summary>
    /// Disposes the unmanaged resources in JS runtime associated
    /// with the crypto key.
    /// </summary>
    /// <exception cref="ObjectDisposedException">
    /// Crypto key disposed without having a JS runtime attached.
    /// </exception>
    public async ValueTask
    DisposeAsync()
    {
        if (this.SubtleCryptoRef is null)
        {
            throw new ObjectDisposedException(
                $"Crypto key {this.Identifier} is disposed " +
                "but no SubtleCrypto instance is attached. Possible memory leak. " +
                "A Crypto key must always be attached to a SubtleCrypto instance when created.");
        }

        JSResultVoid delRes = await this.SubtleCryptoRef.DeleteKey(this);
        if (!delRes)
        {
            // ?? What we can do here then...? Logging?
        }
    }
}
