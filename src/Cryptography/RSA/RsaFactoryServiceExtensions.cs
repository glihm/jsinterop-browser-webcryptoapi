using Glihm.JSInterop.Browser.WebCryptoAPI.Interfaces;

using Microsoft.Extensions.DependencyInjection;

namespace Glihm.JSInterop.Browser.WebCryptoAPI.Cryptography.RSA;

/// <summary>
/// IServiceCollection extensions.
/// </summary>
public static class RsaFactoryServiceExtensions
{
    /// <summary>
    /// Extends service collection to add AesFactory and it's dependencies.
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection
    AddWebCryptoRsa(this IServiceCollection services)
    {
        // Ensure crypto is imported.
        services.AddSingleton<Crypto>();
        services.AddSingleton<RsaFactory>();
        return services;
    }
}
