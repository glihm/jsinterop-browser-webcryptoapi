using JSInterop.Browser.Samples;

using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

using JSInterop.Browser.WebCryptoAPI.Cryptography.Random;
using JSInterop.Browser.WebCryptoAPI.Cryptography.AES;
using JSInterop.Browser.WebCryptoAPI.Cryptography.RSA;
using JSInterop.Browser.WebCryptoAPI.Interfaces;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddWebCryptoRandom();
builder.Services.AddWebCryptoAes();
builder.Services.AddWebCryptoRsa();

// Used only for low level calls.
builder.Services.AddSingleton<Crypto>();

await builder.Build().RunAsync();
