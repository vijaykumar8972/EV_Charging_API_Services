using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace EVCharging.Utilities.Helpers
{
    public class CryptoHelper
    {
        private const string HashAlgorithm = "HMACSHA256";

        private const string HashKey = "9E4B25E11B351C6137574D6C6130A8A1644F1F6F65FE11903839D046727B6025E85E96D9EB745D08A682A6C61D512B007191FF7A8D5D642156E8469BD0D3E1A9";
        private const string EncryptionAlgorithm = "AES";
        private const string EncryptionKey = "0AF8702E523EF5D9BEF2ED87EEB1F234EBE8E0726539C74F1DD1D7A03BA6649E";
        public string CreatePasswordHash(string password, string salt)
        {
            var valueBytes = KeyDerivation.Pbkdf2(
                                password: password,
                                salt: Encoding.UTF8.GetBytes(salt),
                                prf: KeyDerivationPrf.HMACSHA512,
                                iterationCount: 10000,
                                numBytesRequested: 256 / 8);
            return Convert.ToBase64String(valueBytes);
        }



        public bool Validate(string password, string salt, string hash)
        {
            return CreatePasswordHash(password, salt) == hash;
        }

        public string CreateSalt()
        {
            byte[] randomBytes = new byte[128 / 8];
            using (var generator = RandomNumberGenerator.Create())
            {
                generator.GetBytes(randomBytes);
                return Convert.ToBase64String(randomBytes);
            }
        }

        public byte[] Encode(byte[] data)
        {
            byte[] encryptedData;
            byte[] iv;

            using (var ms = new MemoryStream())
            {
                using (var symmetricAlgorithm = CreateSymmetricAlgorithm())
                {
                    // generate a new IV each time the Encode is called
                    symmetricAlgorithm.GenerateIV();
                    iv = symmetricAlgorithm.IV;

                    using (var cs = new CryptoStream(ms, symmetricAlgorithm.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(data, 0, data.Length);
                        cs.FlushFinalBlock();
                    }

                    encryptedData = ms.ToArray();
                }
            }

            byte[] signedData;

            // signing IV || encrypted data
            using (var hashAlgorithm = CreateHashAlgorithm())
            {
                signedData = hashAlgorithm.ComputeHash(iv.Concat(encryptedData).ToArray());
            }

            return iv.Concat(encryptedData).Concat(signedData).ToArray();
        }
        public string CreateRandomNumber()
        {
            Random random = new Random();
            return random.Next(0, 1000000).ToString("D6");
        }
        public byte[] Decode(byte[] encodedData)
        {
            // extract parts of the encoded data
            using (var symmetricAlgorithm = CreateSymmetricAlgorithm())
            {
                using (var hashAlgorithm = CreateHashAlgorithm())
                {
                    var iv = new byte[symmetricAlgorithm.BlockSize / 8];
                    var signature = new byte[hashAlgorithm.HashSize / 8];
                    var data = new byte[encodedData.Length - iv.Length - signature.Length];

                    Array.Copy(encodedData, 0, iv, 0, iv.Length);
                    Array.Copy(encodedData, iv.Length, data, 0, data.Length);
                    Array.Copy(encodedData, iv.Length + data.Length, signature, 0, signature.Length);

                    // validate the signature
                    var mac = hashAlgorithm.ComputeHash(iv.Concat(data).ToArray());

                    if (!mac.SequenceEqual(signature))
                    {
                        // message has been tampered
                        throw new ArgumentException();
                    }

                    symmetricAlgorithm.IV = iv;

                    using (var ms = new MemoryStream())
                    {
                        using (var cs = new CryptoStream(ms, symmetricAlgorithm.CreateDecryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(data, 0, data.Length);
                            cs.FlushFinalBlock();
                        }
                        return ms.ToArray();
                    }
                }
            }
        }

        private SymmetricAlgorithm CreateSymmetricAlgorithm()
        {
            var algorithm = SymmetricAlgorithm.Create(EncryptionAlgorithm);
            algorithm.Key = ToByteArray(EncryptionKey);
            return algorithm;
        }
        private HMAC CreateHashAlgorithm()
        {
            var algorithm = HMAC.Create(HashAlgorithm);
            algorithm.Key = ToByteArray(HashKey);
            return algorithm;
        }

        private static byte[] ToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length).
                Where(x => 0 == x % 2).
                Select(x => Convert.ToByte(hex.Substring(x, 2), 16)).
                ToArray();
        }
        public string GeneratePassword()
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[8];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            var finalString = new String(stringChars);
            return finalString;
        }

    }
}

