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
		static async Task Main(string[] args)
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

				await form.Init(
					new ConfigurationsProvider(
						config.ConfigurationPath,
						new HttpClientInternal(new HttpClient())));

				Application.Run(form);
			}
			catch (VersionMismatchException e)
			{
				MessageBox.Show(
					e.Message,
					"Обновите Kraken",
					MessageBoxButtons.OK,
					MessageBoxIcon.Information);
				return;
			}
			catch (Exception e)
			{
				MessageBox.Show($"Ошибка при запуске приложения {Environment.NewLine} {string.Join(Environment.NewLine, args)} {Environment.NewLine} {e}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
		}
	}
}
