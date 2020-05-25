using System.Collections.Generic;

namespace Kraken.Engine
{
    /// <summary>
    /// Результат работы Configuration provider-a
    /// </summary>
    public class ConfigurationsLoadResult
    {
        public ConfigurationsLoadResult(
            IReadOnlyCollection<FileConfiguration> configurations, 
            IReadOnlyCollection<string> errors)
        {
            Configurations = configurations;
            Errors = errors;
        }

        /// <summary>
        /// Успешно загруженые конфигурации
        /// </summary>
        public IReadOnlyCollection<FileConfiguration> Configurations { get; }
        
        /// <summary>
        /// Ошибки при загрузке
        /// </summary>
        public IReadOnlyCollection<string> Errors { get; }
    }
}