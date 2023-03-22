using Prism.Mvvm;
using System.ComponentModel;

namespace DataGridDemo.Models;

public class Student : BindableBase, IDataErrorInfo
{
    private string? _name;

    public string? Name
    {
        get { return _name; }
        set
        {
            if (_name == value)
            {
                return;
            }

            _name = value;
            SetProperty(ref _name, value);
        }
    }

    private int _age;

    public int Age
    {
        get { return _age; }
        set
        {
            if (_age == value)
            {
                return;
            }

            _age = value;
            SetProperty(ref _age, value);
        }
    }

    public Student(string name, int age)
    {
        Name = name;
        Age = age;
    }

    public string this[string columnName]
    {
        get
        {
            string? result = null;
            if (columnName != "Age")
            {
                return result;
            }

            if (Age is < 1 or > 100)
            {
                result = "年龄必须在1到100之间。";
            }

            return result;
        }
    }

    public string Error
    {
        get { return null; }
    }
}