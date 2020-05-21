using System.Collections.Generic;

namespace Kraken.Engine
{
    /// <summary>
    /// Структура файла с настройками
    /// </summary>
    public class KrakenFile : FileWithVersion
    {
        /// <summary>
        /// Конфигурации отдельных артефактов, входящие в файл
        /// </summary>
        public List<FileConfiguration> Configurations { get; set; }
    }
    
    /// <summary>
    /// Структура файла с версией
    /// </summary>
    public class FileWithVersion
    {
        /// <summary>
        /// Версия файла
        /// </summary>
        public int? Version { get; set; }
    }
}