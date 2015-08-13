/**
 Copyright 2015 Martin Hohenberg martinhohenberg@gmail.com
 This file is part of BTCSharp.

 This program is free software: you can redistribute it and/or modify 
 it under the terms of the GNU General Public License as published by
 the Free Software Foundation, either version 2 of the License, or
 (at your option) any later version.

 This program is distributed in the hope that it will be useful,
 but WITHOUT ANY WARRANTY; without even the implied warranty of
 MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
 GNU General Public License for more details.

 You should have received a copy of the GNU General Public License
 along with this program.If not, see<http://www.gnu.org/licenses/>.
 **/
 
using System;
using System.Linq;
using System.Security.Cryptography;
using NUnit.Framework;

namespace SharpBTC
{
    public enum Prefix
    {
        Pubkey_hash,
        Script_hash,
        Private_key_uncompressed,
        Private_key_compressed,
        BIP32_pubkey,
        BIP32_private_key,
        Testnet_pubkey_hash,
        Testnet_script_hash,
        Testnet_Private_key_uncompressed,
        Testnet_Private_key_compressed,
        Testnet_BIP32_pubkey,
        Testnet_BIP32_private_key,
        UNKNOWN
    }

    public class BTCValidator
    {
        [TestCase]
        public void ValidateBitcoinAddressTest()
        {
            Assert.IsTrue(ValidateBitcoinAddress("1AGNa15ZQXAZUgFiqJ2i7Z2DPU2J6hW62i")); // VALID
            Assert.Throws<Exception>(() => ValidateBitcoinAddress("1AGNa15ZQXAZUgFiqJ2i7Z2DPU2J6hW62X")); // checksum changed, original data
            Assert.Throws<Exception>(() => ValidateBitcoinAddress("1ANNa15ZQXAZUgFiqJ2i7Z2DPU2J6hW62i")); // data changed, original checksum
            Assert.Throws<Exception>(() => ValidateBitcoinAddress("1A Na15ZQXAZUgFiqJ2i7Z2DPU2J6hW62i")); // invalid chars
            Assert.Throws<Exception>(() => ValidateBitcoinAddress("BZbvjr")); // checksum is fine, address too short
            Assert.Throws<Exception>(() => ValidateBitcoinAddress("3EktnHQD7RiAE6uzMj2ZifT9YgRrkSgzQX")); // Multisig address
            Assert.Throws<Exception>(() => ValidateBitcoinAddress("mipcBbFg9gMiCh81Kj8tqqdgoZub1ZJRfn")); // Testnet address
            Assert.Throws<Exception>(() => ValidateBitcoinAddress("2MzQwSSnBHWHqSAqtTVQ6v47XtaisrJa1Vc")); // Testnet Multisig address

            Assert.IsTrue(ValidateBitcoinMultisigAddress("3EktnHQD7RiAE6uzMj2ZifT9YgRrkSgzQX")); // VALID
            Assert.Throws<Exception>(() => ValidateBitcoinMultisigAddress("3EktnHQD7RiAE6uzMj2ZifT9YgRrkSgzQA")); // checksum changed, original data
            Assert.Throws<Exception>(() => ValidateBitcoinMultisigAddress("3EktnHQD7RiAE4uzMj2ZifT9YgRrkSgzQX")); // data changed, original checksum
            Assert.Throws<Exception>(() => ValidateBitcoinMultisigAddress("3E ktnHQD7RiAE6uzMj2ZifT9YgRrkSgzQX")); // invalid chars
            Assert.Throws<Exception>(() => ValidateBitcoinMultisigAddress("BZbvjr")); // checksum is fine, address too short
            Assert.Throws<Exception>(() => ValidateBitcoinMultisigAddress("1Q1pE5vPGEEMqRcVRMbtBK842Y6Pzo6nK9")); // Bitcoin address
            Assert.Throws<Exception>(() => ValidateBitcoinMultisigAddress("mipcBbFg9gMiCh81Kj8tqqdgoZub1ZJRfn")); // Testnet address
            Assert.Throws<Exception>(() => ValidateBitcoinMultisigAddress("2MzQwSSnBHWHqSAqtTVQ6v47XtaisrJa1Vc")); // Testnet Multisig address
            
            Assert.IsTrue(ValidateTestnetAddress("mipcBbFg9gMiCh81Kj8tqqdgoZub1ZJRfn")); // VALID
            Assert.Throws<Exception>(() => ValidateTestnetAddress("mipcBbFg9gMiCh81Kj8tqqdgoZub1ZJRfG")); // checksum changed, original data
            Assert.Throws<Exception>(() => ValidateTestnetAddress("mipcBbFg9gMiCh82Kj8tqqdgoZub1ZJRfn")); // data changed, original checksum
            Assert.Throws<Exception>(() => ValidateTestnetAddress("mipcBbFg9gMiCh81 Kj8tqqdgoZub1ZJRfn")); // invalid chars
            Assert.Throws<Exception>(() => ValidateTestnetAddress("BZbvjr")); // checksum is fine, address too short
            Assert.Throws<Exception>(() => ValidateTestnetAddress("3EktnHQD7RiAE6uzMj2ZifT9YgRrkSgzQX")); // Multisig address
            Assert.Throws<Exception>(() => ValidateTestnetAddress("1Q1pE5vPGEEMqRcVRMbtBK842Y6Pzo6nK9")); // Bitcoin address
            Assert.Throws<Exception>(() => ValidateTestnetAddress("2MzQwSSnBHWHqSAqtTVQ6v47XtaisrJa1Vc")); // Testnet Multisig address

            Assert.IsTrue(ValidateTestnetMultisigAddress("2MzQwSSnBHWHqSAqtTVQ6v47XtaisrJa1Vc")); // VALID
            Assert.Throws<Exception>(() => ValidateTestnetMultisigAddress("2MzQwSSnBHWHqSAqtTVQ6v47XtaisrJa1VA")); // checksum changed, original data
            Assert.Throws<Exception>(() => ValidateTestnetMultisigAddress("2MzQwSSnbHWHqSAqtTVQ6v47XtaisrJa1Vc")); // data changed, original checksum
            Assert.Throws<Exception>(() => ValidateTestnetMultisigAddress("2M zQwSSnBHWHqSAqtTVQ6v47XtaisrJa1Vc")); // invalid chars
            Assert.Throws<Exception>(() => ValidateTestnetMultisigAddress("BZbvjr")); // checksum is fine, address too short
            Assert.Throws<Exception>(() => ValidateTestnetMultisigAddress("3EktnHQD7RiAE6uzMj2ZifT9YgRrkSgzQX")); // Multisig address
            Assert.Throws<Exception>(() => ValidateTestnetMultisigAddress("1Q1pE5vPGEEMqRcVRMbtBK842Y6Pzo6nK9")); // Bitcoin address
            Assert.Throws<Exception>(() => ValidateTestnetMultisigAddress("mipcBbFg9gMiCh81Kj8tqqdgoZub1ZJRfn")); // Testnet address
            
        }

