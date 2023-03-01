using JSInterop.Browser.WebCryptoAPI.Interfaces;
using JSInterop.Browser.WebCryptoAPI.Interfaces.CryptoKeys;

namespace JSInterop.Browser.WebCryptoAPI.Cryptography.AES;

/// <summary>
/// Base class to factorize common functionalities
/// of AES operations.
/// </summary>
public abstract class AesBase : UnmanagedSymmetricBase
{

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="crypto">Crypto Web API instance.</param>
    /// <param name="key">Key.</param>
    protected AesBase(Crypto crypto, CryptoKeyDescriptor key)
        : base(crypto, key)
    {
    }

}
