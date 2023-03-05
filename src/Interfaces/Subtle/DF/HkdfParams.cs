using System.Text.Json.Serialization;

using Glihm.JSInterop.Browser.WebCryptoAPI.Interfaces.Subtle.Params;
using Glihm.JSInterop.Browser.WebCryptoAPI.Interfaces.Subtle.SHA;

namespace Glihm.JSInterop.Browser.WebCryptoAPI.Interfaces.Subtle.DF;

/// <summary>
/// HKDF params.
/// </summary>
public class HkdfParams : IDerivationParams
{
    /// <summary>
    /// Set to HKDF.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Digest function to use.
    /// </summary>
    public ShaAlgorithm Hash { get; }

    /// <summary>
    /// Salt.
    /// </summary>
    public byte[] Salt { get; }

    /// <summary>
    /// Application-specific contextual information.
    /// May be an empty array.
    /// </summary>
    public byte[] Info { get; }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="hash">Digest function to use.</param>
    /// <param name="salt">Salt.</param>
    /// <param name="info">Application-specific contextual information. Empty array if provided as null.</param>
    [JsonConstructor]
    public HkdfParams(ShaAlgorithm hash, byte[] salt, byte[]? info = null)
    {
        this.Name = "HKDF";
        this.Hash = hash;
        this.Salt = salt;
        this.Info = info ?? Array.Empty<byte>();
    }

}
