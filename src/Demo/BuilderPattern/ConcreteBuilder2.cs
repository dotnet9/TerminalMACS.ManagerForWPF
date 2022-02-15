namespace BuilderPattern;

internal class ConcreteBuilder2 : Builder
{
    private readonly Product product = new();

    // 建造具体的两个部件是部件A和部件B
    public override void BuildPartA()
    {
        product.Add("部件X");
    }

    public override void BuildPartB()
    {
        product.Add("部件Y");
    }

    public override Product GetResult()
    {
        return product;
    }
}