using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace PathAnimationDemo;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private double PathWidth = 20;

    /// <summary>
    /// 正转
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void btnAnimo_Click(object sender, RoutedEventArgs e)
    {
        AnimationByPath(this.cvsMain, this.path1, PathWidth, false, 3);
        AnimationByPath(this.cvsMain, this.path2, PathWidth, false, 3);
        AnimationByPath(this.cvsMain, this.path3, PathWidth, false, 3);

        StoryByOrient(this.imgFan, 0, 3);
    }
    /// <summary>
    /// 反转
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void btnReback_Click(object sender, RoutedEventArgs e)
    {
        AnimationByPath(this.cvsMain, this.path1, PathWidth, true, 3);
        AnimationByPath(this.cvsMain, this.path2, PathWidth, true, 3);
        AnimationByPath(this.cvsMain, this.path3, PathWidth, true, 3);

        StoryByOrient(this.imgFan, 1, 3);
    }

    /// <summary>
    /// 旋转动画
    /// </summary>
    /// <param name="img">动画对象</param>
    /// <param name="orientation">顺时针/逆时针</param>
    /// <param name="duration"></param>
    private void StoryByOrient(Image img, int orientation, int duration = 5)
    {
        Storyboard storyboard = new Storyboard();//创建故事板
        DoubleAnimation doubleAnimation = new DoubleAnimation();//实例化一个Double类型的动画
        RotateTransform rotate = new RotateTransform();//旋转转换实例
        img.RenderTransform = rotate;//给图片空间一个转换的实例
        storyboard.RepeatBehavior = RepeatBehavior.Forever;//设置重复为 一直重复
        storyboard.SpeedRatio = 2;//播放的数度
                                  //设置从0 旋转360度
        doubleAnimation.From = 0;
        if (orientation == 0)//顺时针
        {
            doubleAnimation.To = 360;
        }
        else//逆时针
        {
            doubleAnimation.To = -360;
        }
        doubleAnimation.Duration = new Duration(TimeSpan.FromSeconds(duration));//播放时间长度为2秒
        Storyboard.SetTarget(doubleAnimation, img);//给动画指定对象
        Storyboard.SetTargetProperty(doubleAnimation,
    new PropertyPath("RenderTransform.Angle"));//给动画指定依赖的属性
        storyboard.Children.Add(doubleAnimation);//将动画添加到动画板中
        storyboard.Begin(img);//启动动画
    }

    /// <summary>
    /// 路径动画
    /// </summary>
    /// <param name="cvs">画板</param>
    /// <param name="path">路径</param>
    /// <param name="targetWidth">动画对象宽高</param>
    /// <param name="isInverse">是否反向</param>
    /// <param name="duration">动画时间</param>
    private void AnimationByPath(Canvas cvs, Path path, double targetWidth, bool isInverse = false, int duration = 5)
    {
        Polygon target = new Polygon();
        target.Points = new PointCollection()
    {
        new Point(0,0),
        new Point(targetWidth/2,0),
        new Point(targetWidth,targetWidth/2),
        new Point(targetWidth/2,targetWidth),
        new Point(0,targetWidth),
        new Point(targetWidth/2,targetWidth/2)
    };

        if (isInverse)//反向
        {
            target.Fill = new SolidColorBrush(Colors.DeepSkyBlue);
        }
        else//正向
        {
            target.Fill = new SolidColorBrush(Colors.Orange);
        }

        cvs.Children.Add(target);
        Canvas.SetLeft(target, -targetWidth / 2);
        Canvas.SetTop(target, -targetWidth / 2);
        target.RenderTransformOrigin = new Point(0.5, 0.5);

        MatrixTransform matrix = new MatrixTransform();
        TransformGroup groups = new TransformGroup();
        groups.Children.Add(matrix);
        target.RenderTransform = groups;
        string registname = "matrix" + Guid.NewGuid().ToString().Replace("-", "");
        this.RegisterName(registname, matrix);
        MatrixAnimationUsingPath matrixAnimation = new MatrixAnimationUsingPath();
        if (!isInverse)//正向
        {
            matrixAnimation.PathGeometry = PathGeometry.CreateFromGeometry(Geometry.Parse(path.Data.ToString()));
        }
        else//反向
        {
            string data = ConvertPathData(path.Data.ToString());
            matrixAnimation.PathGeometry = PathGeometry.CreateFromGeometry(Geometry.Parse(data));
        }
        matrixAnimation.Duration = new Duration(TimeSpan.FromSeconds(duration));
        matrixAnimation.DoesRotateWithTangent = true;//旋转
        matrixAnimation.RepeatBehavior = RepeatBehavior.Forever;
        Storyboard story = new Storyboard();
        story.Children.Add(matrixAnimation);
        Storyboard.SetTargetName(matrixAnimation, registname);
        Storyboard.SetTargetProperty(matrixAnimation, new PropertyPath(MatrixTransform.MatrixProperty));

        story.FillBehavior = FillBehavior.Stop;
        story.Begin(target, true);
    }

    private string ConvertPathData(string data)
    {
        data = data.Replace("M", "");
        Regex regex = new Regex("[a-z]", RegexOptions.IgnoreCase);
        MatchCollection mc = regex.Matches(data);
        //item1 从上一个位置到当前位置开始的字符 (match.Index=原始字符串中发现捕获的子字符串的第一个字符的位置。)
        //item2 当前发现的匹配符号(L C Z M)
        List<Tuple<string, string>> tmps = new List<Tuple<string, string>>();
        int index = 0;
        for (int i = 0; i < mc.Count; i++)
        {
            Match match = mc[i];
            if (match.Index != index)
            {
                string str = data.Substring(index, match.Index - index);
                tmps.Add(new Tuple<string, string>(str, match.Value));
            }
            index = match.Index + match.Length;
            if (i + 1 == mc.Count)//last 
            {
                tmps.Add(new Tuple<string, string>(data.Substring(index), match.Value));
            }
        }
        List<string[]> arrys = new List<string[]>();
        Regex regexnum = new Regex(@"(\-?\d+\.?\d*)", RegexOptions.IgnoreCase);
        for (int i = 0; i < tmps.Count; i++)
        {
            MatchCollection childMcs = regexnum.Matches(tmps[i].Item1);
            if (childMcs.Count % 2 != 0)
            {
                continue;
            }
            int groups = childMcs.Count / 2;
            var strTmp = new string[groups];
            for (int j = 0; j < groups; j++)
            {
                string cdatas = childMcs[j * 2] + "," + childMcs[j * 2 + 1];//重组数据
                strTmp[j] = cdatas;
            }
            arrys.Add(strTmp);
        }

        List<string> result = new List<string>();
        for (int i = arrys.Count - 1; i >= 0; i--)
        {
            string[] clist = arrys[i];
            for (int j = clist.Length - 1; j >= 0; j--)
            {
                if (j == clist.Length - 2 && i > 0)//对于第二个元素增加 L或者C的标识
                {
                    var pointWord = tmps[i - 1].Item2;//获取标识
                    result.Add(pointWord + clist[j]);
                }
                else
                {
                    result.Add(clist[j]);
                    if (clist.Length == 1 && i > 0)//说明只有一个元素 ex L44.679973,69.679973
                    {
                        result.Add(tmps[i - 1].Item2);
                    }
                }
            }
        }
        return "M" + string.Join(" ", result);

    }
}
