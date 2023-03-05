using System.Text.Json;
using System.Text.Json.Serialization;

namespace Glihm.JSInterop.Browser.WebCryptoAPI.Interfaces.Subtle.EC;

/// <summary>
/// EC algorithms.
/// </summary>
[JsonConverter(typeof(AesAlgorithmConverter))]
public enum EcAlgorithm
{
    ECDSA,
    ECDH,
}

/// <summary>
/// JSON converter for <see cref="EcAlgorithm"/>.
/// </summary>
internal class AesAlgorithmConverter : JsonConverter<EcAlgorithm>
{
    /// <inheritdoc/>
    public override bool
    CanConvert(Type typeToConvert)
    {
        return typeToConvert == typeof(string) || typeToConvert == typeof(EcAlgorithm);
    }

    /// <inheritdoc/>
    public override EcAlgorithm
    Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string fmtstr = reader.GetString() ?? "";

        return fmtstr switch
        {
            "ECDSA" => EcAlgorithm.ECDSA,
            "ECDH" => EcAlgorithm.ECDH,
            _ => throw new InvalidCastException($"String {fmtstr} can't be converted to a EcAlgorithm.")
        };
    }

    /// <inheritdoc/>
    public override void
    Write(Utf8JsonWriter writer, EcAlgorithm value, JsonSerializerOptions options)
    {
        string fmtstr = value switch
        {
            EcAlgorithm.ECDSA => "ECDSA",
            EcAlgorithm.ECDH => "ECDH",
            _ => throw new InvalidCastException($"EcAlgorithm {value} is not mapped to a string.")
        };

        writer.WriteStringValue(fmtstr);
    }
}