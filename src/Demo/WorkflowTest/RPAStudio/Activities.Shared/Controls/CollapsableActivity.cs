using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Activities.Shared.Controls
{
    /// <summary>
    /// 折叠活动类
    /// </summary>
    public class CollapsableActivity : ContentControl
    {
        static CollapsableActivity()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CollapsableActivity), new FrameworkPropertyMetadata(typeof(CollapsableActivity)));
        }
    }
}