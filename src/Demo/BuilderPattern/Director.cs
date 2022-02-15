namespace BuilderPattern;

internal class Director
{
    // 用来指挥建造过程
    public void Construct(Builder builder)
    {
        builder.BuildPartA();
        builder.BuildPartB();
    }
}