using Discord.Commands;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AttentionBot.Modules
{
    public class Admin : ModuleBase<SocketCommandContext>
    {
        [Command("admin")]
        public async Task adminRoles(string _roleID = null)
        {
            if (Context.User.Id == Context.Guild.OwnerId)
            {
                if (_roleID != null)
                {
                    if (!Program.roleID.Contains(Convert.ToUInt64(_roleID)))
                        Program.roleID.Add(Convert.ToUInt64(_roleID));
                    Program.roleIDs = Program.roleID.ToArray();

                    BinaryWriter roleWriter = new BinaryWriter(File.Open("roles.txt", FileMode.Truncate));
                    foreach (var value in Program.roleIDs)
                    {
                        roleWriter.Write(value);
                    }
                    roleWriter.Close();

                    await Context.Channel.SendMessageAsync("Role has been added as an administrative role.");
                }
                else
                    await Context.Channel.SendMessageAsync("No role ID was given. Please try again.");
            }
            else
                await Context.Channel.SendMessageAsync("You are not the owner of the server and cannot use this command.");

        }

        [Command("mentions")]
        public async Task mentionsPermitted(string _mentions)
        {
            bool hasRole = false;
            for (int i = 0; i < Program.roleIDs.Length; i++)
            {
                hasRole = Context.Guild.GetUser(Context.User.Id).Roles.Contains(Context.Guild.GetRole(Program.roleIDs[i]));
                if (hasRole)
                    break;
            }
            if (Context.User.Id == Context.Guild.OwnerId || hasRole)
            {
                string _mentionsEnabled;

                if (_mentions == "0")
                {
                    if (Program.mentionID.Contains(Context.Guild.Id))
                        Program.mentionID.Remove(Context.Guild.Id);

                    _mentionsEnabled = "Disabled";
                }
                else if (_mentions == "1")
                {
                    if (!Program.mentionID.Contains(Context.Guild.Id))
                        Program.mentionID.Add(Context.Guild.Id);

                    _mentionsEnabled = "Enabled";
                }
                else
                {
                    await Context.Channel.SendMessageAsync("Invalid Parameter. Valid parameters are \"disabled\" and \"enabled\".");
                    return;
                }

                Program.mentionIDs = Program.mentionID.ToArray();

                BinaryWriter mentWriter = new BinaryWriter(File.Open("mentions.txt", FileMode.Truncate));
                foreach (var value in Program.mentionIDs)
                {
                    mentWriter.Write(value);
                }
                mentWriter.Close();

                await Context.Channel.SendMessageAsync("Mentions " + _mentionsEnabled + "!");
            }
            else
                await Context.Channel.SendMessageAsync("You are not the owner or admin of the server and cannot use this command.");
        }

        [Command("announce")]
        public async Task announceChan(string _chanID = null)
        {
            bool hasRole = false;
            for (int i = 0; i < Program.roleIDs.Length; i++)
            {
                hasRole = Context.Guild.GetUser(Context.User.Id).Roles.Contains(Context.Guild.GetRole(Program.roleIDs[i]));
                if (hasRole)
                    break;
            }

            if (Context.User.Id == Context.Guild.OwnerId || hasRole)
            {
                if (_chanID != null)
                {
                    if (!Program.chanID.Contains(Convert.ToUInt64(_chanID)))
                    {
                        if (Program.servID.Contains(Context.Guild.Id))
                        {
                            Program.chanID.Remove(Convert.ToUInt64(_chanID));
                            Program.servID.Remove(Context.Guild.Id);
                        }

                        Program.chanID.Add(Convert.ToUInt64(_chanID));
                        Program.servID.Add(Context.Guild.Id);

                        Program.chanIDs = Program.chanID.ToArray();
                        Program.servIDs = Program.servID.ToArray();

                        BinaryWriter chanWriter = new BinaryWriter(File.Open("channels.txt", FileMode.Truncate));
                        foreach (var value in Program.chanIDs)
                        {
                            chanWriter.Write(value);
                        }
                        chanWriter.Close();

                        BinaryWriter servWriter = new BinaryWriter(File.Open("servers.txt", FileMode.Truncate));
                        foreach (var value in Program.servIDs)
                        {
                            servWriter.Write(value);
                        }
                        servWriter.Close();
                    }

                    await Context.Channel.SendMessageAsync("The announcements channel is now the channel with ID " + _chanID + ".");
                }
                else
                    await Context.Channel.SendMessageAsync("No channel ID given. Please try again.");
            }
            else
                await Context.Channel.SendMessageAsync("You are not the owner or admin of the server and cannot use this command.");
        }
    }
}
