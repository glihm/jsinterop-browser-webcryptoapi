using JSInterop.Browser.WebCryptoAPI.Interfaces;
using JSInterop.Browser.WebCryptoAPI.Interfaces.CryptoKeys;

namespace JSInterop.Browser.WebCryptoAPI.Cryptography.EC;

/// <summary>
/// Base class to factorize common functionalities
/// of EC operations.
/// </summary>
public abstract class EcBase : UnmanagedPublicKeyBase
{

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="crypto">Crypto Web API instance.</param>
    /// <param name="keyPair">Key pair.</param>
    protected EcBase(Crypto crypto, CryptoKeyPairDescriptor keyPair)
        : base(crypto, keyPair)
    {
    }

}
