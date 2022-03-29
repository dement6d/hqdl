namespace IO
{
    public class Output
    {
        public static void Inform(string message) {
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("[INFO] ");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(message);
            Console.ForegroundColor = ConsoleColor.White;
        }
        public static void Error(string message) {
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("[ERROR] ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ForegroundColor = ConsoleColor.White;
        }
        public static void Success(string message) {
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("[SUCCESS] ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(message);
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}