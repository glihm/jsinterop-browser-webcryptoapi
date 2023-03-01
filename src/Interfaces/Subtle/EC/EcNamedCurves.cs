using System.Text.Json;
using System.Text.Json.Serialization;

namespace JSInterop.Browser.WebCryptoAPI.Interfaces.Subtle.EC;

/// <summary>
/// EC named curves.
/// </summary>
[JsonConverter(typeof(EcNamedCurveConverter))]
public enum EcNamedCurve
{
    P_256,
    P_384,
    P_521
}

/// <summary>
/// JSON converter for <see cref="EcNamedCurve"/>.
/// </summary>
internal class EcNamedCurveConverter : JsonConverter<EcNamedCurve>
{
    /// <inheritdoc/>
    public override bool
    CanConvert(Type typeToConvert)
    {
        return typeToConvert == typeof(string) || typeToConvert == typeof(EcNamedCurve);
    }

    /// <inheritdoc/>
    public override EcNamedCurve
    Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string fmtstr = reader.GetString() ?? "";

        return fmtstr switch
        {
            "P-256" => EcNamedCurve.P_256,
            "P-384" => EcNamedCurve.P_384,
            "P-521" => EcNamedCurve.P_521,
            _ => throw new InvalidCastException($"String {fmtstr} can't be converted to a EcNamedCurve.")
        };
    }

    /// <inheritdoc/>
    public override void
    Write(Utf8JsonWriter writer, EcNamedCurve value, JsonSerializerOptions options)
    {
        string fmtstr = value switch
        {
            EcNamedCurve.P_256 => "P-256",
            EcNamedCurve.P_384 => "P-384",
            EcNamedCurve.P_521 => "P-521",
            _ => throw new InvalidCastException($"EcNamedCurve {value} is not mapped to a string.")
        };

        writer.WriteStringValue(fmtstr);
    }
}