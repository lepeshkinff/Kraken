﻿using Kraken.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kraken
{
	public partial class MainForm : Form
	{
		private readonly string defaultEnvironment;
		private readonly ConfigurationsProvider configurationsProvider;
		private readonly OctopusWorker octopusWorker;
		private readonly EnvironmentsProvider environmentsProvider;
		private Dictionary<string, FileConfiguration[]> fileConfigurations;

		public MainForm(
			string defaultPath,
			string defaultEnvironment,
			ConfigurationsProvider configurationsProvider,
			OctopusWorker octopusWorker,
			EnvironmentsProvider environmentsProvider)
		{
			InitializeComponent();
			selectedPathTb.Text = defaultPath;
			this.defaultEnvironment = string.IsNullOrWhiteSpace(defaultEnvironment) 
                ? Properties.Settings.Default.LastEnvironment ?? ""
                : defaultEnvironment;

			this.configurationsProvider = configurationsProvider;
			this.octopusWorker = octopusWorker;
			this.environmentsProvider = environmentsProvider;
		}

		private async Task InitConfigurationsList()
		{
			var configurationsLoadResult = await configurationsProvider.GetConfigurations();

			if (configurationsLoadResult.Errors.Any())
			{
				var errors = string.Join(Environment.NewLine, configurationsLoadResult.Errors.Distinct());
				MessageBox.Show(
					$"При загрузке конфигураций возникли следующие ошибки: {Environment.NewLine}" +
					$"{Environment.NewLine}{errors}{Environment.NewLine}" +
					$"{Environment.NewLine}Часть конфигураций может быть недоступна",
					"Ошибки при загрузке",
					MessageBoxButtons.OK,
					MessageBoxIcon.Information);
			}

			fileConfigurations = configurationsLoadResult
				.Configurations
				.GroupBy(x => x.ComponentName)
				.ToDictionary(x => x.Key, v => v.ToArray());

			configurationsList.Items.Add("All");
			configurationsList.SelectedIndex = 0;
			foreach (var item in fileConfigurations.Keys)
			{
				configurationsList.Items.Add(item);
			}

			fileConfigurations["All"] = configurationsLoadResult.Configurations.ToArray();
		}

		private async Task InitEnvironmentsList()
		{
			var environments = await Task.Run(async () => await environmentsProvider.GetEnvironments());
			EnvironmentCmb.Items.AddRange(environments);
			if (EnvironmentCmb.Items.Contains(defaultEnvironment))
			{
				EnvironmentCmb.SelectedItem = defaultEnvironment;
			}
        }

		private void button1_Click(object sender, EventArgs e)
		{
			using (var fbd = new FolderBrowserDialog
			{
				ShowNewFolderButton = false,
				Description = "Укажите папку солюшена, файлы коныигураций которого надо подменить",
			})
			{
				if (!string.IsNullOrWhiteSpace(selectedPathTb.Text))
				{
					fbd.SelectedPath = selectedPathTb.Text;
					SendKeys.Send("{TAB}{TAB}{RIGHT}");
				}

				var result = fbd.ShowDialog();
				if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
				{
					selectedPathTb.Text = fbd.SelectedPath;
				}
			}
		}

		private async void button2_Click(object sender, EventArgs e) =>
			await ApplyInternal(octopusWorker.ApplyConfigurations);

		private async void button3_Click(object sender, EventArgs e) =>
			await ApplyInternal(octopusWorker.ApplyVariables);

		private async Task ApplyInternal(Func<string, FileConfiguration[], string, Task> applyAction)
		{
			hideAllPanel.Visible = true;
            UpdateLastSelectedEnvironment(EnvironmentCmb.Text);

			try
			{
				if (string.IsNullOrWhiteSpace(selectedPathTb.Text))
				{
					MessageBox.Show("А шо ж вы папку солюшена, то не выбрали?");
					return;
				}

				var environment = EnvironmentCmb.Text;
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
				await applyAction(selectedPathTb.Text, matchingCofigurtions, environment);
				var result = MessageBox.Show($"Готово!{Environment.NewLine}Если надо ещё что-то поменять, нажминет Ok иначе -- Cancel", "Рэзультат", MessageBoxButtons.OKCancel);
				if(result == DialogResult.Cancel)
				{
					Application.Exit();
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show($"чёт не вышло {ex}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			finally
			{
				hideAllPanel.Visible = false;
			}
		}

        private void UpdateLastSelectedEnvironment(string environment)
        {
            if (Properties.Settings.Default.LastEnvironment != environment)
            {
                Properties.Settings.Default.LastEnvironment = environment;
                Properties.Settings.Default.Save();
            }
        }

		private async void MainForm_Load(object sender, EventArgs e)
        {
            hideAllPanel.Visible = true;
			await InitConfigurationsList();
			await InitEnvironmentsList();
            hideAllPanel.Visible = false;
		}
    }
}
