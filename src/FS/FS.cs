using System.Net;

namespace FS
{
    public class FileSystem
    {
        public static async Task<bool> Download(string url, string path) {
            try {
                if (path.StartsWith("~/")) path = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + path.Substring(1);
                if (path.ToLower() == "desktop") path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                WebClient client = new WebClient();
                await client.DownloadFileTaskAsync(new Uri(url), path);
                IO.Output.Success("Successfully downloaded");
            }
            catch {
                IO.Output.Error("Failed to download");
                return false;
            }
            return true;
        }
    }
}