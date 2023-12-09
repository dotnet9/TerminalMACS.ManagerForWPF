using System;
using System.Text;

namespace ByteTestFX47.Helpers;

public static class ByteHelper
{
    public static Encoding DefaultEncoding = Encoding.UTF8;

    /// <summary>
    /// 获取字符串二进制数据：字符串二进制数据=4个字节int表示字符串实际值长度+n个字节表示字符串实际值
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static byte[] GetBytes(string? str)
    {
        if (string.IsNullOrEmpty(str))
        {
            return BitConverter.GetBytes(0);
        }

        var strValueBuffer = DefaultEncoding.GetBytes(str);
        var strValueLen = strValueBuffer.Length;
        var strValueLenBuffer = BitConverter.GetBytes(strValueLen);

        var strBufferLen = sizeof(int) + strValueLen;
        var strBuffer = new byte[strBufferLen];

        var index = 0;
        Array.Copy(strValueLenBuffer, 0, strBuffer, index, sizeof(int));
        index += sizeof(int);

        Array.Copy(strValueBuffer, 0, strBuffer, index, strValueLen);

        return strBuffer;
    }
}