using GalaSoft.MvvmLight;
using RPA.Shared.Utils;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Xml;

namespace RPAStudio.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class AboutViewModel : ViewModelBase
    {
        private List<string> _currentVersionUpdateLogList { get; set; } = new List<string>();

        /// <summary>
        /// Initializes a new instance of the AboutPageViewModel class.
        /// </summary>
        public AboutViewModel()
        {
            Init();
        }


        private void Init()
        {
            CurrentVersionName = "v" + Common.GetProgramVersion();

            initRPAUpgradeClientConfig();
            CurrentVersionUpdateLog = "";
            foreach (var item in _currentVersionUpdateLogList)
            {
                CurrentVersionUpdateLog += " ● " + item + System.Environment.NewLine;
            }

            
        }


        /// <summary>
        /// 初始化前端升级配置文件
        /// </summary>
        private void initRPAUpgradeClientConfig()
        {
            _currentVersionUpdateLogList.Clear();

            XmlDocument doc = new XmlDocument();

            var content = Common.GetResourceContentByUri($"pack://application:,,,/Resources/Config/RPAUpgradeClientConfig.xml");
            doc.LoadXml(content);
           
            var rootNode = doc.DocumentElement;

            var updateLogElement = rootNode.SelectSingleNode("UpdateLog");
            var items = updateLogElement.SelectNodes("Item");
            foreach (var item in items)
            {
                var text = (item as XmlElement).InnerText;
                _currentVersionUpdateLogList.Add(text);
            }
        }

        
        /// <summary>
        /// The <see cref="CurrentVersionName" /> property's name.
        /// </summary>
        public const string CurrentVersionNamePropertyName = "CurrentVersionName";

        private string _currentVersionNameProperty = "";

        /// <summary>
        /// 当前版本名称
        /// </summary>
        public string CurrentVersionName
        {
            get
            {
                return _currentVersionNameProperty;
            }

            set
            {
                if (_currentVersionNameProperty == value)
                {
                    return;
                }

                _currentVersionNameProperty = value;
                RaisePropertyChanged(CurrentVersionNamePropertyName);
            }
        }


        /// <summary>
        /// The <see cref="CurrentVersionUpdateLog" /> property's name.
        /// </summary>
        public const string CurrentVersionUpdateLogPropertyName = "CurrentVersionUpdateLog";

        private string _currentVersionUpdateLogProperty = "";

        /// <summary>
        ///当前版本更新日志
        /// </summary>
        public string CurrentVersionUpdateLog
        {
            get
            {
                return _currentVersionUpdateLogProperty;
            }

            set
            {
                if (_currentVersionUpdateLogProperty == value)
                {
                    return;
                }

                _currentVersionUpdateLogProperty = value;
                RaisePropertyChanged(CurrentVersionUpdateLogPropertyName);
            }
        }


    }
}