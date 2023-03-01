'use strict';

/**
 * Fills a buffer with random value.
 * The generated buffer's length is defined
 * by the given byte count.
 * 
 * @param {number} {byteCount} - Number of bytes to be randomly generated.
 * @returns {Uint8Array} - Buffer with random bytes.
 */
function getRandomValuesFromByteCount(byteCount) {
    const buf = new Uint8Array(byteCount);
    self.crypto.getRandomValues(buf);
    return buf;
}

/**
 * Verifies if the Web Crypto API is available.
 * 
 * @throws {Error} - The Web Crypto API is not available.
 */
function verifyCryptoSupport() {
    if (self.crypto == null) {
        throw new Error("self.crypto not defined.");
    }
}

export {
    getRandomValuesFromByteCount,
    verifyCryptoSupport,

}
