using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TerminalMACS.TestDemo.Views.Tree
{
	/// <summary>
	/// menu type
	/// </summary>
	public enum CusMenuType { RemoveChild = 1 }
	/// <summary>
	/// the class of client brower view menu details info
	/// </summary>
	public class CusMenuInfo : BindableBase
	{
		public CusMenuInfo(int level, string name,string icon)
		{
			this.Level = level;
			this.Name = name;
			this.Icon = icon;
		}
		public int Level { get; set; }
		/// <summary>
		/// get or set the client type
		/// </summary>
		public string ClientType { get; set; }
		/// <summary>
		/// get or set the menu type
		/// </summary>
		public int MenuType { get; set; }
		private bool _IsEnabled;
		/// <summary>
		/// 获取或者设置菜单是否可用
		/// </summary>
		public bool IsEnabled
		{
			get { return this._IsEnabled; }
			set { this.SetProperty(ref _IsEnabled, value); }
		}

		private string _Name;
		/// <summary>
		/// get or set the menu name
		/// </summary>
		public string Name
		{
			get { return this._Name; }
			set { this.SetProperty(ref _Name, value); }
		}
		private ObservableCollection<CusMenuInfo> _Children;
		/// <summary>
		/// get or set the children
		/// </summary>
		public ObservableCollection<CusMenuInfo> Children
		{
			get { return this._Children; }
			set { this.SetProperty(ref _Children, value); }
		}

		private bool _IsSelected = false;
		/// <summary>
		/// new IsSelected
		/// </summary>
		public bool IsSelected
		{
			get { return this._IsSelected; ; }
			set
			{
				this.SetProperty(ref _IsSelected, value);
			}
		}
		public string Icon { get; set; }

		/// <summary>
		/// 获取或者父级目录
		/// </summary>
		public CusMenuInfo Parent { get; set; }
	}
}
