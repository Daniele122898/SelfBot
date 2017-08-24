using System.Threading.Tasks;
using Discord.Commands;
using SelfBot.Services;

namespace SelfBot.Modules
{
    public class MediaDownloaderModule : ModuleBase<SocketCommandContext>
    {
        private MediaDownloaderService _downloader;
        public MediaDownloaderModule(MediaDownloaderService downloaderService)
        {
            _downloader = downloaderService;
        }

        [Command("download", RunMode = RunMode.Async), Alias("dl")]
        public async Task DownloadMedia(string folderName, int amount)
        {
            await _downloader.DonwloadImages(Context, folderName, amount);
        }
    }
}