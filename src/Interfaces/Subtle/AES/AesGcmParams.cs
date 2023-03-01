using System.Text.Json.Serialization;

using JSInterop.Browser.WebCryptoAPI.Interfaces.Subtle.Params;

namespace JSInterop.Browser.WebCryptoAPI.Interfaces.Subtle.AES;

/// <summary>
/// AES-GCM parameters.
/// <br />
/// <see href="https://developer.mozilla.org/en-US/docs/Web/API/AesGcmParams"/>
/// </summary>
public class AesGcmParams : IEncryptParams,
                            IDecryptParams,
                            IWrapParams,
                            IUnwrapParams
{
    /// <summary>
    /// Set to <see cref="AesAlgorithm.GCM"/>.
    /// </summary>
    public AesAlgorithm Name { get; }

    /// <summary>
    /// Initialization vector.
    /// </summary>
    public byte[] Iv { get; }

    /// <summary>
    /// Additional data that will not be encrypted but will be authenticated
    /// along with the encrypted data.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public byte[]? AdditionalData { get; }

    /// <summary>
    /// Size in bits of the authentication tag generated in
    /// the encryption operation and used for authentication in
    /// the corresponding decryption.
    /// Must have one of the following values: 32, 64, 96, 104, 112, 120, or 128.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? TagLength { get; }

    /// <summary>
    /// Allowed tag lengths. 
    /// </summary>
    private static readonly int[] _tagLengths = { 32, 64, 96, 104, 112, 120, 128 };

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="iv">Initialization vector, recommended to be 96 bits (12 bytes) long.</param>
    /// <param name="additionalData">Unencrypted but authenticated additional data.</param>
    /// <param name="tagLength">Size in bits of the authentication tag.</param>
    public AesGcmParams(byte[] iv, byte[]? additionalData = null, int? tagLength = null)
    {
        this.Name = AesAlgorithm.GCM;
        this.Iv = iv;
        this.AdditionalData = additionalData;

        if (tagLength is not null && !_tagLengths.Contains(tagLength.Value))
        {
            string lengths = string.Join(", ", _tagLengths);
            throw new ArgumentOutOfRangeException($"AES GCM Tag Length must be one of {lengths}.");
        }

        this.TagLength = tagLength;
    }

}
