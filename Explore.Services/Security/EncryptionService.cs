using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Explore.Services.Security
{
    public class EncryptionService : IEncryptionService
    {

        /// <summary>
        /// 创建salt键
        /// </summary>
        /// <param name="size">健长度</param>
        /// <returns>Salt键</returns>
        public virtual string CreateSaltKey(int size)
        {
            //生成加密随机数
            using (var provider = new RNGCryptoServiceProvider())
            {
                var buff = new byte[size];
                provider.GetBytes(buff);

                // 返回随机数的base64字符串表示形式
                return Convert.ToBase64String(buff);
            }
        }

        /// <summary>
        /// 创建哈希密码
        /// </summary>
        /// <param name="password">密码</param>
        /// <param name="saltkey">Salt键</param>
        /// <param name="passwordFormat">密码格式（哈希算法）</param>
        /// <returns>哈希密码</returns>
        public virtual string CreatePasswordHash(string password, string saltkey, string passwordFormat = "SHA1")
        {
            return CreateHash(Encoding.UTF8.GetBytes(String.Concat(password, saltkey)), passwordFormat);
        }

        /// <summary>
        /// 创建数据哈希
        /// </summary>
        /// <param name="data">用于计算hash的数据</param>
        /// <param name="hashAlgorithm">哈希算法</param>
        /// <returns>哈希数据</returns>
        public virtual string CreateHash(byte[] data, string hashAlgorithm = "SHA1")
        {
            if (String.IsNullOrEmpty(hashAlgorithm))
                hashAlgorithm = "SHA1";

            //return FormsAuthentication.HashPasswordForStoringInConfigFile(saltAndPassword, passwordFormat);
            var algorithm = HashAlgorithm.Create(hashAlgorithm);
            if (algorithm == null)
                throw new ArgumentException("Unrecognized hash name");

            var hashByteArray = algorithm.ComputeHash(data);
            return BitConverter.ToString(hashByteArray).Replace("-", "");
        }

        /// <summary>
        /// 加密字符串
        /// </summary>
        /// <param name="plainText">要加密的文本</param>
        /// <param name="encryptionPrivateKey">加密私钥</param>
        /// <returns>加密好的文本</returns>
        public virtual string EncryptText(string plainText, string encryptionPrivateKey = "")
        {
            if (string.IsNullOrEmpty(plainText))
                return plainText;

            if (String.IsNullOrEmpty(encryptionPrivateKey))
                encryptionPrivateKey = "273ece6f97dd844d";

            using (var provider = new TripleDESCryptoServiceProvider())
            {
                provider.Key = Encoding.ASCII.GetBytes(encryptionPrivateKey.Substring(0, 16));
                provider.IV = Encoding.ASCII.GetBytes(encryptionPrivateKey.Substring(8, 8));

                byte[] encryptedBinary = EncryptTextToMemory(plainText, provider.Key, provider.IV);
                return Convert.ToBase64String(encryptedBinary);
            }
        }

        /// <summary>
        /// 解密字符串
        /// </summary>
        /// <param name="cipherText">要解密的文本</param>
        /// <param name="encryptionPrivateKey">加密私钥</param>
        /// <returns>解密好的文本</returns>
        public virtual string DecryptText(string cipherText, string encryptionPrivateKey = "")
        {
            if (String.IsNullOrEmpty(cipherText))
                return cipherText;

            if (String.IsNullOrEmpty(encryptionPrivateKey))
                encryptionPrivateKey = "273ece6f97dd844d";

            using (var provider = new TripleDESCryptoServiceProvider())
            {
                provider.Key = Encoding.ASCII.GetBytes(encryptionPrivateKey.Substring(0, 16));
                provider.IV = Encoding.ASCII.GetBytes(encryptionPrivateKey.Substring(8, 8));

                byte[] buffer = Convert.FromBase64String(cipherText);
                return DecryptTextFromMemory(buffer, provider.Key, provider.IV);
            }
        }

        #region Utilities

        private byte[] EncryptTextToMemory(string data, byte[] key, byte[] iv)
        {
            using (var ms = new MemoryStream())
            {
                using (var cs = new CryptoStream(ms, new TripleDESCryptoServiceProvider().CreateEncryptor(key, iv), CryptoStreamMode.Write))
                {
                    byte[] toEncrypt = Encoding.Unicode.GetBytes(data);
                    cs.Write(toEncrypt, 0, toEncrypt.Length);
                    cs.FlushFinalBlock();
                }

                return ms.ToArray();
            }
        }

        private string DecryptTextFromMemory(byte[] data, byte[] key, byte[] iv)
        {
            using (var ms = new MemoryStream(data))
            {
                using (var cs = new CryptoStream(ms, new TripleDESCryptoServiceProvider().CreateDecryptor(key, iv), CryptoStreamMode.Read))
                {
                    using (var sr = new StreamReader(cs, Encoding.Unicode))
                    {
                        return sr.ReadToEnd();
                    }
                }
            }
        }

        #endregion
    }
}
