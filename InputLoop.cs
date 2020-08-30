using System;

namespace Rollback
{
    public class InputLoop
    {
        private readonly CommandStorage _commandStorage;

        public event Action OnGetCommand;

        public InputLoop(CommandStorage commandStorage)
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