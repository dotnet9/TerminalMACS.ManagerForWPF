using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System.Windows;

namespace RPAStudio.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MessageDetailsViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the MessageDetailsViewModel class.
        /// </summary>
        public MessageDetailsViewModel()
        {
        }



        /// <summary>
        /// The <see cref="WindowTitle" /> property's name.
        /// </summary>
        public const string WindowTitlePropertyName = "WindowTitle";

        private string _windowTitleProperty = "";

        /// <summary>
        /// Sets and gets the WindowTitle property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string WindowTitle
        {
            get
            {
                return _windowTitleProperty;
            }

            set
            {
                if (_windowTitleProperty == value)
                {
                    return;
                }

                _windowTitleProperty = value;
                RaisePropertyChanged(WindowTitlePropertyName);
            }
        }


        /// <summary>
        /// The <see cref="MsgDetails" /> property's name.
        /// </summary>
        public const string MsgDetailsPropertyName = "MsgDetails";

        private string _msgDetailsProperty = "";

        /// <summary>
        /// 消息详情
        /// </summary>
        public string MsgDetails
        {
            get
            {
                return _msgDetailsProperty;
            }

            set
            {
                if (_msgDetailsProperty == value)
                {
                    return;
                }

                _msgDetailsProperty = value;
                RaisePropertyChanged(MsgDetailsPropertyName);
            }
        }


        private RelayCommand _copyCommand;

        /// <summary>
        /// 复制
        /// </summary>
        public RelayCommand CopyCommand
        {
            get
            {
                return _copyCommand
                    ?? (_copyCommand = new RelayCommand(
                    () =>
                    {
                        Clipboard.SetDataObject(MsgDetails);
                    }));
            }
        }



    }
}