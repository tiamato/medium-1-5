using System;
using System.Collections.Generic;

namespace Rollback
{
    public class CommandStorage
    {
        private readonly List<Command> _items = new List<Command>();

        public IEnumerable<Command> GetItems()
        {
            return _items;
        }

        public event Action OnStorageUpdated;
        public event EventHandler<ErrorEventArgs> OnError;

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
            catch (ArgumentOutOfRangeException)
            {
                OnError?.Invoke(this, new ErrorEventArgs("Команд для отмены нет! Повторите попытку"));
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

        public readonly string Name;

        protected Command(string name)
        {
            if (EmptyCommandName.Equals(name))
            {
                throw new ArgumentException("Введена пустая команда!");
            }
            Name = name;
        }

        public static Command CreateInstance(string name)
        {
            if (UndoCommandName.Equals(name))
            {
                return new UndoCommand(name);
            }

            return new RegularCommand(name);
        }

        public abstract void Execute(List<Command> items);
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

        public override void Execute(List<Command> items)
        {
            items.RemoveAt(items.Count - 1);
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

        public override void Execute(List<Command> items)
        {
            items.Add(this);
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