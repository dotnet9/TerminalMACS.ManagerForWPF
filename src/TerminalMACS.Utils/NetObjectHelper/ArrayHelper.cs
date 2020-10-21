using System;
using System.Text;

namespace TerminalMACS.Utils.NetObjectHelper
{
    /// <summary>
    /// 二进制数组帮助类
    /// </summary>
    public class ArrayHelper
    {
        #region 公开属性

        /// <summary>
        /// 获取或者设置二进制数组
        /// </summary>
        public byte[] Arrays { get; set; }
        /// <summary>
        /// 获取或者设置读取或者写入索引
        /// </summary>
        public int Position { get; set; }
        /// <summary>
        /// 获取或者设置使用的字符编码
        /// </summary>
        public Encoding UserEncoding { get; set; }

        #endregion

        #region 公开方法

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="encod">字符串使用的字符集</param>
        /// <param name="arrays">操作byte数组</param>
        /// <param name="position">数组操作索引</param>
        public ArrayHelper(Encoding encod, byte[] arrays = null, int position = 0)
        {
            this.Arrays = arrays;
            this.Position = position;
            if (encod == null)
                this.UserEncoding = Encoding.UTF8;
            else
                this.UserEncoding = encod;
        }

        /// <summary>
        /// 获取整型
        /// </summary>
        /// <returns>整型值</returns>
        public UInt16 DequeueUInt16()
        {
            ushort uint16Value = default(ushort);
            int uint16Len = 2;     //int长度
            if (Arrays == null)
                throw new ArgumentNullException("Arrays");

            //数据包长度不足以装下长度
            if (Arrays.Length < (Position + uint16Len))
                throw new ArgumentOutOfRangeException(string.Format("Parameter: Length, TotalLen: {0}, UInt16 Range({1},{2})", Arrays.Length, Position, Position + uint16Len));

            uint16Value = BitConverter.ToUInt16(Arrays, Position);
            Position += uint16Len;

            return uint16Value;
        }

        /// <summary>
        /// 添加整型
        /// </summary>
        /// <param name="value">整型值</param>
        public void Enqueue(UInt16 value)
        {
            int uint16len = 2;
            int minLen = Position + uint16len;
            if (Arrays == null)
                Arrays = new byte[minLen];
            else if (Arrays.Length < minLen)
            {
                byte[] tmpBts = new byte[minLen];
                Array.Copy(Arrays, 0, tmpBts, 0, Arrays.Length);
                Arrays = tmpBts;
            }
            byte[] bts = BitConverter.GetBytes(value);
            Array.Copy(bts, 0, Arrays, Position, bts.Length);
            Position += uint16len;
        }

        /// <summary>
        /// 获取整型
        /// </summary>
        /// <returns>整型值</returns>
        public Int16 DequeueInt16()
        {
            short int16Value = default(short);
            int uint16Len = 2;     //int长度
            if (Arrays == null)
                throw new ArgumentNullException("Arrays");

            //数据包长度不足以装下长度
            if (Arrays.Length < (Position + uint16Len))
                throw new ArgumentOutOfRangeException(string.Format("Parameter: Length,TotalLen: {0}, Int16 Range({1},{2})", Arrays.Length, Position, Position + uint16Len));

            int16Value = BitConverter.ToInt16(Arrays, Position);
            Position += uint16Len;

            return int16Value;
        }

        /// <summary>
        /// 添加整型
        /// </summary>
        /// <param name="value">整型值</param>
        public void Enqueue(Int16 value)
        {
            int int16len = 2;
            int minLen = Position + int16len;
            if (Arrays == null)
                Arrays = new byte[minLen];
            else if (Arrays.Length < minLen)
            {
                byte[] tmpBts = new byte[minLen];
                Array.Copy(Arrays, 0, tmpBts, 0, Arrays.Length);
                Arrays = tmpBts;
            }
            byte[] bts = BitConverter.GetBytes(value);
            Array.Copy(bts, 0, Arrays, Position, bts.Length);
            Position += int16len;
        }

