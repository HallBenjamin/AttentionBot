using System;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
using System.Collections.Generic;

namespace AttentionBot
{
    public class Program
    {
        static void Main(string[] args)
            => new Program().StartAsync().GetAwaiter().GetResult();

        public String token = "Removed Token for Security";

        private DiscordSocketClient _client;

        private CommandHandler _handler;

        public static bool isConsole = Console.OpenStandardInput(1) != Stream.Null;

        public static string botID = "3949";

        public static List<ulong> chanID = new List<ulong>();

        public static ulong[] chanIDs;

        public static bool loadedChans = false;

        public static BinaryReader reader = new BinaryReader(File.Open("channels.txt", FileMode.OpenOrCreate));
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

            if(!loadedChans)
            {
                for (int i = 0; i < reader.BaseStream.Length; i = i + 8)
                {
                    chanID.Add(reader.ReadUInt64());
                }
                chanIDs = chanID.ToArray();
                reader.Close();
                loadedChans = true;
            }

            await Task.Delay(-1);
        }
    }
}
