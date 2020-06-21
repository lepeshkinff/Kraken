using Kraken.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Kraken
{
    internal class ConsoleWorker
    {
        [DllImport("kernel32.dll")]
        static extern bool AttachConsole(int dwProcessId);
        private const int ATTACH_PARENT_PROCESS = -1;

        private readonly ConfigurationsProvider configurationsProvider;
        private readonly OctopusWorker octopusWorker;

        public ConsoleWorker(
            ConfigurationsProvider configurationsProvider,
            OctopusWorker octopusWorker,
            EnvironmentsProvider environmentsProvider)
        {

            this.configurationsProvider = configurationsProvider;
            this.octopusWorker = octopusWorker;
            this.environmentsProvider = environmentsProvider;
        }

        public async Task Run(string[] args, string environment, string solutionPath, IEnumerable<string> components)
        {
            AttachConsole(ATTACH_PARENT_PROCESS);

            var (env, solution, configs, success) = await PrepareParams(environment, solutionPath, components);
            if(!success)
            {
                Console.WriteLine("Hmm, ok. Goodbye!");
                return;
            }

            var processTask = args.Any(x => "-ART".Equals(x, StringComparison.OrdinalIgnoreCase))
                ? octopusWorker.ApplyConfigurations(solution, configs, env)
                : octopusWorker.ApplyVariables(solution, configs, env);

            Console.WriteLine("Processing");
            Console.WriteLine();
            bool exit = false;
            await Task.WhenAny(
                processTask.ContinueWith(t =>
                {
                    Console.WriteLine(t.Exception.Flatten());
                }, TaskContinuationOptions.OnlyOnFaulted),
                Task.Run(async () =>
                {
                    var loader = "|/-\\";
                    var i = 0;
                    do
                    {
                        Console.SetCursorPosition(0, Console.CursorTop);
                        Console.Write(loader[i]);
                        i++;
                        if(i == 4)
                        {
                            i = 0;
                        }
                        await Task.Delay(100);
                    } while (!exit);
                }));
            exit = true;

            Console.SetCursorPosition(0, Console.CursorTop);
            Console.WriteLine("done");
        }

        private async Task<(string environment, string solutionPath, FileConfiguration[] configurations, bool success)> 
            PrepareParams(string environment, string solutionPath, IEnumerable<string> components)
        {
            if (string.IsNullOrEmpty(environment) &&
                !ReadConsole("Environment", out environment))
            {
                return (null, null, null, false);
            }

            if (string.IsNullOrEmpty(solutionPath) &&
                !ReadConsole("Solution Path", out solutionPath))
            {
                return (null, null, null, false);
            }

            if (!components.Any())
            {
                if (!ReadConsole("Component", out var component))
                {
                    return (null, null, null, false);
                }
                components = new[] { component };
            }

            var configurationsLoadResult = await configurationsProvider.GetConfigurations();

            if (configurationsLoadResult.Errors.Any())
            {
                var errors = string.Join(Environment.NewLine, configurationsLoadResult.Errors.Distinct());
                Console.WriteLine(
                    $"При загрузке конфигураций возникли следующие ошибки: {Environment.NewLine}" +
                    $"{Environment.NewLine}{errors}{Environment.NewLine}" +
                    $"{Environment.NewLine}Часть конфигураций может быть недоступна");
                return (null, null, null, false);
            }

            var configurations = configurationsLoadResult
                .Configurations
                .Where(x => components.Any(y => y.Equals(x.ComponentName, StringComparison.OrdinalIgnoreCase)))
                .ToArray();

            if (configurations.Length == 0)
            {
                Console.WriteLine("Указанный компонент не найден");
                return (null, null, null, false);
            }

            return (environment, solutionPath, configurations, true);
        }

        private static bool ReadConsole(string name, out string value)
        {
            Console.WriteLine($"{name}:");
            value = Console.ReadLine();
            if ("quit".Equals(value, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }
            return true;
        }
    }
}
