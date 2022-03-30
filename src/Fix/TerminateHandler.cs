namespace Fix
{
    public class TerminateHandler
    {
        public static void FixCursor(){
            Console.CancelKeyPress += (s, ev) => {                
                // show the cursor back to the user
                Main.SetText.DisplayCursor(true);

                // re-enable quickedit
                if (Environment.OSVersion.Platform != PlatformID.Unix)
                    Windows.QuickEdit(true);

                // exit the app
                ev.Cancel = true;
                Environment.Exit(0);
            };
        }
    }
}