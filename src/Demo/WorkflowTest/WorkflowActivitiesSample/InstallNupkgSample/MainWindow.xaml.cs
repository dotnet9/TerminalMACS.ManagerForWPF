using NuGet;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace InstallNupkgSample
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private string _nupkgInstalledDirectory = "NupkgInstalledDirectory";

        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 选择一个Nupkg类型的包文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BrowserButton_Click(object sender, RoutedEventArgs e)
        {
            string filter = "Nupkg包文件|*.nupkg";
            string title = "请选择一个Nupkg包文件";

            string select_file = "";

            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Multiselect = false; //只选择一个文件
            fileDialog.Filter = filter;
            if (!string.IsNullOrEmpty(title))
            {
                fileDialog.Title = title;
            }

            if (fileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                foreach (string file in fileDialog.FileNames)
                {
                    select_file = file;
                }
            }

            NupkgPathTextBox.Text = select_file;
        }

        /// <summary>
        /// 安装Nupkg包，通过将Nupkg包所在的目录当成包源，来执行安装操作
        /// 注意：这种方式的安装和用Visual Studio的安装Nupkg包的目录结果并不一样，后续会介绍其它形式的安装
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InstallButton_Click(object sender, RoutedEventArgs e)
        {
            var packagePath = NupkgPathTextBox.Text;

            if(!System.IO.File.Exists(packagePath))
            {
                System.Windows.Forms.MessageBox.Show("请选择一个合法的Nupkg文件路径！");
                return;
            }

            var packageDir = System.IO.Path.GetDirectoryName(packagePath);
            var packageFileName = System.IO.Path.GetFileName(packagePath);

            var repo = PackageRepositoryFactory.Default.CreateRepository(packageDir);
            var packageManager = new PackageManager(repo, _nupkgInstalledDirectory);

            packageManager.PackageInstalled += PackageManager_PackageInstalled;

            var zipPackage = new ZipPackage(packagePath);

            var packageDirectory = packageManager.PathResolver.GetPackageDirectory(zipPackage.Id, zipPackage.Version);

            var fullNupkgInstalledPath = System.IO.Path.Combine(_nupkgInstalledDirectory, packageDirectory);

            if(System.IO.Directory.Exists(fullNupkgInstalledPath))
            {
                System.Windows.Forms.MessageBox.Show($"{packageFileName}已经安装，无须安装");
            }
            else
            {
                packageManager.InstallPackage(zipPackage.Id, zipPackage.Version, true, true);//只安装当前包，忽略依赖包的安装
            }
        }


        /// <summary>
        /// 安装Nupkg包成功后调用
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PackageManager_PackageInstalled(object sender, PackageOperationEventArgs e)
        {
            System.Windows.Forms.MessageBox.Show($"安装成功，路径为{e.InstallPath}");
        }


        /// <summary>
        /// 获取依赖项信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GetDependenciesButton_Click(object sender, RoutedEventArgs e)
        {
            var packagePath = NupkgPathTextBox.Text;

            if (!System.IO.File.Exists(packagePath))
            {
                System.Windows.Forms.MessageBox.Show("请选择一个合法的Nupkg文件路径！");
                return;
            }

            var zipPackage = new ZipPackage(packagePath);

            foreach(var set in zipPackage.DependencySets)
            {
                foreach(var d in set.Dependencies)
                {
                    ShowDependenciesTextBox.Text += d.ToString();
                    ShowDependenciesTextBox.Text += Environment.NewLine;
                }
            }
        }

        /// <summary>
        /// 定位到Nupkg包的安装目录，方便查看
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BrowserInstalledDirectoryButton_Click(object sender, RoutedEventArgs e)
        {
            if(!System.IO.Directory.Exists(_nupkgInstalledDirectory))
            {
                System.IO.Directory.CreateDirectory(_nupkgInstalledDirectory);
            }

            Process.Start("explorer.exe", _nupkgInstalledDirectory);
        }
    }
}
