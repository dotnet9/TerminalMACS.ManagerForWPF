using System.Reactive.Subjects;

var inputs = new Subject<string>();
inputs.Subscribe(Console.WriteLine);
for (var i = 0; i < 10; i++)
{
    inputs.OnNext(i.ToString());
}

Console.ReadLine();