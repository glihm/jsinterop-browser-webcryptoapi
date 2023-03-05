using System.Text.Json.Serialization;

using Glihm.JSInterop.Browser.WebCryptoAPI.Interfaces.Subtle.Params;
using Glihm.JSInterop.Browser.WebCryptoAPI.Interfaces.Subtle.SHA;

namespace Glihm.JSInterop.Browser.WebCryptoAPI.Interfaces.Subtle.HMAC;


/// <summary>
/// HMAC generator params.
/// <br />
/// <see href="https://developer.mozilla.org/en-US/docs/Web/API/HmacKeyGenParams"/>.
/// </summary>
public class HmacKeyGenParams : IGenParams,
                                IDerivedKeyParams
{
    /// <summary>
    /// Set to HMAC.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Digest function to use.
    /// </summary>
    public ShaAlgorithm Hash { get; }

    /// <summary>
    /// Length in bits of the key.
    /// If this is omitted, the length of the key is equal to
    /// the block size of the hash function you have chosen
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? Length { get; }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="hash"></param>
    /// <param name="length"></param>
    [JsonConstructor]
    public HmacKeyGenParams(ShaAlgorithm hash, int? length = null)
    {
        this.Name = "HMAC";
        this.Hash = hash;
        this.Length = length;
    }
}
