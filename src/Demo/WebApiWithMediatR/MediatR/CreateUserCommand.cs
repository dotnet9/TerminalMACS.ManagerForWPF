using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiWithMediatR.MediatR
{
  public class CreateUserCommand : IRequest<string>
  {
    public string Name { get; set; }
  }
}
