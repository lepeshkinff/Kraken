using Kraken.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kraken
{
	public partial class MainForm : Form
	{
		private readonly OctopusWorker octopusWorker;
		private Dictionary<string, FileConfiguration[]> fileConfigurations;

		public MainForm(
			string defaultPath,
			string defaultEnvironment,
			OctopusWorker octopusWorker)
		{
			InitializeComponent();
			selectedPathLabel.Text = defaultPath;
			this.octopusWorker = octopusWorker;
			EnvironmentTb.Text = defaultEnvironment;
		}

		public void Init(ConfigurationsProvider configurationsProvider)
		{
			var items = configurationsProvider.GetConfigurations();

			fileConfigurations = items
				.GroupBy(x => x.ComponentName)
				.ToDictionary(x => x.Key, v => v.ToArray());

			configurationsList.Items.Add("All");
			configurationsList.SelectedIndex = 0;
			foreach (var item in fileConfigurations.Keys)
			{
				configurationsList.Items.Add(item);
			}
			fileConfigurations["All"] = items.ToArray();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			using (var fbd = new FolderBrowserDialog
			{
				ShowNewFolderButton = false,
				Description = "Укажите папку солюшена, файлы коныигураций которого надо подменить",
			})
			{
				if (!string.IsNullOrWhiteSpace(selectedPathLabel.Text))
				{
					fbd.SelectedPath = selectedPathLabel.Text;
					SendKeys.Send("{TAB}{TAB}{RIGHT}");
				}

				var result = fbd.ShowDialog();
				if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
				{
					selectedPathLabel.Text = fbd.SelectedPath;
				}
			}
		}

		private async void button2_Click(object sender, EventArgs e)
		{
			var cts = new CancellationTokenSource();
			try
			{
				Progress(cts.Token);
				if (string.IsNullOrWhiteSpace(selectedPathLabel.Text))
				{
					MessageBox.Show("А шо ж вы папку солюшена, то не выбрали?");
					return;
				}

				var environment = EnvironmentTb.Text;
				if (string.IsNullOrWhiteSpace(environment))
				{
					MessageBox.Show("В аргументах отсутствует имя среды, для которой нужно получить" +
						"конфигурационные файлы. Введите имя octopus среды (CaseSensetive)");
					return;
				}

				if (configurationsList.SelectedIndex < 0)
				{
					MessageBox.Show("Э! выбрать конфиг надобно");
					return;
				}
				var item = configurationsList.SelectedItem as string;

				var matchingCofigurtions = fileConfigurations[item];
				await octopusWorker.ApplyConfigurations(selectedPathLabel.Text, matchingCofigurtions, environment);
				MessageBox.Show("Готово!");
			}
			catch (Exception ex)
			{
				MessageBox.Show($"чёт не вышло {ex}");
			}
			finally
			{
				cts.Cancel();
			}
		}

		private async void Progress(CancellationToken cancellationToken)
		{
			hideAllPanel.Visible = true;

			var i = 0;
			while (!cancellationToken.IsCancellationRequested)
			{
				await Task.Delay(200);
				progressBar.Value = i;
				if (i == 100)
				{
					i = 0;
				}
				i++;
			}

			hideAllPanel.Visible = false;
		}
	}
}
