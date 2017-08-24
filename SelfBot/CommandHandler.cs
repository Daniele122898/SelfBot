using System;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using SelfBot.Services;

namespace SelfBot
{
    public class CommandHandler
    {
        private CommandService _commands;
        private DiscordSocketClient _client;
        private IServiceProvider _provider;

        public CommandHandler(IServiceProvider provider, DiscordSocketClient client, CommandService commands)
        {
            _commands = commands;
            _client = client;
            _provider = provider;

            _client.MessageReceived += MessageReceived;
        }

        public async Task InitializeAsync(IServiceProvider provider)
        {
            _provider = provider;
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly());
        }

        private async Task MessageReceived(SocketMessage rawMessage)
        {
            // Ignore system messages and messages from bots
            if (!(rawMessage is SocketUserMessage message)) return;
            if (message.Source != MessageSource.User) return;
            
            int argPos = Utility.Prefix.Length-1;
            if (message.HasStringPrefix(Utility.Prefix, ref argPos)) return;
            
            var context = new SocketCommandContext(_client, message);
            var result = await _commands.ExecuteAsync(context, argPos, _provider);
        }
    }
}