using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AttentionBot.Modules
{
    public class InterServer : ModuleBase<SocketCommandContext>
    {
        public async Task<bool> HasAdmin()
        {
            bool hasAdmin = Context.Guild.GetUser(Context.User.Id).GuildPermissions.Has(GuildPermission.Administrator); // Is admin?

            return await Task.Run(() => hasAdmin);
        }

        [Command("interserver-settings")]
        public async Task InterServerChatSettings(string _botID = null)
        {
            if (_botID == SecurityInfo.botID || _botID == null)
            {
                bool hasAdmin = await HasAdmin();

                if (Context.User.Id == Context.Guild.OwnerId || hasAdmin)
                {
                    string interServerChan = Program.interServerChats.ContainsKey(Context.Guild.Id) ? Program.interServerChats[Context.Guild.Id].ToString()
                        : "No InterServer Chat channel has been assigned.\n\u200b";

                    string showUserServer = Program.showUserServer.Contains(Context.Guild.Id) ? "Enabled" : "Disabled";

                    string broadcastServerName = Program.broadcastServerName.Contains(Context.Guild.Id) ? "Enabled" : "Disabled";

                    string wlEnabled = Program.wlEnable.Contains(Context.Guild.Id) ? "Enabled" : "Disabled";

                    EmbedBuilder interServSettings = new EmbedBuilder();
                    interServSettings.WithColor(SecurityInfo.botColor);
                    interServSettings.WithTitle("__Current InterServer Chat Configuration__");

                    EmbedFieldBuilder interServChanEmb = new EmbedFieldBuilder();
                    interServChanEmb.WithIsInline(false);
                    interServChanEmb.WithName("InterServer Chat Channel");
                    if (interServerChan != "No InterServer Chat channel has been assigned.\n\u200b")
                    {
                        interServerChan = $"Name: {Context.Guild.GetChannel(Convert.ToUInt64(interServerChan)).Name}\n" +
                            $"ID: {interServerChan}\n\u200b";
                    }
                    interServChanEmb.WithValue(interServerChan);
                    interServSettings.AddField(interServChanEmb);

                    EmbedFieldBuilder wlEnabledEmb = new EmbedFieldBuilder();
                    wlEnabledEmb.WithIsInline(false);
                    wlEnabledEmb.WithName("Whitelist");
                    wlEnabledEmb.WithValue(wlEnabled + "\n\u200b");
                    interServSettings.AddField(wlEnabledEmb);

                    EmbedFieldBuilder showUserServEmb = new EmbedFieldBuilder();
                    showUserServEmb.WithIsInline(true);
                    showUserServEmb.WithName("Show User's Guild");
                    showUserServEmb.WithValue(showUserServer);
                    interServSettings.AddField(showUserServEmb);

                    EmbedFieldBuilder broadcastServNameEmb = new EmbedFieldBuilder();
                    broadcastServNameEmb.WithIsInline(true);
                    broadcastServNameEmb.WithName("Broadcast Guild Name");
                    broadcastServNameEmb.WithValue(broadcastServerName);
                    interServSettings.AddField(broadcastServNameEmb);

                    if (_botID == SecurityInfo.botID)
                    {
                        await Context.User.SendMessageAsync("", false, interServSettings.Build());
                    }
                    else
                    {
                        await Context.Channel.SendMessageAsync("", false, interServSettings.Build());
                    }
                }
            }
        }

        [Command("interserver-chat")]
        public async Task SetInterServerChat(string _chanID) // Channel ID given
        {
            bool hasAdmin = await HasAdmin();

            if (Context.User.Id == Context.Guild.OwnerId || hasAdmin)
            {
                if (_chanID == "-")
                {
                    if (Program.interServerChats.Keys.Contains(Context.Guild.Id))
                    {
                        Program.interServerChats.Remove(Context.Guild.Id);
                        await Files.WriteToFile(Program.interServerChats, "interservers.txt", "interchannels.txt");

                        await Context.Channel.SendMessageAsync("InterServer Chat disabled!");
                    }
                    else
                    {
                        await Context.Channel.SendMessageAsync("There is no InterServer Chat to disable.");
                    }
                }
                else if (Program.interServerChats.ContainsKey(Context.Guild.Id) && Program.interServerChats[Context.Guild.Id] == Convert.ToUInt64(_chanID))
                {
                    await Context.Channel.SendMessageAsync("Channel is already setup for InterServer Chat.");
                }
                else if (Context.Guild.TextChannels.Contains(Context.Guild.TextChannels.FirstOrDefault(x => x.Id == Convert.ToUInt64(_chanID))))
                {
                    Program.interServerChats.Put(Context.Guild.Id, Convert.ToUInt64(_chanID));
                    await Files.WriteToFile(Program.interServerChats, "interservers.txt", "interchannels.txt");

                    await Context.Channel.SendMessageAsync($"The InterServer Chat is now {(Context.Guild.GetChannel(Convert.ToUInt64(_chanID)) as SocketTextChannel)?.Mention} with ID {_chanID}.");
                }
                else
                {
                    await Context.Channel.SendMessageAsync("Error: Invalid channel ID given. Please give a valid ID of a text channel on the server.");
                }
            }
            else
            {
                await Context.Channel.SendMessageAsync("Error: You are not the Server Owner or an admin and cannot use this command.");
            }
        }

        [Command("interserver-chat")]
        public async Task SetInterServerChat(SocketTextChannel _channel) // Channel mention given
        {
            bool hasAdmin = await HasAdmin();

            if (Context.User.Id == Context.Guild.OwnerId || hasAdmin)
            {
                if (Program.interServerChats.ContainsKey(Context.Guild.Id) && Program.interServerChats[Context.Guild.Id] == _channel.Id)
                {
                    await Context.Channel.SendMessageAsync("Channel is already setup for InterServer Chat.");
                }
                else
                {
                    Program.interServerChats.Put(Context.Guild.Id, Convert.ToUInt64(_channel.Id));
                    await Files.WriteToFile(Program.interServerChats, "interservers.txt", "interchannels.txt");

                    await Context.Channel.SendMessageAsync($"The InterServer Chat is now {_channel.Mention} with ID {_channel.Id}.");
                }
            }
            else
            {
                await Context.Channel.SendMessageAsync("Error: You are not the Server Owner or an admin and cannot use this command.");
            }
        }

        [Command("display-user-guild")]
        public async Task DisplayUserGuild(string _showGuild)
        {
            bool hasAdmin = await HasAdmin();

            if (Context.User.Id == Context.Guild.OwnerId || hasAdmin)
            {
                string _showGuildEnabled;

                if (_showGuild == "0")
                {
                    if (Program.showUserServer.Contains(Context.Guild.Id))
                    {
                        Program.showUserServer.Remove(Context.Guild.Id);
                        _showGuildEnabled = "Hidden";

                        await Files.WriteToFile(Program.showUserServer, "show-guild.txt");
                    }
                    else
                    {
                        _showGuildEnabled = "already hidden";
                    }
                }
                else if (_showGuild == "1")
                {
                    if (Program.showUserServer.Contains(Context.Guild.Id))
                    {
                        _showGuildEnabled = "already visible";
                    }
                    else
                    {
                        Program.showUserServer.Add(Context.Guild.Id);
                        _showGuildEnabled = "Visible";

                        await Files.WriteToFile(Program.showUserServer, "show-guild.txt");
                    }
                }
                else
                {
                    await Context.Channel.SendMessageAsync("Invalid Parameter. Valid parameters are \"0\" and \"1\".");
                    return;
                }

                await Context.Channel.SendMessageAsync($"User guilds {_showGuildEnabled}!");
            }
        }

        [Command("broadcast-guild-name")]
        public async Task BroadcastGuildName(string _broadcast)
        {
            bool hasAdmin = await HasAdmin();

            if (Context.User.Id == Context.Guild.OwnerId || hasAdmin)
            {
                string _broadcastEnabled;

                if (_broadcast == "0")
                {
                    if (Program.broadcastServerName.Contains(Context.Guild.Id))
                    {
                        Program.broadcastServerName.Remove(Context.Guild.Id);
                        _broadcastEnabled = "Hidden";

                        await Files.WriteToFile(Program.broadcastServerName, "broadcast-guild.txt");
                    }
                    else
                    {
                        _broadcastEnabled = "already hidden";
                    }
                }
                else if (_broadcast == "1")
                {
                    if (Program.broadcastServerName.Contains(Context.Guild.Id))
                    {
                        _broadcastEnabled = "already visible";
                    }
                    else
                    {
                        Program.broadcastServerName.Add(Context.Guild.Id);
                        _broadcastEnabled = "Visible";

                        await Files.WriteToFile(Program.broadcastServerName, "broadcast-guild.txt");
                    }
                }
                else
                {
                    await Context.Channel.SendMessageAsync("Invalid Parameter. Valid parameters are \"0\" and \"1\".");
                    return;
                }

                await Context.Channel.SendMessageAsync($"Server name {_broadcastEnabled}!");
            }
        }

        [Command("enable-whitelist")]
        public async Task EnableWhitelistOnly(string enable)
        {
            bool hasAdmin = await HasAdmin();

            if (Context.User.Id == Context.Guild.OwnerId || hasAdmin)
            {
                string _enable;

                if (enable == "0")
                {
                    if (Program.wlEnable.Contains(Context.Guild.Id))
                    {
                        Program.wlEnable.Remove(Context.Guild.Id);
                        _enable = "Disabled";

                        await Files.WriteToFile(Program.wlEnable, "wlenabled.txt");
                    }
                    else
                    {
                        _enable = "already disabled";
                    }
                }
                else if (enable == "1")
                {
                    if (Program.wlEnable.Contains(Context.Guild.Id))
                    {
                        _enable = "already enabled";
                    }
                    else
                    {
                        Program.wlEnable.Add(Context.Guild.Id);
                        _enable = "Enabled";

                        await Files.WriteToFile(Program.wlEnable, "wlenabled.txt");
                    }
                }
                else
                {
                    await Context.Channel.SendMessageAsync("Invalid Parameter. Valid parameters are \"0\" and \"1\".");
                    return;
                }

                await Context.Channel.SendMessageAsync($"InterServer Chat Whitelist {_enable}!");
            }
        }

        [Command("whitelist")]
        public async Task Whitelist(string serverID)
        {
            bool hasAdmin = await HasAdmin();

            if (Context.User.Id == Context.Guild.OwnerId || hasAdmin)
            {
                ulong id = Convert.ToUInt64(serverID);

                string action = "added to";

                if (Program.ischatWhitelist.ContainsKey(Context.Guild.Id))
                {
                    List<ulong> wlIDs = Program.ischatWhitelist[Context.Guild.Id];

                    if (wlIDs.Contains(id))
                    {
                        wlIDs.Remove(id);
                        action = "removed from";
                    }
                    else
                    {
                        wlIDs.Add(id);
                    }

                    Program.ischatWhitelist[Context.Guild.Id] = wlIDs;
                }
                else
                {
                    List<ulong> wl = new List<ulong>
                    {
                        id
                    };

                    Program.ischatWhitelist.Add(Context.Guild.Id, wl);
                }

                await Files.WriteDictListToFile(Program.ischatWhitelist, "wlserver.txt", "wl-");

                await Context.Channel.SendMessageAsync($"Server {action} the whitelist!");
            }
        }

        [Command("blacklist")]
        public async Task Blacklist(string serverID)
        {
            bool hasAdmin = await HasAdmin();

            string action = "added to";

            if (Context.User.Id == Context.Guild.OwnerId || hasAdmin)
            {
                ulong id = Convert.ToUInt64(serverID);

                if (Program.ischatBlacklist.ContainsKey(Context.Guild.Id))
                {
                    List<ulong> blIDs = Program.ischatBlacklist[Context.Guild.Id];

                    if (blIDs.Contains(id))
                    {
                        blIDs.Remove(id);
                        action = "removed from";
                    }
                    else
                    {
                        blIDs.Add(id);
                    }

                    Program.ischatBlacklist[Context.Guild.Id] = blIDs;
                }
                else
                {
                    List<ulong> bl = new List<ulong>
                    {
                        id
                    };

                    Program.ischatBlacklist.Add(Context.Guild.Id, bl);
                }

                await Files.WriteDictListToFile(Program.ischatBlacklist, "blserver.txt", "bl-");

                await Context.Channel.SendMessageAsync($"Server {action} the blacklist!");
            }
        }
    }
}
