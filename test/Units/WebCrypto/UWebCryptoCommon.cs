using Glihm.JSInterop.Browser.WebCryptoAPI.Cryptography;
using Glihm.JSInterop.Browser.WebCryptoAPI.Interfaces;
using Glihm.JSInterop.Browser.WebCryptoAPI.Interfaces.CryptoKeys;
using Glihm.JSInterop.Browser.WebCryptoAPI.Interfaces.Subtle.Params;
using Glihm.JSInterop.Browser.WebCryptoAPI.JSHelpers;

namespace TestApp.Units.WebCrypto;

public static class UWebCryptoCommon
{
    /// <summary>
    /// Encrypt/Decrypt testing with key generation.
    /// </summary>
    /// <param name="gen"></param>
    /// <param name="prs"></param>
    /// <returns></returns>
    public static async Task<UnitTestResult>
    EncryptDecrypt(Crypto crypto,
                   IGenParams gen,
                   IEncryptParams ePrs,
                   IDecryptParams dPrs,
                   byte[]? data = null)
    {
        if (data is null)
        {
            data = new byte[] { 1, 2, 3 };
        }

        JSResultValue<CryptoKeyDescriptor> res = await crypto.Subtle.GenerateKey(
            gen,
            false,
            CryptoKeyUsage.Encrypt | CryptoKeyUsage.Decrypt);

        if (!res)
        {
            return UnitTestResult.Failed(res.Error?.Message);
        }

        CryptoKeyDescriptor ck = res.GetValueOrThrow();
        return await EncryptDecrypt(crypto, ck, ePrs, dPrs, data);
    }

    /// <summary>
    /// Encrypt/Decrypt from existing key.
    /// </summary>
    /// <param name="gen"></param>
    /// <param name="prs"></param>
    /// <returns></returns>
    public static async Task<UnitTestResult>
    EncryptDecrypt(Crypto crypto,
                   CryptoKeyDescriptor ck,
                   IEncryptParams ePrs,
                   IDecryptParams dPrs,
                   byte[]? data = null)
    {
        if (data is null)
        {
            data = new byte[] { 1, 2, 3 };
        }

        JSResultValue<byte[]> cipherRes = await crypto.Subtle.Encrypt(ePrs, ck, data);
        if (!cipherRes)
        {
            return UnitTestResult.Failed(cipherRes.Error?.Message);
        }
        byte[] cipher = cipherRes.GetValueOrThrow();

        JSResultValue<byte[]> plainRes = await crypto.Subtle.Decrypt(dPrs, ck, cipher);
        if (!plainRes)
        {
            return UnitTestResult.Failed(plainRes.Error?.Message);
        }
        byte[] plain = plainRes.GetValueOrThrow();

        return plain.SequenceEqual(data) ? UnitTestResult.Passed() :
                                           UnitTestResult.Failed("Plaintext is not equal to original data.");
    }

    /// <summary>
    /// Encrypt/Decrypt from existing key pair.
    /// </summary>
    /// <param name="crypto"></param>
    /// <param name="ckp"></param>
    /// <param name="ePrs"></param>
    /// <param name="dPrs"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    public static async Task<UnitTestResult>
    EncryptDecrypt(Crypto crypto,
                   CryptoKeyPairDescriptor ckp,
                   IEncryptParams ePrs,
                   IDecryptParams dPrs,
                   byte[]? data = null)
    {
        if (data is null)
        {
            data = new byte[] { 1, 2, 3 };
        }

        if (ckp.PublicKey is null || ckp.PrivateKey is null)
        {
            return UnitTestResult.Failed("missing keys.");
        }

        JSResultValue<byte[]> cipherRes = await crypto.Subtle.Encrypt(ePrs, ckp.PublicKey, data);
        if (!cipherRes)
        {
            return UnitTestResult.Failed(cipherRes.Error?.Message);
        }
        byte[] cipher = cipherRes.GetValueOrThrow();

        JSResultValue<byte[]> plainRes = await crypto.Subtle.Decrypt(dPrs, ckp.PrivateKey, cipher);
        if (!plainRes)
        {
            return UnitTestResult.Failed(plainRes.Error?.Message);
        }
        byte[] plain = plainRes.GetValueOrThrow();

        return plain.SequenceEqual(data) ? UnitTestResult.Passed() :
                                           UnitTestResult.Failed("Plaintext is not equal to original data.");
    }

    /// <summary>
    /// Sign/Verify with random key pair generation.
    /// </summary>
    /// <returns></returns>
    public static async Task<UnitTestResult>
    SignVerify(Crypto crypto,
               IGenParams gen,
               ISignParams signPrs,
               IVerifyParams verifrs,
               byte[]? data = null)
    {
        if (data is null)
        {
            data = new byte[] { 1, 2, 3 };
        }

        JSResultValue<CryptoKeyPairDescriptor> resKp = await crypto.Subtle.GenerateKeyPair(
            gen,
            false,
            CryptoKeyUsage.Sign | CryptoKeyUsage.Verify);

        if (!resKp)
        {
            return UnitTestResult.Failed(resKp.Error?.Message);
        }

        CryptoKeyPairDescriptor kp = resKp.GetValueOrThrow();
        return await SignVerify(crypto, kp, signPrs, verifrs, data);
    }

    /// <summary>
    /// Sign/Verify from existing keys pair.
    /// </summary>
    /// <returns></returns>
    public static async Task<UnitTestResult>
    SignVerify(Crypto crypto,
               CryptoKeyPairDescriptor ckp,
               ISignParams signPrs,
               IVerifyParams verifPrs,
               byte[]? data = null)
    {
        if (data is null)
        {
            data = new byte[] { 1, 2, 3 };
        }

        if (ckp.PublicKey is null || ckp.PrivateKey is null)
        {
            return UnitTestResult.Failed("missing keys.");
        }

        JSResultValue<byte[]> resSign = await crypto.Subtle.Sign(
            signPrs,
            ckp.PrivateKey,
            data);

        if (!resSign)
        {
            return UnitTestResult.Failed(resSign.Error?.Message);
        }

        byte[] signature = resSign.GetValueOrThrow();

        JSResultValue<bool> resVerify = await crypto.Subtle.Verify(
            verifPrs,
            ckp.PublicKey,
            signature,
            data);

        bool isSignatureValid = resVerify.GetValueOrThrow();

        return isSignatureValid ? UnitTestResult.Passed() :
                                  UnitTestResult.Failed("Signature is invalid.");
    }

    /// <summary>
    /// Sign/Verify from existing crypto key.
    /// </summary>
    /// <returns></returns>
    public static async Task<UnitTestResult>
    SignVerify(Crypto crypto,
               CryptoKeyDescriptor ck,
               ISignParams signPrs,
               IVerifyParams verifPrs,
               byte[]? data = null)
    {
        if (data is null)
        {
            data = new byte[] { 1, 2, 3 };
        }

        JSResultValue<byte[]> resSign = await crypto.Subtle.Sign(
            signPrs,
            ck,
            data);

        if (!resSign)
        {
            return UnitTestResult.Failed(resSign.Error?.Message);
        }

        byte[] signature = resSign.GetValueOrThrow();

        JSResultValue<bool> resVerify = await crypto.Subtle.Verify(
            verifPrs,
            ck,
            signature,
            data);

        bool isSignatureValid = resVerify.GetValueOrThrow();

        return isSignatureValid ? UnitTestResult.Passed() :
                                  UnitTestResult.Failed("Signature is invalid.");
    }

}
