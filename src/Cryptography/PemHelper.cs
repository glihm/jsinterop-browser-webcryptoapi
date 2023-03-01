using System.Text;

namespace JSInterop.Browser.WebCryptoAPI.Cryptography;

/// <summary>
/// PEM format helper.
/// </summary>
public static class PemHelper
{
    /// <summary>
    /// Extracts the buffer key from the base64 text
    /// inside the PEM string.
    /// </summary>
    /// <param name="pem">PEM formatted string.</param>
    /// <returns></returns>
    public static byte[]?
    KeyExtract(string pem)
    {
        if (string.IsNullOrEmpty(pem))
        {
            return null;
        }

        string[] frags = pem.Split(
            Environment.NewLine,
            StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

        if (frags.Length < 3)
        {
            // PEM is expected at least 3 lines. BEGIN hdr, KEY, END hdr.
            return null;
        }

        StringBuilder b64Key = new();
        for (int i = 1; i < frags.Length - 1; i++)
        {
            b64Key.Append(frags[i]);
        }

        return Convert.FromBase64String(b64Key.ToString());
    }

    /// <summary>
    /// Generates a PEM formatted string
    /// from the given private key.
    /// </summary>
    /// <param name="privateKey">Private key to distribute in PEM format.</param>
    /// <returns>PEM formatted string.</returns>
    public static string
    PrivateKeyPem(byte[] privateKey)
    {
        string hdrBegin = "----- BEGIN PRIVATE KEY -----";
        string hdrEnd = "----- END PRIVATE KEY -----";

        return PemFromKey(privateKey, hdrBegin, hdrEnd);
    }

    /// <summary>
    /// Generates a PEM formatted string
    /// from the given public key.
    /// </summary>
    /// <param name="publicKey">Public key to distribute in PEM format.</param>
    /// <returns>PEM formatted string.</returns>
    public static string
    PublicKeyPem(byte[] publicKey)
    {
        string hdrBegin = "----- BEGIN PUBLIC KEY -----";
        string hdrEnd = "----- END PUBLIC KEY -----";

        return PemFromKey(publicKey, hdrBegin, hdrEnd);
    }

    /// <summary>
    /// Generates a PEM formatted key with
    /// the given headers.
    /// </summary>
    /// <param name="key">Key.</param>
    /// <param name="hdrBegin">Header begin.</param>
    /// <param name="hdrEnd">Header end.</param>
    /// <returns></returns>
    private static string
    PemFromKey(byte[] key, string hdrBegin, string hdrEnd)
    {
        string b64Key = Convert.ToBase64String(key);
        int lineLength = 64;
        double nLines = Math.Ceiling(b64Key.Length / 64.0);
        int lastLineLength = b64Key.Length % 64;

        StringBuilder pem = new();
        pem.AppendLine(hdrBegin);
        for (int i = 0; i < nLines; i++)
        {
            int startIdx = lineLength * i;
            int len = i == nLines - 1 ? lastLineLength : lineLength;
            pem.AppendLine(b64Key.Substring(startIdx, len));
        }
        pem.AppendLine(hdrEnd);

        return pem.ToString();
    }
}
