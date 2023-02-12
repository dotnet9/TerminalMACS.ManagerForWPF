using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPA.Interfaces.Activities
{
    public interface IActivityFavoritesService
    {
        List<ActivityGroupOrLeafItem> Query();
        void Add(string typeOf);
        void Remove(string typeOf);
    }
}
