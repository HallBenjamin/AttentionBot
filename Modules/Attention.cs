using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AttentionBot.Modules
{
    public class Attention : ModuleBase<SocketCommandContext>
    {
        [Command("attention")]
        public async Task attention(string position = null)
        {
            Random rnd = new Random();
            int Number = rnd.Next(1, 10);

            string[] Letter = new string[10] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J" };

            int number = Number;

            string letter = Letter[rnd.Next(0, 10)];

            if (position != null)
            {
                for (int i = 1; i <= 9; i++)
                {
                    if (position.Contains(i.ToString()))
                    {
                        number = i;
                        break;
                    }
                }

                for (int i = 0; i <= 9; i++)
                {
                    if (position.Contains(Letter[i]) || position.Contains(Letter[i].ToLower()))
                    {
                        letter = Letter[i];
                        break;
                    }
                }
            }

            string[] Text = new string[3] { "Attention to the designated grid square!", "Attention to the designated grid zone!", "Attention to the map!" };

            string text = Text[rnd.Next(0, 3)];

            await Context.Channel.SendMessageAsync(text + " (" + letter + number + ")");
        }

        [Command("help")]
        public async Task help(string _botID = null)
        {
            if (_botID == Program.botID)
                await Context.Channel.SendMessageAsync("**Attention! Bot v1.3.0.2  -  Coded using Discord.Net**\n\n__Prefix:__ \\\n__Commands:__\n\n\\help 3949\n  - Lists all available commands for the bot.\n\n\\admin [role id]\n  - **SERVER OWNERS:** Sets the specified role as an administrative role for the bot's admin commands.\n\n\\announce [channel id]\n  - **ADMINS/SERVER OWNERS:** Sets the specified channel as the channel for bot announcements.\n\n\\attention [position]\n  - Position can contain one letter A-J and/or one number 1-9. Order and capitalization do not matter. Position is optional.");
        }

        [Command("admin")]
        public async Task adminRoles(string _roleID = null)
        {
            if (Context.User.Id == Context.Guild.OwnerId)
            {
                if(_roleID != null)
                {
                    Program.roleID.Add(Convert.ToUInt64(_roleID));
                    Program.roleIDs = Program.roleID.ToArray();

                    BinaryWriter roler = new BinaryWriter(File.Open("roles.txt", FileMode.Open));
                    foreach (var value in Program.roleIDs)
                    {
                        roler.Write(value);
                    }
                    roler.Close();

                    await Context.Channel.SendMessageAsync("Role has been added as an administrative role.");
                }
                else
                    await Context.Channel.SendMessageAsync("No role ID was given. Please try again.");
            }
            else
                await Context.Channel.SendMessageAsync("You are not the owner of the server and cannot use this command.");
                
        }

        [Command("announce")]
        public async Task announceChan(string _chanID = null)
        {
            bool hasRole = false;
            for(int i = 0; i < Program.roleIDs.Length; i++)
            {
                hasRole = Context.Guild.GetUser(Context.User.Id).Roles.Contains(Context.Guild.GetRole(Program.roleIDs[i]));
                if (hasRole)
                    break;
            }

            if (Context.User.Id == Context.Guild.OwnerId || hasRole)
            {
                if (_chanID != null)
                {
                    bool alreadyExists = Program.chanID.Contains(Convert.ToUInt64(_chanID));
                    bool alreadyExist = Program.servID.Contains(Context.Guild.Id);
                    if (!alreadyExists)
                    {
                        if(alreadyExist)
                        {
                            Program.chanID.Remove(Convert.ToUInt64(_chanID));
                            Program.servID.Remove(Context.Guild.Id);
                            
                        }

                        Program.chanID.Add(Convert.ToUInt64(_chanID));
                        Program.servID.Add(Context.Guild.Id);

                        Program.chanIDs = Program.chanID.ToArray();
                        Program.servIDs = Program.servID.ToArray();

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
                    }

                    await Context.Channel.SendMessageAsync("The announcements channel is now the channel with id " + _chanID + ".");
                }
                else
                    await Context.Channel.SendMessageAsync("No channel ID given. Please try again.");
            }
            else
                await Context.Channel.SendMessageAsync("You are not the owner of the server and cannot use this command.");
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
        public async Task exitAttentionBot(string _botID = null)
        {
            if (_botID == Program.botID || _botID == null)
            {
                Program.chanIDs = Program.chanID.ToArray();
                Program.servIDs = Program.servID.ToArray();
                Program.roleIDs = Program.roleID.ToArray();

                for (int i = 0; i < Program.chanIDs.Length; i++)
                {
                    await Context.Client.GetGuild(Program.servIDs[i]).GetTextChannel(Program.chanIDs[i]).SendMessageAsync("Attention! Bot is now offline.");
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
