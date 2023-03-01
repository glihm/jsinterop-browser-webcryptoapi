using System.Text.Json;
using System.Text.Json.Serialization;

namespace JSInterop.Browser.WebCryptoAPI.Interfaces.CryptoKeys;

/// <summary>
/// CryptoKey types.
/// </summary>
[JsonConverter(typeof(CryptoKeyTypeConveter))]
public enum CryptoKeyType
{
    Secret,
    Private,
    Public,
}

/// <summary>
/// JSON converter for <see cref="CryptoKeyType"/>.
/// </summary>
public class CryptoKeyTypeConveter : JsonConverter<CryptoKeyType>
{
    /// <inheritdoc/>
    public override bool
    CanConvert(Type typeToConvert)
    {
        return typeToConvert == typeof(string) || typeToConvert == typeof(CryptoKeyType);
    }

    /// <inheritdoc/>
    public override CryptoKeyType
    Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string typestr = reader.GetString() ?? "";

        return typestr switch
        {
            "secret" => CryptoKeyType.Secret,
            "private" => CryptoKeyType.Private,
            "public" => CryptoKeyType.Public,
            _ => throw new InvalidCastException($"String {typestr} can't be converted to a CryptoKeyType.")
        };
    }

    /// <inheritdoc/>
    public override void
    Write(Utf8JsonWriter writer, CryptoKeyType value, JsonSerializerOptions options)
    {
        string typestr = value switch
        {
            CryptoKeyType.Secret => "secret",
            CryptoKeyType.Private => "private",
            CryptoKeyType.Public => "public",
            _ => throw new InvalidCastException($"CryptoKeyType {value} is not mapped to a string.")
        };

        writer.WriteStringValue(typestr);
    }
}
