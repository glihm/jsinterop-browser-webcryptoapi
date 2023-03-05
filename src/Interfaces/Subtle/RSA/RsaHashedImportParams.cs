using Glihm.JSInterop.Browser.WebCryptoAPI.Interfaces.Subtle.Params;
using Glihm.JSInterop.Browser.WebCryptoAPI.Interfaces.Subtle.SHA;

namespace Glihm.JSInterop.Browser.WebCryptoAPI.Interfaces.Subtle.RSA;

/// <summary>
/// RSA import params.
/// <br />
/// <see href="https://developer.mozilla.org/en-US/docs/Web/API/RsaHashedImportParams"/>.
/// </summary>
public class RsaHashedImportParams : IImportParams,
                                     IUnwrappedKeyParams
{
    /// <summary>
    /// Algorithm to use.
    /// </summary>
    public RsaAlgorithm Name { get; }

    /// <summary>
    /// Digest function to use.
    /// </summary>
    public ShaAlgorithm Hash { get; }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="name">Algorithm to use.</param>
    /// <param name="hash">Digest function to use.</param>
    public RsaHashedImportParams(RsaAlgorithm name, ShaAlgorithm hash)
    {
        this.Name = name;
        this.Hash = hash;
    }
}
