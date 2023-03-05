using System.Text.Json;
using System.Text.Json.Serialization;

namespace Glihm.JSInterop.Browser.WebCryptoAPI.Interfaces.Subtle.SHA;


/// <summary>
/// SHA algorithm supported by SubtleCrypto.
/// </summary>
[JsonConverter(typeof(ShaHashConverter))]
public enum ShaAlgorithm
{
    SHA1,
    SHA256,
    SHA384,
    SHA512
}

/// <summary>
/// JSON converter for <see cref="ShaAlgorithm"/>.
/// </summary>
public class ShaHashConverter : JsonConverter<ShaAlgorithm>
{
    /// <inheritdoc/>
    public override bool
    CanConvert(Type typeToConvert)
    {
        return typeToConvert == typeof(string) || typeToConvert == typeof(ShaAlgorithm);
    }

    /// <inheritdoc/>
    public override ShaAlgorithm
    Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException("Expected StartObject token.");
        }

        if (reader.Read() && reader.TokenType != JsonTokenType.PropertyName)
        {
            throw new JsonException("Expected PropertyName token.");
        }

        if (reader.GetString() != "name")
        {
            throw new JsonException("Expected 'name' property name.");
        }

        if (reader.Read() && reader.TokenType != JsonTokenType.String)
        {
            throw new JsonException("Expected String token.");
        }

        ShaAlgorithm sha = FromString(reader.GetString() ?? "__");

        // end of object.
        reader.Read();

        return sha;
    }

    /// <inheritdoc/>
    public override void
    Write(Utf8JsonWriter writer, ShaAlgorithm value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteString("name", ToString(value));
        writer.WriteEndObject();
        //writer.WriteStringValue(ShaAlgorithms.ToString(value));

    }

    /// <summary>
    /// Converts string to <see cref="ShaAlgorithm"/>.
    /// </summary>
    /// <param name="hash"></param>
    /// <returns></returns>
    /// <exception cref="InvalidCastException"></exception>
    private static ShaAlgorithm
    FromString(string hash)
    {
        return hash switch
        {
            "SHA-1" => ShaAlgorithm.SHA1,
            "SHA-256" => ShaAlgorithm.SHA256,
            "SHA-384" => ShaAlgorithm.SHA384,
            "SHA-512" => ShaAlgorithm.SHA512,
            _ => throw new InvalidCastException($"String {hash} can't be casted into ShaAlgorithm.")
        };
    }

    /// <summary>
    /// Convert <see cref="ShaAlgorithm"/> to string.
    /// </summary>
    /// <param name="algo"></param>
    /// <returns></returns>
    /// <exception cref="InvalidCastException"></exception>
    private static string
    ToString(ShaAlgorithm algo)
    {
        return algo switch
        {
            ShaAlgorithm.SHA1 => "SHA-1",
            ShaAlgorithm.SHA256 => "SHA-256",
            ShaAlgorithm.SHA384 => "SHA-384",
            ShaAlgorithm.SHA512 => "SHA-512",
            _ => throw new InvalidCastException($"ShaAlgorithm {algo} can't be mapped into a known string.")
        };
    }
}
