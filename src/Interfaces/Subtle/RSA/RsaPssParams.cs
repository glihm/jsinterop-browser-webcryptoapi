using JSInterop.Browser.WebCryptoAPI.Interfaces.Subtle.Params;

namespace JSInterop.Browser.WebCryptoAPI.Interfaces.Subtle.RSA;

/// <summary>
/// RSA PSS Params.
/// <br />
/// <see href="https://developer.mozilla.org/en-US/docs/Web/API/RsaPssParams"/>.
/// </summary>
public class RsaPssParams : ISignParams,
                            IVerifyParams
{
    /// <summary>
    /// Set to <see cref="RsaAlgorithm.PSS"/>.
    /// </summary>
    public RsaAlgorithm Name { get; }

    /// <summary>
    /// Length in bytes of the random salt to use.
    /// Refer to <see href="https://developer.mozilla.org/en-US/docs/Web/API/RsaPssParams"/>
    /// to see recommendations for salt length.
    /// </summary>
    public long SaltLength { get; }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="saltLength">Salt length.</param>
    public RsaPssParams(long saltLength = 0)
    {
        this.Name = RsaAlgorithm.PSS;
        this.SaltLength = saltLength;
    }

}
