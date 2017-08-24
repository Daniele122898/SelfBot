using System;
using System.Collections.Generic;
using System.IO;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using SelfBot.Services;

namespace SelfBot
{
    class Program
    {
        static void Main(string[] args) => new Program().MainAsync().GetAwaiter().GetResult();

        private DiscordSocketClient _client;
        private Dictionary<string, string> _data = new Dictionary<string, string>();
        
        public async Task MainAsync()
        {
            _client = new DiscordSocketClient();
            _client.Log += Log;
            
            //Setup config
            ConfigService.InitializeLoader();
            var found =ConfigService.LoadConfig();
            if (!found)
            {
                Console.WriteLine("Enter your token (github.com/TheRacingLion/Discord-SelfBot/wiki/Discord-Token-Tutorial): ");
                string readToken = Console.ReadLine();
                _data.Add("token", readToken);
                Console.WriteLine("Enter desired prefix: ");
                string readPrefix = Console.ReadLine();
                _data.Add("prefix", readPrefix);
                CreateConfig();
                
                ConfigService.LoadConfig();
            }
            bool trigger = false;

            string prefix = ConfigService.GetConfigData("prefix");
            if (string.IsNullOrWhiteSpace(prefix))
            {
                prefix = GetPrefix();
                trigger = true;
            }

            Utility.SetPrefix(prefix);
            if (!_data.ContainsKey("prefix"))
                _data.Add("prefix", prefix);
            else
                _data["prefix"] = prefix;

            var services = ConfigureServices();
            
            //initialize command handler
            await services.GetRequiredService<CommandHandler>().InitializeAsync(services);

            string token = ConfigService.GetConfigData("token");
            if (string.IsNullOrWhiteSpace(token))
            {
                token = GetToken();
                trigger = true;
            }
            if(!_data.ContainsKey("token"))
                _data.Add("token", token);
            else
                _data["token"] = token;
            
            if(trigger)
                CreateConfig();
            
            await _client.LoginAsync(TokenType.User,token);
            await _client.StartAsync();

            await Task.Delay(-1);
        }

        private void CreateConfig()
        {
            if (File.Exists("config.json"))
            {
                File.Delete("config.json");
            }
            ConfigService.SaveConfig(_data);
        }

        private string GetPrefix()
        {
            Console.WriteLine("Enter desired prefix: ");
            return Console.ReadLine();
        }

        private IServiceProvider ConfigureServices()
        {
            return new ServiceCollection()
                .AddSingleton(_client)
                .AddSingleton<CommandService>()
                .AddSingleton<CommandHandler>()
                .BuildServiceProvider();
        }

        private string GetToken()
        {
            Console.WriteLine("Enter your token (github.com/TheRacingLion/Discord-SelfBot/wiki/Discord-Token-Tutorial): ");
            return Console.ReadLine();
        }
        
        private Task Log(LogMessage m)
        {
            switch (m.Severity)
            {
                case LogSeverity.Warning: Console.ForegroundColor = ConsoleColor.Yellow; break;
                case LogSeverity.Error: Console.ForegroundColor = ConsoleColor.Red; break;
                case LogSeverity.Critical: Console.ForegroundColor = ConsoleColor.DarkRed; break;
                case LogSeverity.Verbose: Console.ForegroundColor = ConsoleColor.White; break;
            }
            
            Console.WriteLine(m.ToString());
            if(m.Exception != null)
                Console.WriteLine(m.Exception);
            Console.ForegroundColor = ConsoleColor.Gray;

            return Task.CompletedTask;
        }


    }
}
