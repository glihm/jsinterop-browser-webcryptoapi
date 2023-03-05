# Glihm.JSInterop.Browser.WebCryptoAPI

A convenient way to use browser cryptography from `C#`.

## Motivation

As a `C#` developer (and sometimes Angular), when I started Blazor I was frustrated with not having
any easy way to interact with cryptography operations proposed by `SubtleCrypto`.

As `System.Security.Cryptography` namespace is very limited in support into web-browsers,
this library aims at giving some quick access and `C#` idiomatic methods to work with
web-browser cryptography.

Some resources on which this library is based:
* [MDN web docs](https://developer.mozilla.org/en-US/docs/Web/API/Web_Crypto_API)
* [W3C specification](https://w3c.github.io/webcrypto/)


## Library design

`CryptoKey` we are used to work with from `javascript` runtime are living inside
the `javascript` runtime, and `C#` works with them using `CryptoKeyDescriptor`.
This allows conserving the maximum benefits of the browser runtime, including
the `extractable` property of the keys.

More details can be found in the readme of the `src/interfaces` directory.


## Designed for Blazor, simple to use

This library was design to work with Blazor in a very easy way and few configuration.
The two main namespaces of the library are:

* `interfaces`: low-level `C#` classes close to the Web Cryptography API interfaces.
* `cryptography`: high-level `C#` classes that are use to abstract the implementation details of the Web Cryptography API.

Samples of code will be added progressively into the `samples` project, where the folder `Pages` will contains the actual code.

> Caution: at the moment, this library was only tested with `Blazor WASM`)

With `cryptography` namespace, you can encrypt data with `AES` like this:

1. Inject the AES factory into the app.
```csharp
// Program.cs

using Glihm.JSInterop.Browser.WebCryptoAPI.Cryptography.AES;

...

builder.Services.AddWebCryptoAes();
```

2. Request the service into any blazor component requiring AES.
```csharp
// BlazorComponent.razor

@using Glihm.JSInterop.Browser.WebCryptoAPI.Cryptography.AES
@inject AesFactory _aesFactory

...

private async ValueTask<byte[]?>
EncryptData(byte[] key, byte[] plaintext, byte[] iv)
{
    await using AesGcm? aesGcm = await this._aesFactory.Create<AesGcm>(key);
    if (aesGcm is not null)
    {
        return await aesGcm.Encrypt(plaintext, iv);
    }
}
```


## Note about umanaged resources

As you can notice, the last example is making use of `await using`.
In fact, as the `CryptoKey` are managed by `javascript` runtime,
they are considered unmanaged resources for `C#`.

You can find more information about this in the `src/interfaces` readme.
For memory-efficient usage, you can:

1. Keep a reference on the `CryptoKeyDescriptor` for long-living keys.
2. Dispose a `CryptoKeyDescriptor` to delete the underlying `CryptoKey` in the `javascript` runtime.


## Limitations

The library is (for now) only limiting the possibilities you have to pass wrong argument
to the cryptography operations. But clearly, the library is not checking for
all the arguments you pass to the Web Cryptography API.

Please be aware of that, and refer to the following documentation for more exhaustive details:
* [MDN web docs](https://developer.mozilla.org/en-US/docs/Web/API/Web_Crypto_API)
* [W3C specification](https://w3c.github.io/webcrypto/)

Also, this library is not responsible for the key managment on your cryptography designs.
This is the application responsability to efficiently and correctly handle it's cryptographic keys lifecycle.
You can check [this link](https://www.crypto101.io/) given from MDN documentation about cryptography basics.


## Coding Style and Contribution

### Contribution
Any issue/PR are very welcome to keep improving this tool.

Basic rules are the one we can find in major repositories:
1. Short commit sentence: imperative, all lower case no punctuation.
2. Longer commit description: written english, with punctuation.
3. Small commits if it's possible, to keep changes very localized.

### Code style and cleanup
I am trying to follow [dotnet runtime coding style rules](https://github.com/dotnet/runtime/blob/main/docs/coding-guidelines/coding-style.md)
but with some slight variations:

1. Always use `this` keyword to refer to an instance field/property/method.
2. Don't use `s_` or `t_` prefix for static fields.
3. I personally never use `var`, but not against if it follows the coding style linked above.
4. I always have two lines for methods declaration because of the `C#` huge name's length for some types:  
    * First line with access modifiers and return type.
    * Second line with method name and arguments.  

You can find a `.editorconfig` file with the configuration I generated with VS studio.
I am personally using other editors, but please let me know if you have issue with it.

Even if I try to be as consistent as I can, don't hesitate to report any issue.

## Library as a package

You can find the package on [NuGet](https://www.nuget.org/packages/Glihm.JSInterop.Browser.WebCryptoAPI/).


## TODO

1. continue `samples` to add more examples + possibility to load the keys of MDN examples too.
2. A documentation for `cryptography` objects.
3. Rework the `test` application to be more in "unit testing" fashion. As it more a "bulk test" for now.
4. Test the compatibility with `Blazor Server`.
5. Add CI with GitHub action to publish the package.
