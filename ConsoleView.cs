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
            if (_commandStorage.IsEmpty)
            {
                Console.WriteLine("Стек команд пуст!");
            }
            else
            {
                Console.WriteLine("Список команд в стеке:");
                foreach (var command in _commandStorage.Items)
                {
                    Console.WriteLine(command);
                }
                Console.WriteLine("-- (конец списка)");
            }
            Console.WriteLine();
        }
    }
}