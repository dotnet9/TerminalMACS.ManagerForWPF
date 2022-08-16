using System;
using System.Windows;
using System.Windows.Controls;

namespace WpfAppForZoomInAndZoomOut.Helper
{
    public static class TextBoxAssist
    {
        // This strange default value is on purpose it makes the initialization problem very unlikely.
        // If the default value matches the default value of the property in the ViewModel,
        // the propertyChangedCallback of the FrameworkPropertyMetadata is initially not called
        // and if the property in the ViewModel is not changed it will never be called.
        private const int CaretIndexPropertyDefault = -485609317;

        public static readonly DependencyProperty CaretIndexProperty =
            DependencyProperty.RegisterAttached(
                "CaretIndex",
                typeof(int),
                typeof(TextBoxAssist),
                new FrameworkPropertyMetadata(
                    CaretIndexPropertyDefault,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    CaretIndexChanged));

        public static readonly DependencyProperty SelectionLengthProperty =
            DependencyProperty.RegisterAttached(
                "SelectionLength",
                typeof(int),
                typeof(TextBoxAssist),
                new FrameworkPropertyMetadata(
                    CaretIndexPropertyDefault,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    SelectionLengthChanged));

        public static void SetCaretIndex(DependencyObject dependencyObject, int i)
        {
            dependencyObject.SetValue(CaretIndexProperty, i);
        }

        public static int GetCaretIndex(DependencyObject dependencyObject)
        {
            return (int)dependencyObject.GetValue(CaretIndexProperty);
        }

        public static void SetSelectionLength(DependencyObject dependencyObject, int i)
        {
            dependencyObject.SetValue(SelectionLengthProperty, i);
        }

        public static bool GetSelectionLength(DependencyObject dependencyObject)
        {
            return (bool)dependencyObject.GetValue(SelectionLengthProperty);
        }

        private static void CaretIndexChanged(DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs eventArgs)
        {
            if (dependencyObject is not TextBox textBox || eventArgs.OldValue is not int oldValue ||
                eventArgs.NewValue is not int newValue)
            {
                return;
            }

            if (oldValue == CaretIndexPropertyDefault && newValue != CaretIndexPropertyDefault)
            {
                textBox.SelectionChanged += SelectionChangedForCaretIndex;
            }
            else if (oldValue != CaretIndexPropertyDefault && newValue == CaretIndexPropertyDefault)
            {
                textBox.SelectionChanged -= SelectionChangedForCaretIndex;
            }

            if (newValue != textBox.CaretIndex)
            {
                textBox.CaretIndex = newValue < 0 ? 0 : newValue;
            }
        }

        private static void SelectionChangedForCaretIndex(object sender, RoutedEventArgs eventArgs)
        {
            if (sender is TextBox textBox)
            {
                SetCaretIndex(textBox, textBox.CaretIndex);
            }
        }
        private static void SelectionChangedForSelectionLength(object sender, RoutedEventArgs eventArgs)
        {
            if (sender is TextBox textBox)
            {
                SetSelectionLength(textBox, textBox.SelectionLength);
            }
        }

        private static void SelectionLengthChanged(DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs eventArgs)
        {
            if (dependencyObject is not TextBox textBox || eventArgs.OldValue is not int oldValue ||
                eventArgs.NewValue is not int newValue)
            {
                return;
            }

            if (oldValue == CaretIndexPropertyDefault && newValue != CaretIndexPropertyDefault)
            {
                textBox.SelectionChanged += SelectionChangedForSelectionLength;
            }
            else if (oldValue != CaretIndexPropertyDefault && newValue == CaretIndexPropertyDefault)
            {
                textBox.SelectionChanged -= SelectionChangedForSelectionLength;
            }

            if (newValue != textBox.SelectionLength)
            {
                textBox.SelectionLength = newValue < 0 ? 0 : newValue;
            }
        }
    }
}