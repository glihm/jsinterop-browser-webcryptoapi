using JSInterop.Browser.TestApp;
using JSInterop.Browser.WebCryptoAPI.Interfaces;
using JSInterop.Browser.WebCryptoAPI.Cryptography.AES;
using JSInterop.Browser.WebCryptoAPI.Cryptography.RSA;
using JSInterop.Browser.WebCryptoAPI.Cryptography.EC;
using JSInterop.Browser.WebCryptoAPI.Cryptography.HMAC;
using JSInterop.Browser.WebCryptoAPI.Cryptography.Random;

using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddSingleton<Crypto>();
builder.Services.AddWebCryptoAes();
builder.Services.AddWebCryptoRsa();
builder.Services.AddWebCryptoEc();
builder.Services.AddWebCryptoHmac();
builder.Services.AddWebCryptoRandom();

#if DEBUG
builder.Logging.SetMinimumLevel(LogLevel.Trace);
#endif

await builder.Build().RunAsync();
