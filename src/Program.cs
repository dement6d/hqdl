using Main;
using IO;

Initialize();

// get url
string? url = "";
url = Input.GetUrl();

// get download path
string? folderPath = "";
// check if default download path exists
if (File.Exists("config.txt")) try { folderPath = File.ReadAllLinesAsync("config.txt").Result[0]; } catch {}
if (string.IsNullOrEmpty(folderPath))
    folderPath = Input.GetInput($"the path to save to", new[] 
    {   new Input.AdditionalInfo{ info = "Set a default download path by creating a 'config.txt' file in the same location as the executable and writing the path on the first line", error = false },
        new Input.AdditionalInfo { info = $"Leave empty to save in '{Environment.CurrentDirectory}'", error = false }});


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

// set window title
Console.Title = "High Quality Downloader - Working";

// separate urls
List<string> urls = urls = url.Split(" & ", StringSplitOptions.RemoveEmptyEntries).ToList();
foreach (string entry in urls) {
    if (urls.Count > 1) Output.Inform($"Handling entry '{SetText.Cyan}{entry}{SetText.ResetAll}'");

    // soundcloud
    if (entry.Contains("soundcloud.com")) await Handlers.HandleSoundcloud(entry, downloadFolder);
    // instagram
    else if (entry.Contains("instagram.com") || entry.StartsWith('@')) await Handlers.HandleInstagram(entry, downloadFolder);
    // youtube
    else if (entry.Contains("youtube.com") || entry.Contains("youtu.be")) await Handlers.HandleYoutube(entry, downloadFolder);
    // discord pfp
    else if (entry.Trim().Length == 18 && long.TryParse(entry.Trim(), out long userId)) await Handlers.HandleDiscord(userId, downloadFolder);
    // unsupported
    else Output.Inform($"'{SetText.Cyan}{entry}{SetText.ResetAll}' is not a valid entry");
}

// dont exit instantly
Console.Title = "High Quality Downloader - Finished";
Output.Inform("Press any key to exit");
Console.ReadKey();
SetText.DisplayCursor(true);

static void Initialize() {

    // set window title
    Console.Title = "High Quality Downloader";

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