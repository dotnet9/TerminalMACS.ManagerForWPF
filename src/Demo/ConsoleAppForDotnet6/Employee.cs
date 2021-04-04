using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ConsoleAppForDotnet6
{
	public static class SerialHelper
	{
		public static void WriteObject<T>(string fileName, T t)
		{
			Console.WriteLine(
				"Creating a Person object and serializing it.");
			FileStream writer = new FileStream(fileName, FileMode.Create);
			DataContractSerializer ser =
				new DataContractSerializer(typeof(T));
			ser.WriteObject(writer, t);
			writer.Close();
		}

		public static T ReadObject<T>(string fileName)
		{
			Console.WriteLine("Deserializing an instance of the object.");
			FileStream fs = new FileStream(fileName,
			FileMode.Open);
			XmlDictionaryReader reader =
				XmlDictionaryReader.CreateTextReader(fs, new XmlDictionaryReaderQuotas());
			DataContractSerializer ser = new DataContractSerializer(typeof(T));

			// Deserialize the data and read it from the instance.
			T t =
				(T)ser.ReadObject(reader, true);
			reader.Close();
			fs.Close();
			return t;
		}
	}

	[DataContract(Name = "Employee", Namespace = "https://lqclass.com")]
	class Employee : ICloneable
	{
		[DataMember()]
		public string IDCode { get; set; }
		[DataMember()]
		public int Age { get; set; }
		[DataMember()]
		public Department Department { get; set; }

		#region ICloneable 成员

		public object Clone()
		{
			return this.MemberwiseClone();
		}

		#endregion


		public Employee DeepClone()
		{
			string fileName = "tmp";
			SerialHelper.WriteObject<Employee>(fileName, this);
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
	class Department
	{
		[DataMember()]
		public string Name { get; set; }
		public override string ToString()
		{
			return this.Name;
		}
	}

}