        const string Alphabet = "123456789ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz";
        const int Size = 25;

        /// <summary>
        /// Validates that the address is an valid example of Prefix
        /// </summary>
        /// <param name="address"></param>
        /// <param name="expectedType"></param>
        /// <returns></returns>
        public static bool ValidateAddress(string address, Prefix expectedType)
        {
            if (address.Length < 26 || address.Length > 35) throw new Exception("wrong length");
            var decoded = DecodeBase58(address);
            var d1 = Hash(decoded.SubArray(0, 21));
            var d2 = Hash(d1);

            Prefix p = identifyByPrefix(address);

            if (p != expectedType)
            {
                throw new Exception("Not a Bitcoin address!");
            }

            if (!decoded.SubArray(21, 4).SequenceEqual(d2.SubArray(0, 4))) throw new Exception("bad digest");
            return true;
        }

        /// <summary>
        /// Wrapper function vor ValidateAddress for Bitcoin addresses
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public static bool ValidateBitcoinAddress(string address)
        {
            return ValidateAddress(address, Prefix.Pubkey_hash);
        }

        /// <summary>
        /// Wrapper function vor ValidateAddress for Multisig addresses
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public static bool ValidateBitcoinMultisigAddress(string address)
        {
            return ValidateAddress(address, Prefix.Script_hash);
        }

        /// <summary>
        /// Wrapper function vor ValidateAddress for Testnet addresses
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public static bool ValidateTestnetAddress(string address)
        {
            return ValidateAddress(address, Prefix.Testnet_pubkey_hash);
        }

        /// <summary>
        /// Wrapper function vor ValidateAddress for Testnet Multisig addresses
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public static bool ValidateTestnetMultisigAddress(string address)
        {
            return ValidateAddress(address, Prefix.Testnet_script_hash);
        }

        /// <summary>
        /// Tries to identify an address by it's prefix.
        /// Prefix definition by https://en.bitcoin.it/wiki/List_of_address_prefixes
        /// </summary>
        /// <param name="address">The address to be identified</param>
        /// <returns>Prefix type</returns>
        private static Prefix identifyByPrefix(string address)
        {
            if (address.IndexOf('1') == 0) { return Prefix.Pubkey_hash; }
            else if (address.IndexOf('3') == 0) { return Prefix.Script_hash; }
            else if (address.IndexOf('5') == 0) { return Prefix.Private_key_uncompressed; }
            else if (address.IndexOf('K') == 0) { return Prefix.Private_key_compressed; }
            else if (address.IndexOf('L') == 0) { return Prefix.Private_key_compressed; }
            else if (address.IndexOf("xpub") == 0) { return Prefix.BIP32_pubkey; }
            else if (address.IndexOf("xprv") == 0) { return Prefix.BIP32_private_key; }
            else if (address.IndexOf('m') == 0) { return Prefix.Testnet_pubkey_hash; }
            else if (address.IndexOf('n') == 0) { return Prefix.Testnet_pubkey_hash; }
            else if (address.IndexOf('2') == 0) { return Prefix.Testnet_script_hash; }
            else if (address.IndexOf('9') == 0) { return Prefix.Testnet_Private_key_uncompressed; }
            else if (address.IndexOf('c') == 0) { return Prefix.Testnet_Private_key_compressed; }
            else if (address.IndexOf("tpub") == 0) { return Prefix.Testnet_BIP32_pubkey; }
            else if (address.IndexOf("tprv") == 0) { return Prefix.Testnet_BIP32_private_key; }
            else { return Prefix.UNKNOWN; }
        }

        private static byte[] DecodeBase58(string input)
        {
            var output = new byte[Size];
            foreach (var t in input)
            {
                var currentCharacter = Alphabet.IndexOf(t);
                if (currentCharacter == -1) throw new Exception("invalid character found");
                var j = Size;
                while (--j > 0)
                {
                    currentCharacter += 58 * output[j];
                    output[j] = (byte)(currentCharacter % 256);
                    currentCharacter /= 256;
                }
                if (currentCharacter != 0) throw new Exception("address too long");
            }
            return output;
        }

        private static byte[] Hash(byte[] bytes)
        {
            var hasher = new SHA256Managed();
            return hasher.ComputeHash(bytes);
        }
    }

    public static class ArrayExtensions
    {
        public static T[] SubArray<T>(this T[] data, int index, int length)
        {
            var result = new T[length];
            Array.Copy(data, index, result, 0, length);
            return result;
        }
    }
}
