using System.Threading.Tasks;
using Discord.Commands;

namespace SelfBot.Modules
{
    public class Misc : ModuleBase<SocketCommandContext>
    {
        [Command("ping")]
        public async Task Ping()
        {
            await Context.Channel.SendMessageAsync($"Pong: {Context.Client.Latency} :ping_pong:");
        }
    }
}