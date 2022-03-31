using Microsoft.Playwright;
using IO;
using FS;

namespace Main
{
    public class Handlers
    {
        public static async Task HandleInstagram(string url, string folderPath) {

            // go to url
            using var playwright = await Playwright.CreateAsync();
            await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = true });
            var page = await browser.NewPageAsync();

            if (url.StartsWith('@')) {
                string at = url.Replace("@", "");

                // goto instabig
                await page.GotoAsync($"https://instabig.net/fullsize/{at}", new PageGotoOptions{
                    WaitUntil = WaitUntilState.NetworkIdle
                });
                
                // get pfp
                var pfpSrc = await page.Locator("#fullsizeLink").GetAttributeAsync("href");

                // download
                Output.Inform($"Downloading {SetText.Blue}{SetText.Bold}{at}{SetText.ResetAll}'s Instagram profile picture");
                await FileSystem.Download(pfpSrc, GetDownloadPath(folderPath, $"{at} insta pfp.png"));
                return;
            }

            // get username
            await page.GotoAsync(url, new PageGotoOptions{
                WaitUntil = WaitUntilState.NetworkIdle
            });
            string? username = null;
            try { username = await page.Locator(".e1e1d > div:nth-child(1) > a:nth-child(1)").TextContentAsync(new LocatorTextContentOptions { Timeout = 3000 }); } catch {}
            if (string.IsNullOrEmpty(username)) username = "unknown";

            if (url.Contains("/p/")) {

                // goto page
                await page.GotoAsync("https://igdownloader.com/", new PageGotoOptions{
                    WaitUntil = WaitUntilState.NetworkIdle
                });

                // search for post
                await page.TypeAsync(".ig[placeholder=\"ex: https://www.instagram.com/p/CBBp48HA8u7/\"]", url);
                await page.PressAsync(".ig[placeholder=\"ex: https://www.instagram.com/p/CBBp48HA8u7/\"]", "Enter");
                await page.WaitForRequestFinishedAsync();

                // get image src
                var downloads = await page.QuerySelectorAllAsync("a[class=download-button]");

                Output.Inform($"Downloading {SetText.Blue}{SetText.Bold}{username}{SetText.ResetAll}'s Instagram post");
                for (int i = 0; i < downloads.Count; i++)
                {
                    var downloadLink = downloads[i].GetAttributeAsync("href").Result.Replace("&dl=1", "");
                    await FileSystem.Download(downloadLink, GetDownloadPath(folderPath, $"{username} {new Random().Next(10000, 99999)}." + (downloadLink.Contains("dst-jpg") ? ".png" : ".mp4")));
                }
                return;
            }

