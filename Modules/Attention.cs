using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Linq;
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
    }
}
