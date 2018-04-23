using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            List<ulong> guilds = new List<ulong>();
            List<ulong> channels = new List<ulong>();
            foreach (SocketGuild guild in Context.Client.Guilds)
            {
                guilds.Add(guild.Id);

                foreach (SocketGuildChannel channel in Context.Client.GetGuild(guild.Id).Channels)
                {
                    channels.Add(channel.Id);
                }
            }

            for (int i = 0; i < Program.servID.ToList().Count; i++)
            {
                if (!guilds.Contains(Program.servID[i]) || !channels.Contains(Program.chanID[i]))
                {
                    Program.chanID.Remove(Program.chanID[i]);
                    Program.servID.Remove(Program.servID[i]);

                    BinaryWriter chanWriter = new BinaryWriter(File.Open("channels.txt", FileMode.Truncate));
                    foreach (var value in Program.chanID)
                    {
                        chanWriter.Write(value.ToString());
                    }
                    chanWriter.Close();

                    BinaryWriter servWriter = new BinaryWriter(File.Open("servers.txt", FileMode.Truncate));
                    foreach (var value in Program.servID)
                    {
                        servWriter.Write(value.ToString());
                    }
                    servWriter.Close();
                }
            }

            for (int i = 0; i < Program.chanID.Count; i++)
            {
                await Context.Client.GetGuild(Program.servID[i]).GetTextChannel(Program.chanID[i]).SendMessageAsync("Attention! Bot is now online.");
            }
        }

        [Command("announcement")]
        [RequireOwner]
        public async Task announcement(string announceMessage)
        {
            List<ulong> guilds = new List<ulong>();
            List<ulong> channels = new List<ulong>();
            foreach (SocketGuild guild in Context.Client.Guilds)
            {
                guilds.Add(guild.Id);

                foreach (SocketGuildChannel channel in Context.Client.GetGuild(guild.Id).Channels)
                {
                    channels.Add(channel.Id);
                }
            }

            for (int i = 0; i < Program.servID.ToList().Count; i++)
            {
                if (!guilds.Contains(Program.servID[i]) || !channels.Contains(Program.chanID[i]))
                {
                    Program.chanID.Remove(Program.chanID[i]);
                    Program.servID.Remove(Program.servID[i]);

                    BinaryWriter chanWriter = new BinaryWriter(File.Open("channels.txt", FileMode.Truncate));
                    foreach (var value in Program.chanID)
                    {
                        chanWriter.Write(value.ToString());
                    }
                    chanWriter.Close();

                    BinaryWriter servWriter = new BinaryWriter(File.Open("servers.txt", FileMode.Truncate));
                    foreach (var value in Program.servID)
                    {
                        servWriter.Write(value.ToString());
                    }
                    servWriter.Close();
                }
            }

            for (int i = 0; i < Program.chanID.Count; i++)
            {
                await Context.Client.GetGuild(Program.servID[i]).GetTextChannel(Program.chanID[i]).SendMessageAsync("Attention! " + announceMessage);
            }
        }

        [Command("restart")]
        [RequireOwner]
        public async Task restartWarning(string _time = "2", string _botID = "all", string _length = null, string _reason = null)
        {
            List<ulong> guilds = new List<ulong>();
            List<ulong> channels = new List<ulong>();
            foreach (SocketGuild guild in Context.Client.Guilds)
            {
                guilds.Add(guild.Id);

                foreach (SocketGuildChannel channel in Context.Client.GetGuild(guild.Id).Channels)
                {
                    channels.Add(channel.Id);
                }
            }

            for (int i = 0; i < Program.servID.ToList().Count; i++)
            {
                if (!guilds.Contains(Program.servID[i]) || !channels.Contains(Program.chanID[i]))
                {
                    Program.chanID.Remove(Program.chanID[i]);
                    Program.servID.Remove(Program.servID[i]);

                    BinaryWriter chanWriter = new BinaryWriter(File.Open("channels.txt", FileMode.Truncate));
                    foreach (var value in Program.chanID)
                    {
                        chanWriter.Write(value.ToString());
                    }
                    chanWriter.Close();

                    BinaryWriter servWriter = new BinaryWriter(File.Open("servers.txt", FileMode.Truncate));
                    foreach (var value in Program.servID)
                    {
                        servWriter.Write(value.ToString());
                    }
                    servWriter.Close();
                }
            }

            if (_botID == Program.botID)
            {
                for (int i = 0; i < Program.chanID.Count; i++)
                {
                    await Context.Client.GetGuild(Program.servID[i]).GetTextChannel(Program.chanID[i]).SendMessageAsync("Attention! Bot will go offline for an update in " + _time + " minutes.");
                }
            }
            else if (_botID == "all")
            {
                if (_length == null)
                {
                    for (int i = 0; i < Program.chanID.Count; i++)
                    {
                        await Context.Client.GetGuild(Program.servID[i]).GetTextChannel(Program.chanID[i]).SendMessageAsync("Attention! Bot's server will restart in " + _time + " minutes.");
                    }
                }
                else
                {
                    if (_reason == null)
                    {
                        for (int i = 0; i < Program.chanID.Count; i++)
                        {
                            await Context.Client.GetGuild(Program.servID[i]).GetTextChannel(Program.chanID[i]).SendMessageAsync("Attention! Bot's server will shut down in " + _time + " minutes for " + _length + " hours.");
                        }
                    }
                    else
                    {
                        for (int i = 0; i < Program.chanID.Count; i++)
                        {
                            await Context.Client.GetGuild(Program.servID[i]).GetTextChannel(Program.chanID[i]).SendMessageAsync("Attention! Bot's server will shut down in " + _time + " minutes for " + _length + " hours due to " + _reason + ".");
                        }
                    }
                }
            }
        }

        [Command("exit")]
        [RequireOwner]
        public async Task exitAttentionBot(string _botID = "all", string _length = null, string _reason = null)
        {
            List<ulong> guilds = new List<ulong>();
            List<ulong> channels = new List<ulong>();
            foreach (SocketGuild guild in Context.Client.Guilds)
            {
                guilds.Add(guild.Id);

                foreach (SocketGuildChannel channel in Context.Client.GetGuild(guild.Id).Channels)
                {
                    channels.Add(channel.Id);
                }
            }

            for (int i = 0; i < Program.servID.ToList().Count; i++)
            {
                if (!guilds.Contains(Program.servID[i]) || !channels.Contains(Program.chanID[i]))
                {
                    Program.chanID.Remove(Program.chanID[i]);
                    Program.servID.Remove(Program.servID[i]);

                    BinaryWriter chanWriter = new BinaryWriter(File.Open("channels.txt", FileMode.Truncate));
                    foreach (var value in Program.chanID)
                    {
                        chanWriter.Write(value.ToString());
                    }
                    chanWriter.Close();

                    BinaryWriter servWriter = new BinaryWriter(File.Open("servers.txt", FileMode.Truncate));
                    foreach (var value in Program.servID)
                    {
                        servWriter.Write(value.ToString());
                    }
                    servWriter.Close();
                }
            }

            if (_botID == Program.botID)
            {
                for (int i = 0; i < Program.chanID.Count; i++)
                {
                    await Context.Client.GetGuild(Program.servID[i]).GetTextChannel(Program.chanID[i]).SendMessageAsync("Attention! Bot is now offline.");
                }
            }
            else if (_botID == "all")
            {
                if (_length == null)
                {
                    for (int i = 0; i < Program.chanID.Count; i++)
                    {
                        await Context.Client.GetGuild(Program.servID[i]).GetTextChannel(Program.chanID[i]).SendMessageAsync("Attention! Bot's server is now restarting.");
                    }
                }
                else
                {
                    if (_reason == null)
                    {
                        for (int i = 0; i < Program.chanID.Count; i++)
                        {
                            await Context.Client.GetGuild(Program.servID[i]).GetTextChannel(Program.chanID[i]).SendMessageAsync("Attention! Bot's server is now offline for " + _length + " hours.");
                        }
                    }
                    else
                    {
                        for (int i = 0; i < Program.chanID.Count; i++)
                        {
                            await Context.Client.GetGuild(Program.servID[i]).GetTextChannel(Program.chanID[i]).SendMessageAsync("Attention! Bot's server is now offline for " + _length + " hours due to " + _reason + ".");
                        }
                    }

                }

            }

            if (Program.isConsole)
                Console.WriteLine("Attention! Bot Offline");

            Thread.Sleep(1000);
            Environment.Exit(0);
        }
    }
}
