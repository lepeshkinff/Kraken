using Kraken.Engine;
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

            var rootNode = configurationTree.Nodes.Add("All");
			foreach (var itemGroup in fileConfigurations)
			{
                var componentNode = rootNode.Nodes.Add(itemGroup.Key);
                foreach (var item in itemGroup.Value)
                {
	                var leaf = componentNode.Nodes.Add(item.PathToFile);
	                leaf.Tag = item;
                }
			}
			rootNode.Expand();
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

				var matchingCofigurtions = GetSelectedConfigurations(configurationTree.Nodes[0]).ToArray();
                if (!matchingCofigurtions.Any())
                {
	                MessageBox.Show("Э! выбрать конфиг надобно");
	                return;
                }
                
				await applyAction(selectedPathTb.Text, matchingCofigurtions , environment);
				MessageBox.Show($"Готово!", "Рэзультат", MessageBoxButtons.OK, MessageBoxIcon.Information);
				Application.Exit();
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

		private IEnumerable<FileConfiguration> GetSelectedConfigurations(TreeNode rootNode)
		{
			foreach (TreeNode node in rootNode.Nodes)
			{
				if (node.Checked)
				{
					if (node.Tag != null)
					{
						yield return (FileConfiguration) node.Tag;
					}
				}
				
				foreach (var childConfiguration in GetSelectedConfigurations(node))
				{
					yield return childConfiguration;
				}
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

		private void configurationTree_AfterCheck(object sender, TreeViewEventArgs e)
        {
	        if (e.Action == TreeViewAction.Unknown)
	        {
		        //это сделала сама программа в рекурсивном методе ниже. Игнорируем.
		        return;
	        }
	        SeCheckedRecursively(e.Node, e.Node.Checked);
        }

        private void SeCheckedRecursively(TreeNode node, bool shouldBeChecked)
        {
            node.Checked = shouldBeChecked;
			foreach (TreeNode childNode in node.Nodes)
            {
                SeCheckedRecursively(childNode, shouldBeChecked);
            }
		}
    }
}
