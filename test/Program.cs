using TestApp;

using Glihm.JSInterop.Browser.WebCryptoAPI.Interfaces;
using Glihm.JSInterop.Browser.WebCryptoAPI.Cryptography.AES;
using Glihm.JSInterop.Browser.WebCryptoAPI.Cryptography.RSA;
using Glihm.JSInterop.Browser.WebCryptoAPI.Cryptography.EC;
using Glihm.JSInterop.Browser.WebCryptoAPI.Cryptography.HMAC;
using Glihm.JSInterop.Browser.WebCryptoAPI.Cryptography.Random;

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
