using Kraken.Configuration;
using Kraken.Engine;
using System;
using System.Linq;
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
		static async Task Main(string[] args)
		{
			try
			{
				var config = ConfigurationLoader.Load(args);
				var configurationsProvider = new ConfigurationsProvider(
					config.ConfigurationPath,
					new HttpClientInternal(new HttpClient()));

				if (args?.Any(x => "-cmd".Equals(x, StringComparison.OrdinalIgnoreCase)) == true)
				{
					var console = new ConsoleWorker(
						configurationsProvider, 
						new OctopusWorker(new ArtifactsProvider(config.OctopusApiKey, config.OctopusEndpoint)),
						new EnvironmentsProvider(config.OctopusApiKey, config.OctopusEndpoint));
					await console.Run(args, config.Environment, config.SolutionFolder, config.SelectedComponents);
					return;
				}

				Application.EnableVisualStyles();
				Application.SetCompatibleTextRenderingDefault(false);

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
