<h1 align="center">High Quality Download</h1>
<p align="center">Simple and easy to use tool for downloading original quality images from various social medias</p>

<p align="center">
  <img width="400" align="center" src="https://user-images.githubusercontent.com/93228501/160738873-692fa6af-d923-4d1b-871c-33787a7f29ec.png"/>
</p>
<p align="center">
  <img width="400" align="center" src="https://user-images.githubusercontent.com/93228501/160738270-edbff342-7b44-450d-afc4-f3513e7dfd85.png"/>
</p>

<p align="center"><a href="https://github.com/microsoft/playwright-dotnet"><img src="https://img.shields.io/badge/powered%20by-playwright-0077a3"/></a></p>


# Capabilities

What can it download? Here's a list:
- **Instagram** posts (both videos & images)
- **Discord** profile picture & banners (gif & png)
- **YouTube** thumbnails & channel profile pictures
- **Soundcloud** artwork & artist profile pictures

# How to use

Run the program and paste in a URL or Discord User ID, press enter

Optionally, run the program with the `-o` & `-v` arguments. For help, run `hqdl --help`

If you didn't set a default download path by creating a 'config.txt' file in the same directory as the executable, type a path to where you want the files to be saved or leave it empty to have them downloaded to the same folder as the executable

### Helpful tip
Use 'desktop' at the beginning of your download path if you want the items to be downloaded to your desktop or use 'desktop/folder' to have them downloaded inside a folder on your desktop. If the folder doesn't exist, it will be created.


# How to build

- Install .NET 6.0
- `cd src`
- `dotnet add package Microsoft.Playwright`
- `dotnet run`
