using System.ComponentModel;

namespace WpfAppForZoomInAndZoomOut
{
    public class TestModel : INotifyPropertyChanged
    {
        public int Index { get; set; }

        public string Name { get; set; }

        private bool isSelected;

        public bool IsSelected
        {
            get => this.isSelected;
            set
            {
                isSelected = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(IsSelected)));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public override string ToString()
        {
            return $"{Index}, {Name}, {IsSelected}";
        }
    }
}
