using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using GongSolutions.Wpf.DragDrop;
using WpfAppForZoomInAndZoomOut.Models;

namespace WpfAppForZoomInAndZoomOut.Converters;

internal class DropTargetAdornersVisibilityConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        if (values.Length < 4
            || values[0] is not TreeItemModel dragSourceTreeItemModel
            || values[1] is not TreeItemModel dropTargetTreeItemModel
            || values[2] is not Grid grid
            || values[3] is not int dropTargetPosition
            || dragSourceTreeItemModel != dropTargetTreeItemModel) return Visibility.Collapsed;

        grid.Margin = new Thickness(dropTargetTreeItemModel.Margin.Left + dropTargetPosition * 5, 0, 0, 0);
        return Visibility.Visible;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}