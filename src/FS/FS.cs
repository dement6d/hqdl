using System.Net;

namespace FS
{
    public class FileSystem
    {
        public static async Task<bool> Download(string url, string path) {
            try {
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