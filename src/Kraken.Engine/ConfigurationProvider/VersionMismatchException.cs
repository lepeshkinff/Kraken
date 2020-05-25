using System;

namespace Kraken.Engine
{
    public class VersionMismatchException : Exception
    {
        /// <inheritdoc />
        internal VersionMismatchException(int fileVersion, int toolVersion) 
            : base($"Ваш Kraken не дорос! Среди файлов с настройками есть файл с версией {fileVersion}, а ваш Kraken способен переварить лишь файлы не старше {toolVersion}-й версии. " +
                   $"{Environment.NewLine}" +
                   $"Обновите Kraken и приходите мериться версиями снова.")
        {
        }
    }
}