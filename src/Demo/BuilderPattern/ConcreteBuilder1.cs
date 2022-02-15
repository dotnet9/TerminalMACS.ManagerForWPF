namespace BuilderPattern;

internal class ConcreteBuilder1 : Builder
{
    private readonly Product product = new();

    // 建造具体的两个部件是部件A和部件B
    public override void BuildPartA()
    {
        product.Add("部件A");
    }

    public override void BuildPartB()
    {
        product.Add("部件B");
    }

    public override Product GetResult()
    {
        return product;
    }
}