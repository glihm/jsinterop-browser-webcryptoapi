using JSInterop.Browser.WebCryptoAPI.Interfaces.Subtle.Params;

namespace JSInterop.Browser.WebCryptoAPI.Interfaces.Subtle.RSA;

/// <summary>
/// RSA SSA PKCS1 params.
/// <br />
/// <see href="https://developer.mozilla.org/en-US/docs/Web/API/SubtleCrypto/sign"/>.
/// </summary>
public class RsaSsaPkcs1Params : ISignParams,
                                 IVerifyParams
{
    /// <summary>
    /// Set to <see cref="RsaAlgorithm.SSA_PKCS1_v1_5"/>.
    /// </summary>
    public RsaAlgorithm Name { get; }

    /// <summary>
    /// Constructor.
    /// </summary>
    public RsaSsaPkcs1Params()
    {
        this.Name = RsaAlgorithm.SSA_PKCS1_v1_5;
    }

}
