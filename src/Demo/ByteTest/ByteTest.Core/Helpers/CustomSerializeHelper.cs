using ByteTest.Core.Models;

namespace ByteTest.Core.Helpers;

public class CustomSerializeHelper : ISerializeHelper
{
    public string Name()
    {
        return "Custom";
    }

    public byte[] Serialize(Organization data)
    {
        // 1、计算Id
        var idBuffer = BitConverter.GetBytes(data.Id);

        // 2、计算Tag数组
        var tagBuffer = GetBytes(data.Tags);

        // 3、计算Members
        var membersBuffer = GetBytes(data.Members);

        return GetBytes(new[] { idBuffer, tagBuffer, membersBuffer });
    }

    public Organization? Deserialize(byte[] buffer)
    {
        var data = new Organization();
        var index = 0;

        data.Id = BitConverter.ToInt32(buffer, index);
        index += sizeof(int);

        data.Tags = GetTags(buffer, ref index);
        data.Members = GetMembers(buffer, ref index);
        return data;
    }

    /// <summary>
    /// 获取字符串列表byte[]
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    private byte[] GetBytes(string[]? data)
    {
        var dataCount = data?.Length ?? 0;
        var dataCountBuffer = BitConverter.GetBytes(dataCount);

        if (dataCount <= 0)
        {
            return dataCountBuffer;
        }

        var dataValueBuffers = data!.Select(item => ByteHelper.GetBytes(item)!).ToArray();
        var dataValueBuffer = GetBytes(dataValueBuffers);
        return GetBytes(new[] { dataCountBuffer, dataValueBuffer });
    }

    /// <summary>
    /// 获取成功列表byte[]
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    private byte[] GetBytes(List<Member>? data)
    {
        var dataCount = data?.Count ?? 0;
        var dataCountBuffer = BitConverter.GetBytes(dataCount);

        if (dataCount <= 0)
        {
            return dataCountBuffer;
        }

        var dataValueBuffers = data!.Select(item =>
        {
            var idBuffer = BitConverter.GetBytes(item.Id);
            var nameBuffer = ByteHelper.GetBytes(item.Name);
            var descriptionBuffer = ByteHelper.GetBytes(item.Description);
            var addressBuffer = ByteHelper.GetBytes(item.Address);
            var valueBuffer = BitConverter.GetBytes(item.Value);
            var updateTimeBuffer = BitConverter.GetBytes(item.UpdateTime);

            var buffer = GetBytes(new byte[][]
                { idBuffer, nameBuffer, descriptionBuffer, addressBuffer, valueBuffer, updateTimeBuffer });
            return buffer;
        }).ToArray();
        var dataValueBuffer = GetBytes(dataValueBuffers);
        return GetBytes(new[] { dataCountBuffer, dataValueBuffer });
    }

    private byte[] GetBytes(byte[][] data)
    {
        var dataBufferLen = data.Sum(itemBuffer => itemBuffer.Length);
        var dataBuffer = new byte[dataBufferLen];
        var dataIndex = 0;
        for (var i = 0; i < data.Length; i++)
        {
            var itemBuffer = data[i];
            Array.Copy(itemBuffer, 0, dataBuffer, dataIndex, itemBuffer.Length);

            dataIndex += itemBuffer.Length;
        }

        return dataBuffer;
    }

    private string[]? GetTags(byte[] buffer, ref int index)
    {
        var count = BitConverter.ToInt32(buffer, index);
        index += sizeof(int);

        if (count <= 0)
        {
            return default;
        }

        var data = new string[count];
        for (var i = 0; i < count; i++)
        {
            data[i] = GetString(buffer, ref index);
        }

        return data;
    }

    private List<Member>? GetMembers(byte[] buffer, ref int index)
    {
        var count = BitConverter.ToInt32(buffer, index);
        index += sizeof(int);

        if (count <= 0)
        {
            return default;
        }

        var data = new List<Member>();
        for (var i = 0; i < count; i++)
        {
            var people = new Member();

            people.Id = BitConverter.ToInt32(buffer, index);
            index += sizeof(int);

            people.Name = GetString(buffer, ref index);

            people.Description = GetString(buffer, ref index);

            people.Address = GetString(buffer, ref index);

            people.Value = BitConverter.ToDouble(buffer, index);
            index += sizeof(double);

            people.UpdateTime = BitConverter.ToInt64(buffer, index);
            index += sizeof(long);

            data.Add(people);
        }

        return data;
    }

    private string GetString(byte[] buffer, ref int index)
    {
        var count = BitConverter.ToInt32(buffer, index);
        index += sizeof(int);

        if (count <= 0)
        {
            return string.Empty;
        }

        var data = ByteHelper.DefaultEncoding.GetString(buffer, index, count);
        index += count;
        return data;
    }
}