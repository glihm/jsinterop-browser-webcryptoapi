using System.Text.Json.Serialization;

using JSInterop.Browser.WebCryptoAPI.Interfaces.Subtle.Params;

namespace JSInterop.Browser.WebCryptoAPI.Interfaces.Subtle.AES;

/// <summary>
/// AES generator params.
/// <br />
/// <see href="https://developer.mozilla.org/en-US/docs/Web/API/AesKeyGenParams"/>
/// </summary>
public class AesKeyGenParams : IGenParams,
                               IDerivedKeyParams
{
    /// <summary>
    /// Algorithm to use.
    /// </summary>
    public AesAlgorithm Name { get; }

    /// <summary>
    /// Length in bits of the key to generate.
    /// Must be 128, 192 or 256.
    /// </summary>
    public int Length { get; }

    /// <summary>
    /// Length in bits of the key, allowed by the CryptoSubtle API.
    /// </summary>
    private static readonly int[] _keyLengths = { 128, 192, 256 };

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="name">Algorithm to use.</param>
    /// <param name="length">Length in bits of the key to generate.</param>
    [JsonConstructor]
    public AesKeyGenParams(AesAlgorithm name, int length)
    {
        if (!_keyLengths.Contains(length))
        {
            string lengths = string.Join(", ", _keyLengths);
            throw new ArgumentOutOfRangeException($"AES key's length must be one of {lengths}.");
        }

        this.Name = name;
        this.Length = length;
    }
}
