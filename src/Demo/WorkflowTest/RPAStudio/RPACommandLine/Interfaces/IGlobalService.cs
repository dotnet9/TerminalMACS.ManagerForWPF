using RPACommandLine.Args;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RPACommandLine.Interfaces
{
    public interface IGlobalService
    {
        AutoResetEvent AutoResetEvent { get; set; }

        Options Options { get; set; }
    }
}
