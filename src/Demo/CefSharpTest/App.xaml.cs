using CefSharp;
using CefSharp.Wpf;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace CefSharpTest
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		private static readonly string[] DllList =
		{
			"api-ms-win-core-console-l1-1-0.dll", "api-ms-win-core-datetime-l1-1-0.dll",
			"api-ms-win-core-debug-l1-1-0.dll", "api-ms-win-core-errorhandling-l1-1-0.dll",
			"api-ms-win-core-file-l1-1-0.dll", "api-ms-win-core-file-l1-2-0.dll",
			"api-ms-win-core-file-l2-1-0.dll", "api-ms-win-core-handle-l1-1-0.dll",
			"api-ms-win-core-heap-l1-1-0.dll", "api-ms-win-core-interlocked-l1-1-0.dll",
			"api-ms-win-core-libraryloader-l1-1-0.dll", "api-ms-win-core-localization-l1-2-0.dll",
			"api-ms-win-core-memory-l1-1-0.dll", "api-ms-win-core-namedpipe-l1-1-0.dll",
			"api-ms-win-core-processenvironment-l1-1-0.dll", "api-ms-win-core-processthreads-l1-1-0.dll",
			"api-ms-win-core-processthreads-l1-1-1.dll", "api-ms-win-core-profile-l1-1-0.dll",
			"api-ms-win-core-rtlsupport-l1-1-0.dll", "api-ms-win-core-string-l1-1-0.dll",
			"api-ms-win-core-synch-l1-1-0.dll", "api-ms-win-core-synch-l1-2-0.dll",
			"api-ms-win-core-sysinfo-l1-1-0.dll", "api-ms-win-core-timezone-l1-1-0.dll",
			"api-ms-win-core-util-l1-1-0.dll", "api-ms-win-crt-conio-l1-1-0.dll",
			"api-ms-win-crt-convert-l1-1-0.dll", "api-ms-win-crt-environment-l1-1-0.dll",
			"api-ms-win-crt-filesystem-l1-1-0.dll", "api-ms-win-crt-heap-l1-1-0.dll",
			"api-ms-win-crt-locale-l1-1-0.dll", "api-ms-win-crt-math-l1-1-0.dll",
			"api-ms-win-crt-multibyte-l1-1-0.dll", "api-ms-win-crt-private-l1-1-0.dll",
			"api-ms-win-crt-process-l1-1-0.dll", "api-ms-win-crt-runtime-l1-1-0.dll",
			"api-ms-win-crt-stdio-l1-1-0.dll", "api-ms-win-crt-string-l1-1-0.dll",
			"api-ms-win-crt-time-l1-1-0.dll", "api-ms-win-crt-utility-l1-1-0.dll",
			"ucrtbase.dll"
		};

		private static bool _dllLoaded;

		static App()
		{
			_dllLoaded = IsVc2015Installed();
		}

		public App()
		{
			//Add Custom assembly resolver
			AppDomain.CurrentDomain.AssemblyResolve += Resolver;

			//Any CefSharp references have to be in another method with NonInlining
			// attribute so the assembly rolver has time to do it's thing.
			InitializeCefSharp();
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private static void InitializeCefSharp()
		{
			var settings = new CefSettings();

			// Set BrowserSubProcessPath based on app bitness at runtime
			settings.BrowserSubprocessPath = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase,
				Environment.Is64BitProcess ? "x64" : "x86",
				"CefSharp.BrowserSubprocess.exe");

			// Make sure you set performDependencyCheck false
			Cef.Initialize(settings, performDependencyCheck: false, browserProcessHandler: null);
		}

		// Will attempt to load missing assembly from either x86 or x64 subdir
		// Required by CefSharp to load the unmanaged dependencies when running using AnyCPU
		private static Assembly Resolver(object sender, ResolveEventArgs args)
		{
			if (args.Name.StartsWith("CefSharp"))
			{
				CheckDll();
				string assemblyName = args.Name.Split(new[] { ',' }, 2)[0] + ".dll";
				string archSpecificPath = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase,
					Environment.Is64BitProcess ? "x64" : "x86",
					assemblyName);

				return File.Exists(archSpecificPath)
					? Assembly.LoadFile(archSpecificPath)
					: null;
			}

			return null;
		}



		[DllImport("kernel32.dll")]
		private static extern IntPtr LoadLibrary(string lpFileName);

		public static bool IsVc2015Installed()
		{
			var dependenciesPath = @"SOFTWARE\Classes\Installer\Dependencies";
			var plat = Environment.Is64BitProcess ? "x64" : "x86";
			using (var dependencies = Registry.LocalMachine.OpenSubKey(dependenciesPath))
			{
				if (dependencies == null)
				{
					return false;
				}

				foreach (var subKeyName in dependencies.GetSubKeyNames().Where(n =>
					!n.ToLower().Contains("dotnet") && !n.ToLower().Contains("microsoft")))
				{
					using (var subDir = Registry.LocalMachine.OpenSubKey(dependenciesPath + "\\" + subKeyName))
					{
						var value = subDir.GetValue("DisplayName")?.ToString() ?? null;
						if (string.IsNullOrEmpty(value))
						{
							continue;
						}

						if (Regex.IsMatch(value, $@"C\+\+ 2015.*\({plat}\)")) //here u can specify your version.
						{
							return true;
						}
					}
				}
			}

			return false;
		}

		private static void CheckDll() // 检查浏览器的DLL是否载入
		{
			if (_dllLoaded)
			{
				return;
			}

			var dir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Environment.Is64BitProcess ? "x64" : "x86");
			foreach (var fname in DllList)
			{
				try
				{
					var path = Path.Combine(dir, fname);
					if (File.Exists(path))
					{
						LoadLibrary(path);
					}
				}
				catch
				{
				}
			}

			_dllLoaded = true;
		}
	}
}
