using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HZ.Framework.IO
{
    public class FileHelper
    {
        /// <summary>
        /// 删除文件(到回收站[可选])
        /// </summary>
        /// <param name="fileName">要删除的文件名</param>
        /// <param name="isSendToRecycleBin">是否删除到回收站</param>
        public static void Delete(string fileName, bool isSendToRecycleBin = false)
        {
            if (isSendToRecycleBin)
            {
                FileSystem.DeleteFile(fileName, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
            }
            else
            {
                File.Delete(fileName);
            }
        }


        /// <summary>
        /// 设置或取消文件的属性
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="attribute"></param>
        /// <param name="isSet">true设置 false取消</param>
        public static void SetAttribute(string fileName, FileAttributes attribute, bool isSet)
        {
            FileInfo file = new FileInfo(fileName);
            if (!file.Exists)
            {
                throw new FileNotFoundException("要设置属性的文件不存在", fileName);
            }
            if (isSet)
            {
                file.Attributes = file.Attributes | attribute;
            }
            else
            {
                file.Attributes = file.Attributes & ~attribute;
            }
        }

        /// <summary>
        /// 获取文件的版本号
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string GetVersion(string fileName)
        {
            if (File.Exists(fileName))
            {
                FileVersionInfo fileVersion = FileVersionInfo.GetVersionInfo(fileName);
                return fileVersion.ToString();
            }
            return null;
        }

        /// <summary>
        /// 获取文件的MD5值
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <returns>32为MD5</returns>
        public static string GetFileMD5(string fileName)
        {
            FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
            const int bufferSize = 1024 * 1024;
            byte[] buffer = new byte[bufferSize];

            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            md5.Initialize();

            long offset = 0;
            long fsLength=fs.Length;
            while (offset < fsLength)
            {
                long readSize = bufferSize;
                if (offset + readSize > fsLength)
                {
                    readSize = fsLength - offset;
                }
                fs.Read(buffer, 0, (int)readSize);
                if (offset + readSize < fsLength)
                {
                    md5.TransformBlock(buffer, 0, (int)bufferSize, buffer, 0);
                }
                else
                {
                    md5.TransformFinalBlock(buffer, 0, (int)bufferSize);
                }
                offset += bufferSize;
            }
            fs.Close();
            byte[] result = md5.Hash;
            md5.Clear();
            StringBuilder sb = new StringBuilder(32);
            foreach (byte b in result)
            {
                sb.Append(b.ToString("X2"));
            }
            return sb.ToString();
        }
    }
}
