using System;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Threading.Tasks;

namespace AttentionBot
{
    public class Program
    {
        static void Main(string[] args)
            => new Program().StartAsync().GetAwaiter().GetResult();

        public static void endMain()
            => new Program().StopAsync().GetAwaiter();

        public String token = "Removed for Security";

        private DiscordSocketClient _client;

        private CommandHandler _handler;
        public async Task StartAsync()
        {
            Console.Title = "Attention! Bot for Discord";

            _client = new DiscordSocketClient();

            await _client.LoginAsync(TokenType.Bot, token);

            await _client.StartAsync();

            _handler = new CommandHandler(_client);

            Console.WriteLine("Attention! Bot Online");

            await Task.Delay(-1);
        }
        public async Task StopAsync()
        {
            Console.WriteLine("Attention! Bot Offline");

            await _client.LogoutAsync();

            await _client.StopAsync();
        }
    }
}
