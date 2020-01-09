using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Common.Services
{
    /// <summary>
    /// A custom service that contains many commonly-used methods used for encryptions and security.
    /// </summary>
    public class Security
    {
        private static readonly Dictionary<string, string> EncryptionReplacements = new Dictionary<string, string>
        {
            {"+", "_"},
            {"/", "-"},
            {"=", "!"}
        };

        public static string Encrypt(object value)
        {
            return Encrypt(value, GlobalSettings.EncryptionKeys.General);
        }
        public static string Encrypt(object value, object key)
        {
            var toEncrypt = JsonConvert.SerializeObject(value);
            var toEncryptArray = Encoding.UTF8.GetBytes(toEncrypt);
            var hashmd5 = new MD5CryptoServiceProvider();
            var keyArray = hashmd5.ComputeHash(Encoding.UTF8.GetBytes(key.ToString()));
            hashmd5.Clear();

            var tdes = new TripleDESCryptoServiceProvider
            {
                Key = keyArray,
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };

            var cTransform = tdes.CreateEncryptor();
            var resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            tdes.Clear();

            var result = Convert.ToBase64String(resultArray, 0, resultArray.Length);

            result = EncryptionReplacements.Aggregate(result, (current, item) => current.Replace(item.Key, item.Value));
            return HttpUtility.UrlEncode(result);
        }

        public static dynamic Decrypt(string cipherString)
        {
            return Decrypt(cipherString, GlobalSettings.EncryptionKeys.General);
        }
        public static dynamic Decrypt(string cipherString, object key)
        {
            cipherString = (HttpUtility.UrlDecode(cipherString) != cipherString)
                ? HttpUtility.UrlDecode(cipherString)
                : cipherString;
            cipherString = EncryptionReplacements.Aggregate(cipherString,
                (current, item) => current.Replace(item.Value, item.Key));

            var toEncryptArray = Convert.FromBase64String(cipherString);
            var hashmd5 = new MD5CryptoServiceProvider();
            var keyArray = hashmd5.ComputeHash(Encoding.UTF8.GetBytes(key.ToString()));
            hashmd5.Clear();

            var tdes = new TripleDESCryptoServiceProvider
            {
                Key = keyArray,
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };

            var cTransform = tdes.CreateDecryptor();
            var resultArray = cTransform.TransformFinalBlock(
                toEncryptArray, 0, toEncryptArray.Length);
            tdes.Clear();

            var result = Encoding.UTF8.GetString(resultArray);
            return JsonConvert.DeserializeObject(result);
        }


        // Hashing
        public static string GetHashString(string inputString)
        {
            var sb = new StringBuilder();
            foreach (var b in ComputeHash(inputString))
                sb.Append(b.ToString("X2"));

            return sb.ToString();
        }
        public static byte[] ComputeHash(string plainText)
        {
            var sha = new SHA256Managed();
            return sha.ComputeHash(Encoding.UTF8.GetBytes(plainText));
        }

        // Decryption for Exigo silent login
        public static string AdminTokenDecrypt(string encrypted, string key, string iv)
        {
            string plaintext = null;

            using (var aesAlg = new AesManaged())
            using (var hasher = new SHA256Managed())
            {
                aesAlg.Key = hasher.ComputeHash(Encoding.UTF8.GetBytes(key));
                aesAlg.IV = Encoding.UTF8.GetBytes(iv);

                var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (var msDecrypt = new MemoryStream(Convert.FromBase64String(encrypted)))
                {
                    using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (var srDecrypt = new StreamReader(csDecrypt))
                        {
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
            return plaintext;
        }

        public static string AESEncrypt(string plainText, string key, string iv)
        {
            byte[] encrypted;

            using (var aesAlg = new AesManaged())
            using (var hasher = new SHA256Managed())
            {
                aesAlg.Key = hasher.ComputeHash(Encoding.UTF8.GetBytes(key));
                aesAlg.IV = Encoding.UTF8.GetBytes(iv);

                var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                using (var msEncrypt = new MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }
            return Convert.ToBase64String(encrypted);
        }

        public static string AESDecrypt(string encrypted, string key, string iv)
        {
            string plaintext = null;

            using (var aesAlg = new AesManaged())
            using (var hasher = new SHA256Managed())
            {
                aesAlg.Key = hasher.ComputeHash(Encoding.UTF8.GetBytes(key));
                aesAlg.IV = Encoding.UTF8.GetBytes(iv);


                var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (var msDecrypt = new MemoryStream(Convert.FromBase64String(encrypted)))
                {
                    using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (var srDecrypt = new StreamReader(csDecrypt))
                        {
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
            return plaintext;
        }
    }
}