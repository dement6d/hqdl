using Main;
using IO;

Initialize();

// get url
string? url = "";
while (string.IsNullOrEmpty(url.Trim())) url = Input.GetInput("URL / Discord User ID");

// get download path
string? folderPath = "";
// check if default download path exists
if (File.Exists("config.txt")) folderPath = File.ReadAllLinesAsync("config.txt").Result[0];
else {
    Output.Inform("Set a default download path by creating a 'config.txt' file in the same location as the executable and writing the path on the first line");
    folderPath = Input.GetInput($"Folder to save in {SetText.Gray}(Leave empty to save in {Environment.CurrentDirectory})");
}

// fix up the path
var downloadFolder = string.IsNullOrEmpty(folderPath.Trim()) ? "" : folderPath + (folderPath.EndsWith(Path.DirectorySeparatorChar) ? "" : Path.DirectorySeparatorChar);
if (downloadFolder.StartsWith("~/")) downloadFolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + downloadFolder.Substring(1);
downloadFolder = downloadFolder.Replace('/', Path.DirectorySeparatorChar);
downloadFolder = downloadFolder.Replace('\\', Path.DirectorySeparatorChar);
if (downloadFolder.ToLower().StartsWith("desktop" + Path.DirectorySeparatorChar)) {
    downloadFolder = downloadFolder.Replace("desktop", Environment.GetFolderPath(Environment.SpecialFolder.Desktop));
    Output.Inform("Download path set to " + downloadFolder);
}
if (!string.IsNullOrEmpty(downloadFolder) && downloadFolder != "desktop") Directory.CreateDirectory(downloadFolder);

// soundcloud
if (url.Contains("soundcloud.com")) await Handlers.HandleSoundcloud(url, downloadFolder);
// youtube
else if (url.Contains("youtube.com") || url.Contains("youtu.be")) await Handlers.HandleYoutube(url, downloadFolder);
// discord pfp
else if (url.Trim().Length == 18 && long.TryParse(url.Trim(), out long userId)) await Handlers.HandleDiscord(userId, downloadFolder);
// unsupported
else Output.Inform("Website not supported");

// dont exit instantly
Output.Inform("Press any key to exit");
Console.ReadKey();
SetText.DisplayCursor(true);

static void Initialize() {
    // clear console
    Console.Clear();

    // custom terminate handler
    Fix.TerminateHandler.FixCursor();

    // fix windows bugs
    if (Environment.OSVersion.Platform != PlatformID.Unix) {
        Fix.Windows.QuickEdit(false);
        Fix.Windows.FixCmd();
    }

    // disable cursor
    SetText.DisplayCursor(false);

    // install playwright browsers
    var exitCode = Microsoft.Playwright.Program.Main(new[] {"install"});
    if (exitCode != 0)
    {
        Output.Error($"Failed to install playwright");
        Environment.Exit(exitCode);
    }
}