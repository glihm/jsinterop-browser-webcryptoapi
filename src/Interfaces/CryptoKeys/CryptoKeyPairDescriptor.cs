namespace Glihm.JSInterop.Browser.WebCryptoAPI.Interfaces.CryptoKeys;

/// <summary>
/// A convenient way to group public and private key
/// for public-key algorithms.
/// <br />
/// Depending the scenario, one of the key may be null.
/// Indeed, a client application may only import a public key
/// to encrypt some data with RSA, or verify some signatures
/// with ECDSA.
/// </summary>
public class CryptoKeyPairDescriptor
{
    /// <summary>
    /// Public key.
    /// </summary>
    public CryptoKeyDescriptor? PublicKey { get; }

    /// <summary>
    /// Private key.
    /// </summary>
    public CryptoKeyDescriptor? PrivateKey { get; }

    /// <summary>
    /// Internal constructor.
    /// </summary>
    /// <param name="publicKey"></param>
    /// <param name="privateKey"></param>
    internal CryptoKeyPairDescriptor(CryptoKeyDescriptor? publicKey = null, CryptoKeyDescriptor? privateKey = null)
    {
        this.PublicKey = publicKey;
        this.PrivateKey = privateKey;
    }

    /// <summary>
    /// Gets the public key or throw.
    /// </summary>
    /// <param name="caller">A message from mthe caller to be logged in the exception.</param>
    /// <returns>Public key, if any.</returns>
    /// <exception cref="InvalidOperationException">Public key is null.</exception>
    public CryptoKeyDescriptor
    GetPublicKeyOrThrow(string? caller = null)
    {
        if (this.PublicKey is null)
        {
            throw new InvalidOperationException($"{caller}: a public key is required for this operation.");
        }

        return this.PublicKey;
    }

    /// <summary>
    /// Gets the private key or throw.
    /// </summary>
    /// <param name="caller">A message from mthe caller to be logged in the exception.</param>
    /// <returns>Private key, if any.</returns>
    /// <exception cref="InvalidOperationException">Private key is null.</exception>
    public CryptoKeyDescriptor
    GetPrivateKeyOrThrow(string? caller = null)
    {
        if (this.PrivateKey is null)
        {
            throw new InvalidOperationException($"{caller}: a private key is required for this operation.");
        }

        return this.PrivateKey;
    }
}
