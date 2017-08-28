using System;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Threading.Tasks;
using System.IO;
using System.Threading;

namespace AttentionBot
{
    public class Program
    {
        static void Main(string[] args)
            => new Program().StartAsync().GetAwaiter().GetResult();

        public String token = "Removed for Security";

        private DiscordSocketClient _client;

        private CommandHandler _handler;

        public static bool isConsole = Console.OpenStandardInput(1) != Stream.Null;

        public static string botID = "3949";
        public async Task StartAsync()
        {
            if(isConsole)
                Console.Title = "Attention! Bot for Discord";

            _client = new DiscordSocketClient();

            await _client.LoginAsync(TokenType.Bot, token);

            await _client.StartAsync();

            await _client.SetGameAsync("Attention! \\help 3949");

            _handler = new CommandHandler(_client);

            if(isConsole)
                Console.WriteLine("Attention! Bot Online");

            await Task.Delay(-1);
        }
    }
}
