using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPA.Interfaces.Activities
{
    public class ActivityLeafItem : ActivityGroupOrLeafItem
    {
        public string Icon { get; set; }
        public string TypeOf { get; set; }
        public string ToolTip { get; set; }
    }
}
