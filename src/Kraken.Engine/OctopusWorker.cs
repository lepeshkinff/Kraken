using System;
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
                var artifacts = await artifactsProvider.GetArtifacts(
                    environment, 
                    projectConfigurations.Key,
                    projectConfigurations
                        .Where(x => !x.IsSubstitutionsOnly)
                        .Select(x => x.OctopusArtifactName)
                        .ToArray());

                foreach (var fileConfiguration in projectConfigurations)
                {
                    var file = Path.Combine(pathToSolutionRoot, fileConfiguration.PathToFile);

                    string text = !fileConfiguration.IsSubstitutionsOnly
                        ? ApplySubstitutions(artifacts[fileConfiguration.OctopusArtifactName], fileConfiguration.Substitutions)
                        : ApplySubstitutions(File.ReadAllText(file), fileConfiguration.Substitutions ?? new Dictionary<string, string>());

                    File.WriteAllText(file, text);
                }
            }
        }

        public async Task ApplyVariables(string pathToSolutionRoot, IEnumerable<FileConfiguration> matchingCofigurtions, string environment)
        {
            foreach (var projectConfigurations in matchingCofigurtions.GroupBy(x => x.OctopusProject))
            {
                var vars = await artifactsProvider.GetVariables(environment, projectConfigurations.Key);
                foreach (var fileConfiguration in projectConfigurations)
                {
                    var file = Path.Combine(pathToSolutionRoot, fileConfiguration.PathToFile);
                    var text = File.ReadAllText(file);

                    if (!fileConfiguration.IsSubstitutionsOnly)
                    {
                        text = ApplyVariables(vars, text);
                    }
                    text = ApplySubstitutions(text, fileConfiguration.Substitutions);

                    File.WriteAllText(file, text);
                }
            }
        }

        private static string ApplyVariables(IDictionary<string, string> vars, string text)
        {
            foreach (var pair in vars)
            {
                var match = Regex.Match(text, $"key=\"{pair.Key}\"[ ]*value=\"(.*)\"", RegexOptions.IgnoreCase);
                if (match.Success)
                {
                    ReplaceText(ref text, match, match.Groups[1].Value, pair.Value);
                    continue;
                }

                match = Regex.Match(text, $"name=\"{pair.Key}\"(.*)connectionString=\"(.*)\"", RegexOptions.IgnoreCase);
                if (match.Success)
                {
                    ReplaceText(ref text, match, match.Groups[2].Value, pair.Value);
                    continue;
                }

                match = Regex.Match(text, $"\"{pair.Key}\"[ ]*:[ ]*((\\d+|true|false)|(\"(.*)\"))", RegexOptions.IgnoreCase);
                if (match.Success)
                {
                    var replace = string.IsNullOrEmpty(match.Groups[2].Value)
                        ? match.Groups[4].Value
                        : match.Groups[2].Value;
                    var value = pair.Value?
                        .Replace("\\", "\\\\")
                        .Replace("\"", "\\\"");
                    ReplaceText(ref text, match, replace, value);
                    continue;
                }
            }

            return text;
        }

        private static void ReplaceText(ref string text, Match match, string replace, string value)
        {
            if (string.IsNullOrEmpty(replace))
            {
                replace = "\"\"";
                value = $"\"{value}\"";
            }
            text = text.Replace(match.Value, match.Value.Replace(replace, value));
        }

        private static string ApplySubstitutions(string artifactText, Dictionary<string, string> substitutions)
        {
            foreach (var substitution in substitutions)
            {
                artifactText = Regex.Replace(artifactText, substitution.Key, substitution.Value, RegexOptions.IgnoreCase);
            }

            return artifactText;
        }
    }
}
