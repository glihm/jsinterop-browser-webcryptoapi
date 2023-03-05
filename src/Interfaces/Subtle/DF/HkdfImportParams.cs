using Glihm.JSInterop.Browser.WebCryptoAPI.Interfaces.Subtle.Params;

namespace Glihm.JSInterop.Browser.WebCryptoAPI.Interfaces.Subtle.DF;

/// <summary>
/// HKDF import params.
/// </summary>
public class HkdfImportParams : IImportParams
{
    /// <summary>
    /// Set to HKDF.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Constructor.
    /// </summary>
    public HkdfImportParams()
    {
        this.Name = "HKDF";
    }

}
