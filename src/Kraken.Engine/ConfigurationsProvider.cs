using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Kraken.Engine
{
   public class ConfigurationsProvider
   {
      private string configurationPath;

      public ConfigurationsProvider(string configurationPath)
      {
         this.configurationPath = configurationPath;
      }

      //любители локального iis смогут доработать этот метод так, чтобы можно было писать локальные перегрузки и не коммитить свои правила в общйи гит
      //а для меня норм и так
      public IReadOnlyCollection<FileConfiguration> GetConfigurations()
      {
         var assembly = Assembly.GetExecutingAssembly();
         using (Stream stream = assembly.GetManifestResourceStream(configurationPath))
         using (StreamReader reader = new StreamReader(stream))
         {
            return JsonConvert.DeserializeObject<List<FileConfiguration>>(reader.ReadToEnd());
         }
      }
   }
}
