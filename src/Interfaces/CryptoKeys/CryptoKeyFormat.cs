using System.Text.Json;
using System.Text.Json.Serialization;

namespace Glihm.JSInterop.Browser.WebCryptoAPI.Interfaces.CryptoKeys;

/// <summary>
/// CryptoKey's formats.
/// </summary>
[JsonConverter(typeof(CryptoKeyFormatConverter))]
public enum CryptoKeyFormat
{
    JSONWebKey,
    Raw,
    PKCS8,
    SubjectPublicKeyInfo,
}

/// <summary>
/// JSON converter for <see cref="CryptoKeyFormat"/>.
/// </summary>
public class CryptoKeyFormatConverter : JsonConverter<CryptoKeyFormat>
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="typeToConvert"></param>
    /// <returns></returns>
    public override bool
    CanConvert(Type typeToConvert)
    {
        return typeToConvert == typeof(string) || typeToConvert == typeof(CryptoKeyFormat);
    }

    /// <inheritdoc/>
    public override CryptoKeyFormat
    Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string fmtstr = reader.GetString() ?? "";

        return fmtstr switch
        {
            "raw" => CryptoKeyFormat.Raw,
            "pkcs8" => CryptoKeyFormat.PKCS8,
            "spki" => CryptoKeyFormat.SubjectPublicKeyInfo,
            "jwk" => CryptoKeyFormat.JSONWebKey,
            _ => throw new InvalidCastException($"String {fmtstr} can't be converted to a CryptoKeyFormat.")
        };
    }

    /// <inheritdoc/>
    public override void
    Write(Utf8JsonWriter writer, CryptoKeyFormat value, JsonSerializerOptions options)
    {
        string fmtstr = value switch
        {
            CryptoKeyFormat.Raw => "raw",
            CryptoKeyFormat.PKCS8 => "pkcs8",
            CryptoKeyFormat.SubjectPublicKeyInfo => "spki",
            CryptoKeyFormat.JSONWebKey => "jwk",
            _ => throw new InvalidCastException($"CryptoKeyFormat {value} is not mapped to a string.")
        };

        writer.WriteStringValue(fmtstr);
    }
}
