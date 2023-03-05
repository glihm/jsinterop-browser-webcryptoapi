using Glihm.JSInterop.Browser.WebCryptoAPI.Interfaces.CryptoKeys;

namespace Glihm.JSInterop.Browser.WebCryptoAPI.Cryptography;

/// <summary>
/// Describes a crypto key properties to be imported.
/// </summary>
public class CryptoKeyImport
{
    /// <summary>
    /// The key itself.
    /// </summary>
    public byte[] Key { get; set; }

    /// <summary>
    /// Expected format of the key.
    /// </summary>
    public CryptoKeyFormat Format { get; set; }

    /// <summary>
    /// Usages of the crypto key.
    /// </summary>
    public CryptoKeyUsage Usages { get; set; }

    /// <summary>
    /// If the crypto key can later be exported.
    /// </summary>
    public bool Extractable { get; set; }

    /// <summary>
    /// Constructor.
    /// </summary>
    public CryptoKeyImport(byte[] key,
                           CryptoKeyFormat format,
                           CryptoKeyUsage usages,
                           bool extractable)
    {
        this.Key = key;
        this.Format = format;
        this.Usages = usages;
        this.Extractable = extractable;
    }
}
