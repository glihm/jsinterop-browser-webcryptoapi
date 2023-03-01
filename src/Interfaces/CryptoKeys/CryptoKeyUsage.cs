using System.Text.Json;
using System.Text.Json.Serialization;

namespace JSInterop.Browser.WebCryptoAPI.Interfaces.CryptoKeys;

/// <summary>
/// CryptoKey's usages.
/// </summary>
[Flags]
[JsonConverter(typeof(CryptoKeyUsageConveter))]
public enum CryptoKeyUsage
{
    Encrypt = 1 << 0,
    Decrypt = 1 << 1,
    Sign = 1 << 2,
    Verify = 1 << 3,
    DeriveKey = 1 << 4,
    DeriveBits = 1 << 5,
    WrapKey = 1 << 6,
    UnwrapKey = 1 << 7,
}

/// <summary>
/// JSON converter for <see cref="CryptoKeyUsage"/>.
/// </summary>
public class CryptoKeyUsageConveter : JsonConverter<CryptoKeyUsage>
{
    /// <inheritdoc/>
    public override bool
    CanConvert(Type typeToConvert)
    {
        return typeToConvert == typeof(string) || typeToConvert == typeof(CryptoKeyUsage);
    }

    /// <inheritdoc/>
    public override CryptoKeyUsage
    Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartArray)
        {
            throw new JsonException("Expected StartArray token.");
        }

        List<string> values = new();

        while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
        {
            values.Add(reader.GetString() ?? "");
        }

        return this.FromStringArray(values.ToArray());
    }

    /// <inheritdoc/>
    public override void
    Write(Utf8JsonWriter writer, CryptoKeyUsage value, JsonSerializerOptions options)
    {
        string[] usages = this.ToStringArray(value);

        writer.WriteStartArray();

        foreach (string u in usages)
        {
            writer.WriteStringValue(u);
        }

        writer.WriteEndArray();
    }

    /// <summary>
    /// Converts a bitmask of usages to string array.
    /// </summary>
    /// <param name="usage">CryptoKeyUsage.</param>
    /// <returns>String representation of the usages.</returns>
    private string[]
    ToStringArray(CryptoKeyUsage usage)
    {
        List<string> usages = new();
        foreach (CryptoKeyUsage u in Enum.GetValues(typeof(CryptoKeyUsage)))
        {
            if (u == 0)
            {
                continue;
            }

            if (usage.HasFlag(u))
            {
                usages.Add(this.ToCamelCase(u.ToString()));
            }
        }

        return usages.ToArray();
    }

    /// <summary>
    /// Converts a string array to a bitmask of usages.
    /// </summary>
    /// <param name="usages">String representation of the type.</param>
    /// <returns>CryptoKeyUsage.</returns>
    /// <exception cref="InvalidCastException">Given string is not mapped to a CryptoKeyUsage.</exception>
    private CryptoKeyUsage
    FromStringArray(string[] usages)
    {
        CryptoKeyUsage usage = 0;
        foreach (string u in usages)
        {
            CryptoKeyUsage uFlag = u switch
            {
                "encrypt" => CryptoKeyUsage.Encrypt,
                "decrypt" => CryptoKeyUsage.Decrypt,
                "sign" => CryptoKeyUsage.Sign,
                "verify" => CryptoKeyUsage.Verify,
                "wrapKey" => CryptoKeyUsage.WrapKey,
                "unwrapKey" => CryptoKeyUsage.UnwrapKey,
                "deriveBits" => CryptoKeyUsage.DeriveBits,
                "deriveKey" => CryptoKeyUsage.DeriveKey,
                _ => throw new InvalidCastException($"String {u} is not mapped to a CrytpoKeyUsage.")
            };

            usage |= uFlag;
        }

        return usage;
    }

    /// <summary>
    /// Formats a string to camel case string.
    /// </summary>
    /// <param name="s">String to be formatted.</param>
    /// <returns></returns>
    private string
    ToCamelCase(string s)
    {
        if (string.IsNullOrWhiteSpace(s))
        {
            return s;
        }

        if (s.Length == 1)
        {
            return s.ToLower();
        }

        return s.Substring(0, 1).ToLower() + s.Substring(1);
    }
}
