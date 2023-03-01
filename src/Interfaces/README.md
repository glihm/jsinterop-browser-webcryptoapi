# Web Cryptography API interfaces

This folder contains `C#` classes related to the interfaces
described in the Web Cryptography API.

Some resources:
* MDN web docs: https://developer.mozilla.org/en-US/docs/Web/API/Web_Crypto_API
* W3C specification: https://w3c.github.io/webcrypto/

The `C#` classes attempt to be as close as possible from the specification.
For obvious reason of how this library is implemented, some minor changes
may be found, but this is not altering the underlying process done by the
web-broswer API.


## CryptoKey interface

The most divergent interface is the `CryptoKey` interface.
In fact, this library is relying on `javascript` objects, but
`C#` must keep track of the `javascript` objects to interact with them.

In the current design of this library, the `CryptoKey` that are generated
using the `SubtleCrypto` interface are **always** managed by the `javascript` runtime.
This allows to converve very important properties of `CryptoKey` interface, such as
the `extractable` property.

To easily allow `C#` to interact with the `CryptoKey` that are stored in the
`javascript` runtime, a `CryptoKeyDescriptor` containing an identifier is used as a link between the two
runtimes. As such, anytime `C#` must call `javascript` runtime to process some
data using the `SubtleCrypto` interface, `C#` passes it's `CryptoKeyDescriptor` object
with the `identifier` of the the `CryptoKey`. Then, `javascript` uses
the identifier to retrieve the `CryptoKey` to make the cryptographic computation.

Perhaps a better implementation can be done, by keeping references to `javascript`
object directly from `C#`. But for now, the current design is using
a simple `string` identififer inside the `CryptoKeyDescriptor`.

Finally, the `javascript` allocated `CryptoKey` are then considered as `unmanaged` resources
for `C#`. For this reason, a `CryptoKeyDescriptor` in `C#` is implementing the `IAsyncDisposable` interface.
This allows the caller to use `await using` to easily dispose the `javascript` allocated resources, or
call `DisposeAsync` directly.
