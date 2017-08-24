using System;
using System.Threading.Tasks;
using Discord.Commands;
using SelfBot.Services;

namespace SelfBot.Modules
{
    public class Misc : ModuleBase<SocketCommandContext>
    {
        [Command("ping")]
        public async Task Ping()
        {
            if (!Utility.StealthMode)
            {
                await Context.Channel.SendMessageAsync($"Pong: {Context.Client.Latency} :ping_pong:");
            }
            else
            {
                Console.WriteLine($"Pong: {Context.Client.Latency} :ping_pong:");
            }
        }
    }
}