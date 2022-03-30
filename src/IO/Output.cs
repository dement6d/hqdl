using Main;

namespace IO
{
    public class Output
    {
        public static void Inform(string message) => Console.WriteLine($"{SetText.Gray}     i {SetText.ResetAll}{message}{SetText.ResetAll}");
        public static void Error(string message) => Console.WriteLine($"{SetText.Gray}     x {SetText.Red}{message}{SetText.ResetAll}");
        public static void Success(string message) => Console.WriteLine($"{SetText.Gray}     ✓ {SetText.Green}{message}{SetText.ResetAll}");
    }
}