using System.ComponentModel;
using System.Windows;
using Prism.Mvvm;

namespace WpfAppForZoomInAndZoomOut.Models
{
    public class TestModel : BindableBase
    {
        private int _index;

        public int Index
        {
            get => this._index;
            set => this.SetProperty(ref this._index, value);
        }

        private Thickness _margin;

        public Thickness Margin
        {
            get => this._margin;
            set => this.SetProperty(ref this._margin, value);
        }

        private string? _name;

        public string? Name
        {
            get => this._name;
            set => this.SetProperty(ref this._name, value);
        }

        public int ChildCount { get; set; }


        private int _caretIndex;

        public int CaretIndex
        {
            get => _caretIndex;
            set => this.SetProperty(ref this._caretIndex, value);
        }

        private bool _isSelected;

        public bool IsSelected
        {
            get => _isSelected;
            set => this.SetProperty(ref this._isSelected, value);
        }

        private int _selectionLength;

        public int SelectionLength
        {
            get => _selectionLength;
            set => this.SetProperty(ref this._selectionLength, value);
        }

        public override string ToString()
        {
            return $"{Index}, {Name}, {IsSelected}";
        }
    }
}