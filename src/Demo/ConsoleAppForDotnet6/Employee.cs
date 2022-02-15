using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace ConsoleAppForDotnet6;

public static class SerialHelper
{
    public static void WriteObject<T>(string fileName, T t)
    {
        Console.WriteLine(
            "Creating a Person object and serializing it.");
        var writer = new FileStream(fileName, FileMode.Create);
        var ser =
            new DataContractSerializer(typeof(T));
        ser.WriteObject(writer, t);
        writer.Close();
    }

    public static T ReadObject<T>(string fileName)
    {
        Console.WriteLine("Deserializing an instance of the object.");
        var fs = new FileStream(fileName,
            FileMode.Open);
        var reader =
            XmlDictionaryReader.CreateTextReader(fs, new XmlDictionaryReaderQuotas());
        var ser = new DataContractSerializer(typeof(T));

        // Deserialize the data and read it from the instance.
        var t =
            (T)ser.ReadObject(reader, true);
        reader.Close();
        fs.Close();
        return t;
    }
}

[DataContract(Name = "Employee", Namespace = "https://lqclass.com")]
internal class Employee : ICloneable
{
    [DataMember] public string IDCode { get; set; }

    [DataMember] public int Age { get; set; }

    [DataMember] public Department Department { get; set; }

    #region ICloneable 成员

    public object Clone()
    {
        return MemberwiseClone();
    }

    #endregion


    public Employee DeepClone()
    {
        var fileName = "tmp";
        SerialHelper.WriteObject(fileName, this);
        var newOjb = SerialHelper.ReadObject<Employee>(fileName);
        File.Delete(fileName);
        return newOjb;
    }

    public Employee ShallowClone()
    {
        return Clone() as Employee;
    }
}

[DataContract(Name = "Department", Namespace = "https://lqclass.com")]
internal class Department
{
    [DataMember] public string Name { get; set; }

    public override string ToString()
    {
        return Name;
    }
}