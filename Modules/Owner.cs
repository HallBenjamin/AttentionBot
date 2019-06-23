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

                foreach (SocketTextChannel channel in Context.Client.GetGuild(guild.Id).TextChannels.ToList())
                {
                    channels.Add(channel.Id);
                }
            }

            foreach (ulong servID in Program.servChanID.Keys.ToList())
            {
                if (!guilds.Contains(servID) || !channels.Contains(Program.servChanID[servID]))
                {
                    Program.servChanID.Remove(servID);
                }
            }

            await Files.WriteToFile(Program.servChanID, "servers.txt", "channels.txt");

            // InterServer Chats
            foreach (ulong servID in Program.interServerChats.Keys.ToList())
            {
                if (!guilds.Contains(servID) || !channels.Contains(Program.interServerChats[servID]))
                {
                    Program.interServerChats.Remove(servID);
                }
            }

            await Files.WriteToFile(Program.interServerChats, "interservers.txt", "interchannels.txt");

            // InterServer Show Guild
            foreach (ulong servID in Program.showUserServer.ToList())
            {
                if (!guilds.Contains(servID))
                {
                    Program.showUserServer.Remove(servID);
                }
            }

            await Files.WriteToFile(Program.showUserServer, "show-guild.txt");

            // InterServer Broadcast Guild
            foreach (ulong servID in Program.broadcastServerName.ToList())
            {
                if (!guilds.Contains(servID))
                {
                    Program.broadcastServerName.Remove(servID);
                }
            }

            await Files.WriteToFile(Program.broadcastServerName, "broadcast-guild.txt");

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

            // WL Enable
            foreach (ulong serv in Program.wlEnable.ToList())
            {
                if (!guilds.Contains(serv))
                {
                    Program.wlEnable.Remove(serv);
                }
            }

            await Files.WriteToFile(Program.wlEnable, "wlenabled.txt");

            // Whitelist
            foreach (ulong serv in Program.ischatWhitelist.Keys.ToList())
            {
                if (Program.ischatWhitelist[serv].Count() == 0 || !guilds.Contains(serv))
                {
                    Program.ischatWhitelist.Remove(serv);
                    File.Delete($"wl-{serv}.txt");
                }
            }

            await Files.WriteToFile(Program.ischatWhitelist.Keys.ToList(), "wlserver.txt");

            // Blacklist
            foreach (ulong serv in Program.ischatBlacklist.Keys.ToList())
            {
                if (Program.ischatBlacklist[serv].Count() == 0 || !guilds.Contains(serv))
                {
                    Program.ischatBlacklist.Remove(serv);
                    File.Delete($"bl-{serv}.txt");
                }
            }

            await Files.WriteToFile(Program.ischatBlacklist.Keys.ToList(), "blserver.txt");

            // End
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

            List<Task> sendMessage = new List<Task>();

            foreach (ulong serv in Program.servChanID.Keys.ToList())
            {
                SocketGuild guild = Context.Client.GetGuild(serv);
                SocketTextChannel channel = guild.GetTextChannel(Program.servChanID[serv]);

                if (PermissionChecker.HasSend(guild, channel))
                {
                    sendMessage.Add(channel.SendMessageAsync($"Attention! {announceMessage}"));
                }
            }

            await Task.WhenAll(sendMessage.ToArray());
        }

        [Command("restart")]
        [RequireOwner]
        public async Task RestartWarning(string _time = "5", string _reason = null, string _length = null)
        {
            await CleanupFiles(false);

            string restart = "restart";

            if (_reason != null)
            {
                _reason = $" due to {_reason}";
            }

            if (_length != null)
            {
                _length = $" for {_length} hours";
                restart = "shut down";
            }

            List<Task> sendMessage = new List<Task>();

            foreach (ulong serv in Program.servChanID.Keys.ToList())
            {
                SocketGuild guild = Context.Client.GetGuild(serv);
                SocketTextChannel channel = guild.GetTextChannel(Program.servChanID[serv]);

                if (PermissionChecker.HasSend(guild, channel))
                {
                    sendMessage.Add(channel.SendMessageAsync($"Attention! Bot's server will {restart} in {_time} minutes{_length}{_reason}."));
                }
            }

            await Task.WhenAll(sendMessage.ToArray());
        }

        [Command("exit")]
        [RequireOwner]
        public async Task ExitAttentionBot(string _reason = null, string _length = null)
        {
            await CleanupFiles(false);
            if (_reason != null)
            {
                _reason = $" due to {_reason}";
            }

            if (_length == null)
            {
                _length = "restarting";
            }
            else
            {
                _length = $"offline for {_length} hours";
            }

            List<Task> sendMessage = new List<Task>();

            foreach (ulong serv in Program.servChanID.Keys.ToList())
            {
                SocketGuild guild = Context.Client.GetGuild(serv);
                SocketTextChannel channel = guild.GetTextChannel(Program.servChanID[serv]);

                if (PermissionChecker.HasSend(guild, channel))
                {
                    sendMessage.Add(channel.SendMessageAsync($"Attention! Bot's server is now {_length}{_reason}."));
                }
            }

            await Task.WhenAll(sendMessage.ToArray());

            if (Program.isConsole)
            {
                Console.WriteLine("Attention! Bot Offline");
            }

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
