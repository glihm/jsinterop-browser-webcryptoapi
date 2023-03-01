using JSInterop.Browser.WebCryptoAPI.Interfaces.Subtle.Params;

namespace JSInterop.Browser.WebCryptoAPI.Interfaces.Subtle.AES;

/// <summary>
/// AES-CTR parameters.
/// <br />
/// <see href="https://developer.mozilla.org/en-US/docs/Web/API/AesCtrParams"/>
/// </summary>
public class AesCtrParams : IEncryptParams,
                            IDecryptParams,
                            IWrapParams,
                            IUnwrapParams
{
    /// <summary>
    /// Set to <see cref="AesAlgorithm.CTR"/>.
    /// </summary>
    public AesAlgorithm Name { get; }

    /// <summary>
    /// Initial value of the counter block.
    /// Must be 16 bytes long.
    /// </summary>
    public byte[] Counter { get; }

    /// <summary>
    /// Number of bits in the counter block
    /// that are used for the actual counter.
    /// </summary>
    public int Length { get; }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="counter">Initial value of the counter block.</param>
    /// <param name="length">
    /// Number bits in the counter block to use.
    /// Default is 64, which means half of the counter is the nonce, and half is the counter.
    /// </param>
    public AesCtrParams(byte[] counter, int length = 64)
    {
        if (counter.Length != 16)
        {
            throw new ArgumentOutOfRangeException("AES CTR counter must be 16 bytes long.");
        }

        this.Name = AesAlgorithm.CTR;
        this.Counter = counter;
        this.Length = length;
    }

}
