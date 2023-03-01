using JSInterop.Browser.WebCryptoAPI.Interfaces;
using JSInterop.Browser.WebCryptoAPI.JSHelpers;

using Microsoft.Extensions.Logging;

namespace JSInterop.Browser.WebCryptoAPI.Cryptography.Random;

public class CryptoRandom
{
    /// <summary>
    /// Logger.
    /// </summary>
    private readonly ILogger<CryptoRandom> _logger;

    /// <summary>
    /// Web Crypto API.
    /// </summary>
    private readonly Crypto _crypto;

    /// <summary>
    /// Constructor dependency injection.
    /// </summary>
    /// <param name="crypto">Web Crypto API interop.</param>
    /// <param name="logger">Logger.</param>
    public CryptoRandom(Crypto crypto, ILogger<CryptoRandom> logger)
    {
        this._crypto = crypto;
        this._logger = logger;
    }

    /// <summary>
    /// Generates an randomUUID using Web Crypto API.
    /// 
    /// Not that useful.. as dotnet already implements this.
    /// </summary>
    /// <returns></returns>
    public async Task<Guid?>
    RandomUUID()
    {
        JSResultValue<Guid> res = await this._crypto.RandomUUID()
                                                    .ConfigureAwait(false);
        if (!res)
        {
            this._logger.LogError(res.Error?.Message);
            return null;
        }

        return res.GetValueOrThrow();
    }

    /// <summary>
    /// Generate random values.
    /// </summary>
    /// <param name="byteCount">Number of random bytes required.</param>
    /// <returns>Buffer with random bytes on success, null otherwise.</returns>
    public async Task<byte[]?>
    GenerateRandomValues(int byteCount)
    {
        JSResultValue<byte[]> res = await this._crypto.GetRandomValues(byteCount)
                                                      .ConfigureAwait(false);
        if (!res)
        {
            this._logger.LogError(res.Error?.Message);
            return null;
        }

        return res.GetValueOrThrow();
    }

    /// <summary>
    /// Implements GetBytes like function for populating existing
    /// buffer with random values.
    /// </summary>
    /// <param name="buf"></param>
    /// <returns></returns>
    public async Task
    GetBytes(byte[] buf)
    {
        JSResultVoid res = await this._crypto.GetRandomValues(buf)
                                             .ConfigureAwait(false);
        if (!res)
        {
            this._logger.LogError(res.Error?.Message);
        }
    }


}
