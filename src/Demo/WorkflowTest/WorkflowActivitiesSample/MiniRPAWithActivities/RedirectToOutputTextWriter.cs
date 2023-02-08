using System.IO;
using System.Text;

namespace MiniRPAWithActivities
{
    /// <summary>
    /// 输出重定向
    /// </summary>
    public class RedirectToOutputTextWriter : TextWriter
    {
        private MainWindow mainWindow;

        public RedirectToOutputTextWriter(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
        }

        //字符集编码
        public override Encoding Encoding
        {
            get
            {
                return Encoding.UTF8;
            }
        }

        /// <summary>
        /// 输出文本并换行
        /// </summary>
        /// <param name="value">内容</param>
        public override void WriteLine(string value)
        {
            //重定向WriteLine组件的输出到输出面板中
            mainWindow.OutputLine(value);
        }
    }
}