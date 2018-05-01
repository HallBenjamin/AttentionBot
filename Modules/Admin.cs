using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AttentionBot.Modules
{
    public class Admin : ModuleBase<SocketCommandContext>
    {
        [Command("settings")]
        public async Task getSettings()
        {
            bool hasRole = Context.Guild.GetUser(Context.User.Id).GuildPermissions.Has(Discord.GuildPermission.Administrator); // Is admin?
            foreach (ulong role in Program.roleID)
            {
                hasRole = hasRole ? hasRole : Context.Guild.GetUser(Context.User.Id).Roles.Contains(Context.Guild.GetRole(role)); // If already true, ignore
                if (hasRole)
                {
                    break;
                }
            }

            if (Context.User.Id == Context.Guild.OwnerId || hasRole)
            {
                string announceChan = Program.servChanID.ContainsKey(Context.Guild.Id) ? Program.servChanID[Context.Guild.Id].ToString()
                    : "No bot announcements channel has been assigned.";

                List<ulong> adminRoles = new List<ulong>();
                foreach (ulong role in Program.roleID)
                {
                    if (Context.Guild.Roles.FirstOrDefault(x => x.Id == role).Id == role)
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
                if (announceChan != "No announcements channel has been assigned.")
                {
                    announceChan = "Name: " + Context.Guild.GetChannel(Convert.ToUInt64(announceChan)).Name + "\nID: " + announceChan + "\n\u200b";
                }
                announceEmb.WithValue(announceChan);
                servSettings.AddField(announceEmb);

                EmbedFieldBuilder adminEmb = new EmbedFieldBuilder();
                adminEmb.WithIsInline(false);
                adminEmb.WithName("Admin Roles");
                string roles = adminRoles.Count == 0 ? "No admin roles have been assigned." : "";
                foreach (ulong role in adminRoles)
                {
                    roles += "Name: " + Context.Guild.GetRole(role).Name + "\nID: " + role + "\n\u200b\n";
                }
                adminEmb.WithValue(roles);
                servSettings.AddField(adminEmb);

                EmbedFieldBuilder mentionEmb = new EmbedFieldBuilder();
                mentionEmb.WithIsInline(false);
                mentionEmb.WithName("User Mentions");
                mentionEmb.WithValue(mentions);
                servSettings.AddField(mentionEmb);

                await Context.Channel.SendMessageAsync("", false, servSettings);
            }
        }

        [Command("admin")]
        public async Task adminRoles(string _roleID)
        {
            if (Context.User.Id == Context.Guild.OwnerId)
            {
                if (!Program.roleID.Contains(Convert.ToUInt64(_roleID)))
                {
                    Program.roleID.Add(Convert.ToUInt64(_roleID));

                    BinaryWriter roleWriter = new BinaryWriter(File.Open("roles.txt", FileMode.Truncate));
                    foreach (var value in Program.roleID)
                    {
                        roleWriter.Write(value.ToString());
                    }
                    roleWriter.Close();

                    await Context.Channel.SendMessageAsync("Role has been added as an administrative role.");
                }
                else
                {
                    Program.roleID.Remove(Convert.ToUInt64(_roleID));

                    BinaryWriter roleWriter = new BinaryWriter(File.Open("roles.txt", FileMode.Truncate));
                    foreach (var value in Program.roleID)
                    {
                        roleWriter.Write(value.ToString());
                    }
                    roleWriter.Close();

                    await Context.Channel.SendMessageAsync("Role is no longer an administrative role.");
                }
            }
            else
            {
                await Context.Channel.SendMessageAsync("You are not the owner of the server and cannot use this command.");
            }

        }

        [Command("mentions")]
        public async Task mentionsPermitted(string _mentions)
        {
            bool hasRole = Context.Guild.GetUser(Context.User.Id).GuildPermissions.Has(Discord.GuildPermission.Administrator); // Is admin?
            foreach (ulong role in Program.roleID)
            {
                hasRole = hasRole ? hasRole : Context.Guild.GetUser(Context.User.Id).Roles.Contains(Context.Guild.GetRole(role)); // If already true, ignore
                if (hasRole)
                {
                    break;
                }
            }

            if (Context.User.Id == Context.Guild.OwnerId || hasRole)
            {
                string _mentionsEnabled;

                if (_mentions == "0")
                {
                    if (Program.mentionID.Contains(Context.Guild.Id))
                    {
                        Program.mentionID.Remove(Context.Guild.Id);
                    }

                    _mentionsEnabled = "Disabled";
                }
                else if (_mentions == "1")
                {
                    if (!Program.mentionID.Contains(Context.Guild.Id))
                    {
                        Program.mentionID.Add(Context.Guild.Id);
                    }

                    _mentionsEnabled = "Enabled";
                }
                else
                {
                    await Context.Channel.SendMessageAsync("Invalid Parameter. Valid parameters are \"disabled\" and \"enabled\".");
                    return;
                }

                BinaryWriter mentWriter = new BinaryWriter(File.Open("mentions.txt", FileMode.Truncate));
                foreach (var value in Program.mentionID)
                {
                    mentWriter.Write(value.ToString());
                }
                mentWriter.Close();

                await Context.Channel.SendMessageAsync("Mentions " + _mentionsEnabled + "!");
            }
            else
            {
                await Context.Channel.SendMessageAsync("You are not the owner or admin of the server and cannot use this command.");
            }
        }

        [Command("announce")]
        public async Task announceChan(string _chanID)
        {
            bool hasRole = Context.Guild.GetUser(Context.User.Id).GuildPermissions.Has(Discord.GuildPermission.Administrator); // Is admin?
            foreach(ulong role in Program.roleID)
            {
                hasRole = hasRole ? hasRole : Context.Guild.GetUser(Context.User.Id).Roles.Contains(Context.Guild.GetRole(role)); // If already true, ignore
                if (hasRole)
                {
                    break;
                }
            }

            if (Context.User.Id == Context.Guild.OwnerId || hasRole)
            {
                Program.servChanID.Add(Context.Guild.Id, Convert.ToUInt64(_chanID));

                BinaryWriter servWriter = new BinaryWriter(File.Open("servers.txt", FileMode.Truncate));
                BinaryWriter chanWriter = new BinaryWriter(File.Open("channels.txt", FileMode.Truncate));
                foreach (ulong serv in Program.servChanID.Keys)
                {
                    chanWriter.Write(Program.servChanID[serv].ToString());
                    servWriter.Write(serv.ToString());
                }
                chanWriter.Close();
                servWriter.Close();

                await Context.Channel.SendMessageAsync("The announcements channel is now the channel with ID " + _chanID + ".");
            }
            else
            {
                await Context.Channel.SendMessageAsync("You are not the owner or admin of the server and cannot use this command.");
            }
        }
    }
}
