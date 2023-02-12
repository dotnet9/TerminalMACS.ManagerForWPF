using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace RPA.Shared.Utils
{
    public static class Common
    {
        [DllImport("kernel32.dll")]
        public static extern bool AllocConsole();

        [DllImport("kernel32.dll")]
        public static extern bool FreeConsole();

        private readonly static Logger _logger = LogManager.GetCurrentClassLogger();
        private static ImageSourceConverter _imageSourceConverter = new ImageSourceConverter();


        public static void OpenConsole()
        {
            AllocConsole();
        }

        public static void CloseConsole()
        {
            FreeConsole();
        }

        public static string GetProgramVersion()
        {
            FileVersionInfo myFileVersion = FileVersionInfo.GetVersionInfo(System.Windows.Forms.Application.ExecutablePath);
            return myFileVersion.FileVersion;
        }

        public static void MakeSureDirectoryExists(string dir)
        {
            if (!System.IO.Directory.Exists(dir))
            {
                System.IO.Directory.CreateDirectory(dir);
            }
        }

        public static string GetValidDirectoryName(string path, string name, string suffix_format = " ({0})", int begin_index = 2)
        {
            var validName = name;
            if (Directory.Exists(path + @"\" + name))
            {
                for (int i = begin_index; ; i++)
                {
                    var format_i = string.Format(suffix_format, i);
                    if (!Directory.Exists(path + @"\" + name + format_i))
                    {
                        validName = name + format_i;
                        break;
                    }
                }
            }

            return validName;
        }

        public static DispatcherOperation InvokeAsyncOnUI(Action callback, DispatcherPriority priority = DispatcherPriority.Normal)
        {
            return Application.Current.Dispatcher.InvokeAsync(callback, priority);
        }

        public static void InvokeOnUI(Action callback, DispatcherPriority priority = DispatcherPriority.Normal)
        {
            Application.Current.Dispatcher.Invoke(callback, priority);
        }


        public static string GetResourceContentByUri(string uri)
        {
            var streamResourceInfo = Application.GetResourceStream(new Uri(uri, UriKind.Absolute));
            try
            {
                using (StreamReader reader = new StreamReader(streamResourceInfo.Stream))
                {
                    var content = reader.ReadToEnd();

                    return content;
                }
            }
            catch (Exception err)
            {
                _logger.Error(err);
            }

            return null;
        }


        public static bool WriteResourceContentByUri(string path,string uri)
        {
            var content = GetResourceContentByUri(uri);
            if(string.IsNullOrEmpty(content))
            {
                return false;
            }

            try
            {
                File.WriteAllText(path, content);
            }
            catch (Exception err)
            {
                _logger.Error(err);

                return false;
            }

            return true;
        }


        /// <summary>
        /// 构造相对路径
        /// </summary>
        /// <param name="baseDir">基准目录</param>
        /// <param name="filePath">文件路径</param>
        /// <returns>相对路径</returns>
        public static string MakeRelativePath(string baseDir, string filePath)
        {
            if (string.IsNullOrEmpty(baseDir)) throw new ArgumentNullException("baseDir");
            if (string.IsNullOrEmpty(filePath)) throw new ArgumentNullException("filePath");

            if (!baseDir.EndsWith(@"\") && !baseDir.EndsWith(@"/"))
            {
                baseDir += @"\";
            }

            Uri fromUri = new Uri(baseDir);
            Uri toUri = new Uri(filePath);

            if (fromUri.Scheme != toUri.Scheme) { return filePath; }

            Uri relativeUri = fromUri.MakeRelativeUri(toUri);
            string relativePath = Uri.UnescapeDataString(relativeUri.ToString());

            if (toUri.Scheme.Equals("file", StringComparison.InvariantCultureIgnoreCase))
            {
                relativePath = relativePath.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
            }

            return relativePath;
        }


        /// <summary>
        /// 在资源管理器中定位目录
        /// </summary>
        /// <param name="dir">目录</param>
        public static void LocateDirInExplorer(string dir)
        {
            Process.Start("explorer.exe", dir);
        }


        /// <summary>
        /// 弹出单选文件对话框
        /// </summary>
        /// <param name="filter">过滤条件</param>
        /// <param name="title">标题</param>
        /// <returns>单选的文件</returns>
        public static string ShowSelectSingleFileDialog(string filter = null, string title = "")
        {
            string selectFile = "";

            System.Windows.Forms.OpenFileDialog fileDialog = new System.Windows.Forms.OpenFileDialog();
            fileDialog.Multiselect = false; //不可以选择多个文件
            fileDialog.Filter = filter;
            if (!string.IsNullOrEmpty(title))
            {
                fileDialog.Title = title;
            }

            if (fileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                foreach (string file in fileDialog.FileNames)
                {
                    selectFile = file;
                }
            }

            return selectFile;
        }



        private static void InitDirectory(DirectoryInfo di, List<DirOrFileItem> items, Predicate<DirOrFileItem> match)
        {
            //当前目录文件夹遍历
            DirectoryInfo[] dis = di.GetDirectories();
            for (int j = 0; j < dis.Length; j++)
            {
                var item = new DirItem();
                item.Path = dis[j].FullName;
                item.ParentPath = Path.GetDirectoryName(item.Path);
                item.Name = dis[j].Name;
                item.FileSystemInfo = dis[j];

                if (match == null)
                {
                    items.Add(item);
                    item.Children = QueryDirectoryAndFiles(item.Path, match);
                }
                else
                {
                    if (match(item))
                    {
                        items.Add(item);
                        item.Children = QueryDirectoryAndFiles(item.Path, match);
                    }
                }
            }

            //当前目录文件遍历
            FileInfo[] fis = di.GetFiles();
            for (int i = 0; i < fis.Length; i++)
            {
                var item = new FileItem();
                item.Path = fis[i].FullName;
                item.ParentPath = Path.GetDirectoryName(item.Path);
                item.Name = fis[i].Name;
                item.Extension = fis[i].Extension;
                item.FileSystemInfo = fis[i];

                var icon = System.Drawing.Icon.ExtractAssociatedIcon(item.Path);

                System.Windows.Media.ImageSource imageSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHIcon(
                icon.Handle,
                Int32Rect.Empty,
                System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());

                item.AssociatedIcon = imageSource;

                if (match == null)
                {
                    items.Add(item);
                }
                else
                {
                    if (match(item))
                    {
                        items.Add(item);
                    }
                }
            }
        }

        public static List<DirOrFileItem> QueryDirectoryAndFiles(string path, Predicate<DirOrFileItem> match = null)
        {
            var di = new DirectoryInfo(path);

            var items = new List<DirOrFileItem>();

            InitDirectory(di, items, match);

            return items;
        }

        /// <summary>
        /// 获取有效的文件名
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="name">名称</param>
        /// <param name="prefix_format">前缀格式</param>
        /// <param name="suffix_format">后缀格式</param>
        /// <param name="begin_index">起始序号</param>
        /// <returns>有效文件名</returns>
        public static string GetValidFileName(string path, string name, string prefix_format = "", string suffix_format = " ({0})", int begin_index = 2)
        {
            var ext = Path.GetExtension(path + @"\" + name);
            var fileNameWithoutExt = Path.GetFileNameWithoutExtension(path + @"\" + name);

            var validName = name;
            if (File.Exists(path + @"\" + name))
            {
                if (File.Exists(path + @"\" + fileNameWithoutExt + prefix_format + ext))
                {
                    for (int i = begin_index; ; i++)
                    {
                        var format_i = string.Format(prefix_format + suffix_format, i);
                        if (!File.Exists(path + @"\" + fileNameWithoutExt + format_i + ext))
                        {
                            validName = fileNameWithoutExt + format_i + ext;
                            break;
                        }
                    }
                }
                else
                {
                    validName = fileNameWithoutExt + prefix_format + ext;
                }


            }

            return validName;
        }

        /// <summary>
        /// 获取不包含后缀样式的文件名
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="pattern">样式</param>
        /// <returns>文件名称</returns>
        public static string GetFileNameWithoutSuffixFormat(string name, string pattern = @"(.*) \([0-9]+\)")
        {
            var ext = Path.GetExtension(name);
            var fileNameWithoutExt = Path.GetFileNameWithoutExtension(name);

            Match match = Regex.Match(fileNameWithoutExt, pattern);
            if (match.Success)
            {
                if (match.Groups.Count == 2)
                {
                    fileNameWithoutExt = match.Groups[1].Value;
                }
            }

            return fileNameWithoutExt + ext;
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="file">文件路径</param>
        /// <returns>是否删除成功</returns>
        public static bool DeleteFile(string file)
        {
            try
            {
                File.Delete(file);
            }
            catch (Exception err)
            {
                _logger.Debug(err);
                return false;
            }

            return true;
        }



        public static ImageSource ToImageSource(string uri)
        {
            if (string.IsNullOrEmpty(uri))
            {
                return null;
            }

            return _imageSourceConverter.ConvertFromInvariantString(uri) as ImageSource;
        }


        /// <summary>
        /// 获取不包含后缀形式的目录名
        /// </summary>
        /// <param name="name">名字</param>
        /// <param name="pattern">样式</param>
        /// <returns>目录名</returns>
        public static string GetDirectoryNameWithoutSuffixFormat(string name, string pattern = @"(.*) \([0-9]+\)")
        {
            Match match = Regex.Match(name, pattern);
            if (match.Success)
            {
                if (match.Groups.Count == 2)
                {
                    name = match.Groups[1].Value;
                }
            }

            return name;
        }



        public static bool DeleteDir(string path)
        {
            try
            {
                DirectoryInfo di = new DirectoryInfo(path);
                di.Delete(true);
            }
            catch (Exception err)
            {
                _logger.Debug(err);
                return false;
            }

            return true;
        }

        /// <summary>
        /// 判断文件中是否包含某字符串
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="searchString">搜索字符串</param>
        /// <returns>是否包含</returns>
        public static bool IsStringInFile(string fileName, string searchString)
        {
            return File.ReadAllText(fileName).Contains(searchString);
        }


        /// <summary>
        /// 遍历一个目录所有的子目录及文件，通过函数进行判断，如果返回true，则直接返回，标明已经确认某个条件成立
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public delegate bool CheckDeleage(object item, object param);

        public static bool DirectoryChildrenForEach(DirectoryInfo di, CheckDeleage checkFun, object param = null)
        {
            //当前目录文件夹遍历
            DirectoryInfo[] dis = di.GetDirectories();
            for (int j = 0; j < dis.Length; j++)
            {
                DirectoryInfo diItem = dis[j];
                if (checkFun != null && checkFun(diItem, param))
                {
                    return true;
                }

                if (DirectoryChildrenForEach(diItem, checkFun, param))
                {
                    return true;
                }
            }

            //当前目录文件遍历
            FileInfo[] fis = di.GetFiles();
            for (int i = 0; i < fis.Length; i++)
            {
                FileInfo fiItem = fis[i];

                if (checkFun != null && checkFun(fiItem, param))
                {
                    return true;
                }
            }


            return false;

        }



        /// <summary>
        /// 弹出多选文件对话框
        /// </summary>
        /// <param name="filter">过滤条件</param>
        /// <param name="title">标题</param>
        /// <returns>选择的文件列表</returns>
        public static List<string> ShowSelectMultiFileDialog(string filter = null, string title = "")
        {
            List<string> fileList = new List<string>();

            System.Windows.Forms.OpenFileDialog fileDialog = new System.Windows.Forms.OpenFileDialog();
            fileDialog.Multiselect = true; //可以选择多个文件
            fileDialog.Filter = filter;
            if (!string.IsNullOrEmpty(title))
            {
                fileDialog.Title = title;
            }

            if (fileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                foreach (string file in fileDialog.FileNames)
                {
                    fileList.Add(file);
                }
            }

            return fileList;
        }


        public static ContextMenu ShowContextMenu(object dataContext, string resKey)
        {
            var view = Application.Current.MainWindow;
            var cm = view.FindResource(resKey) as ContextMenu;
            cm.DataContext = dataContext;
            cm.Placement = PlacementMode.MousePoint;
            cm.IsOpen = true;
            return cm;
        }


        /// <summary>
        /// Brush转BitmapSource
        /// </summary>
        /// <param name="drawingBrush"></param>
        /// <param name="size"></param>
        /// <param name="dpi"></param>
        /// <returns></returns>
        public static BitmapSource BitmapSourceFromBrush(Brush drawingBrush, int size = 32, int dpi = 96)
        {
            var pixelFormat = PixelFormats.Pbgra32;
            RenderTargetBitmap rtb = new RenderTargetBitmap(size, size, dpi, dpi, pixelFormat);

            var drawingVisual = new DrawingVisual();
            using (DrawingContext context = drawingVisual.RenderOpen())
            {
                context.DrawRectangle(drawingBrush, null, new Rect(0, 0, size, size));
            }

            rtb.Render(drawingVisual);
            return rtb;
        }


        //选择保存路径(另存为)
        public static bool ShowSaveAsFileDialog(out string user_sel_path, string show_file, string filter_ext, string filter_desc)
        {
            System.Windows.Forms.SaveFileDialog sfd = new System.Windows.Forms.SaveFileDialog();
            sfd.FileName = show_file;
            //设置文件类型 
            //Excel表格（*.xls）|*.xls"
            sfd.Filter = string.Format("{0}(*{1})|*{1}", filter_desc, filter_ext);

            //设置默认文件类型显示顺序 
            sfd.FilterIndex = 1;

            //保存对话框是否记忆上次打开的目录 
            sfd.RestoreDirectory = true;

            //点了保存按钮进入 
            if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                user_sel_path = sfd.FileName;
                return true;
            }

            user_sel_path = "";
            return false;
        }


        public static void LocateFileInExplorer(string file)
        {
            Process.Start("explorer.exe", "/select," + file);
        }


        public static ImageSource BitmapFromUri(Uri source)
        {
            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = source;
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.EndInit();
            return bitmap;
        }


    }
}
