using Microsoft.Playwright;
using IO;
using FS;

namespace Main
{
    public class Handlers
    {
        public static async Task HandleDiscord(long userId, string folderPath) {
            // go to url
            using var playwright = await Playwright.CreateAsync();
            await using var browser = await playwright.Firefox.LaunchAsync(new BrowserTypeLaunchOptions {
                Headless = true
            });
            var page = await browser.NewPageAsync();
            await page.GotoAsync("https://discord.id", new PageGotoOptions{
                WaitUntil = WaitUntilState.NetworkIdle
            });

            await page.TypeAsync("#inputid", userId.ToString());
            await page.ClickAsync("button.form-control");
            await page.ClickAsync(".frc-button");
            
            string? pfpSrc = null;
            try { pfpSrc = await page.Locator("img >> nth=0").GetAttributeAsync("src"); }
            catch {
                Output.Error("Failed to get User profile, keep in mind that Server IDs aren't supported");
                return;
            }
            var pfpDownloadPath = GetDownloadPath(folderPath, "discord_pfp.png");
            Output.Inform("Downloading Discord profile picture PNG");
            await FileSystem.Download(pfpSrc.Replace("?size=1024", "?size=2048"), pfpDownloadPath);
            Output.Inform("Downloading Discord profile picture GIF");
            await FileSystem.Download(pfpSrc.Replace("?size=1024", "?size=2048").Replace(".png", ".gif"), pfpDownloadPath.Replace(".png", ".gif"));

            string? bannerSrc = null;
            try { bannerSrc = await page.QuerySelectorAsync("img[alt=\"Banner\"]").Result.GetAttributeAsync("src"); }
            catch {
                Output.Error("User doesn't have a banner");
                return;
            }
            var bannerDownloadPath = GetDownloadPath(folderPath, "discord_banner.png");
            Output.Inform("Downloading Discord banner PNG");
            await FileSystem.Download(bannerSrc.Replace("?size=1024", "?size=2048"), bannerDownloadPath);
            Output.Inform("Downloading Discord banner GIF");
            await FileSystem.Download(bannerSrc.Replace("?size=1024", "?size=2048").Replace(".png", ".gif"), bannerDownloadPath.Replace(".png", ".gif"));

            await page.PauseAsync();
        }

        public static async Task HandleYoutube(string url, string folderPath) {

            // go to url
            using var playwright = await Playwright.CreateAsync();
            await using var browser = await playwright.Firefox.LaunchAsync(new BrowserTypeLaunchOptions {
                Headless = true
            });
            var page = await browser.NewPageAsync();
            await page.GotoAsync(url, new PageGotoOptions{
                WaitUntil = WaitUntilState.NetworkIdle
            });
            
            // update link
            url = page.Url;

            // check if profile page or video
            if (url.Contains("watch?v=")) {
                // download thumbnail
                Output.Inform("Downloading YouTube video thumbnail");

                // get original thumbnail link
                var videoId = url.Replace("https://", "").Replace("http://", "").Replace("www.youtube.com/watch?v=", "");
                var downloadLink = $"https://img.youtube.com/vi/{videoId}/maxresdefault.jpg";

                var downloadPath = GetDownloadPath(folderPath, "hq_yt_thumbnail.png");
                await FileSystem.Download(downloadLink, downloadPath);
            }
            else if (url.Contains("/channel/") || url.Contains("/c/")) {
                // download pfp
                Output.Inform("Downloading YouTube channel profile picture");

                var pfp = await page.QuerySelectorAsync("#channel-header-container #img");
                var srcAttribute = await pfp.GetAttributeAsync("src");
                var pfpLink = srcAttribute.Replace("=s88-c-k-c0x00ffffff-no-rj", "");

                var downloadPath = GetDownloadPath(folderPath, "hq_yt_pfp.png");
                await FileSystem.Download(pfpLink, downloadPath);
            }
            else Output.Inform("Unrecognized link, only video and channel links are supported");
        }

        public static async Task HandleSoundcloud(string url, string folderPath) {

            // go to url
            using var playwright = await Playwright.CreateAsync();
            await using var browser = await playwright.Firefox.LaunchAsync(new BrowserTypeLaunchOptions {
                Headless = true
            });
            var page = await browser.NewPageAsync();
            await page.GotoAsync(url, new PageGotoOptions{
                WaitUntil = WaitUntilState.NetworkIdle
            });

            // check if profile page
            var profileStats = await page.QuerySelectorAsync("article[class=infoStats]");
            if (profileStats == null) {
                Output.Inform("Downloading Soundcloud song artwork");

                // get original image url
                var styleAttribute = await page.Locator(".image__full").First.GetAttributeAsync("style");
                var substringStyleAttribute = styleAttribute.Substring(styleAttribute.IndexOf("https"));
                var imageUrl = substringStyleAttribute.Substring(0, substringStyleAttribute.IndexOf("\");"));
                var originalImageUrl = imageUrl.Replace("t500x500", "original");

                // download
                var downloadPath = GetDownloadPath(folderPath, "hq_sc_cover.png");
                await FileSystem.Download(originalImageUrl, downloadPath);
            }
            else {
                Output.Inform("Downloading Soundcloud profile picture");

                // get original image url
                var styleAttribute = await page.Locator(".image__noOutline > span:nth-child(1)").First.GetAttributeAsync("style");
                var substringStyleAttribute = styleAttribute.Substring(styleAttribute.IndexOf("https"));
                var imageUrl = substringStyleAttribute.Substring(0, substringStyleAttribute.IndexOf("\");"));
                var originalImageUrl = imageUrl.Replace("t200x200", "original");

                // download
                var downloadPath = GetDownloadPath(folderPath, "hq_sc_avatar.png");
                await FileSystem.Download(originalImageUrl, downloadPath);
            }
            
            // exit browser
            await page.CloseAsync();
        }
        public static string GetDownloadPath(string folderPath, string fileName) {
            return string.IsNullOrEmpty(folderPath.Trim()) ? fileName : folderPath + (folderPath.EndsWith(Path.DirectorySeparatorChar) ? "" : Path.DirectorySeparatorChar) + fileName;
        }
    }
}