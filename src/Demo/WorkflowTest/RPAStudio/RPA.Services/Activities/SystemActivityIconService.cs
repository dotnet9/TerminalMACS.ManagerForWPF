using RPA.Interfaces.Activities;
using RPA.Shared.Utils;
using System;
using System.Activities.Presentation;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace RPA.Services.Activities
{
    public class SystemActivityIconService : ISystemActivityIconService
    {
        private Dictionary<string, Brush> _typeOfBrushDict = new Dictionary<string, Brush>();

        public SystemActivityIconService()
        {
            RegisterSystemIcons();
        }

        private void RegisterSystemIcons()
        {
            //修改系统图标为自定义图标，如TypeOfBrushDict["Sequence"] = XXXXX;
        }

        public ImageSource GetIcon(string typeOf)
        {
            typeOf = typeOf.Split('`')[0];
            if (_typeOfBrushDict.ContainsKey(typeOf))
            {
                return Common.BitmapSourceFromBrush(_typeOfBrushDict[typeOf]);
            }
            else
            {
                Type activitiesIcons = typeof(WorkflowDesignerIcons.Activities);
                var info = activitiesIcons.GetProperty(typeOf);
                if (info != null)
                {
                    _typeOfBrushDict[typeOf] = (DrawingBrush)info.GetValue(null);//缓存起来
                    return Common.BitmapSourceFromBrush(_typeOfBrushDict[typeOf]);
                }
            }

            return null;
        }
    }
}
