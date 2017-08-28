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

        public String token = "MzQ2MDY0OTkwMTUyODE4Njkw.DHEZrg.PWW68ODzek95ma8lX8PmdcC2kaI";

        private DiscordSocketClient _client;

        private CommandHandler _handler;
        public async Task StartAsync()
        {
            _client = new DiscordSocketClient();

            await _client.LoginAsync(TokenType.Bot, token);

            await _client.StartAsync();

            _handler = new CommandHandler(_client);

            await Task.Delay(-1);
        }
    }
}
