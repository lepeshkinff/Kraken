using Octopus.Client;
using Octopus.Client.Model;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Kraken.Engine
{
   /// <summary>
   /// Получает конфигурационные файлы и прочие артефакты из октопус
   /// </summary>
   public class ArtifactsProvider
   {
      private readonly string octopusEndpoint;
      private readonly string octopusApiKey;

      public ArtifactsProvider(string octopusApiKey, string octopusEndpoint)
      {
         this.octopusEndpoint = octopusEndpoint;
         this.octopusApiKey = octopusApiKey;
      }
      
      public async Task<Dictionary<string, string>> Get(string environmentName, string projectName, params string[] artifactNames)
      {
         var endpoint = new OctopusServerEndpoint(octopusEndpoint, octopusApiKey);
         var client = await OctopusAsyncClient.Create(endpoint);
         OctopusAsyncRepository repository = new OctopusAsyncRepository(client);

         var artifacts = await GetArtifactsList(repository, environmentName, projectName);

         var artifactsByName = new Dictionary<string, string>();
         foreach (var artifactName in artifactNames)
         {
            var matchingArtifact = artifacts.Items.Single(a => a.Filename == artifactName);
            if (matchingArtifact == null)
            {
               throw new ArtifactsProviderException(
                   $"У деплоя проекта {projectName} на среду {environmentName} отсутствует артефакт {artifactName}. " +
                   "Проверьте свежесть и полноту деплоя.");
            }
            using (var stream = await repository.Artifacts.GetContent(matchingArtifact))
            using (var streamReader = new StreamReader(stream))
            {
               artifactsByName[artifactName] = await streamReader.ReadToEndAsync();
            }
         }

         return artifactsByName;
      }

      private static async Task<ResourceCollection<ArtifactResource>> GetArtifactsList(
          OctopusAsyncRepository repository,
          string environmentName,
          string projectName)
      {
         var environment = await repository.Environments.FindOne(x => x.Name.Contains(environmentName));
         if (environment == null)
         {
            throw new ArtifactsProviderException($"Среда {environmentName} не найдена. Проверьте написание среды (case sensetive)");
         }

         var project = await repository.Projects.FindOne(x => x.Name.Contains(projectName));
         if (project == null)
         {
            throw new ArtifactsProviderException($"Проект {projectName} не найден в октопусе. Проверьте конфигурацию");
         }

         var deployments = await repository.Deployments.FindBy(new[] { project.Id }, new[] { environment.Id }, take: 1);
         var deployment = deployments.Items.SingleOrDefault();
         if (deployment == null)
         {
            throw new ArtifactsProviderException($"Не удалось найти деплой проекта {projectName} на среду {environmentName}");
         }

         return await repository.Artifacts.FindRegarding(new DeploymentResource { Id = deployment.Id });
      }
   }
}
