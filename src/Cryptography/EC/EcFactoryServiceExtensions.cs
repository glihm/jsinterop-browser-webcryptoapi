using JSInterop.Browser.WebCryptoAPI.Interfaces;

using Microsoft.Extensions.DependencyInjection;

namespace JSInterop.Browser.WebCryptoAPI.Cryptography.EC;

/// <summary>
/// IServiceCollection extensions.
/// </summary>
public static class EcFactoryServiceExtensions
{
    /// <summary>
    /// Extends service collection to add AesFactory and it's dependencies.
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection
    AddWebCryptoEc(this IServiceCollection services)
    {
        // Ensure crypto is imported.
        services.AddSingleton<Crypto>();
        services.AddSingleton<EcFactory>();
        return services;
    }
}