        /// <summary>
        /// 获取整型
        /// </summary>
        /// <returns>整型值</returns>
        public UInt32 DequeueUInt32()
        {
            UInt32 int32Value = default(UInt32);
            int uint32Len = 4;     //int长度
            if (Arrays == null)
                throw new ArgumentNullException("Arrays");

            //数据包长度不足以装下长度
            if (Arrays.Length < (Position + uint32Len))
                throw new ArgumentOutOfRangeException(string.Format("Parameter: Length, TotalLen: {0}, UInt32 Range({1},{2})", Arrays.Length, Position, Position + uint32Len));

            int32Value = BitConverter.ToUInt32(Arrays, Position);
            Position += uint32Len;

            return int32Value;
        }

        /// <summary>
        /// 添加整型
        /// </summary>
        /// <param name="value">整型值</param>
        public void Enqueue(UInt32 value)
        {
            int int32Len = 4;
            int minLen = Position + int32Len;
            if (Arrays == null)
                Arrays = new byte[minLen];
            else if (Arrays.Length < minLen)
            {
                byte[] tmpBts = new byte[minLen];
                Array.Copy(Arrays, 0, tmpBts, 0, Arrays.Length);
                Arrays = tmpBts;
            }
            byte[] bts = BitConverter.GetBytes(value);
            Array.Copy(bts, 0, Arrays, Position, bts.Length);
            Position += int32Len;
        }

        /// <summary>
        /// 获取整型
        /// </summary>
        /// <returns>整型值</returns>
        public Int32 DequeueInt32()
        {
            Int32 int32Value = default(Int32);
            int int32Len = 4;     //int长度
            if (Arrays == null)
                throw new ArgumentNullException("Arrays");

            //数据包长度不足以装下长度
            if (Arrays.Length < (Position + int32Len))
                throw new ArgumentOutOfRangeException(string.Format("Parameter: Length, TotalLen: {0}, Int32 Range({1},{2})", Arrays.Length, Position, Position + int32Len));

            int32Value = BitConverter.ToInt32(Arrays, Position);
            Position += int32Len;

            return int32Value;
        }

        /// <summary>
        /// 添加整型
        /// </summary>
        /// <param name="value">整型值</param>
        public void Enqueue(Int32 value)
        {
            int int32Len = 4;
            int minLen = Position + int32Len;
            if (Arrays == null)
                Arrays = new byte[minLen];
            else if (Arrays.Length < minLen)
            {
                byte[] tmpBts = new byte[minLen];
                Array.Copy(Arrays, 0, tmpBts, 0, Arrays.Length);
                Arrays = tmpBts;
            }
            byte[] bts = BitConverter.GetBytes(value);
            Array.Copy(bts, 0, Arrays, Position, bts.Length);
            Position += int32Len;
        }

        /// <summary>
        /// 获取长整型
        /// </summary>
        /// <returns>长整型值</returns>
        public long DequeueLong()
        {
            long intValue = default(long);
            int longLen = 8;     //int长度

            if (Arrays == null)
                throw new ArgumentNullException("Arrays");

            //数据包长度不足以装下长度
            if (Arrays.Length < (Position + longLen))
                throw new ArgumentOutOfRangeException(string.Format("Parameter: Length, TotalLen: {0}, Long Range({1},{2})", Arrays.Length, Position, Position + longLen));

            intValue = BitConverter.ToInt64(Arrays, Position);
            Position += longLen;

            return intValue;
        }

        /// <summary>
        /// 添加整型
        /// </summary>
        /// <param name="value">整型值</param>
        public void Enqueue(long value)
        {
            int len = 8;
            int minLen = Position + len;
            if (Arrays == null)
                Arrays = new byte[minLen];
            else if (Arrays.Length < minLen)
            {
                byte[] tmpBts = new byte[minLen];
                Array.Copy(Arrays, 0, tmpBts, 0, Arrays.Length);
                Arrays = tmpBts;
            }
            byte[] bts = BitConverter.GetBytes(value);
            Array.Copy(bts, 0, Arrays, Position, bts.Length);
            Position += len;
        }

        /// <summary>
        /// 获取长整型
        /// </summary>
        /// <returns>长整型值</returns>
        public ulong DequeueULong()
        {
            ulong intValue = default(ulong);
            int ulongLen = 8;     //int长度

            if (Arrays == null)
                throw new ArgumentNullException("Arrays");

            //数据包长度不足以装下长度
            if (Arrays.Length < (Position + ulongLen))
                throw new ArgumentOutOfRangeException(string.Format("Parameter: Length, TotalLen: {0}, ULong Range({1},{2})", Arrays.Length, Position, Position + ulongLen));

            intValue = BitConverter.ToUInt64(Arrays, Position);
            Position += ulongLen;

            return intValue;
        }

