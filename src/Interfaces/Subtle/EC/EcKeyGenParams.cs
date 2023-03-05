using System.Text.Json.Serialization;

using Glihm.JSInterop.Browser.WebCryptoAPI.Interfaces.Subtle.Params;

namespace Glihm.JSInterop.Browser.WebCryptoAPI.Interfaces.Subtle.EC;

/// <summary>
/// EC generator params.
/// <br />
/// <see href="https://developer.mozilla.org/en-US/docs/Web/API/EcKeyGenParams"/>.
/// </summary>
public class EcKeyGenParams : IGenParams
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
    /// <param name="namedCurve">Name of the elliptic curve to use.</param>
    [JsonConstructor]
    public EcKeyGenParams(EcAlgorithm name, EcNamedCurve namedCurve)
    {
        this.Name = name;
        this.NamedCurve = namedCurve;
    }
}
