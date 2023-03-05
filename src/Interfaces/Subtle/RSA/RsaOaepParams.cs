using System.Text.Json.Serialization;

using Glihm.JSInterop.Browser.WebCryptoAPI.Interfaces.Subtle.Params;

namespace Glihm.JSInterop.Browser.WebCryptoAPI.Interfaces.Subtle.RSA;

/// <summary>
/// RSA OAEP params.
/// <br />
/// <see href="https://developer.mozilla.org/en-US/docs/Web/API/RsaOaepParams"/>.
/// </summary>
public class RsaOaepParams : IEncryptParams,
                             IDecryptParams,
                             IWrapParams,
                             IUnwrapParams
{
    /// <summary>
    /// Set to <see cref="RsaAlgorithm.OAEP"/>.
    /// </summary>
    public RsaAlgorithm Name { get; }

    /// <summary>
    /// Label bound to the ciphertext.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public byte[]? Label { get; }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="label">Optional label bound to the ciphertext.</param>
    public RsaOaepParams(byte[]? label = null)
    {
        this.Name = RsaAlgorithm.OAEP;
        this.Label = label;
    }

}
