using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickApp.Helpers
{
    public class FileHelper
    {
        /// <summary>
        /// 写入
        /// </summary>
        /// <param name="str"></param>
        /// <param name="file"></param>
        public static void WriteFile(string str, string file)
        {
            if (!Directory.Exists(Path.GetDirectoryName(file)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(file));
            }
            System.IO.StreamWriter fileWrite = new System.IO.StreamWriter(file, false);
            fileWrite.WriteLine(str);
            fileWrite.Flush();
            fileWrite.Close();
            fileWrite.Dispose();
        }
        /// <summary>
        /// 读取
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string ReadFile(string filePath)
        {
            if (!Directory.Exists(Path.GetDirectoryName(filePath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(filePath));
            }
            System.IO.StreamReader fileWrite = new System.IO.StreamReader(filePath, false);
            string str = fileWrite.ReadToEnd();
            fileWrite.Close();
            fileWrite.Dispose();
            return str;
        }
    }
}
