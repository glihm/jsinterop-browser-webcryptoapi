using Glihm.JSInterop.Browser.WebCryptoAPI.Interfaces.Subtle.Params;

namespace Glihm.JSInterop.Browser.WebCryptoAPI.Interfaces.Subtle.HMAC;

/// <summary>
/// HMAC params.
/// <br />
/// <see href="https://developer.mozilla.org/en-US/docs/Web/API/SubtleCrypto/sign"/>.
/// </summary>
public class HmacParams : ISignParams,
                          IVerifyParams
{
    /// <summary>
    /// Set to HMAC.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Constructor.
    /// </summary>
    public HmacParams()
    {
        this.Name = "HMAC";
    }

}
