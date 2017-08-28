using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;
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

            if(position != null)
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
            if(_botID == Program.botID)
                await Context.Channel.SendMessageAsync("Attention! Bot v1.0.1.4  -  Coded using Discord.Net\n\nPrefix: \\\nCommands: \\attention [position]\n\nPosition can contain one letter A-J and/or one number 1-9.\n\nExamples:\n\\attention\n\\attention a\n\\attention A\n\\attention 4\n\\attention A4\n\\attention a4\n\\attention 4A\n\\attention 4a");
        }

        [Command("exit")]
        [RequireOwner]
        public async Task exitAttentionBot(string _botID = null)
        {
            if(_botID == Program.botID)
            {
                await Context.Channel.SendMessageAsync("Attention! Bot is now offline.");

                if (Program.isConsole)
                    Console.WriteLine("Attention! Bot Offline");

                Thread.Sleep(1000);
                Environment.Exit(0);
            }
        }
    }
}
