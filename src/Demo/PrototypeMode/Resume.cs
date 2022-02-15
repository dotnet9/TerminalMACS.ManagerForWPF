using System;

namespace PrototypeMode;

//简历
internal class Resume : ICloneable
{
    private string age;
    private string name;
    private string sex;
    private readonly WorkExperience work;

    public Resume(string name)
    {
        this.name = name;
        work = new WorkExperience();
    }

    private Resume(WorkExperience work)
    {
        this.work = (WorkExperience)work.Clone();
    }

    public object Clone()
    {
        // 调用私有的构造方法,让“工作经历”克隆完成,然后再给这个“简历”
        // 对象的相关字段赋值,最终返回一个深复制的简历对象
        var obj = new Resume(work);
        obj.name = name;
        obj.sex = sex;
        obj.age = age;

        return obj;
    }

    //设置个人信息
    public void SetPersonalInfo(string sex, string age)
    {
        this.sex = sex;
        this.age = age;
    }

    //设置工作经历
    public void SetWorkExperience(string timeArea, string company)
    {
        work.WorkDate = timeArea;
        work.Company = company;
    }

    //显示
    public void Display()
    {
        Console.WriteLine($"{name} {sex} {age}");
        Console.WriteLine($"工作经历:{work.WorkDate} {work.Company}");
    }
}