using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AttentionBot.Modules
{
    public class Attention : ModuleBase<SocketCommandContext>
    {
        // Spam
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

                foreach (string lett in Letter)
                {
                    if (position.Contains(lett) || position.Contains(lett.ToLower()))
                    {
                        letter = lett;
                        break;
                    }
                }
            }

            string[] Text = new string[3] { "Attention to the designated grid square!", "Attention to the designated grid zone!", "Attention to the map!" };

            string text = Text[rnd.Next(0, 3)];

            SocketUser user = Context.Guild.Users.FirstOrDefault(x => x.Id == Convert.ToUInt64(_mentionID));
            if (Program.mentionID.Contains(Context.Guild.Id) && _mentionID != null && Context.Guild.Users.Contains(user))
            {
                await Context.Channel.SendMessageAsync(user.Mention + " " + text + " (" + letter + number + ")");
            }
            else
            {
                await Context.Channel.SendMessageAsync(text + " (" + letter + number + ")");
            }
        }

        [Command("gary")]
        public async Task saveMyFamily(string _mentionID = null)
        {
            SocketUser user = Context.Guild.Users.FirstOrDefault(x => x.Id == Convert.ToUInt64(_mentionID));
            if (Program.mentionID.Contains(Context.Guild.Id) && _mentionID != null && Context.Guild.Users.Contains(user))
            {
                await Context.Channel.SendMessageAsync(user.Mention + " We must save my family!");
            }
            else
            {
                await Context.Channel.SendMessageAsync("We must save my family!");
            }
        }

        [Command("bandits")]
        public async Task banditsComing(string _mentionID = null)
        {
            SocketUser user = Context.Guild.Users.FirstOrDefault(x => x.Id == Convert.ToUInt64(_mentionID));
            if (Program.mentionID.Contains(Context.Guild.Id) && _mentionID != null && Context.Guild.Users.Contains(user))
            {
                await Context.Channel.SendMessageAsync(user.Mention + " The bandits are coming!");
            }
            else
            {
                await Context.Channel.SendMessageAsync("The bandits are coming!");
            }
        }

        // Useful
        [Command("membercount")]
        public async Task memberCount()
        {
            long total = (long) Context.Guild.MemberCount;
            long totalBots = 0L, onlineBots = 0L, offlineBots = 0L;
            long totalUsers = total, onlineUsers = 0L, awayUsers = 0L, doNotDisturbUsers = 0L, invisibleUsers = 0L, offlineUsers = 0L;

            foreach (SocketGuildUser user in Context.Guild.Users)
            {
                if(!user.IsBot)
                {
                    switch (user.Status)
                    {
                        case UserStatus.AFK:
                        case UserStatus.Idle:
                            awayUsers++;
                            break;
                        case UserStatus.DoNotDisturb:
                            doNotDisturbUsers++;
                            break;
                        case UserStatus.Invisible:
                            invisibleUsers++;
                            break;
                        case UserStatus.Online:
                            onlineUsers++;
                            break;
                        case UserStatus.Offline:
                        default:
                            break;
                    }
                }
                else
                {
                    totalUsers--;
                    totalBots++;

                    switch (user.Status)
                    {
                        case UserStatus.AFK:
                        case UserStatus.Idle:
                        case UserStatus.DoNotDisturb:
                        case UserStatus.Invisible:
                        case UserStatus.Online:
                            onlineBots++;
                            break;
                        case UserStatus.Offline:
                        default:
                            break;
                    }
                }
            }

            offlineUsers = totalUsers - (onlineUsers + awayUsers + doNotDisturbUsers + invisibleUsers);
            offlineBots = totalBots - onlineBots;

            EmbedBuilder onlineMessage = new EmbedBuilder();
            onlineMessage.WithColor(SecurityInfo.botColor);
            onlineMessage.WithTitle("__User Count__");
            onlineMessage.WithCurrentTimestamp();

            EmbedFieldBuilder totalBuilder = new EmbedFieldBuilder();
            totalBuilder.WithIsInline(false);
            totalBuilder.WithName("Total");
            totalBuilder.WithValue(total);
            onlineMessage.AddField(totalBuilder);

            EmbedFieldBuilder userBuilder = new EmbedFieldBuilder();
            userBuilder.WithIsInline(true);
            userBuilder.WithName("\nUsers: " + totalUsers);
            userBuilder.WithValue(
                "\nOnline: " + onlineUsers +
                "\nAway: " + awayUsers +
                "\nDo Not Disturb: " + doNotDisturbUsers +
                "\nInvisible: " + invisibleUsers +
                "\nOffline: " + offlineUsers + "\n");
            onlineMessage.AddField(userBuilder);

            EmbedFieldBuilder botBuilder = new EmbedFieldBuilder();
            botBuilder.WithIsInline(true);
            botBuilder.WithName("\nBots: " + totalBots);
            botBuilder.WithValue(
                "\nOnline: " + onlineBots +
                "\nOffline: " + offlineBots);
            onlineMessage.AddField(botBuilder);

            await Context.Channel.SendMessageAsync("", false, onlineMessage);
        }

        [Command("changelog")]
        public async Task changelog(string _botID = null)
        {
            if (_botID == SecurityInfo.botID || _botID == null)
            {
                await Context.Channel.SendMessageAsync("Changelog can be found at:\n" +
                    "https://github.com/josedolf-staller/AttentionBot#release-notes");
            }
        }

        [Command("help")]
        public async Task help(string _botID = null)
        {
            if (_botID == SecurityInfo.botID || _botID == null)
            {
                EmbedBuilder helpMessage = new EmbedBuilder();

                helpMessage.WithTitle("Attention! Bot for Discord");
                helpMessage.WithDescription("Bot Version 1.5.6.3  -  Programmed using Discord.Net 1.0.2 and Microsoft .NET Framework 4.7.1");
                helpMessage.WithColor(SecurityInfo.botColor);
                helpMessage.WithCurrentTimestamp();

                EmbedFieldBuilder prefixField = new EmbedFieldBuilder();
                prefixField.WithIsInline(false);
                prefixField.WithName("Prefix");
                prefixField.WithValue(CommandHandler.prefix.ToString() + "\n\u200b");
                helpMessage.AddField(prefixField);

                EmbedFieldBuilder usefulField = new EmbedFieldBuilder();
                usefulField.WithIsInline(true);
                usefulField.WithName("Useful");
                usefulField.WithValue(
                    "\\help [3949 (optional)]\n" +
                    "  - Lists all available commands for the bot.\n\n" +
                    "\\changelog [3949 (optional)]\n" +
                    "  - Sends a link to the version history (changelog) of the bot.\n\n" +
                    "\\membercount\n" +
                    "  - Lists number of users and bots on the server by status.\n\u200b");
                helpMessage.AddField(usefulField);

                EmbedFieldBuilder spamField = new EmbedFieldBuilder();
                spamField.WithIsInline(true);
                spamField.WithName("Fun Spams");
                spamField.WithValue(
                    "\\attention [position (optional)] [user ID (optional)]\n" +
                    "  - Position can contain one letter A-J and/or one number 1-10. Order and capitalization do not matter.\n" +
                    "  - User ID only works if \\mentions is set to 1. Set the User ID to the ID of the user you want to mention.\n\n" +
                    "\\gary [user ID (optional)]\n" +
                    "  - User ID only works if \\mentions is set to 1. Set the User ID to the ID of the user you want to mention.\n\n" +
                    "\\bandits [user ID (optional)]\n" +
                    "  - User ID only works if \\mentions is set to 1. Set the User ID to the ID of the user you want to mention.\n\u200b");
                helpMessage.AddField(spamField);

                EmbedFieldBuilder adminField = new EmbedFieldBuilder();
                adminField.WithIsInline(false);
                adminField.WithName("Admins");
                adminField.WithValue(
                    "*NOTE: Users with the \"Administrator\" power are considered Server Owners for these commands. \"Admins\" are the role(s) the Server Owners have designated as \"Admin\" roles.*\n\n" +
                    "\\settings [3949 (optional)]\n" +
                    "  - **ADMINS/SERVER OWNERS:** Displays the current configuration of the bot.\n\n" +
                    "\\admin [role id]\n" +
                    "  - **SERVER OWNERS:** Sets/removes the specified role as an administrative role for the bot's admin commands.\n\n" +
                    "\\announce [channel id]\n" +
                    "  - **ADMINS/SERVER OWNERS:** Sets the specified channel as the channel for bot announcements.\n\n" +
                    "\\mentions [0/1]\n" +
                    "  - **ADMINS/SERVER OWNERS:** Enables (1) or disables (0) user mentions for the bot.");
                helpMessage.AddField(adminField);

                await Context.Channel.SendMessageAsync("", false, helpMessage);
            }
        }
    }
}
