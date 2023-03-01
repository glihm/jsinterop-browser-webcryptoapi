# Cryptography implementations

This folder aims at proposing higher level of abstraction for `C#` developer
with little to no prior knowledge of `SubtleCrypto`.

The original idea, was to propose something very closed to `System.Security.Cryptography` API.
However, as the Web Cryptography API has some particularities, the current implementation
is proposing a first solution that is `C#` like, but with some `SubtleCrypto` touch.


## Factory pattern

The current design is using the factory pattern, with a low degree of templating (if I can say it like this...).
In fact, `SubtleCrypto` proposes some genericity, in the way how algorithms choices and parameters are passed
to the Web Cryptography API.

Usage example:

1. Add the factory of the desired algorithm as a singletion in the `Program.cs` of your blazor application.
2. Inject the factory `@inject AesFactory _aesFactory`.
3. Create the instance `AesGcm? aesGcm = await this._aesFactory.Create<AesGcm>(aesKey);`.
4. If not null, use this instance for encryption/decryption operations.

Perhaps in some usecases it would be interesting to have some `Transient` services.
But in the exploration I did on this way, it looked less natural to use the injected service.
Please let me know if you have an other point of view on this to try them!

## Cryptography Unmanaged

As mentioned in the `interfaces` documentation, the `CryptoKey` are managed by `javascript` runtime.
For this reason, they are considered unmanaged resources by `C#`, which uses `CryptoKeyDescriptor` instead.
The base classes `Unmanaged[Symmetric/PubicKey]Base` aim at factorizing the behavior of such classes that
must take care of the `CryptoKey` resources disposal.

As in Blazor we must use `IAsyncDisposable`, it's then to the developer using the library
to take care adding `await using` if the object must be disposed when the scope ends.
Or calling `await <ClassName>.DisposeAsync()` when the use of this class is no longer required.


## CryptoKey implicit import

In `interfaces` namespace, `CryptoKey` are imported explicitely using the `importKey` operation.
But in `cryptography`, there are some other way to use the keys:

1. Directly import a `CryptoKeyDescriptor`. This is the easiest way to mimic the Web Cryptography API fashion.
2. Importing a raw buffer, which implicitely imports the key for you. For example AES, creating a new instance
like `this._aesFactory.Create<AesGcm>(keyBuffer)` is enough to import the key.
3. Using the `CryptoKeyImport` class, to explicitely defines the format of the key you want to import.

The implicit import is mainly usefull for AES in order to quickly use the key. But in the other hand,
using the implicit `importKey` interface allows to quickly get a `CryptoKeyDescriptor`, which can then be passed
everywhere.

The main purpose of `cryptography` is to abstract the parameters handling and creation, which
is very convenient in `javascript`, but in `C#` it's a bit more heavy to create thoses objects
when a very simple encryption / decryption has to be done for instance.