            Output.Error("Action not supported");
            return;
        }
        public static async Task HandleDiscord(long userId, string folderPath) {
            // go to url
            using var playwright = await Playwright.CreateAsync();
            await using var browser = await playwright.Chromium.LaunchAsync( new BrowserTypeLaunchOptions { Headless = true });
            var page = await browser.NewPageAsync();
            await page.GotoAsync("https://discord.id", new PageGotoOptions{
                WaitUntil = WaitUntilState.NetworkIdle
            });

            // get user profile
            await page.TypeAsync("#inputid", userId.ToString());
            await page.ClickAsync("button.form-control");
            await page.ClickAsync(".frc-button");
            
            // get username
            string? username = null;
            try { username = await page.Locator("div.col-md-4:nth-child(2) > p:nth-child(3) > span:nth-child(3) > span:nth-child(1)").TextContentAsync(new LocatorTextContentOptions {
                Timeout = 3000
            }); }
            catch { 
                try { username = await page.Locator("div.col-md-4.withdarker > p:nth-child(2) > span.resulth > span").TextContentAsync(new LocatorTextContentOptions {
                Timeout = 3000
            }); }
                catch { username = "unknown"; } }

            // get pfp gif and png
            string? pfpSrc = null;
            try { pfpSrc = await page.Locator("img >> nth=0").GetAttributeAsync("src"); }
            catch {
                Output.Error("Failed to get User profile, keep in mind that Server IDs aren't supported");
                return;
            }
            var pfpDownloadPath = GetDownloadPath(folderPath, $"{username} pfp.png");
            Output.Inform($"Downloading {SetText.Blue}{SetText.Bold}{username}{SetText.ResetAll}'s profile picture (PNG)");
            await FileSystem.Download(pfpSrc.Replace("?size=1024", "?size=2048"), pfpDownloadPath);
            Output.Inform($"Downloading {SetText.Blue}{SetText.Bold}{username}{SetText.ResetAll}'s profile picture (GIF)");
            await FileSystem.Download(pfpSrc.Replace("?size=1024", "?size=2048").Replace(".png", ".gif"), pfpDownloadPath.Replace(".png", ".gif"));

            // get banner gif and png
            string? bannerSrc = null;
            try { bannerSrc = await page.QuerySelectorAsync("img[alt=\"Banner\"]").Result.GetAttributeAsync("src"); }
            catch {
                Output.Error("User doesn't have a banner");
                return;
            }
            var bannerDownloadPath = GetDownloadPath(folderPath, $"{username} banner.png");
            Output.Inform($"Downloading {SetText.Blue}{SetText.Bold}{username}{SetText.ResetAll}'s banner (PNG)");
            await FileSystem.Download(bannerSrc.Replace("?size=1024", "?size=2048"), bannerDownloadPath);
            Output.Inform($"Downloading {SetText.Blue}{SetText.Bold}{username}{SetText.ResetAll}'s banner (GIF)");
            await FileSystem.Download(bannerSrc.Replace("?size=1024", "?size=2048").Replace(".png", ".gif"), bannerDownloadPath.Replace(".png", ".gif"));

            // exit browser
            await page.CloseAsync();
        }

        public static async Task HandleYoutube(string url, string folderPath) {

            // go to url
            using var playwright = await Playwright.CreateAsync();
            await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions {
                IgnoreDefaultArgs = new string[] {"--mute-audio", "--user-gesture-required"}
            });
            var page = await browser.NewPageAsync();
            await page.GotoAsync(url, new PageGotoOptions{
                WaitUntil = WaitUntilState.NetworkIdle
            });
            
            // update link
            url = page.Url.Replace("&feature=youtu.be", "");

            // check if profile page or video
            if (url.Contains("watch?v=")) {

                // get video title
                var videoTitle = await page.Locator("yt-formatted-string.ytd-video-primary-info-renderer:nth-child(1)").TextContentAsync();

                // download thumbnail
                Output.Inform($"Downloading thumbnail for '{SetText.Blue}{SetText.Bold}{videoTitle}{SetText.ResetAll}'");

                // get original thumbnail link
                var videoId = url.Replace("https://", "").Replace("http://", "").Replace("www.youtube.com/watch?v=", "");
                var downloadLink = $"https://img.youtube.com/vi/{videoId}/maxresdefault.jpg";

                var downloadPath = GetDownloadPath(folderPath, $"{videoTitle}.png");
                await FileSystem.Download(downloadLink, downloadPath);
            }
            else if (url.Contains("/channel/") || url.Contains("/c/")) {

                // get channel name
                var channelName = await page.Locator("ytd-channel-name.ytd-c4-tabbed-header-renderer > div:nth-child(1) > div:nth-child(1) > yt-formatted-string:nth-child(1)").TextContentAsync();
                
                // download pfp
                Output.Inform($"Downloading {SetText.Blue}{SetText.Bold}{channelName}{SetText.ResetAll}'s YouTube profile picture");

                var pfp = await page.QuerySelectorAsync("#channel-header-container #img");
                var srcAttribute = await pfp.GetAttributeAsync("src");
                var pfpLink = srcAttribute.Replace("=s88-c-k-c0x00ffffff-no-rj", "");

                var downloadPath = GetDownloadPath(folderPath, $"{channelName} yt pfp.png");
                await FileSystem.Download(pfpLink, downloadPath);
            }
            else Output.Inform("Unrecognized link, only video and channel links are supported");

            // exit browser
            await page.CloseAsync();
        }

        public static async Task HandleSoundcloud(string url, string folderPath) {

            // go to url
            using var playwright = await Playwright.CreateAsync();
            await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions {
                Headless = true
            });
            var page = await browser.NewPageAsync();
            await page.GotoAsync(url, new PageGotoOptions{
                WaitUntil = WaitUntilState.NetworkIdle
            });

            // check if profile page
            var profileStats = await page.QuerySelectorAsync("article[class=infoStats]");
            if (profileStats == null) {

                // get song & artist name
                var artistName = await page.Locator(".userBadge__usernameLink > span:nth-child(1)").TextContentAsync();
                var songName = await page.Locator("h1.soundTitle__title > span:nth-child(1)").TextContentAsync();

                // inform what we downloading
                Output.Inform($"Downloading artwork for '{SetText.Blue}{SetText.Bold}{songName}{SetText.ResetAll}' by {SetText.Blue}{SetText.Bold}{artistName}");

                // get original image url
                var styleAttribute = await page.Locator(".image__full").First.GetAttributeAsync("style");
                var substringStyleAttribute = styleAttribute.Substring(styleAttribute.IndexOf("https"));
                var imageUrl = substringStyleAttribute.Substring(0, substringStyleAttribute.IndexOf("\");"));
                var originalImageUrl = imageUrl.Replace("t500x500", "original");

                // download
                var downloadPath = GetDownloadPath(folderPath, $"{artistName} - {songName}.png");
                await FileSystem.Download(originalImageUrl, downloadPath);
            }
            else {
                // get artist name
                var artistName = page.Locator(".profileHeaderInfo__userName").TextContentAsync().Result.Replace("\n", "").Replace(" Verified", "").Trim();

                // inform what we downloading
                Output.Inform($"Downloading {SetText.Blue}{SetText.Bold}{artistName}{SetText.ResetAll}'s Soundcloud profile picture");

                // get original image url
                var styleAttribute = await page.Locator(".image__noOutline > span:nth-child(1)").First.GetAttributeAsync("style");
                var substringStyleAttribute = styleAttribute.Substring(styleAttribute.IndexOf("https"));
                var imageUrl = substringStyleAttribute.Substring(0, substringStyleAttribute.IndexOf("\");"));
                var originalImageUrl = imageUrl.Replace("t200x200", "original");

                // download
                var downloadPath = GetDownloadPath(folderPath, $"{artistName} sc pfp.png");
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