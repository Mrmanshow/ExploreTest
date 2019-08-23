using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explore.Services.Security
{
    public interface IEncryptionService
    {
        /// <summary>
        /// 创建salt键
        /// </summary>
        /// <param name="size">健长度</param>
        /// <returns>Salt键</returns>
        string CreateSaltKey(int size);

        /// <summary>
        /// 创建哈希密码
        /// </summary>
        /// <param name="password">密码</param>
        /// <param name="saltkey">Salt键</param>
        /// <param name="passwordFormat">密码格式（哈希算法）</param>
        /// <returns>哈希密码</returns>
        string CreatePasswordHash(string password, string saltkey, string passwordFormat = "SHA1");

        /// <summary>
        /// 创建数据哈希
        /// </summary>
        /// <param name="data">用于计算hash的数据</param>
        /// <param name="hashAlgorithm">哈希算法</param>
        /// <returns>哈希数据</returns>
        string CreateHash(byte[] data, string hashAlgorithm = "SHA1");

        /// <summary>
        /// 加密字符串
        /// </summary>
        /// <param name="plainText">要加密的文本</param>
        /// <param name="encryptionPrivateKey">加密私钥</param>
        /// <returns>加密好的文本</returns>
        string EncryptText(string plainText, string encryptionPrivateKey = "");

        /// <summary>
        /// 解密字符串
        /// </summary>
        /// <param name="cipherText">要解密的文本</param>
        /// <param name="encryptionPrivateKey">加密私钥</param>
        /// <returns>解密好的文本</returns>
        string DecryptText(string cipherText, string encryptionPrivateKey = "");
    }
}
