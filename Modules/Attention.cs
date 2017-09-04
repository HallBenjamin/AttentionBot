using Discord.Commands;
using System;
using System.IO;
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
                await Context.Channel.SendMessageAsync("**Attention! Bot v1.3.0.3  -  Coded using Discord.Net**\n\n__Prefix:__ \\\n__Commands:__\n\n\\help 3949\n  - Lists all available commands for the bot.\n\n\\admin [role id]\n  - **SERVER OWNERS:** Sets the specified role as an administrative role for the bot's admin commands.\n\n\\announce [channel id]\n  - **ADMINS/SERVER OWNERS:** Sets the specified channel as the channel for bot announcements.\n\n\\attention [position]\n  - Position can contain one letter A-J and/or one number 1-9. Order and capitalization do not matter. Position is optional.");
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
