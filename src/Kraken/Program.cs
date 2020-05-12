using Kraken.Engine;
using System;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace Kraken
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			if (!GetConfigValue("octopusEndpoint", out var octopusEndpoint) ||
				!GetConfigValue("octopusApiKey", out var octopusApiKey))
			{
				return;
			}
			var configurationPath = ConfigurationManager.AppSettings["configurationPath"] ?? 
				Path.Combine(Assembly.GetExecutingAssembly().Location, "configuration.json");
			var environment = Environment.GetEnvironmentVariable("OctopusEnvironment") ?? ConfigurationManager.AppSettings["octopusEnvironment"];
			var folder = ConfigurationManager.AppSettings["solutionsFoldeer"];

			var form = new MainForm(
				folder,
				environment,
				new OctopusWorker(
					new ArtifactsProvider(octopusApiKey, octopusEndpoint)));
			try
			{
				form.Init(new ConfigurationsProvider(configurationPath));
			}
			catch (Exception e)
			{
				MessageBox.Show($"Ошибка при запуске приложения {e}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			
			Application.Run(form);
		}

		private static bool GetConfigValue(string name, out string value)
		{
			value = ConfigurationManager.AppSettings[name];

			if (string.IsNullOrEmpty(value))
			{
				MessageBox.Show($"Таки не хватает важной настнойки {name}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return false;
			}

			return true;
		}
	}
}
