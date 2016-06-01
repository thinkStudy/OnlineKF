using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Security.Cryptography;
using System.IO;
using System.Text;

namespace OnlineKF.Utils
{
    /// <summary>
    /// SecurityHelper 的摘要说明
    /// </summary>
    public class SecurityHelper
    {
        public SecurityHelper()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }

        /// <summary>
        /// md5加密utf-8编码字符串
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public static string MD5Encrypt(string src)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] data = System.Text.Encoding.UTF8.GetBytes(src);//将字符编码为一个字节序列         
            byte[] md5data = md5.ComputeHash(data);//计算data字节数组的哈希值         
            md5.Clear();
            return StringTools.ByteToHex(md5data);

        }

        /// <summary>
        /// SHA1加密utf-8编码字符串
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public static string SHA1Encrypt(string src)
        {
            SHA1 sha = new SHA1CryptoServiceProvider();
            //将mystr转换成byte[]
            byte[] dataToHash = System.Text.Encoding.UTF8.GetBytes(src);
            //Hash运算
            byte[] dataHashed = sha.ComputeHash(dataToHash);
            sha.Clear();
            return StringTools.ByteToHex(dataHashed);
        }

        /// <summary>
        /// AES加密 CBC模式,PKCS7 padding，加密后转换为base64编码
        /// </summary>
        /// <param name="source">utf-8编码字符串</param>
        /// <param name="key">密钥128 192 256位用16进制字符串表示</param>      
        /// <param name="iv">偏移向量128位用16进制字符串表示</param>
        /// <returns></returns>
        public static string AESEncrypt(string source, string key, string iv)
        {
            SymmetricAlgorithm mCSP = new RijndaelManaged();
            mCSP.Key = StringTools.HexToByte(key);
            mCSP.IV = StringTools.HexToByte(iv);//如果是ECB模式不需要IV
            //指定加密的运算模式
            mCSP.Mode = System.Security.Cryptography.CipherMode.CBC;

            //获取或设置加密算法的填充模式
            mCSP.Padding = System.Security.Cryptography.PaddingMode.PKCS7;

            ICryptoTransform ct = mCSP.CreateEncryptor();

            byte[] byt = Encoding.UTF8.GetBytes(source);

            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, ct, CryptoStreamMode.Write);
            cs.Write(byt, 0, byt.Length);
            cs.FlushFinalBlock();
            cs.Close();

            //string result = StringTools.ByteToHex(ms.ToArray());
            string result = Convert.ToBase64String(ms.ToArray());
            return result;

        }

        /// <summary>
        /// AES解密 CBC模式,PKCS7 padding
        /// </summary>
        /// <param name="Value">base64编码的加密串</param>
        /// <param name="key">密钥128 192 256位用16进制字符串表示</param>
        /// <param name="iv">偏移向量128位用16进制字符串表示</param>
        /// <returns></returns>
        public static string AESDecrypt(string Value, string key, string iv)
        {
            SymmetricAlgorithm mCSP = new RijndaelManaged();

            byte[] byt;
            mCSP.Key = StringTools.HexToByte(key);
            mCSP.IV = StringTools.HexToByte(iv);
            mCSP.Mode = System.Security.Cryptography.CipherMode.CBC;
            mCSP.Padding = System.Security.Cryptography.PaddingMode.PKCS7;

            ICryptoTransform ct = mCSP.CreateDecryptor();
            //byt = StringTools.HexToByte(Value);
            //对Value进行解码操作

            try
            {
                byt = Convert.FromBase64String(Value);
            }
            catch (Exception e)
            {
                byt = Convert.FromBase64String(HttpUtility.UrlDecode(Value));

            }
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, ct, CryptoStreamMode.Write);
            cs.Write(byt, 0, byt.Length);
            cs.FlushFinalBlock();
            cs.Close();
            return Encoding.UTF8.GetString(ms.ToArray());//.Replace("\0", "");
        }


        /// <summary>
        /// 3des加密 ECB加密模式 Zeros填充模式
        /// </summary>
        /// <param name="source">utf-8编码字符串</param>
        /// <param name="key">密钥128 192 256位，用16进制字符串表示</param>
        /// <returns></returns>
        public static string TrDESEncrypt(string source, string key)
        {
            SymmetricAlgorithm mCSP = new TripleDESCryptoServiceProvider();
            ICryptoTransform ct;
            MemoryStream ms;
            CryptoStream cs;
            byte[] byt;
            mCSP.Key = StringTools.HexToByte(key);
            //mCSP.IV = Convert.FromBase64String(sIV);
            //指定加密的运算模式
            mCSP.Mode = System.Security.Cryptography.CipherMode.ECB;

            //获取或设置加密算法的填充模式
            mCSP.Padding = System.Security.Cryptography.PaddingMode.Zeros;

            ct = mCSP.CreateEncryptor();

            byt = Encoding.UTF8.GetBytes(source);

            ms = new MemoryStream();
            cs = new CryptoStream(ms, ct, CryptoStreamMode.Write);
            cs.Write(byt, 0, byt.Length);
            cs.FlushFinalBlock();
            cs.Close();

            string result = Convert.ToBase64String(ms.ToArray());
            return result;

        }


        /// <summary>
        /// 3des解密 ECB加密模式 Zeros填充模式
        /// </summary>
        /// <param name="Value"></param>
        /// <param name="key">密钥128 192 256位，用16进制字符串表示</param>
        /// <returns></returns>
        public static string TrDESDecrypt(string Value, string key)
        {
            SymmetricAlgorithm mCSP = new TripleDESCryptoServiceProvider();
            ICryptoTransform ct;
            MemoryStream ms;
            CryptoStream cs;
            byte[] byt;
            mCSP.Key = StringTools.HexToByte(key);
            mCSP.Mode = System.Security.Cryptography.CipherMode.ECB;
            mCSP.Padding = System.Security.Cryptography.PaddingMode.Zeros;

            ct = mCSP.CreateDecryptor();

            byt = Convert.FromBase64String(Value);
            ms = new MemoryStream();
            cs = new CryptoStream(ms, ct, CryptoStreamMode.Write);
            cs.Write(byt, 0, byt.Length);
            cs.FlushFinalBlock();
            cs.Close();
            return Encoding.UTF8.GetString(ms.ToArray()).Replace("\0", "");
        }


    }
}
