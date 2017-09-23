using Discord.Commands;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace AttentionBot.Modules
{
    public class Owner : ModuleBase<SocketCommandContext>
    {
        [Command("online")]
        [RequireOwner]
        public async Task onlineNotify()
        {
            for(int i = 0; i < Program.chanIDs.Length; i++)
            {
                await Context.Client.GetGuild(Program.servIDs[i]).GetTextChannel(Program.chanIDs[i]).SendMessageAsync("Attention! Bot is now online.");
            }
        }

        [Command("announcement")]
        [RequireOwner]
        public async Task announcement(string announceMessage)
        {
            for (int i = 0; i < Program.chanIDs.Length; i++)
            {
                await Context.Client.GetGuild(Program.servIDs[i]).GetTextChannel(Program.chanIDs[i]).SendMessageAsync(announceMessage);
            }
        }

        [Command("restart")]
        [RequireOwner]
        public async Task restartWarning(string time = "2", string _botID = null)
        {
            if (_botID == Program.botID)
            {
                for (int i = 0; i < Program.chanIDs.Length; i++)
                {
                    await Context.Client.GetGuild(Program.servIDs[i]).GetTextChannel(Program.chanIDs[i]).SendMessageAsync("Attention! Bot will go offline for an update in " + time + " minutes.");
                }
            }
            else if (_botID == null)
            {
                for (int i = 0; i < Program.chanIDs.Length; i++)
                {
                    await Context.Client.GetGuild(Program.servIDs[i]).GetTextChannel(Program.chanIDs[i]).SendMessageAsync("Attention! Bot's server will restart in " + time + " minutes.");
                }
            }
        }

        [Command("exit")]
        [RequireOwner]
        public async Task exitAttentionBot(string _botID = "all", string _length = null)
        {
            if (_botID == Program.botID || _botID == "all")
            {
                Program.chanIDs = Program.chanID.ToArray();
                Program.servIDs = Program.servID.ToArray();
                Program.roleIDs = Program.roleID.ToArray();

                if(_botID != "all")
                {
                    for (int i = 0; i < Program.chanIDs.Length; i++)
                    {
                        await Context.Client.GetGuild(Program.servIDs[i]).GetTextChannel(Program.chanIDs[i]).SendMessageAsync("Attention! Bot is now offline.");
                    }
                }
                else
                {
                    if(_length != null)
                    {
                        for (int i = 0; i < Program.chanIDs.Length; i++)
                        {
                            await Context.Client.GetGuild(Program.servIDs[i]).GetTextChannel(Program.chanIDs[i]).SendMessageAsync("Attention! Bot's server is now offline for " + _length + " hours.");
                        }
                    } else
                    {
                        for (int i = 0; i < Program.chanIDs.Length; i++)
                        {
                            await Context.Client.GetGuild(Program.servIDs[i]).GetTextChannel(Program.chanIDs[i]).SendMessageAsync("Attention! Bot's server is now offline.");
                        }
                    }
                }

                if (Program.isConsole)
                    Console.WriteLine("Attention! Bot Offline");

                BinaryWriter writer = new BinaryWriter(File.Open("channels.txt", FileMode.Open));
                foreach (var value in Program.chanIDs)
                {
                    writer.Write(value);
                }
                writer.Close();

                BinaryWriter writers = new BinaryWriter(File.Open("servers.txt", FileMode.Open));
                foreach (var value in Program.servIDs)
                {
                    writers.Write(value);
                }
                writers.Close();

                BinaryWriter roler = new BinaryWriter(File.Open("roles.txt", FileMode.Open));
                foreach (var value in Program.roleIDs)
                {
                    roler.Write(value);
                }
                roler.Close();

                Thread.Sleep(1000);
                Environment.Exit(0);
            }
        }
    }
}
