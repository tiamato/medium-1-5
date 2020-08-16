namespace Rollback
{
    internal static class Program
    {
        private static void Main()
        {
            var storage = new CommandStorage();
            var storageController = new CommandController(storage);

            storageController.StartConsoleInput();
        }
    }
}
