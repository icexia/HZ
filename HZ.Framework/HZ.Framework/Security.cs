using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HZ.Framework
{
    public static class Security
    {
        public static string DESEncrypt(string pToEncrypt, string key)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();

            byte[] inputByteArray = Encoding.Default.GetBytes(pToEncrypt);
            des.Key = System.Text.Encoding.UTF8.GetBytes(key);
            des.IV = System.Text.Encoding.UTF8.GetBytes(key);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();

            return ToHexString(ms.ToArray());
        }

        ///解密方法  
        ///   <summary>  
        ///    
        ///   </summary>  
        ///   <param   name="pToDecrypt"></param>  
        ///   <returns></returns>  
        public static string DESDecrypt(string pToDecrypt, string key)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();

            byte[] inputByteArray = FromHexString(pToDecrypt);

            des.Key = System.Text.Encoding.UTF8.GetBytes(key);
            des.IV = System.Text.Encoding.UTF8.GetBytes(key);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);

            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            return Encoding.Default.GetString(ms.ToArray());
        }

        #region rsa

        /// <summary>
        /// rsa加密
        /// </summary>
        /// <param name="strSrc"></param>
        /// <returns></returns>
        public static string Encrypt(string strSrc, string EncryptString)
        {
            TripleDESCryptoServiceProvider DES = new TripleDESCryptoServiceProvider();
            MD5CryptoServiceProvider hashMD5 = new MD5CryptoServiceProvider();

            DES.Key = hashMD5.ComputeHash(System.Text.Encoding.Default.GetBytes(EncryptString));
            DES.Mode = CipherMode.ECB;

            ICryptoTransform DESEncrypt = DES.CreateEncryptor();

            byte[] Buffer = System.Text.Encoding.Default.GetBytes(strSrc);
            return ToHexString(DESEncrypt.TransformFinalBlock(Buffer, 0, Buffer.Length));
        }

        /// <summary>
        /// rsa解密
        /// </summary>
        /// <param name="strSrc"></param>
        /// <returns></returns>
        public static string Decrypt(string strSrc, string EncryptString)
        {
            TripleDESCryptoServiceProvider DES = new TripleDESCryptoServiceProvider();
            MD5CryptoServiceProvider hashMD5 = new MD5CryptoServiceProvider();

            DES.Key = hashMD5.ComputeHash(System.Text.Encoding.Default.GetBytes(EncryptString));
            DES.Mode = CipherMode.ECB;

            ICryptoTransform DESDecrypt = DES.CreateDecryptor();

            string result = "";
            try
            {
                byte[] Buffer = FromHexString(strSrc);
                result = System.Text.Encoding.Default.GetString(DESDecrypt.TransformFinalBlock(Buffer, 0, Buffer.Length));
            }
            catch (Exception e)
            {
                throw e;
            }

            return result;
        }

        #endregion

        #region md5

        /// <summary>
        /// md5加密
        /// </summary>
        /// <param name="strSrc"></param>
        /// <returns></returns>
        public static string MD5(string strSrc)
        {
            return Security.MD5(strSrc, System.Text.Encoding.UTF8);
        }

        public static string MD5(string strSrc, System.Text.Encoding encoding)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] result = md5.ComputeHash(encoding.GetBytes(strSrc));
            string str = null;
            for (int i = 0; i < result.Length; i++)
            {
                str += result[i].ToString("x2");
            }
            return str;
        }

        #endregion

        public static string ToHexString(byte[] val)
        {
            string newString = "";
            for (int i = 0; i < val.Length; i++)
            {
                byte b = val[i];
                string str = b.ToString("x2");
                if (str.Length > 2)
                {
                    str = str.Substring(0, str.Length - 2);
                }
                if (str.Length < 2)
                {
                    str = "0" + str;
                }
                newString += str;
            }
            return newString.ToUpper();
        }

        public static byte[] FromHexString(string val)
        {
            const int hexNum = 2;

            int count = val.Length / hexNum;
            int beginIndex = 0;
            int endIndex = hexNum;
            byte[] buf = new byte[count];
            for (int i = 0; i < count; i++)
            {
                beginIndex = i * hexNum;
                endIndex = (i + 1) * hexNum;

                string str = val.Substring(beginIndex, endIndex - beginIndex);
                buf[i] = (byte)Int32.Parse(str, System.Globalization.NumberStyles.HexNumber);
            }
            return buf;
        }

    }
}
