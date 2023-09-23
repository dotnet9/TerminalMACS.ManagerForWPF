namespace TestDll;

public class TestTool
{
    /// <summary>
    /// 带数字的优美段落
    /// </summary>
    private readonly List<string> _sentences = new()
    {
        "一，是孤独的象征，寂寞的代言人， 它独自站在诗句的起点，引人遐想。",
        "二，是相对的存在，对立的伴侣， 它们如影随形，互相依存。",
        "三，是完美的数字，三角的稳定， 它给诗歌带来了和谐的节奏。",
        "四，是平衡的象征，四季的轮回， 它让诗歌的结构更加坚实。",
        "五，是生机勃勃的数字，五彩斑斓的花朵， 它们在诗歌中绽放出美丽的画面。 ",
        "六，是平凡的数字，六边形的形状， 它们给诗歌带来了一种稳定的感觉。",
        "七，是神秘的数字，七色的虹霓， 它们在诗歌中散发出神奇的光芒。",
        "八，是无限的数字，八方的宇宙， 它们让诗歌的想象力无限延伸。",
        "九，是完美的数字，九曲的江河， 它们给诗歌带来了一种流动的美感。 ",
        "十，是圆满的数字，十全十美的象征， 它们让诗歌的结尾更加完美。"
    };


    /// <summary>
    /// 取对应数字的段落
    /// </summary>
    /// <param name="number"></param>
    /// <returns></returns>
    public string GetNumberSentence2(int number)
    {
        var mo = number % _sentences.Count;

        // 个位为0，取最后一
        if (mo == 0)
        {
            mo = 10;
        }

        // 新增数字验证方法
        mo = new CalNumber().GetValidNumber(mo);

        var sentencesIndex = mo - 1;
        return _sentences[sentencesIndex];
    }

    /// <summary>
    /// 取对应数字的段落
    /// </summary>
    /// <param name="number"></param>
    /// <returns></returns>
    public string GetNumberSentence(int number)
    {
        var mo = number % _sentences.Count;

        // 个位为0，取最后一
        if (mo == 0)
        {
            mo = 10;
        }

        if (mo == 6)
        {
            mo = 1;
        }

        var sentencesIndex = mo - 1;
        return _sentences[sentencesIndex];
    }
}

internal class CalNumber
{
    internal int GetValidNumber(int number)
    {
        // 这里可以加一些复杂的算法代码
        if (number == 6)
        {
            number = 1;
        }

        return number;
    }
}