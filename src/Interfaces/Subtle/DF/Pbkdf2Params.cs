using System.Text.Json.Serialization;

using JSInterop.Browser.WebCryptoAPI.Interfaces.Subtle.Params;
using JSInterop.Browser.WebCryptoAPI.Interfaces.Subtle.SHA;

namespace JSInterop.Browser.WebCryptoAPI.Interfaces.Subtle.DF;

/// <summary>
/// PBKDF2 params.
/// </summary>
public class Pbkdf2Params : IDerivationParams
{
    /// <summary>
    /// Set to PBKDF2.
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
    /// Number of times the hash function will be executed.
    /// </summary>
    public int Iterations { get; }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="hash">Digest function to use.</param>
    /// <param name="salt">Salt.</param>
    /// <param name="iterations">Number of times the hash function will be executed.</param>
    [JsonConstructor]
    public Pbkdf2Params(ShaAlgorithm hash, byte[] salt, int iterations)
    {
        this.Name = "PBKDF2";
        this.Hash = hash;
        this.Salt = salt;
        this.Iterations = iterations;
    }

}
