namespace IO
{
    public class Input
    {
        public static string? GetInput(string details) {
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Input ");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(details);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("► ");
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            var input = Console.ReadLine();
            Console.ForegroundColor = ConsoleColor.White;
            return input;
        }
    }
}