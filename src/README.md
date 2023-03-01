# Web Cryptography API


## Project structure

This library is composed of two main parts:
1. `Interfaces` folder, containing `C#` classes that correspond to the `javascript` interfaces
described in the Web Cryptography API described in Web Cryptography API.
2. `Cryptography` folder, with an attempt to add a layer of abstraction on the top of
Web Cryptography API, which is supposed to be used over "low-level" interfaces.

Each important folder has (normally) its own `README.md` file to describe what's doing.


## JSResult

An instance of this class is returned when a call is made to `javascript` runtime.
This aims at reducing the need for developer to use excesive `try catch` blocks.

For now, the library is not verifying every input of the developer as the web-browser is doing.
So, you must always verify that the `JSResult` is not presenting any error before continuing.
To avoid nullable context warning and useless checks on nullity, the value of a `JSResult`
can be retrieved safely after checking the `JSResult` validity.

For example:

```
JSResultValue<CryptoKey> res = await this._crypto.Subtle.GenerateKey(....);
if (!res)
{
    // Do not continue, javascript runtime has reported an error.
    // It may be a bad algorithm parameter you provided, or something else.
    this._logger.LogError(res.Error?.Message);
    return;
}

// No need to check if res.Value is null or not, as it will throw
// if something very unexpected happens. But `res` was checked, no reason
// for the value to be null then. If you may have a null value, simply access it.
CryptoKey key = res.GetValueOrThrow();
```

I am not sure this is the best design, any suggestion/comments is very welcome.
But the idea is to keep something simple, without throwing exception everywhere.
Also, the caller can easily check the message returned by the `javascript` runtime. 
