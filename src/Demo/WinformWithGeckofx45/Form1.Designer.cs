
namespace WinformWithGeckofx45
{
	partial class Form1
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.geckoWebBrowser = new Gecko.GeckoWebBrowser();
			this.SuspendLayout();
			// 
			// geckoWebBrowser
			// 
			this.geckoWebBrowser.ConsoleMessageEventReceivesConsoleLogCalls = true;
			this.geckoWebBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
			this.geckoWebBrowser.FrameEventsPropagateToMainWindow = false;
			this.geckoWebBrowser.Location = new System.Drawing.Point(0, 0);
			this.geckoWebBrowser.Name = "geckoWebBrowser";
			this.geckoWebBrowser.Size = new System.Drawing.Size(800, 450);
			this.geckoWebBrowser.TabIndex = 0;
			this.geckoWebBrowser.UseHttpActivityObserver = false;
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.MenuHighlight;
			this.ClientSize = new System.Drawing.Size(800, 450);
			this.Controls.Add(this.geckoWebBrowser);
			this.Name = "Form1";
			this.Text = "Form1";
			this.ResumeLayout(false);

		}

		#endregion

		private Gecko.GeckoWebBrowser geckoWebBrowser;
	}
}

