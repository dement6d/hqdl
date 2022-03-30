using Main;

namespace IO
{
    public class Input
    {
        public static string? GetInput(string details) {
            SetText.DisplayCursor(true);
            Console.Write($"\n  {SetText.ResetAll}Input {SetText.Blue}{details}{SetText.ResetAll}\n► ");
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            var input = Console.ReadLine();
            Console.ForegroundColor = ConsoleColor.White;
            SetText.DisplayCursor(false);
            return input;
        }
    }
}