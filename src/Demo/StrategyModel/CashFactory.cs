namespace StrategyModel;

/// <summary>
///     现金收费工厂类
/// </summary>
internal class CashFactory
{
    public static CashSuper CreateCashAccept(string type)
    {
        CashSuper cs = null;

        switch (type)
        {
            case "正常收费":
                cs = new CashNormal();
                break;
            case "满300返100":
                var cr1 = new CashReturn("300", "100");
                cs = cr1;
                break;
            case "打八折":
                var cr2 = new CashRebate("0.8");
                cs = cr2;
                break;
        }

        return cs;
    }
}