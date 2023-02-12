using GalaSoft.MvvmLight;

namespace RPAStudio.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class ActivityGroupItemViewModel : ActivityBaseItemViewModel
    {
        /// <summary>
        /// Initializes a new instance of the ActivityGroupItemViewModel class.
        /// </summary>
        public ActivityGroupItemViewModel()
        {
        }


        /// <summary>
        /// The <see cref="Icon" /> property's name.
        /// </summary>
        public const string IconPropertyName = "Icon";

        private string _iconProperty = null;

        /// <summary>
        /// Sets and gets the Icon property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string Icon
        {
            get
            {
                return _iconProperty;
            }

            set
            {
                if (_iconProperty == value)
                {
                    return;
                }

                _iconProperty = value;
                RaisePropertyChanged(IconPropertyName);
            }
        }



    }
}