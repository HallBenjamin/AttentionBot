using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AttentionBot.Modules
{
    public class Attention : ModuleBase<SocketCommandContext>
    {
        private static readonly Random rnd = new Random();
        private static readonly string KARFSpecial =
            "Every day, hopeless idiots go out into the world, driving drunk or using the word \"literally\" incorrectly without anyone to explain just how wrong they are.\n" +
            "Seriously Greg, you can LITERALLY go die in a barn fire.";

        public async Task<string> AttentionMessage(string position = null) // Get the message to use
        {
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

            string[] Text = new string[3]
            {
                "Attention to the designated grid square!",
                "Attention to the designated grid zone!",
                "Attention to the map!"
            };

            string text = Text[rnd.Next(0, 3)];

            return await Task.Run(() => $"{text} ({letter}{number})");
        }

        [Command("attention")]
        public async Task AttentionTo(string position = null, string _mentionID = null) // User ID used
        {
            if (_mentionID == null && position != null)
            {
                _mentionID = position;
            }

            if (!ulong.TryParse(_mentionID, out ulong i))
            {
                _mentionID = null;
            }

            SocketUser user = Context.Guild.Users.FirstOrDefault(x => x.Id == Convert.ToUInt64(_mentionID));

            if (Context.Guild.Users.Contains(user))
            {
                position = null;
            }
            else
            {
                _mentionID = null;
            }

            string message = await AttentionMessage(position);

            if (Program.mentionID.Contains(Context.Guild.Id) && _mentionID != null)
            {
                await Context.Channel.SendMessageAsync($"{user.Mention} {message}");
            }
            else
            {
                await Context.Channel.SendMessageAsync(message);
            }
        }

        [Command("attention")]
        public async Task AttentionTo(string position, SocketUser _mention) // User mention used with position
        {
            string message = await AttentionMessage(position);

            if (Program.mentionID.Contains(Context.Guild.Id) && _mention != null)
            {
                await Context.Channel.SendMessageAsync($"{_mention.Mention} {message}");
            }
            else
            {
                await Context.Channel.SendMessageAsync(message);
            }
        }

        [Command("attention")]
        public async Task AttentionTo(SocketUser _mention, string position = null) // User mention used first (position optional)
        {
            string message = await AttentionMessage(position);

            if (Program.mentionID.Contains(Context.Guild.Id) && _mention != null)
            {
                await Context.Channel.SendMessageAsync($"{_mention.Mention} {message}");
            }
            else
            {
                await Context.Channel.SendMessageAsync(message);
            }
        }

        [Command("gary")]
        public async Task SaveMyFamily(string _mentionID = null) // User ID used
        {
            SocketUser user = Context.Guild.Users.FirstOrDefault(x => x.Id == Convert.ToUInt64(_mentionID));
            if (Program.mentionID.Contains(Context.Guild.Id) && _mentionID != null && Context.Guild.Users.Contains(user))
            {
                await Context.Channel.SendMessageAsync($"{user.Mention} We must save my family!");
            }
            else
            {
                await Context.Channel.SendMessageAsync("We must save my family!");
            }
        }

        [Command("gary")]
        public async Task SaveMyFamily(SocketUser _mention) // User mention used
        {
            if (Program.mentionID.Contains(Context.Guild.Id) && Context.Guild.Users.Contains(_mention))
            {
                await Context.Channel.SendMessageAsync($"{_mention.Mention} We must save my family!");
            }
            else
            {
                await Context.Channel.SendMessageAsync("We must save my family!");
            }
        }

        [Command("bandits")]
        public async Task BanditsComing(string _mentionID = null) // User ID used
        {
            SocketUser user = Context.Guild.Users.FirstOrDefault(x => x.Id == Convert.ToUInt64(_mentionID));
            if (Program.mentionID.Contains(Context.Guild.Id) && _mentionID != null && Context.Guild.Users.Contains(user))
            {
                await Context.Channel.SendMessageAsync($"{user.Mention} The bandits are coming!");
            }
            else
            {
                await Context.Channel.SendMessageAsync("The bandits are coming!");
            }
        }

        [Command("bandits")]
        public async Task BanditsComing(SocketUser _mention) // User mention used
        {
            if (Program.mentionID.Contains(Context.Guild.Id) && Context.Guild.Users.Contains(_mention))
            {
                await Context.Channel.SendMessageAsync($"{_mention.Mention} The bandits are coming!");
            }
            else
            {
                await Context.Channel.SendMessageAsync("The bandits are coming!");
            }
        }

        [Command("sword")]
        public async Task Sword(string _mentionID = null) // User ID used
        {
            SocketUser user = Context.Guild.Users.FirstOrDefault(x => x.Id == Convert.ToUInt64(_mentionID));
            if (Program.mentionID.Contains(Context.Guild.Id) && _mentionID != null && Context.Guild.Users.Contains(user))
            {
                await Context.Channel.SendMessageAsync($"{user.Mention} There's a person attached to this sword, you know! I WILL NOT BE OBJECTIFIED!");
            }
            else
            {
                await Context.Channel.SendMessageAsync("There's a person attached to this sword, you know! I WILL NOT BE OBJECTIFIED!");
            }
        }

        [Command("sword")]
        public async Task Sword(SocketUser _mention) // User mention used
        {
            if (Program.mentionID.Contains(Context.Guild.Id) && Context.Guild.Users.Contains(_mention))
            {
                await Context.Channel.SendMessageAsync($"{_mention.Mention} There's a person attached to this sword, you know! I WILL NOT BE OBJECTIFIED!");
            }
            else
            {
                await Context.Channel.SendMessageAsync("There's a person attached to this sword, you know! I WILL NOT BE OBJECTIFIED!");
            }
        }

        public async Task<string> KARFMessage()
        {
            string[] Messages = new string[8]
            {
                "Have you ever been wrong about something?\n" +
                "Of course you have, you microcephalic addlepate!",

                "Here at KARF, we strive to bring knowledge to a world that is so utterly devoid of it.\n" +
                "I also just realized how terrible that acronym sounds and won't be using it again.",

                "Every day, hopeless idiots go out into the world, driving drunk or using the word \"literally\" incorrectly without anyone to explain just how wrong they are.\n" +
                "Seriously Greg, you can LITERALLY go die in a barn fire.",

                "Did you know that over 99% of people are, in fact, idiots? Terrifying, I know.",

                "But the good news is: there's a cure. \\*Idiot shoots himself with a shotgun\\*\n" +
                "The better news is, there's a better cure!",

                "You see, I am one of the select few who are completely immune to ignorant bullshit, and it is my dream to spread my wisdom full-time.",

                "For a monthly donation of your choosing, you can ensure that there will always be at least one soldier in the fight against stupidity's overwhelming hordes.",

                "We may never be able to stop people from doing stupid shit, but with your help, I will always be there to call them on it afterwards.\n" +
                "And really, isn't that what truly matters?",
            };

            string message = Messages[rnd.Next(0, 8)];

            return await Task.Run(() => message);
        }

        [Command("karf")]
        public async Task KiritoIsAlwaysRightFoundation(string _mentionID = null) // User ID used
        {
            string message = await KARFMessage();

            SocketUser user = Context.Guild.Users.FirstOrDefault(x => x.Id == Convert.ToUInt64(_mentionID));
            if (Program.mentionID.Contains(Context.Guild.Id) && _mentionID != null && Context.Guild.Users.Contains(user))
            {
                if (message == KARFSpecial)
                {
                    await Context.Channel.SendMessageAsync("Every day, hopeless idiots go out into the world, driving drunk or using the word \"literally\" incorrectly without anyone to explain just how wrong they are.\n" +
                        $"Seriously {user.Mention}, you can LITERALLY go die in a barn fire.");
                }
                else
                {
                    await Context.Channel.SendMessageAsync($"{user.Mention} {message}");
                }
            }
            else
            {
                await Context.Channel.SendMessageAsync(message);
            }
        }

        [Command("karf")]
        public async Task KiritoIsAlwaysRightFoundation(SocketUser _mention) // User mention used
        {
            string message = await KARFMessage();

            if (Program.mentionID.Contains(Context.Guild.Id) && Context.Guild.Users.Contains(_mention))
            {
                if (message == KARFSpecial)
                {
                    await Context.Channel.SendMessageAsync("Every day, hopeless idiots go out into the world, driving drunk or using the word \"literally\" incorrectly without anyone to explain just how wrong they are.\n" +
                        $"Seriously {_mention.Mention}, you can LITERALLY go die in a barn fire.");
                }
                else
                {
                    await Context.Channel.SendMessageAsync($"{_mention.Mention} {message}");
                }
            }
            else
            {
                await Context.Channel.SendMessageAsync(message);
            }
        }
    }
}
