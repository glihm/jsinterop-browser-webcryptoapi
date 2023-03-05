using Glihm.JSInterop.Browser.WebCryptoAPI.Interfaces.CryptoKeys;
using Glihm.JSInterop.Browser.WebCryptoAPI.Interfaces.Subtle.Params;

namespace Glihm.JSInterop.Browser.WebCryptoAPI.Interfaces.Subtle.EC;

/// <summary>
/// ECDH derive params.
/// <br />
/// <see href="https://developer.mozilla.org/en-US/docs/Web/API/EcdhKeyDeriveParams"/>.
/// </summary>
public class EcdhKeyDeriveParams : IDerivationParams
{
    /// <summary>
    /// Set to <see cref="EcAlgorithm.ECDH"/>.
    /// </summary>
    public EcAlgorithm Name { get; }

    /// <summary>
    /// Public key of the other entity.
    /// </summary>
    public CryptoKeyDescriptor Public { get; }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="publicKey">CryptoKey representing the public key of the other entity.</param>
    public EcdhKeyDeriveParams(CryptoKeyDescriptor publicKey)
    {
        this.Name = EcAlgorithm.ECDH;
        this.Public = publicKey;
    }

}
