using Glihm.JSInterop.Browser.WebCryptoAPI.Interfaces.Subtle.Params;

namespace Glihm.JSInterop.Browser.WebCryptoAPI.Interfaces.Subtle.AES;

/// <summary>
/// AES-CBC parameters.
/// <br />
/// <see href="https://developer.mozilla.org/en-US/docs/Web/API/AesCbcParams"/>
/// </summary>
public class AesCbcParams : IEncryptParams,
                            IDecryptParams,
                            IWrapParams,
                            IUnwrapParams
{
    /// <summary>
    /// Set to <see cref="AesAlgorithm.CBC"/>.
    /// </summary>
    public AesAlgorithm Name { get; }

    /// <summary>
    /// Initialization vector, fixed to 16 bytes.
    /// </summary>
    public byte[] Iv { get; }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="iv">Initialization vector.</param>
    public AesCbcParams(byte[] iv)
    {
        if (iv.Length != 16)
        {
            throw new ArgumentOutOfRangeException("AES CBC initialization vector must be 16 bytes long.");
        }

        this.Name = AesAlgorithm.CBC;
        this.Iv = iv;
    }

}
