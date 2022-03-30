using Main;

namespace IO
{
    public class Input
    {
        public static string? GetInput(string details, AdditionalInfo[]? additionalInfo=null) {
            Console.Write("\n");

            // print additional info
            if (additionalInfo != null)
                for (int i = 0; i < additionalInfo.Length; i++) {
                    if (!additionalInfo[i].error) Output.Inform(additionalInfo[i].info);
                    else Output.Error(additionalInfo[i].info);
                }

            // get input
            SetText.DisplayCursor(true);
            Console.Write($"  {SetText.ResetAll}Input {SetText.Blue}{details}{SetText.ResetAll}\n► ");
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            var input = Console.ReadLine();
            Console.ForegroundColor = ConsoleColor.White;
            SetText.DisplayCursor(false);
            return input;
        }

        public static string GetUrl() {
            string? url = GetInput("one or more entries", new[] { new AdditionalInfo{ info = "Use ' & ' to separate entries", error = false }});
            while (string.IsNullOrEmpty(url.Trim())) {
                Console.Clear();
                url = GetInput("one or more entries", new[] {   new AdditionalInfo{ info = "This field can't be empty", error = true },
                                                                new AdditionalInfo{ info = "Use ' & ' to separate entries", error = false }});
            }
            return url!;
        }

        public class AdditionalInfo {
            public string info {get;set;}
            public bool error {get;set;}
        }
    }
}