using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttentionBot.Modules
{
    public class InterServer : ModuleBase<SocketCommandContext>
    {
        public async Task<bool> HasRole()
        {
            bool hasRole = Context.Guild.GetUser(Context.User.Id).GuildPermissions.Has(GuildPermission.Administrator); // Is admin?
            foreach (ulong role in Program.roleID)
            {
                SocketRole socketRole = Context.Guild.Roles.FirstOrDefault(x => x.Id == role);
                hasRole = hasRole ? hasRole : Context.Guild.GetUser(Context.User.Id).Roles.Contains(socketRole); // If already true, ignore
                if (hasRole)
                {
                    break;
                }
            }

            return await Task.Run(() =>
            {
                return hasRole;
            });
        }

        [Command("interserver-settings")]
        public async Task InterServerChatSettings(string _botID = null)
        {
            if (_botID == SecurityInfo.botID || _botID == null)
            {
                bool hasRole = await HasRole();

                if (Context.User.Id == Context.Guild.OwnerId || hasRole)
                {
                    string interServerChan = Program.interServerChats.ContainsKey(Context.Guild.Id) ? Program.interServerChats[Context.Guild.Id].ToString()
                        : "No InterServer Chat channel has been assigned.\n\u200b";

                    string showUserServer = Program.showUserServer.Contains(Context.Guild.Id) ? "Enabled" : "Disabled";

                    string broadcastServerName = Program.broadcastServerName.Contains(Context.Guild.Id) ? "Enabled" : "Disabled";

                    EmbedBuilder interServSettings = new EmbedBuilder();
                    interServSettings.WithColor(SecurityInfo.botColor);
                    interServSettings.WithTitle("__Current InterServer Chat Configuration__");

                    EmbedFieldBuilder interServChanEmb = new EmbedFieldBuilder();
                    interServChanEmb.WithIsInline(false);
                    interServChanEmb.WithName("InterServer Chat Channel");
                    if (interServerChan != "No InterServer Chat channel has been assigned.\n\u200b")
                    {
                        interServerChan = "Name: " + Context.Guild.GetChannel(Convert.ToUInt64(interServerChan)).Name + "\nID: " + interServerChan + "\n\u200b";
                    }
                    interServChanEmb.WithValue(interServerChan);
                    interServSettings.AddField(interServChanEmb);

                    EmbedFieldBuilder showUserServEmb = new EmbedFieldBuilder();
                    showUserServEmb.WithIsInline(false);
                    showUserServEmb.WithName("Show User's Guild");
                    showUserServEmb.WithValue(showUserServer + "\n\u200b");
                    interServSettings.AddField(showUserServEmb);
                    
                    EmbedFieldBuilder broadcastServNameEmb = new EmbedFieldBuilder();
                    broadcastServNameEmb.WithIsInline(false);
                    broadcastServNameEmb.WithName("Broadcast Guild Name");
                    broadcastServNameEmb.WithValue(broadcastServerName);
                    interServSettings.AddField(broadcastServNameEmb);

                    await Context.Channel.SendMessageAsync("", false, interServSettings);
                }
            }
        }

        [Command("interserver-chat")]
        public async Task SetInterServerChat(string _chanID) // Channel ID given
        {
            bool hasRole = await HasRole();

            if (Context.User.Id == Context.Guild.OwnerId || hasRole)
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

                    await Context.Channel.SendMessageAsync("The InterServer Chat is now " + (Context.Guild.GetChannel(Convert.ToUInt64(_chanID)) as SocketTextChannel).Mention + " with ID " + _chanID + ".");
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
            bool hasRole = await HasRole();

            if (Context.User.Id == Context.Guild.OwnerId || hasRole)
            {
                if (Program.interServerChats.ContainsKey(Context.Guild.Id) && Program.interServerChats[Context.Guild.Id] == _channel.Id)
                {
                    await Context.Channel.SendMessageAsync("Channel is already setup for InterServer Chat.");
                }
                else
                {
                    Program.interServerChats.Put(Context.Guild.Id, Convert.ToUInt64(_channel.Id));
                    await Files.WriteToFile(Program.interServerChats, "interservers.txt", "interchannels.txt");

                    await Context.Channel.SendMessageAsync("The InterServer Chat is now " + _channel.Mention + " with ID " + _channel.Id + ".");
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
            bool hasRole = await HasRole();

            if (Context.User.Id == Context.Guild.OwnerId || hasRole)
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

                await Context.Channel.SendMessageAsync("User guilds " + _showGuildEnabled + "!");
            }
        }

        [Command("broadcast-guild-name")]
        public async Task BroadcastGuildName(string _broadcast)
        {
            bool hasRole = await HasRole();

            if (Context.User.Id == Context.Guild.OwnerId || hasRole)
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

                await Context.Channel.SendMessageAsync("Server name " + _broadcastEnabled + "!");
            }
        }
    }
}
