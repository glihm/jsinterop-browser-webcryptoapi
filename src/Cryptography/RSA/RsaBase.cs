using Glihm.JSInterop.Browser.WebCryptoAPI.Interfaces;
using Glihm.JSInterop.Browser.WebCryptoAPI.Interfaces.CryptoKeys;

namespace Glihm.JSInterop.Browser.WebCryptoAPI.Cryptography.RSA;

/// <summary>
/// Base class to factorize common functionalities
/// of RSA operations.
/// </summary>
public abstract class RsaBase : UnmanagedPublicKeyBase
{

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="crypto">Crypto Web API instance.</param>
    /// <param name="keyPair">Key pair.</param>
    protected RsaBase(Crypto crypto, CryptoKeyPairDescriptor keyPair)
        : base(crypto, keyPair)
    {
    }

}
