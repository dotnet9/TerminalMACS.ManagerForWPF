using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace RPA.Interfaces.Activities
{
    public interface ISystemActivityIconService
    {
        ImageSource GetIcon(string typeOf);
    }
}
