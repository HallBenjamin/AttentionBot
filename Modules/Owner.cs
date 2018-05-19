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
        [Command("cleanup")]
        [RequireOwner]
        public async Task CleanupFiles(bool reply = true)
        {
            // Servers and channels
            List<ulong> guilds = new List<ulong>();
            List<ulong> channels = new List<ulong>();
            foreach (SocketGuild guild in Context.Client.Guilds.ToList())
            {
                guilds.Add(guild.Id);

                foreach (SocketTextChannel channel in Context.Client.GetGuild(guild.Id).Channels.ToList())
                {
                    channels.Add(channel.Id);
                }
            }

            foreach (ulong servID in Program.servChanID.Keys.ToList())
            {
                if (!guilds.Contains(servID) || !channels.Contains(Program.servChanID[servID]))
                {
                    Program.servChanID.Remove(servID);

                    await Files.WriteToFile(Program.servChanID, "servers.txt", "channels.txt");
                }
            }

            // Mentions
            foreach (ulong serv in Program.mentionID.ToList())
            {
                if (!guilds.Contains(serv))
                {
                    Program.mentionID.Remove(serv);
                }
            }

            await Files.WriteToFile(Program.mentionID, "mentions.txt");

            // Roles
            List<ulong> roles = new List<ulong>();
            foreach (SocketGuild server in Context.Client.Guilds.ToList())
            {
                foreach (ulong ID in Program.roleID.ToList())
                {
                    SocketRole role = server.Roles.FirstOrDefault(x => x.Id == ID);
                    if (server.Roles.Contains(role))
                    {
                        roles.Add(role.Id);
                    }
                }
            }
            Program.roleID = roles;

            await Files.WriteToFile(Program.roleID, "roles.txt");

            if (reply)
            {
                await Context.Channel.SendMessageAsync("Cleanup complete!");
            }
            else
            {
                await Task.Delay(0);
            }
        }

        [Command("reload")]
        [RequireOwner]
        public async Task Reload()
        {
            await Context.Channel.SendMessageAsync("Reloaded bot!");

            System.Diagnostics.Process.Start("AttentionBot.exe");

            Environment.Exit(0);
        }

        [Command("announcement")]
        [RequireOwner]
        public async Task Announcement(string announceMessage)
        {
            await CleanupFiles(false);

            foreach (ulong serv in Program.servChanID.Keys)
            {
                await Context.Client.GetGuild(serv).GetTextChannel(Program.servChanID[serv]).SendMessageAsync("Attention! " + announceMessage);
            }
        }

        [Command("restart")]
        [RequireOwner]
        public async Task RestartWarning(string _time = "2", string _botID = "all", string _length = null, string _reason = null)
        {
            await CleanupFiles(false);

            if (_length != null)
            {
                _length = " for " + _length + " hours.";
            }
            else
            {
                _length = ".";
            }
            if (_botID == SecurityInfo.botID)
            {
                foreach (ulong serv in Program.servChanID.Keys)
                {
                    await Context.Client.GetGuild(serv).GetTextChannel(Program.servChanID[serv]).SendMessageAsync("Attention! Bot will go offline for an update in " + _time + " minutes.");
                }
            }
            else if (_botID == "all")
            {
                if (_length != null)
                {
                    _length = " for " + _length + " hours";

                    if (_reason != null)
                    {
                        _reason = " due to " + _reason;
                    }
                }

                foreach (ulong serv in Program.servChanID.Keys)
                {
                    await Context.Client.GetGuild(serv).GetTextChannel(Program.servChanID[serv]).SendMessageAsync("Attention! Bot's server will shut down in " + _time + " minutes" + _length + _reason + ".");
                }
            }
        }

        [Command("exit")]
        [RequireOwner]
        public async Task ExitAttentionBot(string _botID = "all", string _length = null, string _reason = null)
        {
            await CleanupFiles(false);

            if (_botID == SecurityInfo.botID)
            {
                foreach (ulong serv in Program.servChanID.Keys)
                {
                    await Context.Client.GetGuild(serv).GetTextChannel(Program.servChanID[serv]).SendMessageAsync("Attention! Bot is now offline.");
                }
            }
            else if (_botID == "all")
            {
                if (_length == null)
                {
                    _length = "restarting";
                }
                else
                {
                    _length = "offline for " + _length + " hours";

                    if (_reason != null)
                    {
                        _reason = " due to " + _reason;
                    }

                }

                foreach (ulong serv in Program.servChanID.Keys)
                {
                    await Context.Client.GetGuild(serv).GetTextChannel(Program.servChanID[serv]).SendMessageAsync("Attention! Bot's server is now " + _length + _reason + ".");
                }

            }

            if (Program.isConsole)
                Console.WriteLine("Attention! Bot Offline");

            Thread.Sleep(1000);
            Environment.Exit(0);
        }

        [Command("close")]
        [RequireOwner]
        public async Task Close()
        {
            await CleanupFiles(false);

            await Context.Channel.SendMessageAsync("Attention! Bot shutting down...");
            Environment.Exit(0);
        }
    }
}
