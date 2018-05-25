using Discord.Commands;
using Discord.WebSocket;
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
        }

        private async Task CleanDatabase(SocketGuild g)
        {
            if (Program.servChanID.ContainsKey(g.Id))
            {
                Program.servChanID.Remove(g.Id);
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
            if (msg.HasCharPrefix(prefix, ref argPos) && !context.User.IsBot)
            {
                var result = await _service.ExecuteAsync(context, argPos);

                if (!result.IsSuccess && result.Error != CommandError.UnknownCommand)
                {
                    await context.Channel.SendMessageAsync("Error: " + result.ErrorReason);
                }
            }
        }
    }
}
