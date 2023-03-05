using Glihm.JSInterop.Browser.WebCryptoAPI.Interfaces;

using Microsoft.Extensions.DependencyInjection;

namespace Glihm.JSInterop.Browser.WebCryptoAPI.Cryptography.Random;

/// <summary>
/// IServiceCollection extensions.
/// </summary>
public static class CryptoRandomServiceExtensions
{
    /// <summary>
    /// Extends service collection to add AesFactory and it's dependencies.
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection
    AddWebCryptoRandom(this IServiceCollection services)
    {
        // Ensure crypto is imported.
        services.AddSingleton<Crypto>();
        services.AddSingleton<CryptoRandom>();
        return services;
    }
}
