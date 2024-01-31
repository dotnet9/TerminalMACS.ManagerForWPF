using System.Reflection;

// 加载第三方库  
var assembly = Assembly.LoadFrom("ThirdLibrary.dll");

// 创建TestClass的实例  
var testClassType = assembly.GetType("ThirdLibrary.TestClass");
var testClassInstance = Activator.CreateInstance(testClassType);
var fields = testClassType.GetFields();

// 获取NoParamEvent委托  
var noParamEventField = fields.First(field => "NoParamEvent" == field.Name);
var noParamEventActionType = typeof(Action);
var noParamEventHandler = new Action(EventHandlerMethod);
noParamEventField.SetValue(testClassInstance, noParamEventHandler);

// 获取OneParamEvent委托，并设置事件参数处理程序  
var oneParamEventField = fields.First(field => "OneParamEvent" == field.Name);
var oneParamEventActionType = typeof(Action<>).MakeGenericType(typeof(string));
var oneParamEventHandler = new Action<string>(OneParamEventHandler);
oneParamEventField.SetValue(testClassInstance, oneParamEventHandler);

// 获取TwoParamEvent委托，并设置事件参数处理程序  
var twoParamEventField = fields.First(field => "TwoParamEvent" == field.Name);
var twoParamEventHandler = new Action<string, dynamic>(TwoParamEventHandler); // 使用object作为第二个参数的类型  
twoParamEventField.SetValue(testClassInstance, twoParamEventHandler); // 设置委托的第二个参数为null（因为没有实际参数）  

var callEventMethod = testClassType.GetMethods().First(method => "CallTwoParamEvent" == method.Name);
callEventMethod.Invoke(testClassInstance, null);

// NoParamEvent事件处理程序方法  
void EventHandlerMethod()
{
    Console.WriteLine("NoParamEvent event raised.");
}

// OneParamEvent事件处理程序方法，需要一个字符串参数  
void OneParamEventHandler(string param)
{
    Console.WriteLine($"OneParamEvent event raised with parameter: {param}");
}

// TwoParamEvent事件处理程序方法，需要两个参数：string和EventParam类型（通过反射传递）  
void TwoParamEventHandler(string param1, dynamic param2) // 使用object作为第二个参数的类型，并通过反射传递实际参数值  
{
    // 获取实际的EventParam类型并创建实例（假设其构造函数接受一个字符串参数）  
    var eventParamType = param2.GetType(); // 使用反射获取EventParam类型（假设它是委托的第二个参数的实际类型）  
    var eventParamConstructor =
        eventParamType.GetConstructor(new Type[] { typeof(string) }); // 获取构造函数并创建一个新的实例（假设有一个接受字符串的构造函数）  
    var eventParamInstance = eventParamConstructor.Invoke(new object[] { param1 }); // 创建新的EventParam实例（假设它接受一个字符串参数）  
    // 使用反射调用委托的第二个参数的方法或属性（假设它有一个名为"DoSomething"的方法）  
    var doSomethingMethod = eventParamInstance.GetType().GetMethod("DoSomething"); // 获取DoSomething方法并调用它（假设它存在）  
    doSomethingMethod.Invoke(eventParamInstance, null); // 调用DoSomething方法（假设它没有参数）  
}