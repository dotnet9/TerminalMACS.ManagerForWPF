namespace PrototypeMode;

internal class ConcretePrototypel : Prototype
{
    public ConcretePrototypel(string id) : base(id)
    {
    }

    public override Prototype Clone()
    {
        // 创建当前对象的浅表副本。方法是创建一个新对象,然后将当前
        // 对象的非静态字段复制到该新对象。如果字段是值类型的,则对
        // 该字段执行逐位复制。如果字段是引用类型,则复制引用但不复
        // 制引用的对象;因此,原始对象及其副本引用同对象[MSDNJ
        return (Prototype)MemberwiseClone();
    }
}