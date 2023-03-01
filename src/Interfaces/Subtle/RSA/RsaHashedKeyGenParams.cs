using System.Text.Json.Serialization;

using JSInterop.Browser.WebCryptoAPI.Interfaces.Subtle.Params;
using JSInterop.Browser.WebCryptoAPI.Interfaces.Subtle.SHA;

namespace JSInterop.Browser.WebCryptoAPI.Interfaces.Subtle.RSA;

/// <summary>
/// RSA generator params.
/// <br />
/// <see href="https://developer.mozilla.org/en-US/docs/Web/API/RsaHashedKeyGenParams"/>.
/// </summary>
public class RsaHashedKeyGenParams : IGenParams
{
    /// <summary>
    /// Algorithm to use.
    /// </summary>
    public RsaAlgorithm Name { get; }

    /// <summary>
    /// The length in bits of the RSA modulus.
    /// </summary>
    public int ModulusLength { get; }

    /// <summary>
    /// Public exponent, [0x01, 0x00, 0x01] if no value passed.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public byte[] PublicExponent { get; }

    /// <summary>
    /// Digest function to use.
    /// </summary>
    public ShaAlgorithm Hash { get; }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="name">Algorithm to use.</param>
    /// <param name="modulusLength">Modulus length.</param>
    /// <param name="hash">Digest function to use.</param>
    /// <param name="publicExponent">Public exponent.</param>
    [JsonConstructor]
    public RsaHashedKeyGenParams(RsaAlgorithm name,
                                 int modulusLength,
                                 ShaAlgorithm hash,
                                 byte[]? publicExponent = null)
    {
        this.Name = name;
        this.ModulusLength = modulusLength;
        this.Hash = hash;
        this.PublicExponent = publicExponent ?? new byte[] { 1, 0, 1 };
    }

}
