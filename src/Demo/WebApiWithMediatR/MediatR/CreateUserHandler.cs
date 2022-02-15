using System.Threading;
using System.Threading.Tasks;
using MediatR;
using WebApiWithMediatR.Services;

namespace WebApiWithMediatR.MediatR;

public class CreateUserHandler : IRequestHandler<CreateUserCommand, string>
{
    public CreateUserHandler(ITestService testService)
    {
        testService.WriteLine();
    }

    public async Task<string> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        return await Task.FromResult($"New name is {request.Name}");
    }
}