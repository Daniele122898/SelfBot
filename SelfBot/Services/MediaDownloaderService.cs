using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace SelfBot.Services
{
    public class MediaDownloaderService
    {
        public async Task DonwloadImages(SocketCommandContext context, string folderName, int amount)
        {
            try
            {
                var channel = context.Channel;

                var messages = channel.GetMessagesAsync(amount, CacheMode.AllowDownload);

                var converted = messages.Flatten().Result;

                List<IMessage> hasAttachment = new List<IMessage>();
                foreach (var message in converted)
                {
                    if (message.Attachments.Count > 0)
                    {
                        hasAttachment.Add(message);
                    }
                }
                if (hasAttachment.Count == 0)
                {
                    if (!Utility.StealthMode)
                    {
                        await context.Channel.SendMessageAsync("", embed:
                            Utility.ResultFeedback(Utility.RedFailiureEmbed, Utility.SuccessLevelEmoji[1],
                                $"No attachments found within {amount} messages"));
                    }
                    else
                    {
                        Console.WriteLine($"No attachments found within {amount} messages");
                    }
                    return;
                }
                //Create Folder
                System.IO.Directory.CreateDirectory($"images/{folderName}");
                int count = 0;
                foreach (var message in hasAttachment)
                {
                    foreach (var attachment in message.Attachments)
                    {
                        string type = attachment.Filename.Substring(attachment.Filename.LastIndexOf('.'));
                        Console.WriteLine("TYPE: "+type);
                        Uri requestUri = new Uri(attachment.Url);
                        using(var client = new HttpClient())
                        using(var request = new HttpRequestMessage(HttpMethod.Get, requestUri))
                        using (Stream contentStream = await (await client.SendAsync(request)).Content.ReadAsStreamAsync(),
                            stream = new FileStream($"images/{folderName}/{attachment.Filename}{type}",FileMode.Create, FileAccess.Write, FileShare.None, 3145728, true))
                        {
                            await contentStream.CopyToAsync(stream);
                            await contentStream.FlushAsync();
                            contentStream.Dispose();
                            await stream.FlushAsync();
                            stream.Dispose();
                        }
                        count++;
                    }

                }
                if (!Utility.StealthMode)
                {
                    await context.Channel.SendMessageAsync("", embed:
                        Utility.ResultFeedback(Utility.GreenSuccessEmbed, Utility.SuccessLevelEmoji[0],
                            $"Downloaded {count} files to \"images/{folderName}\""));
                }
                else
                {
                    Console.WriteLine($"Downloaded {count} files to \"images/{folderName}\"");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            
        }
    }
}