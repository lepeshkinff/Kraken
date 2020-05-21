using System;

namespace Kraken.Engine
{
    public class VersionMismatchException : Exception
    {
        /// <inheritdoc />
        internal VersionMismatchException(int fileVersion, int toolVersion) 
            : base($"Ваш Kraken не дорос! Среди файлов с настройками есть файл с версией {fileVersion}, а у вас Kraken одолеет лишь версии не старше {toolVersion}. " +
                   $"{Environment.NewLine}" +
                   $"Обновите Kraken и приходите мериться версиями снова.")
        {
        }
    }
}