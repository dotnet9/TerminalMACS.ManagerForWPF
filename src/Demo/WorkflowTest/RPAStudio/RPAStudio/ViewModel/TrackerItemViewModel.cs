using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using RPA.Shared.Utils;
using RPAStudio.Views;

namespace RPAStudio.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class TrackerItemViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the TrackerItemViewModel class.
        /// </summary>
        public TrackerItemViewModel()
        {
        }


        /// <summary>
        /// The <see cref="Property" /> property's name.
        /// </summary>
        public const string PropertyPropertyName = "Property";

        private string _propertyProperty = "";

        /// <summary>
        /// Sets and gets the Property property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string Property
        {
            get
            {
                return _propertyProperty;
            }

            set
            {
                if (_propertyProperty == value)
                {
                    return;
                }

                _propertyProperty = value;
                RaisePropertyChanged(PropertyPropertyName);
            }
        }


        /// <summary>
        /// The <see cref="Value" /> property's name.
        /// </summary>
        public const string ValuePropertyName = "Value";

        private string _valueProperty = "";

        /// <summary>
        /// Sets and gets the Value property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string Value
        {
            get
            {
                return _valueProperty;
            }

            set
            {
                if (_valueProperty == value)
                {
                    return;
                }

                _valueProperty = value;
                RaisePropertyChanged(ValuePropertyName);
            }
        }

        private RelayCommand _viewMoreCommand;

        /// <summary>
        /// Gets the ViewMoreCommand.
        /// </summary>
        public RelayCommand ViewMoreCommand
        {
            get
            {
                return _viewMoreCommand
                    ?? (_viewMoreCommand = new RelayCommand(
                    () =>
                    {
                        var window = new MessageDetailsWindow();
                        window.Topmost = true;

                        var vm = window.DataContext as MessageDetailsViewModel;
                        vm.WindowTitle = "属性值信息";
                        vm.MsgDetails = Value;

                        CommonWindow.ShowDialog(window);
                    }));
            }
        }



    }
}