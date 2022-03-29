namespace IO
{
    public class Output
    {
        public static void Inform(string message) {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("     i ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(message);
            Console.ForegroundColor = ConsoleColor.White;
        }
        public static void Error(string message) {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("     x ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ForegroundColor = ConsoleColor.White;
        }
        public static void Success(string message) {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("     ✓ ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(message);
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}