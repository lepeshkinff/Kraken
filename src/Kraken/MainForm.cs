using Kraken.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Kraken
{
	public partial class MainForm : Form
	{
		private readonly OctopusWorker octopusWorker;
		private Dictionary<string, FileConfiguration[]> fileConfigurations;

		public MainForm(
			string defaultEnvironment,
			OctopusWorker octopusWorker)
		{
			InitializeComponent();
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
			folderBrowserDialog.ShowDialog();
		}

		private async void button2_Click(object sender, EventArgs e)
		{
			try
			{
				var path = folderBrowserDialog.SelectedPath;
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

				await octopusWorker.ApplyConfigurations(folderBrowserDialog.SelectedPath, matchingCofigurtions, environment);
				MessageBox.Show("Готово!");
			}
			catch (Exception ex)
			{
				MessageBox.Show($"чёт не вышло {ex}");
			}
		}
	}
}
