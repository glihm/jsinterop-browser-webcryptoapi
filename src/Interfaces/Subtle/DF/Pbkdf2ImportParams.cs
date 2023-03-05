using Glihm.JSInterop.Browser.WebCryptoAPI.Interfaces.Subtle.Params;

namespace Glihm.JSInterop.Browser.WebCryptoAPI.Interfaces.Subtle.DF;

/// <summary>
/// PBKDF2 import params.
/// </summary>
public class Pbkdf2ImportParams : IImportParams
{
    /// <summary>
    /// Set to PBKDF2.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Constructor.
    /// </summary>
    public Pbkdf2ImportParams()
    {
        this.Name = "PBKDF2";
    }

}