        /// <summary>
        /// 添加整型
        /// </summary>
        /// <param name="value">整型值</param>
        public void Enqueue(ulong value)
        {
            int len = 8;
            int minLen = Position + len;
            if (Arrays == null)
                Arrays = new byte[minLen];
            else if (Arrays.Length < minLen)
            {
                byte[] tmpBts = new byte[minLen];
                Array.Copy(Arrays, 0, tmpBts, 0, Arrays.Length);
                Arrays = tmpBts;
            }
            byte[] bts = BitConverter.GetBytes(value);
            Array.Copy(bts, 0, Arrays, Position, bts.Length);
            Position += len;
        }

        /// <summary>
        /// 获取整型
        /// </summary>
        /// <returns>整型值</returns>
        public double DequeueDouble()
        {
            double intValue = default(double);
            int doubleLen = 8;     //int长度

            if (Arrays == null)
                throw new ArgumentNullException("Arrays");

            //数据包长度不足以装下长度
            if (Arrays.Length < (Position + doubleLen))
                throw new ArgumentOutOfRangeException(string.Format("Parameter: Length, TotalLen: {0}, Double Range({1},{2})", Arrays.Length, Position, Position + doubleLen));

            intValue = BitConverter.ToDouble(Arrays, Position);
            Position += doubleLen;

            return intValue;
        }

        /// <summary>
        /// 添加双精度
        /// </summary>
        /// <param name="value">双精度</param>
        public void Enqueue(double value)
        {
            int len = 8;
            int minLen = Position + len;
            if (Arrays == null)
                Arrays = new byte[minLen];
            else if (Arrays.Length < minLen)
            {
                byte[] tmpBts = new byte[minLen];
                Array.Copy(Arrays, 0, tmpBts, 0, Arrays.Length);
                Arrays = tmpBts;
            }
            byte[] bts = BitConverter.GetBytes(value);
            Array.Copy(bts, 0, Arrays, Position, bts.Length);
            Position += len;
        }

        /// <summary>
        /// 获取数据包中的下一个bool值
        /// </summary>
        /// <returns>字节数组</returns>
        public bool DequeueBoolean()
        {
            Boolean value = default(Boolean);
            int boolLen = 2;     //int长度

            if (Arrays == null)
                throw new ArgumentNullException("Arrays");

            //数据包长度不足以装下长度
            if (Arrays.Length < (Position + boolLen))
                throw new ArgumentOutOfRangeException(string.Format("Parameter: Length, TotalLen: {0}, Boolean Range({1},{2})", Arrays.Length, Position, Position + boolLen));

            value = BitConverter.ToBoolean(Arrays, Position);
            Position += boolLen;

            return value;
        }

        /// <summary>
        /// 添加双精度
        /// </summary>
        /// <param name="value">双精度</param>
        public void Enqueue(bool value)
        {
            int len = 2;
            int minLen = Position + len;
            if (Arrays == null)
                Arrays = new byte[minLen];
            else if (Arrays.Length < minLen)
            {
                byte[] tmpBts = new byte[minLen];
                Array.Copy(Arrays, 0, tmpBts, 0, Arrays.Length);
                Arrays = tmpBts;
            }
            byte[] bts = BitConverter.GetBytes(value);
            Array.Copy(bts, 0, Arrays, Position, bts.Length);
            Position += len;
        }

        /// <summary>
        /// 获取数据包中的下一个bool值
        /// </summary>
        /// <returns>字节数组</returns>
        public byte DequeueByte()
        {
            byte value = default(byte);
            int byteLen = 1;     //int长度

            if (Arrays == null)
                throw new ArgumentNullException("Arrays");

            //数据包长度不足以装下长度
            if (Arrays.Length < (Position + byteLen))
                throw new ArgumentOutOfRangeException(string.Format("Parameter: Length, TotalLen: {0}, Byte Range({1},{2})", Arrays.Length, Position, Position + byteLen));

            value = Arrays[Position];
            Position += byteLen;

            return value;
        }

