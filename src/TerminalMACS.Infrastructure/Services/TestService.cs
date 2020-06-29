using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TerminalMACS.Infrastructure.Services
{
    public class TestService : ITestService
    {
        public string GetCurrentTime()
        {
            return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff");
        }
    }
}
