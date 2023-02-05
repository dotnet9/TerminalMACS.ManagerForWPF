using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace WorkflowTest;

public class HelloWorkflow : IWorkflow
{
    public string Id => "HelloWorkflow";

    public int Version => 1;

    public void Build(IWorkflowBuilder<object> builder)
    {
        builder.StartWith(context =>
            {
                Console.WriteLine("Hello world");
                return ExecutionResult.Next();
            })
            .Then(context =>
            {
                Thread.Sleep(500);
                Console.WriteLine("sleeped");
                return ExecutionResult.Next();
            })
            .Then(context =>
            {
                Console.WriteLine("Goodbye world");
                return ExecutionResult.Next();
            });
    }
}

public class HelloWorkflow2 : IWorkflow
{
    public string Id => "HelloWorkflow";

    public int Version => 2;

    public void Build(IWorkflowBuilder<object> builder)
    {
        builder.StartWith<HelloWorld>()
            .Then<SleepStep>()
            .Then<GoodbyeWorld>();
    }
}