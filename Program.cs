namespace Rollback
{
    internal static class Program
    {
        private static void Main()
        {
            var storage = new CommandStorage();
            var inputLoop = new InputLoop(storage);

            inputLoop.StartConsoleInput();
        }
    }
}
