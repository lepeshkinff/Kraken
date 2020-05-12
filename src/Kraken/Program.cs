using Kraken.Configuration;
using Kraken.Engine;
using System;
using System.Windows.Forms;

namespace Kraken
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			
			try
			{
				var config = ConfigurationLoader.Load(args);

				var form = new MainForm(
					config.SolutionFolder,
					config.Environment,
					new OctopusWorker(new ArtifactsProvider(config.OctopusApiKey, config.OctopusEndpoint)));

				form.Init(new ConfigurationsProvider(config.ConfigurationPath));

				Application.Run(form);
			}
			catch (Exception e)
			{
				MessageBox.Show($"Ошибка при запуске приложения {e}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
		}
	}
}
