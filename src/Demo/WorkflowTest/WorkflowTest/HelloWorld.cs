using Microsoft.Extensions.Logging;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace WorkflowTest;
public class HelloWorld : StepBody
{
    private ILogger logger;

    public HelloWorld(ILoggerFactory loggerFactory)
    {
        logger = loggerFactory.CreateLogger<HelloWorld>();
    }

    public override ExecutionResult Run(IStepExecutionContext context)
    {
        Console.WriteLine("Hello world, workflow");
        logger.LogInformation("Helloworld workflow");

        return ExecutionResult.Next();
    }
}


public class GoodbyeWorld : StepBody
{
    private ILogger logger;

    public GoodbyeWorld(ILoggerFactory loggerFactory)
    {
        logger = loggerFactory.CreateLogger<GoodbyeWorld>();
    }

    public override ExecutionResult Run(IStepExecutionContext context)
    {
        Console.WriteLine("Workflow, Goodbye");
        logger.LogInformation("Goodbye workflow");

        return ExecutionResult.Next();
    }
}

public class SleepStep : StepBody
{
    private ILogger logger;

    public SleepStep(ILoggerFactory loggerFactory)
    {
        logger = loggerFactory.CreateLogger("SleepStep");
    }

    public override ExecutionResult Run(IStepExecutionContext context)
    {
        Thread.Sleep(1000);

        logger.LogInformation("Sleeped");

        return ExecutionResult.Next();
    }
}