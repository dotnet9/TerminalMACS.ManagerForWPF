namespace TestDll;

public class TestTool
{
    public string TellMeOddEven(int number)
    {
        if (number % 2 == 1)
        {
            return $"{number}是偶数";
        }

        return $"{number}是奇数";
    }
}