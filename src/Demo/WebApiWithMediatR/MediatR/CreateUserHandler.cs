using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace WebApiWithMediatR.MediatR
{
  public class CreateUserHandler : IRequestHandler<CreateUserCommand, string>
  {
    public async Task<string> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
      return await Task.FromResult($"New name is {request.Name}");
    }
  }
}
