
namespace VSTOPowerPointPluginTest
{
	partial class Ribbon1 : Microsoft.Office.Tools.Ribbon.RibbonBase
	{
		/// <summary>
		/// 必需的设计器变量。
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		public Ribbon1()
			: base(Globals.Factory.GetRibbonFactory())
		{
			InitializeComponent();
		}

		/// <summary> 
		/// 清理所有正在使用的资源。
		/// </summary>
		/// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region 组件设计器生成的代码

		/// <summary>
		/// 设计器支持所需的方法 - 不要修改
		/// 使用代码编辑器修改此方法的内容。
		/// </summary>
		private void InitializeComponent()
		{
			Microsoft.Office.Tools.Ribbon.RibbonDialogLauncher ribbonDialogLauncherImpl1 = this.Factory.CreateRibbonDialogLauncher();
			this.tab1 = this.Factory.CreateRibbonTab();
			this.group1 = this.Factory.CreateRibbonGroup();
			this.tab2 = this.Factory.CreateRibbonTab();
			this.group2 = this.Factory.CreateRibbonGroup();
			this.button1 = this.Factory.CreateRibbonButton();
			this.tab1.SuspendLayout();
			this.tab2.SuspendLayout();
			this.group2.SuspendLayout();
			this.SuspendLayout();
			// 
			// tab1
			// 
			this.tab1.ControlId.ControlIdType = Microsoft.Office.Tools.Ribbon.RibbonControlIdType.Office;
			this.tab1.Groups.Add(this.group1);
			this.tab1.Label = "TabAddIns";
			this.tab1.Name = "tab1";
			// 
			// group1
			// 
			this.group1.Label = "group1";
			this.group1.Name = "group1";
			// 
			// tab2
			// 
			this.tab2.Groups.Add(this.group2);
			this.tab2.Label = "Test tab";
			this.tab2.Name = "tab2";
			// 
			// group2
			// 
			this.group2.DialogLauncher = ribbonDialogLauncherImpl1;
			this.group2.Items.Add(this.button1);
			this.group2.Label = "Test Group";
			this.group2.Name = "group2";
			// 
			// button1
			// 
			this.button1.Label = "Show Window";
			this.button1.Name = "button1";
			this.button1.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.button1_Click);
			// 
			// Ribbon1
			// 
			this.Name = "Ribbon1";
			this.RibbonType = "Microsoft.PowerPoint.Presentation";
			this.Tabs.Add(this.tab1);
			this.Tabs.Add(this.tab2);
			this.Load += new Microsoft.Office.Tools.Ribbon.RibbonUIEventHandler(this.Ribbon1_Load);
			this.tab1.ResumeLayout(false);
			this.tab1.PerformLayout();
			this.tab2.ResumeLayout(false);
			this.tab2.PerformLayout();
			this.group2.ResumeLayout(false);
			this.group2.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		internal Microsoft.Office.Tools.Ribbon.RibbonTab tab1;
		internal Microsoft.Office.Tools.Ribbon.RibbonGroup group1;
		private Microsoft.Office.Tools.Ribbon.RibbonTab tab2;
		internal Microsoft.Office.Tools.Ribbon.RibbonGroup group2;
		internal Microsoft.Office.Tools.Ribbon.RibbonButton button1;
	}

	partial class ThisRibbonCollection
	{
		internal Ribbon1 Ribbon1
		{
			get { return this.GetRibbon<Ribbon1>(); }
		}
	}
}
