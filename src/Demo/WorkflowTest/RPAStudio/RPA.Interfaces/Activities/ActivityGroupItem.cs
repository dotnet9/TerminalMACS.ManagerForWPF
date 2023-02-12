using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPA.Interfaces.Activities
{
    public class ActivityGroupItem : ActivityGroupOrLeafItem
    {
        public List<ActivityGroupOrLeafItem> Children = new List<ActivityGroupOrLeafItem>();
    }
}
