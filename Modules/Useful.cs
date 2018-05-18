using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AttentionBot.Modules
{
    public class Useful : ModuleBase<SocketCommandContext>
    {
        [Command("membercount")]
        public async Task MemberCount()
        {
            long total = (long) Context.Guild.MemberCount;
            long totalBots = 0L, onlineBots = 0L, offlineBots = 0L;
            long totalUsers = total, onlineUsers = 0L, awayUsers = 0L, doNotDisturbUsers = 0L, invisibleUsers = 0L, offlineUsers = 0L;

            foreach (SocketGuildUser user in Context.Guild.Users)
            {
                if (!user.IsBot)
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
            onlineMessage.WithTitle("__Member Count__");
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
        public async Task Changelog(string _botID = null)
        {
            if (_botID == SecurityInfo.botID || _botID == null)
            {
                await Context.Channel.SendMessageAsync("Changelog can be found at:\n" +
                    "https://github.com/josedolf-staller/AttentionBot#release-notes");
            }
        }

        [Command("help")]
        public async Task Help(string _botID = null, string _param = null, string _param2 = null, string _param3 = null)
        {
            if (_botID != null && _botID != SecurityInfo.botID)
            {
                _param3 = _botID;
                _botID = null;
            }

            List<string> _params = new List<string>();
            _params.Add((_param == null) ? _param : _param.ToLower());
            _params.Add((_param2 == null) ? _param2 : _param2.ToLower());
            _params.Add((_param3 == null) ? _param3 : _param3.ToLower());

            EmbedBuilder helpMessage = new EmbedBuilder();

            helpMessage.WithTitle("Attention! Bot for Discord");
            helpMessage.WithDescription("Bot Version 2.0.0.0  -  Programmed using Discord.Net 1.0.2 and Microsoft .NET Framework 4.7.1");
            helpMessage.WithColor(SecurityInfo.botColor);
            helpMessage.WithCurrentTimestamp();

            EmbedFieldBuilder prefixField = new EmbedFieldBuilder();
            prefixField.WithIsInline(false);
            prefixField.WithName("Prefix");
            prefixField.WithValue(CommandHandler.prefix.ToString() + "\n\u200b");
            helpMessage.AddField(prefixField);

            EmbedFieldBuilder helpField = new EmbedFieldBuilder();
            helpField.WithIsInline(true);
            helpField.WithName("\\help Parameters");
            helpField.WithValue(
                "***NOTE:** If you choose to supply the 3949 parameter, it must be first. If you do not supply it, only one parameter may be given.*\n\n" +

                "3949\n" +
                "  - DMs you all available help commands for the bot.\n\n" +

                "useful\n" +
                "  - Lists all available useful commands for the bot.\n\n" +

                "spam\n" +
                "  - Lists all available spam commands for the bot.\n\n" +

                "admins\n" +
                "  - Lists all available admin commands for the bot.\n\u200b");

            EmbedFieldBuilder usefulField = new EmbedFieldBuilder();
            usefulField.WithIsInline(true);
            usefulField.WithName("Useful");
            usefulField.WithValue(
                "\\help [parameter(s) (optional)]\n" +
                "  - Lists available commands for the bot.\n\n" +

                "\\changelog [3949 (optional)]\n" +
                "  - Sends a link to the version history (changelog) of the bot.\n\n" +

                "\\membercount\n" +
                "  - Lists number of users and bots on the server by status.\n\u200b");

            EmbedFieldBuilder spamField = new EmbedFieldBuilder();
            spamField.WithIsInline(true);
            spamField.WithName("Fun Spams");
            spamField.WithValue(
                "***NOTE:** User only works if \\mentions is set to 1. Set User to the ID or mention of the user you want to mention.*\n\n" +

                "**References from:** War Thunder\n\n" +

                "\\attention [position (optional)] [user ID/mention (optional)]\n" +
                "  - Message is randomized.\n" +
                "  - Position can contain one letter A-J and/or one number 1-10. Order and capitalization do not matter.\n" +
                "  - Position is randomized if none is given.\n" +
                "  - Order of parameters does not matter.\n\n" +

                "**References from:** Sword Art Online Abridged\n\n" +

                "\\gary [user (optional)]\n" +
                "  - \"We must save my family!\"\n\n" +

                "\\bandits [user (optional)]\n" +
                "  - \"The bandits are coming!\"\n\n" +

                "\\sword [user (optional)]\n" +
                "  - \"There's a person attached to this sword, you know! I WILL NOT BE OBJECTIFIED!\"\n\n" +

                "\\karf [user (optional)]\n" +
                "  - Quote is randomized.\n\u200b");

            EmbedFieldBuilder adminField = new EmbedFieldBuilder();
            adminField.WithIsInline(false);
            adminField.WithName("Admin");
            adminField.WithValue(
                "***NOTE:** Users with the \"Administrator\" power are considered Server Owners for these commands. \"Admins\" are the role(s) the Server Owners have designated as \"Admin\" roles.*\n\n" +
                "***NOTE 2:** The Role and Channel parameters can either be their respective ID or a mention of the channel/role.*\n\n" +

                "\\settings [3949 (optional)]\n" +
                "  - **ADMINS/SERVER OWNERS:** Displays the current configuration of the bot.\n\n" +

                "\\admin [role]\n" +
                "  - **SERVER OWNERS:** Sets/removes the specified role as an administrative role for the bot's admin commands.\n\n" +

                "\\announce [channel]\n" +
                "  - **ADMINS/SERVER OWNERS:** Sets the specified channel as the channel for bot announcements.\n" +
                "  - To disable announcements, type in \"-\" as the channel id.\n\n" +

                "\\mentions [0/1]\n" +
                "  - **ADMINS/SERVER OWNERS:** Enables (1) or disables (0) user mentions for the bot.");

            List<string> parameters = new List<string>();
            parameters.Add("useful");
            parameters.Add("spam");
            parameters.Add("admin");

            bool fieldExists = false;
            foreach(string param in parameters)
            {
                fieldExists = _params.Contains(param);

                if(fieldExists)
                {
                    break;
                }
            }
            if(!fieldExists)
            {
                helpMessage.AddField(helpField);
            }

            if (_botID == null)
            {
                if (_params.Contains("useful"))
                {
                    helpMessage.AddField(usefulField);
                }
                else if (_params.Contains("spam"))
                {
                    helpMessage.AddField(spamField);
                }
                else if (_params.Contains("admin"))
                {
                    helpMessage.AddField(adminField);
                }

                await Context.Channel.SendMessageAsync("", false, helpMessage);
            }
            else if (_botID == SecurityInfo.botID)
            {
                if (_params.Contains("useful"))
                {
                    helpMessage.AddField(usefulField);
                }
                if (_params.Contains("spam"))
                {
                    helpMessage.AddField(spamField);
                }
                if (_params.Contains("admin"))
                {
                    helpMessage.AddField(adminField);
                }

                await Context.User.SendMessageAsync("", false, helpMessage);
            }
        }
    }
}
