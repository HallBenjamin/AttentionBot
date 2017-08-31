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

        public static List<ulong> servID = new List<ulong>();

        public static ulong[] servIDs;

        public static bool loadedChans = false;

        public static bool loadedServs = false;

        public static bool loadedRoles = false;

        public static List<ulong> roleID = new List<ulong>();

        public static ulong[] roleIDs;

        public static BinaryReader reader = new BinaryReader(File.Open("channels.txt", FileMode.OpenOrCreate));
        public static BinaryReader readers = new BinaryReader(File.Open("servers.txt", FileMode.OpenOrCreate));
        public static BinaryReader roleder = new BinaryReader(File.Open("roles.txt", FileMode.OpenOrCreate));
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

            if(!loadedServs)
            {
                for(int i = 0; i < readers.BaseStream.Length; i = i + 8)
                {
                    servID.Add(readers.ReadUInt64());
                }
                servIDs = servID.ToArray();
                readers.Close();
                loadedServs = true;
            }

            if(!loadedRoles)
            {
                for(int i = 0; i < roleder.BaseStream.Length; i = i + 8)
                {
                    roleID.Add(roleder.ReadUInt64());
                }
                roleIDs = roleID.ToArray();
                roleder.Close();
                loadedRoles = true;
            }

            await Task.Delay(-1);
        }
    }
}
