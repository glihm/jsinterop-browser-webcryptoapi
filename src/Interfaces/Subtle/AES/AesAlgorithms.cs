using System.Text.Json;
using System.Text.Json.Serialization;

namespace JSInterop.Browser.WebCryptoAPI.Interfaces.Subtle.AES;

/// <summary>
/// AES modes of operation, mapped to Algorithm
/// of the CryptoSubtle API.
/// </summary>
[JsonConverter(typeof(AesAlgorithmConverter))]
public enum AesAlgorithm
{
    CBC,
    CTR,
    GCM,
    KW,
}

/// <summary>
/// JSON converter for <see cref="AesAlgorithm"/>.
/// </summary>
internal class AesAlgorithmConverter : JsonConverter<AesAlgorithm>
{
    /// <inheritdoc/>
    public override bool
    CanConvert(Type typeToConvert)
    {
        return typeToConvert == typeof(string) || typeToConvert == typeof(AesAlgorithm);
    }

    /// <inheritdoc/>
    public override AesAlgorithm
    Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string fmtstr = reader.GetString() ?? "";

        return fmtstr switch
        {
            "AES-CBC" => AesAlgorithm.CBC,
            "AES-CTR" => AesAlgorithm.CTR,
            "AES-GCM" => AesAlgorithm.GCM,
            "AES-KW" => AesAlgorithm.KW,
            _ => throw new InvalidCastException($"String {fmtstr} can't be converted to a AesAlgorithm.")
        };
    }

    /// <inheritdoc/>
    public override void
    Write(Utf8JsonWriter writer, AesAlgorithm value, JsonSerializerOptions options)
    {
        string fmtstr = value switch
        {
            AesAlgorithm.CBC => "AES-CBC",
            AesAlgorithm.CTR => "AES-CTR",
            AesAlgorithm.GCM => "AES-GCM",
            AesAlgorithm.KW => "AES-KW",
            _ => throw new InvalidCastException($"AesAlgorithm {value} is not mapped to a string.")
        };

        writer.WriteStringValue(fmtstr);
    }
}