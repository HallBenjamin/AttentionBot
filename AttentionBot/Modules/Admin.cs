using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AttentionBot.Modules
{
    public class Admin : ModuleBase<SocketCommandContext>
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

        public async Task<bool> HasAdmin()
        {
            bool hasAdmin = Context.Guild.GetUser(Context.User.Id).GuildPermissions.Has(GuildPermission.Administrator);

            return await Task.Run(() =>
            {
                return hasAdmin;
            });
        }

        [Command("settings")]
        public async Task GetSettings(string _botID = null)
        {
            if (_botID == SecurityInfo.botID || _botID == null)
            {
                bool hasRole = await HasRole();

                if (Context.User.Id == Context.Guild.OwnerId || hasRole)
                {
                    string announceChan = Program.servChanID.ContainsKey(Context.Guild.Id) ? Program.servChanID[Context.Guild.Id].ToString()
                        : "No announcements channel has been assigned.\n\u200b";

                    List<ulong> adminRoles = new List<ulong>();
                    foreach (ulong role in Program.roleID)
                    {
                        SocketRole socketRole = Context.Guild.Roles.FirstOrDefault(x => x.Id == role);
                        if (Context.Guild.Roles.Contains(socketRole))
                        {
                            adminRoles.Add(role);
                        }
                    }
                    string mentions = Program.mentionID.Contains(Context.Guild.Id) ? "Enabled" : "Disabled";

                    EmbedBuilder servSettings = new EmbedBuilder();
                    servSettings.WithColor(SecurityInfo.botColor);
                    servSettings.WithTitle("__Current Bot Configuration__");

                    EmbedFieldBuilder announceEmb = new EmbedFieldBuilder();
                    announceEmb.WithIsInline(false);
                    announceEmb.WithName("Announcements Channel");
                    if (announceChan != "No announcements channel has been assigned.\n\u200b")
                    {
                        announceChan = $"Name: {Context.Guild.GetChannel(Convert.ToUInt64(announceChan)).Name}\n" +
                            $"ID: {announceChan}\n\u200b";
                    }
                    announceEmb.WithValue(announceChan);
                    servSettings.AddField(announceEmb);

                    EmbedFieldBuilder adminEmb = new EmbedFieldBuilder();
                    adminEmb.WithIsInline(false);
                    adminEmb.WithName("Admin Roles");
                    string roles = adminRoles.Count == 0 ? "No admin roles have been assigned.\n\u200b" : "";
                    foreach (ulong role in adminRoles)
                    {
                        roles += $"Name: {Context.Guild.GetRole(role).Name}\n" +
                            $"ID: {role}\n\u200b\n";
                    }
                    adminEmb.WithValue(roles);
                    servSettings.AddField(adminEmb);

                    EmbedFieldBuilder mentionEmb = new EmbedFieldBuilder();
                    mentionEmb.WithIsInline(false);
                    mentionEmb.WithName("User Mentions");
                    mentionEmb.WithValue(mentions);
                    servSettings.AddField(mentionEmb);

                    if (_botID == SecurityInfo.botID)
                    {
                        await Context.User.SendMessageAsync("", false, servSettings.Build());
                    }
                    else
                    {
                        await Context.Channel.SendMessageAsync("", false, servSettings.Build());
                    }
                }
            }
        }

        [Command("admin")]
        public async Task AdminRoles(string _roleID) // Role ID given
        {
            bool hasAdmin = await HasAdmin();

            if (Context.User.Id == Context.Guild.OwnerId || hasAdmin)
            {
                bool alreadyExists = Program.roleID.Contains(Convert.ToUInt64(_roleID));

                if (!alreadyExists && Context.Guild.Roles.Contains(Context.Guild.Roles.FirstOrDefault(x => x.Id == Convert.ToUInt64(_roleID))))
                {
                    Program.roleID.Add(Convert.ToUInt64(_roleID));
                    await Files.WriteToFile(Program.roleID, "roles.txt");

                    await Context.Channel.SendMessageAsync($"\"{Context.Guild.GetRole(Convert.ToUInt64(_roleID)).Name}\" role with ID {_roleID} has been added as an administrative role.");
                }
                else if (alreadyExists)
                {
                    Program.roleID.Remove(Convert.ToUInt64(_roleID));
                    await Files.WriteToFile(Program.roleID, "roles.txt");

                    await Context.Channel.SendMessageAsync($"\"{Context.Guild.GetRole(Convert.ToUInt64(_roleID)).Name}\" role with ID {_roleID} is no longer an administrative role.");
                }
                else
                {
                    await Context.Channel.SendMessageAsync("Error: Invalid Role ID. Please input a valid ID for a role on the server.");
                }
            }
            else
            {
                await Context.Channel.SendMessageAsync("Error: You are not the Server Owner and cannot use this command.");
            }
        }

        [Command("admin")]
        public async Task AdminRoles(SocketRole _role) // Role mention given
        {
            bool hasAdmin = await HasAdmin();

            if (Context.User.Id == Context.Guild.OwnerId || hasAdmin)
            {
                if (!Program.roleID.Contains(_role.Id))
                {
                    Program.roleID.Add(_role.Id);
                    await Files.WriteToFile(Program.roleID, "roles.txt");

                    await Context.Channel.SendMessageAsync($"\"{_role.Name}\" role with ID {_role.Id} has been added as an administrative role.");
                }
                else
                {
                    Program.roleID.Remove(_role.Id);
                    await Files.WriteToFile(Program.roleID, "roles.txt");

                    await Context.Channel.SendMessageAsync($"\"{_role.Name}\" role with ID {_role.Id} is no longer an administrative role.");
                }
            }
            else
            {
                await Context.Channel.SendMessageAsync("Error: You are not the Server Owner and cannot use this command.");
            }
        }

        [Command("mentions")]
        public async Task MentionsPermitted(string _mentions)
        {
            bool hasRole = await HasRole();

            if (Context.User.Id == Context.Guild.OwnerId || hasRole)
            {
                string _mentionsEnabled;

                if (_mentions == "0")
                {
                    if (Program.mentionID.Contains(Context.Guild.Id))
                    {
                        Program.mentionID.Remove(Context.Guild.Id);
                        _mentionsEnabled = "Disabled";

                        await Files.WriteToFile(Program.mentionID, "mentions.txt");
                    }
                    else
                    {
                        _mentionsEnabled = "already disabled";
                    }
                }
                else if (_mentions == "1")
                {
                    if (Program.mentionID.Contains(Context.Guild.Id))
                    {
                        _mentionsEnabled = "already enabled";
                    }
                    else
                    {
                        Program.mentionID.Add(Context.Guild.Id);
                        _mentionsEnabled = "Enabled";

                        await Files.WriteToFile(Program.mentionID, "mentions.txt");
                    }
                }
                else
                {
                    await Context.Channel.SendMessageAsync("Invalid Parameter. Valid parameters are \"0\" and \"1\".");
                    return;
                }

                await Context.Channel.SendMessageAsync($"Mentions {_mentionsEnabled}!");
            }
            else
            {
                await Context.Channel.SendMessageAsync("Error: You are not the Server Owner or an admin and cannot use this command.");
            }
        }

        [Command("announce")]
        public async Task AnnounceChan(string _chanID) // Channel ID given
        {
            bool hasRole = await HasRole();

            if (Context.User.Id == Context.Guild.OwnerId || hasRole)
            {
                if (_chanID == "-")
                {
                    if (Program.servChanID.Keys.Contains(Context.Guild.Id))
                    {
                        Program.servChanID.Remove(Context.Guild.Id);
                        await Files.WriteToFile(Program.servChanID, "servers.txt", "channels.txt");

                        await Context.Channel.SendMessageAsync("Announcements channel disabled!");
                    }
                    else
                    {
                        await Context.Channel.SendMessageAsync("There is no announcements channel to disable.");
                    }
                }
                else if (Program.servChanID.ContainsKey(Context.Guild.Id) && Program.servChanID[Context.Guild.Id] == Convert.ToUInt64(_chanID))
                {
                    await Context.Channel.SendMessageAsync("Channel is already set as the announcements channel.");
                }
                else if (Context.Guild.TextChannels.Contains(Context.Guild.TextChannels.FirstOrDefault(x => x.Id == Convert.ToUInt64(_chanID))))
                {
                    Program.servChanID.Put(Context.Guild.Id, Convert.ToUInt64(_chanID));
                    await Files.WriteToFile(Program.servChanID, "servers.txt", "channels.txt");

                    await Context.Channel.SendMessageAsync($"The announcements channel is now {(Context.Guild.GetChannel(Convert.ToUInt64(_chanID)) as SocketTextChannel).Mention} with ID {_chanID}.");
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

        [Command("announce")]
        public async Task AnnounceChan(SocketTextChannel _channel) // Channel mention given
        {
            bool hasRole = await HasRole();

            if (Context.User.Id == Context.Guild.OwnerId || hasRole)
            {
                if (Program.servChanID.ContainsKey(Context.Guild.Id) && Program.servChanID[Context.Guild.Id] == _channel.Id)
                {
                    await Context.Channel.SendMessageAsync("Channel is already set as the announcements channel.");
                }
                else
                {
                    Program.servChanID.Put(Context.Guild.Id, Convert.ToUInt64(_channel.Id));
                    await Files.WriteToFile(Program.servChanID, "servers.txt", "channels.txt");

                    await Context.Channel.SendMessageAsync($"The announcements channel is now {_channel.Mention} with ID {_channel.Id}.");
                }
            }
            else
            {
                await Context.Channel.SendMessageAsync("Error: You are not the Server Owner or an admin and cannot use this command.");
            }
        }
    }
}
