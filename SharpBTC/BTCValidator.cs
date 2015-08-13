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
    public class BTCValidator
    {
        [TestCase]
        public void ValidateBitcoinAddressTest()
        {
            Assert.IsTrue(ValidateBitcoinAddress("1AGNa15ZQXAZUgFiqJ2i7Z2DPU2J6hW62i")); // VALID
            Assert.IsTrue(ValidateBitcoinAddress("1Q1pE5vPGEEMqRcVRMbtBK842Y6Pzo6nK9")); // VALID
            Assert.Throws<Exception>(() => ValidateBitcoinAddress("1AGNa15ZQXAZUgFiqJ2i7Z2DPU2J6hW62X")); // checksum changed, original data
            Assert.Throws<Exception>(() => ValidateBitcoinAddress("1ANNa15ZQXAZUgFiqJ2i7Z2DPU2J6hW62i")); // data changed, original checksum
            Assert.Throws<Exception>(() => ValidateBitcoinAddress("1A Na15ZQXAZUgFiqJ2i7Z2DPU2J6hW62i")); // invalid chars
            Assert.Throws<Exception>(() => ValidateBitcoinAddress("BZbvjr")); // checksum is fine, address too short
        }

        const string Alphabet = "123456789ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz";
        const int Size = 25;

        public static bool ValidateBitcoinAddress(string address)
        {
            if (address.Length < 26 || address.Length > 35) throw new Exception("wrong length");
            var decoded = DecodeBase58(address);
            var d1 = Hash(decoded.SubArray(0, 21));
            var d2 = Hash(d1);
            if (!decoded.SubArray(21, 4).SequenceEqual(d2.SubArray(0, 4))) throw new Exception("bad digest");
            return true;
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
