namespace Glihm.JSInterop.Browser.WebCryptoAPI.Cryptography;

public class CryptoKeyPairImport
{
    public CryptoKeyImport? PublicKey { get; set; }
    public CryptoKeyImport? PrivateKey { get; set; }

    public bool
    IsEmpty()
    {
        return this.PrivateKey is null && this.PublicKey is null;
    }
}
