using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPA.Shared.Utils
{
    public static class CommonDialog
    {
        //选择保存目录
        private static string defaultSelectDirPath = "";//记录上次选择的目录  
        public static bool ShowSelectDirDialog(string desc, ref string select_dir)
        {
            System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
            dialog.Description = desc;
            if (defaultSelectDirPath != "")
            {
                dialog.SelectedPath = defaultSelectDirPath;
            }

            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (string.IsNullOrEmpty(dialog.SelectedPath))
                {
                    return false;
                }

                select_dir = dialog.SelectedPath;

                defaultSelectDirPath = dialog.SelectedPath;
                return true;
            }

            return false;
        }

    }
}
