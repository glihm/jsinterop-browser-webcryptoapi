using System.Text.Json;
using System.Text.Json.Serialization;

namespace JSInterop.Browser.WebCryptoAPI.Interfaces.Subtle.RSA;

/// <summary>
/// RSA algorithms implemented by SubtleCrypto.
/// </summary>
[JsonConverter(typeof(RsaAlgorithmConverter))]
public enum RsaAlgorithm
{
    SSA_PKCS1_v1_5,
    PSS,
    OAEP,
}

/// <summary>
/// JSON converter for <see cref="RsaAlgorithm"/>.
/// </summary>
internal class RsaAlgorithmConverter : JsonConverter<RsaAlgorithm>
{
    /// <inheritdoc/>
    public override bool
    CanConvert(Type typeToConvert)
    {
        return typeToConvert == typeof(string) || typeToConvert == typeof(RsaAlgorithm);
    }

    /// <inheritdoc/>
    public override RsaAlgorithm
    Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string fmtstr = reader.GetString() ?? "";

        return fmtstr switch
        {
            "RSASSA-PKCS1-v1_5" => RsaAlgorithm.SSA_PKCS1_v1_5,
            "RSA-PSS" => RsaAlgorithm.PSS,
            "RSA-OAEP" => RsaAlgorithm.OAEP,
            _ => throw new InvalidCastException($"String {fmtstr} can't be converted to a RsaAlgorithm.")
        };
    }

    /// <inheritdoc/>
    public override void
    Write(Utf8JsonWriter writer, RsaAlgorithm value, JsonSerializerOptions options)
    {
        string fmtstr = value switch
        {
            RsaAlgorithm.SSA_PKCS1_v1_5 => "RSASSA-PKCS1-v1_5",
            RsaAlgorithm.PSS => "RSA-PSS",
            RsaAlgorithm.OAEP => "RSA-OAEP",
            _ => throw new InvalidCastException($"RsaAlgorithm {value} is not mapped to a string.")
        };

        writer.WriteStringValue(fmtstr);
    }
}