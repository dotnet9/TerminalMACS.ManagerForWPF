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
    /// 带颜色的三态按钮控件扩展
    /// </summary>
    public class ColorButtonThreeState : Button
    {

        /// <summary>
        /// 默认图像
        /// </summary>
        public string def_img
        {
            get { return (string)GetValue(def_imgProperty); }
            set { SetValue(def_imgProperty, value); }
        }

        // Using a DependencyProperty as the backing store for def_img.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty def_imgProperty =
            DependencyProperty.Register("def_img", typeof(string), typeof(ColorButtonThreeState), new PropertyMetadata(""));




        /// <summary>
        /// 默认颜色
        /// </summary>
        public Brush def
        {
            get { return (Brush)GetValue(defProperty); }
            set { SetValue(defProperty, value); }
        }

        // Using a DependencyProperty as the backing store for src.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty defProperty =
            DependencyProperty.Register("def", typeof(Brush), typeof(ColorButtonThreeState), new PropertyMetadata(Brushes.Transparent));

        /// <summary>
        /// 悬浮颜色
        /// </summary>
        public Brush hover
        {
            get { return (Brush)GetValue(hoverProperty); }
            set { SetValue(hoverProperty, value); }
        }

        // Using a DependencyProperty as the backing store for src.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty hoverProperty =
            DependencyProperty.Register("hover", typeof(Brush), typeof(ColorButtonThreeState), new PropertyMetadata(new SolidColorBrush((Color)ColorConverter.ConvertFromString("#d6d6d6"))));

        /// <summary>
        /// 激活颜色
        /// </summary>
        public Brush active
        {
            get { return (Brush)GetValue(activeProperty); }
            set { SetValue(activeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for src.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty activeProperty =
            DependencyProperty.Register("active", typeof(Brush), typeof(ColorButtonThreeState), new PropertyMetadata(new SolidColorBrush((Color)ColorConverter.ConvertFromString("#afafaf"))));



        /// <summary>
        /// 禁用颜色
        /// </summary>
        public Brush forbid
        {
            get { return (Brush)GetValue(forbidProperty); }
            set { SetValue(forbidProperty, value); }
        }

        // Using a DependencyProperty as the backing store for forbid.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty forbidProperty =
            DependencyProperty.Register("forbid", typeof(Brush), typeof(ColorButtonThreeState), new PropertyMetadata(Brushes.Black));



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
            DependencyProperty.Register("def_foreground", typeof(Brush), typeof(ColorButtonThreeState), new PropertyMetadata(Brushes.Black));



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
            DependencyProperty.Register("click_foreground", typeof(Brush), typeof(ColorButtonThreeState), new PropertyMetadata(Brushes.Black));



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
            DependencyProperty.Register("hover_foreground", typeof(Brush), typeof(ColorButtonThreeState), new PropertyMetadata(Brushes.Black));



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
            DependencyProperty.Register("forbid_foreground", typeof(Brush), typeof(ColorButtonThreeState), new PropertyMetadata(new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFB4B4B4"))));


    }
}
