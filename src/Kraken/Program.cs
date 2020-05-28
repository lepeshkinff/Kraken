using Kraken.Configuration;
using Kraken.Engine;
using System;
using System.Net.Http;
using System.Threading.Tasks;
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
				var configurationsProvider = new ConfigurationsProvider(
					config.ConfigurationPath,
					new HttpClientInternal(new HttpClient()));

				var form = new MainForm(
					config.SolutionFolder,
					config.Environment,
					configurationsProvider,
					new OctopusWorker(new ArtifactsProvider(config.OctopusApiKey, config.OctopusEndpoint)),
					new EnvironmentsProvider(config.OctopusApiKey, config.OctopusEndpoint));

				Application.Run(form);
			}
			catch (Exception e)
			{
				MessageBox.Show($"Ошибка при запуске приложения {Environment.NewLine} {string.Join(Environment.NewLine, args)} {Environment.NewLine} {e}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
		}
	}
}
