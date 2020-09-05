using System;
using System.Collections.Generic;

namespace Rollback
{
    public class CommandStorage
    {
        private readonly Stack<Command> _items = new Stack<Command>();

        public bool IsEmpty => _items.Count == 0;

        public event Action OnStorageUpdated;
        public event EventHandler<ErrorEventArgs> OnError;

        public IEnumerable<Command> Items() => _items;

        public void CreateAndExecuteCommand(string commandName)
        {
            try
            {
                if (commandName != null)
                {
                    var command = Command.CreateInstance(commandName);
                    command.Execute(_items);
                }
            }
            catch (ArgumentException exception)
            {
                OnError?.Invoke(this, new ErrorEventArgs($"{exception.Message} Повторите попытку"));
            }

            OnStorageUpdated?.Invoke();
        }
    }

    public abstract class Command
    {
        protected const string UndoCommandName = "undo";
        private const string EmptyCommandName = "";

        protected readonly string Name;

        protected Command(string name)
        {
            if (EmptyCommandName.Equals(name))
            {
                throw new ArgumentException("Введена пустая команда!");
            }
            Name = name;
        }

        public override string ToString()
        {
            return Name;
        }

        public static Command CreateInstance(string name)
        {
            if (UndoCommandName.Equals(name))
            {
                return new UndoCommand(name);
            }

            return new RegularCommand(name);
        }

        public abstract void Execute(Stack<Command> items);
    }

    public class UndoCommand : Command
    {
        public UndoCommand(string name) : base(name)
        {
            if (!UndoCommandName.Equals(Name))
            {
                throw new ArgumentException($"Имя команды не соответствует ее типу - имя: {Name}, тип: {nameof(UndoCommand)}!");
            }
        }

        public override void Execute(Stack<Command> items)
        {
            try
            {
                items.Pop();
            }
            catch (InvalidOperationException)
            {
                throw new ArgumentException("Команд для отмены нет!");
            }
        }
    }

    public class RegularCommand : Command
    {
        public RegularCommand(string name) : base(name)
        {
            if (UndoCommandName.Equals(Name))
            {
                throw new ArgumentException($"Имя команды не соответствует ее типу - имя: {Name}, тип: {nameof(RegularCommand)}! ");
            }
        }

        public override void Execute(Stack<Command> items)
        {
            items.Push(this);
        }
    }

    public class ErrorEventArgs : EventArgs
    {
        public string Error { get; }

        public ErrorEventArgs(string error)
        {
            Error = error;
        }
    }
}