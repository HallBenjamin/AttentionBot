using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.IO;
using System.Threading.Tasks;

namespace AttentionBot.Modules
{
    public class Attention : ModuleBase<SocketCommandContext>
    {
        [Command("attention")]
        public async Task attention(string position = null, string _mentionID = null)
        {
            Random rnd = new Random();

            string[] Letter = new string[10] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J" };

            int number = rnd.Next(1, 11);

            string letter = Letter[rnd.Next(0, 10)];

            if (position != null)
            {
                for (int i = 10; i >= 1; i--)
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

            var _myUser = Context.Guild.GetUser(Convert.ToUInt64(_mentionID));

            foreach(ulong serv in Program.mentionID)
            {
                if(!Context.Client.GetGuild(serv).IsConnected)
                {
                    Program.mentionID.Remove(serv);

                    BinaryWriter mentionWriter = new BinaryWriter(File.Open("mentions.txt", FileMode.Truncate));
                    foreach (var value in Program.mentionID)
                    {
                        mentionWriter.Write(value.ToString());
                    }
                    mentionWriter.Close();
                }
            }

            if (Program.mentionID.Contains(Context.Guild.Id) && _mentionID != null)
            {
                await Context.Channel.SendMessageAsync(_myUser.Mention + " " + text + " (" + letter + number + ")");
            }
            else
            {
                await Context.Channel.SendMessageAsync(text + " (" + letter + number + ")");
            }
        }

        [Command("usercount")]
        public async Task userCount()
        {
            await Context.Guild.DownloadUsersAsync();

            int online = 0, away = 0, doNotDisturb = 0, invisible = 0, offline = 0;

            int total = Context.Guild.MemberCount;

            foreach (SocketGuildUser user in Context.Guild.Users)
            {
                if(!user.IsBot)
                {
                    switch (user.Status)
                    {
                        case UserStatus.AFK:
                        case UserStatus.Idle:
                            away++;
                            break;
                        case UserStatus.DoNotDisturb:
                            doNotDisturb++;
                            break;
                        case UserStatus.Invisible:
                            invisible++;
                            break;
                        case UserStatus.Online:
                            online++;
                            break;
                        case UserStatus.Offline:
                        default:
                            offline++;
                            break;
                    }
                }
                else
                {
                    total--;
                }
            }

            var onlineMessage = new EmbedBuilder();
            onlineMessage.WithColor(23, 90, 150);
            onlineMessage.WithTitle("User Count");
            onlineMessage.WithDescription("**Total:** " + total +
                "\nOnline: " + online +
                "\nAway: " + away +
                "\nDo Not Disturb: " + doNotDisturb +
                "\nInvisible: " + invisible +
                "\nOffline: " + offline);
            onlineMessage.WithCurrentTimestamp();

            await Context.Channel.SendMessageAsync("", false, onlineMessage);
        }

        [Command("changelog")]
        public async Task changelog(string _botID = null)
        {
            if (_botID == Program.botID)
            {
                await Context.Channel.SendMessageAsync("Changelog can be found at:\n" +
                    "https://github.com/josedolf-staller/AttentionBot/blob/master/README.md#release-notes");
            }
        }

        [Command("help")]
        public async Task help(string _botID = null)
        {
            if (_botID == Program.botID)
            {
                await Context.Channel.SendMessageAsync("**Attention! Bot v1.5.2.0  -  Programmed using Discord.Net**\n" +
                    "__Prefix:__ \\\n\n" +
                    "__Commands:__\n\n" +
                    "\\help 3949\n" +
                    "  - Lists all available commands for the bot.\n\n" +
                    "\\changelog 3949\n" +
                    "  - Sends a link to the version history (changelog) of the bot.\n\n" +
                    "\\admin [role id]\n" +
                    "  - **SERVER OWNERS:** Sets/removes the specified role as an administrative role for the bot's admin commands.\n\n" +
                    "\\mentions [0/1]\n" +
                    "  - **ADMINS/SERVER OWNERS:** Enables (1) or disables (0) user mentions for the bot.\n\n" +
                    "\\announce [channel id]\n" +
                    "  - **ADMINS/SERVER OWNERS:** Sets the specified channel as the channel for bot announcements.\n\n" +
                    "\\attention [position (optional)] [user ID (optional)]\n" +
                    "  - Position can contain one letter A-J and/or one number 1-10. Order and capitalization do not matter.\n" +
                    "  - User ID only works if \\mentions is set to 1. Set the User ID to the ID of the user you want to mention.\n\n" +
                    "\\usercount\n" +
                    "  - Lists number of users on the server by status.");
            }
        }
    }
}
