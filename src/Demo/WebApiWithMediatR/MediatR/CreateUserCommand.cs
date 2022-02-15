using MediatR;

namespace WebApiWithMediatR.MediatR;

public class CreateUserCommand : IRequest<string>
{
    public string Name { get; set; }
}