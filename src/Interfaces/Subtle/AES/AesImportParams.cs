using Glihm.JSInterop.Browser.WebCryptoAPI.Interfaces.Subtle.Params;

namespace Glihm.JSInterop.Browser.WebCryptoAPI.Interfaces.Subtle.AES;

/// <summary>
/// AES Import parameters.
/// <br />
/// <see href="https://developer.mozilla.org/en-US/docs/Web/API/importKey"/>
/// </summary>
public class AesImportParams : IImportParams,
                               IUnwrappedKeyParams
{
    /// <summary>
    /// Algorithm to use.
    /// </summary>
    public AesAlgorithm Name { get; }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="name">Algorithm to use.</param>
    public AesImportParams(AesAlgorithm name)
    {
        this.Name = name;
    }
}
