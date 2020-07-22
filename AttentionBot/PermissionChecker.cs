using Discord.WebSocket;

namespace AttentionBot
{
    public static class PermissionChecker
    {
        public static bool HasSend(SocketGuild g, SocketTextChannel channel)
        {
            return g.GetUser(SecurityInfo.botClient).GetPermissions(channel).ViewChannel
                    && g.GetUser(SecurityInfo.botClient).GetPermissions(channel).SendMessages;
        }
    }
}
