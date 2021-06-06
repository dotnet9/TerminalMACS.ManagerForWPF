using Microsoft.Office.Tools.Ribbon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace VSTOPowerPointPluginTest
{
	public partial class Ribbon1
	{
		private void Ribbon1_Load(object sender, RibbonUIEventArgs e)
		{

		}

		private void button1_Click(object sender, RibbonControlEventArgs e)
		{
			MessageBox.Show("Test");
		}
	}
}