        /// <summary>
        /// 添加双精度
        /// </summary>
        /// <param name="value">双精度</param>
        public void Enqueue(byte value)
        {
            int len = 1;
            int minLen = Position + len;
            if (Arrays == null)
                Arrays = new byte[minLen];
            else if (Arrays.Length < minLen)
            {
                byte[] tmpBts = new byte[minLen];
                Array.Copy(Arrays, 0, tmpBts, 0, Arrays.Length);
                Arrays = tmpBts;
            }
            Arrays[Position] = value;
            Position += len;
        }

        /// <summary>
        /// 获取数据包中的下一个bool值
        /// </summary>
        /// <returns>字节数组</returns>
        public char DequeueChar()
        {
            char value = default(char);
            int charLen = 1;     //int长度

            if (Arrays == null)
                throw new ArgumentNullException("Arrays");

            //数据包长度不足以装下长度
            if (Arrays.Length < (Position + charLen))
                throw new ArgumentOutOfRangeException(string.Format("Parameter: Length, TotalLen: {0}, Char Range({1},{2})", Arrays.Length, Position, Position + charLen));

            value = UserEncoding.GetChars(Arrays, Position, 1)[0];
            Position += charLen;

            return value;
        }

        /// <summary>
        /// 添加双精度
        /// </summary>
        /// <param name="value">双精度</param>
        public void Enqueue(char value)
        {
            int len = 1;
            int minLen = Position + len;
            if (Arrays == null)
                Arrays = new byte[minLen];
            else if (Arrays.Length < minLen)
            {
                byte[] tmpBts = new byte[minLen];
                Array.Copy(Arrays, 0, tmpBts, 0, Arrays.Length);
                Arrays = tmpBts;
            }
            byte[] bts = UserEncoding.GetBytes(new char[] { value });
            Array.Copy(bts, 0, Arrays, Position, 1);
            Position += len;
        }

        /// <summary>
        /// 获取数据包中的下一个字符串数据
        /// </summary>
        /// <returns>字符串值</returns>
        public string DequeueStringWithoutEndChar(Encoding encoding)
        {
            string str = string.Empty;
            UInt16 strLength = default(UInt16);

            if (Arrays == null)
                throw new ArgumentNullException("Arrays");

            strLength = DequeueUInt16();

            //数据包长度不足以装下字符串长度数据
            if (Arrays.Length < (Position + strLength))
                throw new ArgumentOutOfRangeException(string.Format("Parameter: Length, TotalLen: {0}, String Range({1},{2})", Arrays.Length, Position, Position + strLength));

            if (strLength > 0)
                str = encoding.GetString(Arrays, Position, strLength);

            Position += strLength;

            return str;
        }

        /// <summary>
        /// 添加字符串
        /// </summary>
        /// <param name="value">字符串</param>
        public void EnqueueWithoutEndChar(string value, Encoding encoding)
        {
            int minLen = default(int);
            byte[] strBytes = null;

            strBytes = GetStringBytesWithoutEndChar(value, encoding);

            minLen = Position + strBytes.Length;

            if (Arrays == null)
                Arrays = new byte[minLen];
            else if (Arrays.Length < minLen)
            {
                byte[] tmpBts = new byte[minLen];
                Array.Copy(Arrays, tmpBts, Arrays.Length);
                Arrays = tmpBts;
            }
            Array.Copy(strBytes, 0, Arrays, Position, strBytes.Length);
            Position += strBytes.Length;
        }

        /// <summary>
        /// 获取数据包中的下一个字节数组
        /// </summary>
        /// <returns>字节数组</returns>
        public byte[] DequeueBytes()
        {
            byte[] bts = null;
            int btLength = default(int);

            btLength = DequeueInt32();

            //数据包长度不足以装下字符串长度数据
            if (Arrays.Length < (Position + btLength))
                throw new ArgumentOutOfRangeException(string.Format("Parameter: Length, TotalLen: {0}, Byte[] Range({1},{2})", Arrays.Length, Position, Position + btLength));

            if (btLength > 1)
            {
                bts = new byte[btLength];
                Array.Copy(this.Arrays, Position, bts, 0, btLength);
            }

            Position += btLength;

            return bts;
        }

