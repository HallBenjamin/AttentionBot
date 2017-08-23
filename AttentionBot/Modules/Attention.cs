using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AttentionBot.Modules
{
    public class Attention : ModuleBase<SocketCommandContext>
    {
        [Command(null)]
        // TODO: Fix command, add randomizer
        public async Task attention()
        {
            await Context.Channel.SendMessageAsync("Attention to A3");
        }
    }
}
