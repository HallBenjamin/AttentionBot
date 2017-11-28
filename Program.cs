using System;
using Discord;
using Discord.WebSocket;
using System.Threading.Tasks;
using System.IO;
using System.Collections.Generic;

namespace AttentionBot
{
    public class Program
    {
        static void Main(string[] args)
            => new Program().StartAsync().GetAwaiter().GetResult();

        String token = "Token Removed for Security";

        private DiscordSocketClient _client;

        private CommandHandler _handler;

        public static bool isConsole = Console.OpenStandardInput(1) != Stream.Null;

        public static string botID = "3949";

        public static List<ulong> chanID = new List<ulong>();

        public static ulong[] chanIDs;

        public static List<ulong> servID = new List<ulong>();

        public static ulong[] servIDs;

        public static List<ulong> roleID = new List<ulong>();

        public static ulong[] roleIDs;

        public static List<ulong> mentionID = new List<ulong>();

        public static ulong[] mentionIDs;

        bool loadedChans = false;

        bool loadedServs = false;

        bool loadedRoles = false;

        bool loadedMentions = false;
        public async Task StartAsync()
        {
            if (isConsole)
                Console.Title = "Attention! Bot for Discord";

            _client = new DiscordSocketClient();

            await _client.LoginAsync(TokenType.Bot, token);

            await _client.StartAsync();

            await _client.SetGameAsync("Attention! \\help 3949");

            _handler = new CommandHandler(_client);

            if (isConsole)
                Console.WriteLine("Attention! Bot Online");

            if (!loadedChans)
            {
                BinaryReader chanReader = new BinaryReader(File.Open("channels.txt", FileMode.OpenOrCreate));
                for (int i = 0; i < chanReader.BaseStream.Length; i += 8)
                {
                    chanID.Add(chanReader.ReadUInt64());
                }
                chanIDs = chanID.ToArray();
                chanReader.Close();
                loadedChans = true;
            }

            if (!loadedServs)
            {
                BinaryReader servReader = new BinaryReader(File.Open("servers.txt", FileMode.OpenOrCreate));
                for (int i = 0; i < servReader.BaseStream.Length; i += 8)
                {
                    servID.Add(servReader.ReadUInt64());
                }
                servIDs = servID.ToArray();
                servReader.Close();
                loadedServs = true;
            }

            if (!loadedRoles)
            {
                BinaryReader roleReader = new BinaryReader(File.Open("roles.txt", FileMode.OpenOrCreate));
                for (int i = 0; i < roleReader.BaseStream.Length; i += 8)
                {
                    roleID.Add(roleReader.ReadUInt64());
                }
                roleIDs = roleID.ToArray();
                roleReader.Close();
                loadedRoles = true;
            }

            if (!loadedMentions)
            {
                BinaryReader mentReader = new BinaryReader(File.Open("mentions.txt", FileMode.OpenOrCreate));
                for (int i = 0; i < mentReader.BaseStream.Length; i += 8)
                {
                    mentionID.Add(mentReader.ReadUInt64());
                }
                mentionIDs = mentionID.ToArray();
                mentReader.Close();
                loadedMentions = true;
            }

            await Task.Delay(-1);
        }

    }
}