        /// <summary>
        /// 添加字节数组
        /// </summary>
        /// <param name="value">字节数组</param>
        public void Enqueue(byte[] value)
        {
            int btLen = default(int);
            int minLen = default(int);

            if (value == null)
                btLen = 0;
            else
                btLen = value.Length;
            Enqueue(btLen);

            minLen = Position + btLen;

            if (Arrays == null)
                Arrays = new byte[minLen];
            else if (Arrays.Length < minLen)
            {
                byte[] tmpBts = new byte[minLen];
                Array.Copy(Arrays, tmpBts, Arrays.Length);
                Arrays = tmpBts;
            }
            if (value != null)
                Array.Copy(value, 0, Arrays, Position, btLen);
            Position += btLen;
        }

        /// <summary>
        /// 获取字符串字节数组(数据包括字符串字节长度及实际内容(加上\0结束符,c++需要识别))
        /// </summary>
        /// <param name="str">需要转换的字符串</param>
        /// <returns>字符串包字节数组</returns>
        private byte[] GetStringBytesWithoutEndChar(string str, Encoding encoding)
        {
            byte[] totalDatas = null;
            int uint16Len = 2;

            byte[] strData = encoding.GetBytes(str);                                    //字符串实际byte[]
            byte[] strDataLenData = BitConverter.GetBytes((UInt16)strData.Length);      //字符串长度byte[]
            totalDatas = new byte[strData.Length + uint16Len];                          //组合包大小=字符串实际byte[]长度+2
            Array.Copy(strDataLenData, totalDatas, strDataLenData.Length);              //复制字符串长度byte[]到组合包开始位置
            Array.Copy(strData, 0, totalDatas, uint16Len, strData.Length);              //复制字符串byte[]到组合包索引位置4处

            return totalDatas;
        }

        #endregion

        #region 静态方法

        /// <summary>
        /// 字符串编码转换
        /// </summary>
        /// <param name="srcEncoding">源编码</param>
        /// <param name="dstEncoding">目标编码</param>
        /// <param name="srcStr">源字符串</param>
        /// <returns>目标字符串</returns>
        public static string TransferEncoding(Encoding srcEncoding, Encoding dstEncoding, string srcStr)
        {
            byte[] srcBytes = srcEncoding.GetBytes(srcStr);
            byte[] dstBytes = TransferEncoding2(srcEncoding, dstEncoding, srcStr);

            StringBuilder sb = new StringBuilder();
            foreach (var b in dstBytes)
            {
                sb.Append((char)b);
            }

            return sb.ToString();

            //var dstStr = dstEncoding.GetString(dstBytes);

            //return dstStr;
        }

        /// <summary>
        /// 字符串编码转换
        /// </summary>
        /// <param name="srcEncoding">源编码</param>
        /// <param name="dstEncoding">目标编码</param>
        /// <param name="srcStr">源字符串</param>
        /// <returns>目标字符串</returns>
        public static byte[] TransferEncoding2(Encoding srcEncoding, Encoding dstEncoding, string srcStr)
        {
            byte[] srcBytes = srcEncoding.GetBytes(srcStr);
            byte[] dstBytes = Encoding.Convert(srcEncoding, dstEncoding, srcBytes);

            return dstBytes;
        }

        /// <summary>
        /// 添加结束符
        /// </summary>
        /// <param name="datas"></param>
        /// <returns></returns>
        public static byte[] AddEndChar(byte[] datas)
        {
            byte[] newDatas = new byte[datas.Length + 1];
            Array.Copy(datas, 0, newDatas, 0, datas.Length);
            newDatas[newDatas.Length - 1] = (byte)'\0';

            return newDatas;
        }

        /// <summary>
        /// 获取字符串编码
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static Encoding GetBytesEncoding(byte[] buffer)
        {
            Encoding encoding = Encoding.Default;
            if (buffer[0] >= 0xEF)
            {
                if (buffer[0] == 0xEF && buffer[1] == 0xBB)
                    encoding = Encoding.UTF8;
                else if (buffer[0] == 0xFE && buffer[1] == 0xFF)
                    encoding = Encoding.BigEndianUnicode;
                else if (buffer[0] == 0xFF && buffer[1] == 0xFE)
                    encoding = Encoding.Unicode;
                else
                    encoding = Encoding.Default;
            }
            else
                encoding = Encoding.Default;

            return encoding;
        }

