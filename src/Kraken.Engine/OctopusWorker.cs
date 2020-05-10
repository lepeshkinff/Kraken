using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Kraken.Engine
{
	public class OctopusWorker
	{
		private readonly ArtifactsProvider artifactsProvider;

		public OctopusWorker(ArtifactsProvider artifactsProvider)
		{
			this.artifactsProvider = artifactsProvider;
		}

		public async Task ApplyConfigurations(string pathToSolutionRoot, IEnumerable<FileConfiguration> matchingCofigurtions, string environment)
		{
			foreach (var projectConfigurations in matchingCofigurtions.GroupBy(x => x.OctopusProject))
			{
				var artifacts = await artifactsProvider.Get(environment, projectConfigurations.Key,
					 projectConfigurations.Select(x => x.OctopusArtifactName).ToArray());
				foreach (var fileConfiguration in projectConfigurations)
				{
					File.WriteAllText(
						 Path.Combine(pathToSolutionRoot, fileConfiguration.PathToFile),
						 ApplySubstitutions(artifacts[fileConfiguration.OctopusArtifactName], fileConfiguration.Substitutions));
				}
			}
		}

		private static string ApplySubstitutions(string artifactText, Dictionary<string, string> substitutions)
		{
			foreach (var substitution in substitutions)
			{
				artifactText = Regex.Replace(artifactText, substitution.Key, substitution.Value);
			}

			return artifactText;
		}
	}
}
