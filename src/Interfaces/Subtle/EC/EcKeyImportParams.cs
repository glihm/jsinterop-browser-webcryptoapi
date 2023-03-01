using JSInterop.Browser.WebCryptoAPI.Interfaces.Subtle.Params;

namespace JSInterop.Browser.WebCryptoAPI.Interfaces.Subtle.EC;

/// <summary>
/// EC import params.
/// <br />
/// <see href="https://developer.mozilla.org/en-US/docs/Web/API/EcKeyImportParams"/>.
/// </summary>
public class EcKeyImportParams : IImportParams,
                                 IUnwrappedKeyParams
{
    /// <summary>
    /// Algorithm to use.
    /// </summary>
    public EcAlgorithm Name { get; }

    /// <summary>
    /// Name of the elliptic curve to use.
    /// </summary>
    public EcNamedCurve NamedCurve { get; }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="name">Algorithm to use.</param>
    /// <param name="namedCurved">Name of the elliptic curve to use.</param>
    public EcKeyImportParams(EcAlgorithm name, EcNamedCurve namedCurved)
    {
        this.Name = name;
        this.NamedCurve = namedCurved;
    }
}
