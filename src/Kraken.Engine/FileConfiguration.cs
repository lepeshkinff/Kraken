using System;
using System.Collections.Generic;

namespace Kraken.Engine
{
    /// <summary>
    /// Настройки для загрузки и последующих модификаций файла.
    /// </summary>
    public class FileConfiguration
    {
        /// <summary>
        /// ID для перегрузки правил
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Имя компонента
        /// </summary>
        public string ComponentName { get; set; }

        /// <summary>
        /// Путь к файлу относительно solution root
        /// </summary>
        public string PathToFile { get; set; }

        /// <summary>
        /// Название проекта в Octopus из которого надо взять артефакт
        /// </summary>
        public string OctopusProject { get; set; }

        /// <summary>
        /// Имя артефакта в октопусе
        /// </summary>
        public string OctopusArtifactName { get; set; }

        /// <summary>
        /// Набор подстановок, которые надо применить после загрузки артефакта.
        /// key - regexp выражение для поиска, value - на что заменить
        /// </summary>
        public Dictionary<string, string> Substitutions { get; set; }

        /// <summary>
        /// Если нужны только локальные замены
        /// </summary>
        public bool IsSubstitutionsOnly { get; set; }

        public override string ToString() => ComponentName;
    }
}
