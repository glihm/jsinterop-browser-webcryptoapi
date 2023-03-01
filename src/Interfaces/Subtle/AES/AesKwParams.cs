using JSInterop.Browser.WebCryptoAPI.Interfaces.Subtle.Params;

namespace JSInterop.Browser.WebCryptoAPI.Interfaces.Subtle.AES;

/// <summary>
/// AES-KW parameters.
/// <br />
/// <see href="https://developer.mozilla.org/en-US/docs/Web/API/SubtleCrypto/wrapKey#aes-kw"/>
/// </summary>
public class AesKwParams : IWrapParams,
                           IUnwrapParams
{
    /// <summary>
    /// Set to <see cref="AesAlgorithm.KW"/>.
    /// </summary>
    public AesAlgorithm Name { get; }

    /// <summary>
    /// Constructor.
    /// </summary>
    public AesKwParams()
    {
        this.Name = AesAlgorithm.KW;
    }

}
