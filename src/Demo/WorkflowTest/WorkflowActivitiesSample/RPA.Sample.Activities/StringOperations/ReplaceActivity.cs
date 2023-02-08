using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using System.ComponentModel;
using System.Drawing;

namespace RPA.Sample.Activities.StringOperations
{
    [Designer(typeof(ReplaceActivityDesigner))]
    //此处用到一个技巧，通过ResFinder类来让GetImageFromResource()方法强制寻找命名空间RPA.Sample.Activities下的Resources.StringOperations.Toolbox.ReplaceActivity.ico
    //注意必须为16X16的ico才能被Visual Studio的工具箱加载并显示图标
    [ToolboxBitmap(typeof(ResFinder), "Resources.StringOperations.Toolbox.ReplaceActivity.ico")]//该资源的生成操作必须是“嵌入的资源”才可以正常显示工具图标
    public sealed class ReplaceActivity : CodeActivity
    {
        [RequiredArgument]
        [Category("输入")]
        [DisplayName("源字符串")]
        public InArgument<string> SourceStr { get; set; }

        [RequiredArgument]
        [Category("输入")]
        [DisplayName("旧字符串")]
        public InArgument<string> OldStr { get; set; }

        [RequiredArgument]
        [Category("输入")]
        [DisplayName("新字符串")]
        public InArgument<string> NewStr { get; set; }

        [Category("输出")]
        [DisplayName("结果字符串")]
        public OutArgument<string> Result { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            string sourceStr = SourceStr.Get(context);//也可以写成context.GetValue(SourceStr);
            string oldStr = OldStr.Get(context);
            string newStr = NewStr.Get(context);

            var ret = sourceStr.Replace(oldStr, newStr);
            Result.Set(context, ret);
        }
    }
}
