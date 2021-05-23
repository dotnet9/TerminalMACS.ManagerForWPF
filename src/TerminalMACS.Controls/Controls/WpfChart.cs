using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace TerminalMACS.Controls.Controls
{
	/// <summary>
	/// 参考文章：https://www.cnblogs.com/shushukui/p/5466343.html
	/// </summary>
	public class WpfChart : Grid
	{
		public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register("ItemsSource",
			typeof(IEnumerable<KeyValuePair<string, double>>), typeof(WpfChart),
			new PropertyMetadata(new List<KeyValuePair<string, double>>(), OnItemsSourcePropertyChanged));


		public static readonly DependencyProperty IsNeedItemSourceChangeAnimationProperty = DependencyProperty.Register("IsNeedItemSourceChangeAnimation",
			typeof(bool), typeof(WpfChart),
			new PropertyMetadata(true, OnItemsSourcePropertyChanged));

		private readonly Brush _maxValueBrush =
			new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF90c68a"));

		private readonly Brush _defaultBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF22b6c4"));
		private readonly Brush _enterBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF3bcfdd"));

		private readonly Grid _gridTextH = new Grid();
		private readonly Grid _gridTextV = new Grid();
		private readonly Grid _gridContent = new Grid();

		public WpfChart()
		{
			InitializeComponent();
		}

		public IEnumerable<KeyValuePair<string, double>> ItemsSource
		{
			get { return (List<KeyValuePair<string, double>>)GetValue(ItemsSourceProperty); }
			set { SetValue(ItemsSourceProperty, value); }
		}

		public bool IsNeedItemSourceChangeAnimation
		{
			get { return (bool)GetValue(IsNeedItemSourceChangeAnimationProperty); }
			set { SetValue(IsNeedItemSourceChangeAnimationProperty, value); }
		}

		protected static void OnItemsSourcePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var chart = (WpfChart)d;
			// 只要变化就全部重新绘制

			if (chart.ItemsSource == null) return;
			if (chart.ItemsSource.Count() == 0) return;
			if (chart.ActualHeight == 0) return;
			chart.DrawHTextCollection();
			chart.DrawVTextCollection();
			chart.DrawCoordinate();
		}

		void InitializeComponent()
		{
			this.Children.Add(_gridTextH);
			this.Children.Add(_gridTextV);
			this.Children.Add(_gridContent);
			this.SizeChanged += WpfChart_SizeChanged;

			if (DesignerProperties.GetIsInDesignMode(this))
			{
				Random rand = new Random();
				List<KeyValuePair<string, double>> list = new List<KeyValuePair<string, double>>();
				for (var i = 0; i < 10; i++)
					list.Add(new KeyValuePair<string, double>(i.ToString(), rand.NextDouble() * 100));
				ItemsSource = list;
			}

		}

		private void WpfChart_SizeChanged(object sender, System.Windows.SizeChangedEventArgs e)
		{
			if (ItemsSource == null) return;
			if (ItemsSource.Count() == 0) return;
			if (this.ActualHeight == 0) return;
			DrawHTextCollection();
			DrawVTextCollection();
			DrawCoordinate();
		}

		private double _maxValue;

		int GetMaxNumber()
		{
			_maxValue = ItemsSource.Select(v => v.Value).Concat(new[] { double.MinValue }).Max();
			return (int)_maxValue;
		}

		IEnumerable<string> GetVTextCollection()
		{
			int maxNumber = GetMaxNumber(); // 获取队列里的最大值。
			int offset = 1;
			if (maxNumber > 1)
			{
				//var tempOffset = int.Parse((maxNumber.ToString()[0].ToString())) < 5 ? "5" : "10";
				//tempOffset += "".PadRight((maxNumber/10).ToString().Length - 1, '0');
				//offset = int.Parse(tempOffset);
				offset = GetReportStepValue(maxNumber);
			}
			List<string> list = new List<string>();
			this._chartMaxNumber = offset * 11;
			for (int i = 10; i >= 0; i--)
			{
				list.Add((i * offset).ToString());
			}

			return list;
		}

		int GetReportStepValue(int maxDataValue)
		{
			int maxValue = 0;
			int i = 2;
			while (maxValue < maxDataValue)
			{
				maxValue = 5 * i;
				i += 2;
			}
			return maxValue / 10;
		}

		void DrawHTextCollection()
		{
			_gridTextH.ColumnDefinitions.Clear();
			_gridTextH.RowDefinitions.Clear();
			_gridTextH.Children.Clear();
			_gridTextH.Margin = new Thickness(_viewPortPad.Left, 0, 0, 0);
			_gridTextH.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
			_gridTextH.RowDefinitions.Add(new RowDefinition()
			{
				Height = new GridLength(_viewPortPad.Bottom, GridUnitType.Pixel)
			});

			TextBlock tb;

			foreach (var v in ItemsSource)
			{
				_gridTextH.Children.Add(
					tb =
						new TextBlock()
						{
							Text = v.Key,
							HorizontalAlignment = HorizontalAlignment.Center,
							VerticalAlignment = VerticalAlignment.Top,
							Margin = new Thickness(0, 10, 0, 0)

						});
				_gridTextH.ColumnDefinitions.Add(new ColumnDefinition());
				Grid.SetRow(tb, 1);
				Grid.SetColumn(tb, _gridTextH.ColumnDefinitions.Count - 1);
			}
		}

		readonly Thickness _viewPortPad = new Thickness(50, 50, 50, 50);

		private int _chartMaxNumber = 0;

		void DrawVTextCollection()
		{
			TextBlock tb;
			Border border;
			_gridTextV.RowDefinitions.Clear();
			_gridTextV.Children.Clear();
			_gridTextV.ColumnDefinitions.Clear();
			_gridTextV.Margin = new Thickness(0, _viewPortPad.Top, 0, _viewPortPad.Bottom);
			_gridTextV.ColumnDefinitions.Add(new ColumnDefinition()
			{
				Width = new GridLength(_viewPortPad.Left, GridUnitType.Pixel)
			});
			_gridTextV.ColumnDefinitions.Add(new ColumnDefinition()
			{
				Width = new GridLength(1, GridUnitType.Star)
			});
			foreach (var v in GetVTextCollection())
			{
				_gridTextV.Children.Add(
					tb =
						new TextBlock()
						{
							Text = v.ToString(),
							Margin = new Thickness(0, 0, 5, 0),
							HorizontalAlignment = HorizontalAlignment.Right,
							VerticalAlignment = VerticalAlignment.Bottom
						});
				_gridTextV.Children.Add(
					border =
						new Border()
						{
							BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFc0c0c0")),
							BorderThickness = new Thickness(0, 0, 0, 1),
							VerticalAlignment = VerticalAlignment.Bottom
						});
				_gridTextV.RowDefinitions.Add(new RowDefinition());
				Grid.SetRow(tb, _gridTextV.RowDefinitions.Count - 1);
				Grid.SetColumn(border, 1);
				Grid.SetRow(border, _gridTextV.RowDefinitions.Count - 1);
			}
		}

		void DrawCoordinate()
		{
			Rectangle rectangle;
			TextBlock tb;
			_gridContent.ColumnDefinitions.Clear();
			_gridContent.RowDefinitions.Clear();
			_gridContent.Children.Clear();

			//_gridContent.Margin = new Thickness(_viewPortPad.Left, 0, 0, _viewPortPad.Bottom);
			_gridContent.ColumnDefinitions.Add(new ColumnDefinition()
			{
				Width = new GridLength(_viewPortPad.Left, GridUnitType.Pixel)
			});
			_gridContent.RowDefinitions.Add(new RowDefinition()
			{
				Height = new GridLength(1, GridUnitType.Star)
			});
			_gridContent.RowDefinitions.Add(new RowDefinition()
			{
				Height = new GridLength(_viewPortPad.Bottom, GridUnitType.Pixel)
			});

			var maxHeight = _gridContent.ActualHeight - _viewPortPad.Top - _viewPortPad.Bottom;
			var maxWidth = _gridContent.ActualWidth - _viewPortPad.Left;

			var list = ItemsSource;
			foreach (var v in list)
			{
				var xx = maxHeight * (v.Value / _chartMaxNumber);
				_gridContent.Children.Add(
					rectangle =
						new Rectangle()
						{
							Fill = _defaultBrush,
							Height = maxHeight * (v.Value / _chartMaxNumber),
							Width = maxWidth / list.Count() * 0.6,
							VerticalAlignment = VerticalAlignment.Bottom
						});

				var heightAnimationToValue = maxHeight * (v.Value / _chartMaxNumber);
				if (IsNeedItemSourceChangeAnimation)
				{
					var doubleAnimation = new DoubleAnimation(0, heightAnimationToValue, new Duration(new TimeSpan(0, 0, 0, 0, 1000)));

					rectangle.BeginAnimation(Rectangle.HeightProperty, doubleAnimation);
				}
				else
				{
					rectangle.Height = heightAnimationToValue;
				}

				rectangle.Tag = v.Value;

				if (v.Value == this._maxValue)
					rectangle.Fill = _maxValueBrush;
				_gridContent.Children.Add(
					tb =
						new TextBlock()
						{
							Text = v.Value.ToString("f"),
							Margin = new Thickness(0, 0, 0, maxHeight * (v.Value / _chartMaxNumber) + 5),
							VerticalAlignment = VerticalAlignment.Bottom,
							HorizontalAlignment = HorizontalAlignment.Center,

						}
					);


				var thicknessAnimationToValue = new Thickness(0, 0, 0, maxHeight * (v.Value / _chartMaxNumber) + 5);
				if (IsNeedItemSourceChangeAnimation)
				{
					var thicknessAnimation = new ThicknessAnimation(new Thickness(0, 0, 0, 0),
						thicknessAnimationToValue,
						new Duration(new TimeSpan(0, 0, 0, 0, 1000)));
					tb.BeginAnimation(TextBlock.MarginProperty, thicknessAnimation);
				}
				else
				{
					tb.Margin = thicknessAnimationToValue;
				}

				rectangle.MouseEnter += Rectangle_MouseEnter;
				rectangle.MouseLeave += Rectangle_MouseLeave;
				_gridContent.ColumnDefinitions.Add(new ColumnDefinition());
				Grid.SetColumn(rectangle, _gridContent.ColumnDefinitions.Count - 1);
				Grid.SetColumn(tb, _gridContent.ColumnDefinitions.Count - 1);
			}
		}

		private void Rectangle_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
		{
			Rectangle rectangle = sender as Rectangle;
			if (rectangle == null) return;
			if ((double)rectangle.Tag == this._maxValue)
				rectangle.Fill = _maxValueBrush;
			else
				rectangle.Fill = _defaultBrush;
		}

		private void Rectangle_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
		{
			Rectangle rectangle = sender as Rectangle;
			if (rectangle != null) rectangle.Fill = _enterBrush;
		}
	}
}