        /// <summary>
        /// 获取字符串的编码
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static Encoding GetStringEncoding(string str)
        {
            var newStr = Encoding.Default.GetString(Encoding.Default.GetBytes(str));

            if (!newStr.Equals(str))
                return Encoding.Unicode;
            else
                return Encoding.Default;
        }

        /// <summary>
        /// 获取上传文件时，本地文件路径与客户端文件路径字节数组
        /// </summary>
        public static byte[] ConvertStringToUtf8(string strFilePath, string spliChar = "/", bool isAddEndChar = true)
        {
            byte[] arrs = null;

            var dir = GetParentDirectoryName(strFilePath, spliChar);
            var fileName = GetFileOrDirectoryName(strFilePath, spliChar);

            var dirEncode = GetStringEncoding(dir);
            var fileEncode = GetStringEncoding(fileName);

            var utf8DirBytes = TransferEncoding2(dirEncode, System.Text.Encoding.UTF8, string.Format("{0}{1}", dir, spliChar));
            var utf8NameBytes = TransferEncoding2(fileEncode, System.Text.Encoding.UTF8, fileName);

            var newFilePathBytes = new byte[utf8DirBytes.Length + utf8NameBytes.Length];
            Array.Copy(utf8DirBytes, 0, newFilePathBytes, 0, utf8DirBytes.Length);
            Array.Copy(utf8NameBytes, 0, newFilePathBytes, utf8DirBytes.Length, utf8NameBytes.Length);

            if (isAddEndChar)
            {
                arrs = AddEndChar(newFilePathBytes);
            }
            else
            {
                arrs = newFilePathBytes;
            }

            return arrs;
        }


        /// <summary>
        /// 获取上传文件时，本地文件路径与客户端文件路径字节数组
        /// </summary>
        public static string ConvertStringToLocal(string strFilePath, bool isAddEndChar = true)
        {
            string returnFilePath = "";
            byte[] arrs = null;
            string spliChar = "\\";
            var dir = GetParentDirectoryName(strFilePath, spliChar);
            var fileName = GetFileOrDirectoryName(strFilePath, spliChar);

            var fileNameEncode = GetStringEncoding(strFilePath);

            byte[] utf8DirBytes = TransferEncoding2(System.Text.Encoding.UTF8, fileNameEncode, string.Format("{0}{1}", dir, spliChar));
            byte[] utf8NameBytes = TransferEncoding2(System.Text.Encoding.UTF8, fileNameEncode, fileName);

            var newFilePathBytes = new byte[utf8DirBytes.Length + utf8NameBytes.Length];
            Array.Copy(utf8DirBytes, 0, newFilePathBytes, 0, utf8DirBytes.Length);
            Array.Copy(utf8NameBytes, 0, newFilePathBytes, utf8DirBytes.Length, utf8NameBytes.Length);

            if (isAddEndChar)
            {
                arrs = AddEndChar(newFilePathBytes);
            }
            else
            {
                arrs = newFilePathBytes;
            }

            if (fileNameEncode == Encoding.Unicode)
            {
                returnFilePath = Encoding.Unicode.GetString(arrs);
            }
            else
            {
                returnFilePath = Encoding.Default.GetString(arrs);
            }

            return returnFilePath;
        }

        /// <summary>
        /// 读取文件名称，不用API的方式，System.IO.Path.GetFileNameWithOutName，
        /// 比如像Linus这种比较奇葩的路径命名方式，路径中带":"等的字符，在window平台下不能识别会出错
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetFileOrDirectoryName(string path, string spliChar = "/")
        {
            int index = path.LastIndexOf(spliChar);
            if (index >= 0)
                return path.Substring(index + 1, path.Length - 1 - index);
            else
                return path;
        }
        /// <summary>
        /// 读取父文件名称，不用API的方式，System.IO.Path.GetDirectoryName，
        /// 比如像Linus这种比较奇葩的路径命名方式，路径中带":"等的字符，在window平台下不能识别会出错
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetParentDirectoryName(string path, string spliChar = "/")
        {
            int index = path.LastIndexOf(spliChar);
            var msg = string.Empty;
            if (index >= 0)
                msg = path.Substring(0, index);
            else
                msg = string.Empty;
            if (string.IsNullOrWhiteSpace(msg))
                msg = "/";
            return msg;
        }

        #endregion
    }
}
