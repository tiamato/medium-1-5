using System;

namespace Rollback
{
    public class CommandController
    {
        private readonly CommandStorage _commandStorage;

        public event Action OnGetCommand;

        public CommandController(CommandStorage commandStorage)
        {
            _commandStorage = commandStorage;
        }

        public void StartConsoleInput()
        {
            var consoleView = new ConsoleView(_commandStorage, this);

            while (true)
            {
                GetCommand();
            }
        }

        private void GetCommand()
        {
            OnGetCommand?.Invoke();
        }
    }
}