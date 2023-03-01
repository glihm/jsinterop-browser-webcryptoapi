using System.Text.Json.Serialization;

using JSInterop.Browser.WebCryptoAPI.Interfaces.Subtle.Params;
using JSInterop.Browser.WebCryptoAPI.Interfaces.Subtle.SHA;

namespace JSInterop.Browser.WebCryptoAPI.Interfaces.Subtle.HMAC;

/// <summary>
/// HMAC import params.
/// <br />
/// <see href="https://developer.mozilla.org/en-US/docs/Web/API/HmacKeyGenParams"/>.
/// </summary>
public class HmacImportParams : IImportParams,
                                IUnwrappedKeyParams
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
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? Length { get; }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="hash">Digest function to use.</param>
    /// <param name="length">Key length in bits.</param>
    public HmacImportParams(ShaAlgorithm hash, int? length = null)
    {
        this.Name = "HMAC";
        this.Hash = hash;
        this.Length = length;
    }
}
