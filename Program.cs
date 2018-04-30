using System;
using Discord;
using Discord.WebSocket;
using System.Threading.Tasks;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace AttentionBot
{
    public class Program
    {
        static void Main()
            => new Program().StartAsync().GetAwaiter().GetResult();

        private DiscordSocketConfig _config;
        private DiscordSocketClient _client;
        private CommandHandler _handler;

        public static bool isConsole = Console.OpenStandardInput(1) != Stream.Null;
        
        public static List<ulong> roleID = new List<ulong>();
        public static List<ulong> mentionID = new List<ulong>();
        public static Dictionary<ulong, ulong> servChanID = new Dictionary<ulong, ulong>();
        
        private bool loadedServChans = false;
        private bool loadedRoles = false;
        private bool loadedMentions = false;

        [DllImport("User32.dll", CharSet = CharSet.Unicode)]
        public static extern int MessageBox(IntPtr h, string m, string c, int type);

        public async Task StartAsync()
        {
            if (isConsole)
            {
                Console.Title = "Attention! Bot for Discord";
            }

            bool isRunning = System.Diagnostics.Process.GetProcessesByName(System.Diagnostics.Process.GetCurrentProcess().ProcessName).Count() > 1;
            if (isRunning)
            {
                await Task.Delay(1000);
                isRunning = System.Diagnostics.Process.GetProcessesByName(System.Diagnostics.Process.GetCurrentProcess().ProcessName).Count() > 1;

                if(isRunning)
                {
                    MessageBox((IntPtr) 0, "Program is already running", "Attention! Bot for Discord", 0);
                    return;
                }
            }

            _config = new DiscordSocketConfig();
            _config.AlwaysDownloadUsers = false;

            _client = new DiscordSocketClient(_config);

            await _client.LoginAsync(TokenType.Bot, SecurityInfo.token);
            await _client.StartAsync();

            await _client.SetGameAsync("Attention! \\help 3949");

            _handler = new CommandHandler(_client);

            if (isConsole)
            {
                Console.WriteLine("Attention! Bot Online");
            }
            
            if (!loadedServChans)
            {
                List<ulong> chanID = new List<ulong>();
                List<ulong> servID = new List<ulong>();

                String chanString;
                BinaryReader chanReader = new BinaryReader(File.Open("channels.txt", FileMode.OpenOrCreate));
                for (int i = 0; i < chanReader.BaseStream.Length; i += chanString.Length + 1)
                {
                    chanString = chanReader.ReadString();
                    chanID.Add(Convert.ToUInt64(chanString));
                }
                chanReader.Close();

                String servString;
                BinaryReader servReader = new BinaryReader(File.Open("servers.txt", FileMode.OpenOrCreate));
                for (int i = 0; i < servReader.BaseStream.Length; i += servString.Length + 1)
                {
                    servString = servReader.ReadString();
                    servID.Add(Convert.ToUInt64(servString));
                }
                servReader.Close();

                for (int i = 0; i < servID.Count; i++)
                {
                    servChanID.Add(servID[i], chanID[i]);
                }

                loadedServChans = true;
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
