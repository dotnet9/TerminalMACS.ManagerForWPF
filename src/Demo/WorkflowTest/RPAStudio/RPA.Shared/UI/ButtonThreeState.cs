using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace RPA.Shared.UI
{
    /// <summary>
    /// 三态按钮扩展类
    /// </summary>
    public class ButtonThreeState : Button
    {
        /// <summary>
        /// 默认图片
        /// </summary>
        public string def
        {
            get { return (string)GetValue(defProperty); }
            set { SetValue(defProperty, value); }
        }

        // Using a DependencyProperty as the backing store for src.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty defProperty =
            DependencyProperty.Register("def", typeof(string), typeof(ButtonThreeState), new PropertyMetadata(""));

        /// <summary>
        /// 鼠标悬浮图片
        /// </summary>
        public string hover
        {
            get { return (string)GetValue(hoverProperty); }
            set { SetValue(hoverProperty, value); }
        }

        // Using a DependencyProperty as the backing store for src.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty hoverProperty =
            DependencyProperty.Register("hover", typeof(string), typeof(ButtonThreeState), new PropertyMetadata(""));

        /// <summary>
        /// 鼠标激活图片
        /// </summary>
        public string active
        {
            get { return (string)GetValue(activeProperty); }
            set { SetValue(activeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for src.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty activeProperty =
            DependencyProperty.Register("active", typeof(string), typeof(ButtonThreeState), new PropertyMetadata(""));



        /// <summary>
        /// 鼠标禁止图片
        /// </summary>
        public string forbid
        {
            get { return (string)GetValue(forbidProperty); }
            set { SetValue(forbidProperty, value); }
        }

        // Using a DependencyProperty as the backing store for forbid.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty forbidProperty =
            DependencyProperty.Register("forbid", typeof(string), typeof(ButtonThreeState), new PropertyMetadata(""));



        /// <summary>
        /// 默认前景色
        /// </summary>
        public Brush def_foreground
        {
            get { return (Brush)GetValue(def_foregroundProperty); }
            set { SetValue(def_foregroundProperty, value); }
        }

        // Using a DependencyProperty as the backing store for def_foreground.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty def_foregroundProperty =
            DependencyProperty.Register("def_foreground", typeof(Brush), typeof(ButtonThreeState), new PropertyMetadata(Brushes.Black));



        /// <summary>
        /// 点击前景色
        /// </summary>
        public Brush click_foreground
        {
            get { return (Brush)GetValue(click_foregroundProperty); }
            set { SetValue(click_foregroundProperty, value); }
        }

        // Using a DependencyProperty as the backing store for click_foreground.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty click_foregroundProperty =
            DependencyProperty.Register("click_foreground", typeof(Brush), typeof(ButtonThreeState), new PropertyMetadata(Brushes.White));



        /// <summary>
        /// 悬浮前景色
        /// </summary>
        public Brush hover_foreground
        {
            get { return (Brush)GetValue(hover_foregroundProperty); }
            set { SetValue(hover_foregroundProperty, value); }
        }

        // Using a DependencyProperty as the backing store for hover_foreground.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty hover_foregroundProperty =
            DependencyProperty.Register("hover_foreground", typeof(Brush), typeof(ButtonThreeState), new PropertyMetadata(Brushes.White));



        /// <summary>
        /// 禁用前景色
        /// </summary>
        public Brush forbid_foreground
        {
            get { return (Brush)GetValue(forbid_foregroundProperty); }
            set { SetValue(forbid_foregroundProperty, value); }
        }

        // Using a DependencyProperty as the backing store for forbid_foreground.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty forbid_foregroundProperty =
            DependencyProperty.Register("forbid_foreground", typeof(Brush), typeof(ButtonThreeState), new PropertyMetadata(new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFB4B4B4"))));

    }
}
