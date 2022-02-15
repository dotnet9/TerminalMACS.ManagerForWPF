using System;

namespace ConsoleAppForDotnet6;

public enum 王道
{
    明,
    昏
}

/// <summary>
///     先帝，陛下，文景，桓灵
/// </summary>
public class 君
{
    public 王道 为君;
    public string 名称;
    public bool 在;

    public 君()
    {
        在 = true;
        为君 = 王道.明;
    }

    public bool 创业(double percentage)
    {
        if (percentage < 0.5)
        {
            在 = false;
            Console.WriteLine($"{名称}创业未半而中道崩殂！");
            return false;
        }

        Console.WriteLine(@"{名称}兴复汉室，还于旧都！");
        return true;
    }

    public void 开张圣听()
    {
        Console.WriteLine("开张圣听，光先帝遗德！");
    }

    public void 恢弘志士之气()
    {
    }

    public void 宾自菲薄()
    {
    }

    public void 引喻失义()
    {
        Console.WriteLine("塞忠谏之路！");
    }

    public void 亲贤臣远小人()
    {
        为君 = 王道.明;
    }

    public void 亲小人远贤臣()
    {
        为君 = 王道.昏;
    }

    public void 治国()
    {
    }

    public void 偏私()
    {
        Console.WriteLine("内外异法！");
    }

    public bool 咨之(string 事)
    {
        if (王道.明 == 为君) return true;

        return false;
    }

    public bool 施行(string 事)
    {
        return true;
    }

    public void 曰(string 言)
    {
        Console.WriteLine(言);
    }

    public void 每与臣论此事()
    {
        Console.WriteLine("叹息痛恨于桓灵。");
    }

    ~君()
    {
    }
}

public enum 臣德
{
    贤,
    奸
}

public class 侍卫之臣
{
    public 臣德 为臣;
    private readonly 君 刘备 = new();
    private readonly 君 刘禅 = new();

    public string 名称;

    public void 不懈于内()
    {
        Console.WriteLine($"侍卫之臣({名称})不懈于内");
    }

    public bool 追先帝之殊遇()
    {
        if (刘备.为君 == 王道.明)
            return true;
        return false;
    }

    public bool 报之于陛下()
    {
        if (刘禅.为君 == 王道.明)
            return true;
        return false;
    }

    public bool 谋事(string 事)
    {
        if (为臣 == 臣德.贤) return true;

        return false;
    }
}

internal class 忠志之士
{
    public 臣德 为臣 = new();
    private readonly 君 刘备 = new();
    private readonly 君 刘禅 = new();

    public string 名称;

    public void 忘身于外()
    {
        Console.WriteLine($"忠志之士({名称})忘身于外!");
    }

    public bool 追先帝之殊遇()
    {
        if (刘备.为君 == 王道.明)
            return true;
        return false;
    }

    public bool 报之于陛下()
    {
        if (刘禅.为君 == 王道.明)
            return true;
        return false;
    }

    public bool 谋事(string 事)
    {
        if (为臣 == 臣德.贤) return true;

        return false;
    }
}

public enum 气候
{
    兴盛,
    疲弊,
    兴隆,
    倾颓
}

/// <summary>
///     曹魏,东吴,益州,先汉,后汉
/// </summary>
public class 国
{
    public 气候 国运;

    public 国()
    {
        国运 = 气候.兴盛;
    }

    public void 付诸有司论其刑赏(侍卫之臣 臣)
    {
        if (臣.为臣 == 臣德.贤)
            Console.WriteLine("赏！");
        else
            Console.WriteLine("刑！");
    }

    ~国()
    {
    }
}

/// <summary>
///     郭攸之，费祎
/// </summary>
public class 侍中 : 侍卫之臣
{
}

/// <summary>
///     董允
/// </summary>
public class 侍郎 : 侍卫之臣
{
}

/// <summary>
///     陈震
/// </summary>
public class 尚书 : 侍卫之臣
{
}

/// <summary>
///     张裔
/// </summary>
public class 长史 : 侍卫之臣
{
}

/// <summary>
///     蒋琬
/// </summary>
public class 参季 : 侍卫之臣
{
}

/// <summary>
///     向宠
/// </summary>
internal class 中都督 : 忠志之士
{
}

/// <summary>
///     诸葛亮
/// </summary>
internal class 丞相 : 侍卫之臣
{
    public void 回首往事()
    {
        Console.WriteLine(
            "臣本布衣，躬耕于南阳，苟全性命于乱世，不求闻达于诸候。先帝不以臣卑鄙，猥自枉屈，三顾臣于草庐之中，咨臣以当世之事，由是感激，遂许先帝以驱驰。后值巅覆，受任于败军之际，奉命于危难之间，尔来二十有一年矣。");
    }

    public void 表忠心()
    {
        Console.WriteLine("先帝知臣谨慎，故临崩寄臣以大事也。受命以来，夙夜忧叹，恐托付不效，以伤先帝之明。故五月渡泸，深入不毛。");
    }

    public void 请战()
    {
        Console.WriteLine("今南方已定，兵甲已足，当奖率三军，北定中原，庶竭驽钝，攘除奸凶，兴复汉室, 还于旧都。");
    }

    public void 道别()
    {
        Console.WriteLine("今当远离, 临表涕零, 不知所言。");
    }
}