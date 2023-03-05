using Glihm.JSInterop.Browser.WebCryptoAPI.Interfaces.Subtle.Params;
using Glihm.JSInterop.Browser.WebCryptoAPI.Interfaces.Subtle.SHA;

namespace Glihm.JSInterop.Browser.WebCryptoAPI.Interfaces.Subtle.EC;

/// <summary>
/// ECDSA params.
/// <br />
/// <see href="https://developer.mozilla.org/en-US/docs/Web/API/EcdsaParams"/>.
/// </summary>
public class EcdsaParams : ISignParams,
                           IVerifyParams
{
    /// <summary>
    /// Set to <see cref="EcAlgorithm.ECDSA"/>.
    /// </summary>
    public EcAlgorithm Name { get; }

    /// <summary>
    /// Digest algorithm to use.
    /// </summary>
    public ShaAlgorithm Hash { get; }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="hash">Digest algorithm to use.</param>
    public EcdsaParams(ShaAlgorithm hash)
    {
        this.Name = EcAlgorithm.ECDSA;
        this.Hash = hash;
    }

}
