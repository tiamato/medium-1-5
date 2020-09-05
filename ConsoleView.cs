using System;

namespace Rollback
{
    public class ConsoleView
    {
        private readonly CommandStorage _commandStorage;

        public ConsoleView(CommandStorage commandStorage, InputLoop inputLoop)
        {
            _commandStorage = commandStorage;
            _commandStorage.OnStorageUpdated += PrintCommands;
            _commandStorage.OnError += PrintError;

            inputLoop.OnGetCommand += InputCommand;
        }

        private static void PrintCommand(Command command)
        {
            Console.WriteLine(command.Name);
        }

        private static void PrintError(object sender, ErrorEventArgs eventArgs)
        {
            PrintError(eventArgs.Error);
        }

        private static void PrintError(string error)
        {
            Console.WriteLine(error);
            Console.WriteLine();
        }

        private void InputCommand()
        {
            Console.Write("Введите команду: ");
            var commandName = Console.ReadLine();
            Console.WriteLine();

            _commandStorage.CreateAndExecuteCommand(commandName);
        }

        private void PrintCommands()
        {
            Console.WriteLine("Список команд:");
            Console.WriteLine("-------------");
            foreach (var command in _commandStorage.Items())
            {
                PrintCommand(command);
            }
            Console.WriteLine("-------------");
            Console.WriteLine();
        }
    }
}