using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace AttentionBot
{
    public class CommandHandler
    {
        public const char prefix = '\\';

        private DiscordSocketClient _client;
        private CommandService _service;

        public CommandHandler(DiscordSocketClient client)
        {
            _client = client;

            _service = new CommandService();
            _service.AddModulesAsync(Assembly.GetEntryAssembly());

            _client.MessageReceived += HandleCommandAsync;
            _client.JoinedGuild += SendWelcomeMessage;
            _client.LeftGuild += CleanDatabase;
            _client.Disconnected += SendDisconnectError;
            _client.Connected += SendConnectMessage;
        }

        private async Task SendDisconnectError(Exception e)
        {
            if (Program.isConsole)
            {
                await Console.Out.WriteLineAsync(e.Message);
            }
        }

        private async Task SendConnectMessage()
        {
            if (Program.isConsole)
            {
                await Console.Out.WriteLineAsync("Attention! Bot Online");
            }
        }

        private async Task CleanDatabase(SocketGuild g)
        {
            if (Program.servChanID.ContainsKey(g.Id))
            {
                Program.servChanID.Remove(g.Id);
            }

            if (Program.interServerChats.ContainsKey(g.Id))
            {
                Program.servChanID.Remove(g.Id);
            }

            if (Program.showUserServer.Contains(g.Id))
            {
                Program.showUserServer.Remove(g.Id);
            }

            if (Program.broadcastServerName.Contains(g.Id))
            {
                Program.broadcastServerName.Remove(g.Id);
            }

            if (Program.mentionID.Contains(g.Id))
            {
                Program.mentionID.Remove(g.Id);
            }

            List<ulong> roles = new List<ulong>();
            foreach (SocketGuild server in _client.Guilds)
            {
                foreach (ulong ID in Program.roleID.ToList())
                {
                    SocketRole role = server.Roles.FirstOrDefault(x => x.Id == ID);
                    if (server.Roles.Contains(role))
                    {
                        roles.Add(role.Id);
                    }
                }
            }
            Program.roleID = roles;

            await Files.WriteToFile(Program.servChanID, "servers.txt", "channels.txt");
            await Files.WriteToFile(Program.mentionID, "mentions.txt");
            await Files.WriteToFile(Program.roleID, "roles.txt");
            await Files.WriteToFile(Program.interServerChats, "interservers.txt", "interchannels.txt");
            await Files.WriteToFile(Program.showUserServer, "show-guild.txt");
            await Files.WriteToFile(Program.broadcastServerName, "broadcast-guild.txt");
        }

        private async Task SendWelcomeMessage(SocketGuild g)
        {
            SocketTextChannel spam = g.TextChannels.FirstOrDefault(x => x.Name == "spam" || x.Name.Contains("bot")) as SocketTextChannel;
            SocketTextChannel test = g.TextChannels.FirstOrDefault(x => x.Name.Contains("test")) as SocketTextChannel;
            SocketTextChannel general = g.TextChannels.FirstOrDefault(x => x.Name == "general") as SocketTextChannel;

            SocketTextChannel channel = spam;
            
            if (g.TextChannels.Contains(channel))
            {
                if (!(await PermissionChecker.HasSend(g, channel)))
                {
                    channel = test;
                }
            }
            else
            {
                channel = test;
            }

            if (g.TextChannels.Contains(channel))
            {
                if (!(await PermissionChecker.HasSend(g, channel)))
                {
                    channel = general;
                }
            }
            else
            {
                channel = general;
            }

            int i = 0;
            bool hasPerm = await PermissionChecker.HasSend(g, channel);

            while (!hasPerm)
            {
                channel = g.TextChannels.FirstOrDefault(x => x.Position == i) as SocketTextChannel;

                if (g.TextChannels.Contains(channel))
                {
                    hasPerm = await PermissionChecker.HasSend(g, channel);
                }

                i++;
            }

            await channel.SendMessageAsync("Hello! I am Attention! Bot.\nTo see a list of my commands, type \"\\help 3949\" (without quotes).");
        }

        private async Task HandleCommandAsync(SocketMessage s)
        {
            SocketUserMessage msg = s as SocketUserMessage;
            if (msg == null)
            {
                return;
            }

            SocketCommandContext context = new SocketCommandContext(_client, msg);

            int argPos = 0;
            if (!context.User.IsBot)
            {
                // Commands
                if (msg.HasCharPrefix(prefix, ref argPos))
                {
                    var result = await _service.ExecuteAsync(context, argPos);

                    if (!result.IsSuccess && result.Error != CommandError.UnknownCommand)
                    {
                        await context.Channel.SendMessageAsync("Error: " + result.ErrorReason);
                    }
                }

                // InterServer Chat
                else if (Program.interServerChats.ContainsKey(context.Guild.Id) && Program.interServerChats[context.Guild.Id] == context.Channel.Id)
                {
                    EmbedBuilder embed = new EmbedBuilder();
                    embed.WithAuthor(msg.Author);
                    embed.WithDescription(msg.Content);

                    EmbedBuilder embedGuild = new EmbedBuilder();
                    embedGuild.WithAuthor(msg.Author);
                    embedGuild.WithDescription(msg.Content);
                    if (Program.broadcastServerName.Contains(context.Guild.Id))
                    {
                        EmbedFooterBuilder footer = new EmbedFooterBuilder();
                        footer.WithIconUrl(context.Guild.IconUrl);
                        footer.WithText(context.Guild.Name);

                        embedGuild.WithFooter(footer);
                    }

                    foreach (ulong serverID in Program.interServerChats.Keys.ToList())
                    {
                        if (serverID != context.Guild.Id)
                        {
                            if (Program.showUserServer.Contains(serverID))
                            {
                                await (context.Client.GetGuild(serverID).GetChannel(Program.interServerChats[serverID]) as SocketTextChannel).SendMessageAsync("", false, embedGuild);
                            }
                            else
                            {
                                await (context.Client.GetGuild(serverID).GetChannel(Program.interServerChats[serverID]) as SocketTextChannel).SendMessageAsync("", false, embed);
                            }
                        }
                    }
                }
            }
        }
    }
}
