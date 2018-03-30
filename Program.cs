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
        static void Main()
            => new Program().StartAsync().GetAwaiter().GetResult();

        String token = "Removed for Security";

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
            {
                Console.WriteLine("Attention! Bot Online");
            }

            if (!loadedChans)
            {
                String chanString;
                BinaryReader chanReader = new BinaryReader(File.Open("channels.txt", FileMode.OpenOrCreate));
                for (int i = 0; i < chanReader.BaseStream.Length; i += chanString.Length + 1)
                {
                    chanString = chanReader.ReadString();
                    chanID.Add(Convert.ToUInt64(chanString));
                }
                chanIDs = chanID.ToArray();
                chanReader.Close();
                loadedChans = true;
            }
            
            if (!loadedServs)
            {
                String servString;
                BinaryReader servReader = new BinaryReader(File.Open("servers.txt", FileMode.OpenOrCreate));
                for (int i = 0; i < servReader.BaseStream.Length; i += servString.Length + 1)
                {
                    servString = servReader.ReadString();
                    servID.Add(Convert.ToUInt64(servString));
                }
                servIDs = servID.ToArray();
                servReader.Close();
                loadedServs = true;
            }

            if (!loadedRoles)
            {
                String roleString;
                BinaryReader roleReader = new BinaryReader(File.Open("roles.txt", FileMode.OpenOrCreate));
                for (int i = 0; i < roleReader.BaseStream.Length; i += roleString.Length + 1)
                {
                    roleString = roleReader.ReadString();
                    roleID.Add(Convert.ToUInt64(roleString));
                }
                roleIDs = roleID.ToArray();
                roleReader.Close();
                loadedRoles = true;
            }

            if (!loadedMentions)
            {
                String mentionString;
                BinaryReader mentReader = new BinaryReader(File.Open("mentions.txt", FileMode.OpenOrCreate));
                for (int i = 0; i < mentReader.BaseStream.Length; i += mentionString.Length + 1)
                {
                    mentionString = mentReader.ReadString();
                    mentionID.Add(Convert.ToUInt64(mentionString));
                }
                mentionIDs = mentionID.ToArray();
                mentReader.Close();
                loadedMentions = true;
            }

            if (isConsole)
            {
                Console.WriteLine("Attention! Bot has finished Loading");
            }

            await Task.Delay(-1);
        }

    }
}
