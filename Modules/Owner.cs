using Discord.Commands;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace AttentionBot.Modules
{
    public class Owner : ModuleBase<SocketCommandContext>
    {
        [Command("online")]
        [RequireOwner]
        public async Task onlineNotify()
        {
            for(int i = 0; i < Program.chanIDs.Length; i++)
            {
                await Context.Client.GetGuild(Program.servIDs[i]).GetTextChannel(Program.chanIDs[i]).SendMessageAsync("Attention! Bot is now online.");
            }
        }

        [Command("announcement")]
        [RequireOwner]
        public async Task announcement(string announceMessage)
        {
            for (int i = 0; i < Program.chanIDs.Length; i++)
            {
                await Context.Client.GetGuild(Program.servIDs[i]).GetTextChannel(Program.chanIDs[i]).SendMessageAsync("Attention! " + announceMessage);
            }
        }

        [Command("restart")]
        [RequireOwner]
        public async Task restartWarning(string time = "2", string _botID = null)
        {
            if (_botID == Program.botID)
            {
                for (int i = 0; i < Program.chanIDs.Length; i++)
                {
                    await Context.Client.GetGuild(Program.servIDs[i]).GetTextChannel(Program.chanIDs[i]).SendMessageAsync("Attention! Bot will go offline for an update in " + time + " minutes.");
                }
            }
            else if (_botID == null)
            {
                for (int i = 0; i < Program.chanIDs.Length; i++)
                {
                    await Context.Client.GetGuild(Program.servIDs[i]).GetTextChannel(Program.chanIDs[i]).SendMessageAsync("Attention! Bot's server will restart in " + time + " minutes.");
                }
            }
        }

        [Command("exit")]
        [RequireOwner]
        public async Task exitAttentionBot(string _botID = "all", string _length = null, string _reason = null)
        {
            if (_botID == Program.botID || _botID == "all")
            {
                Program.chanIDs = Program.chanID.ToArray();
                Program.servIDs = Program.servID.ToArray();
                Program.roleIDs = Program.roleID.ToArray();

                if(_botID != "all")
                {
                    for (int i = 0; i < Program.chanIDs.Length; i++)
                    {
                        await Context.Client.GetGuild(Program.servIDs[i]).GetTextChannel(Program.chanIDs[i]).SendMessageAsync("Attention! Bot is now offline.");
                    }
                }
                else
                {
                    if(_length != null)
                    {
                        if(_reason == null)
                        {
                            for (int i = 0; i < Program.chanIDs.Length; i++)
                            {
                                await Context.Client.GetGuild(Program.servIDs[i]).GetTextChannel(Program.chanIDs[i]).SendMessageAsync("Attention! Bot's server is now offline for " + _length + " hours.");
                            }
                        } else
                        {
                            for (int i = 0; i < Program.chanIDs.Length; i++)
                            {
                                await Context.Client.GetGuild(Program.servIDs[i]).GetTextChannel(Program.chanIDs[i]).SendMessageAsync("Attention! Bot's server is now offline for " + _length + " hours due to " + _reason + ".");
                            }
                        }
                        
                    } else
                    {
                        for (int i = 0; i < Program.chanIDs.Length; i++)
                        {
                            await Context.Client.GetGuild(Program.servIDs[i]).GetTextChannel(Program.chanIDs[i]).SendMessageAsync("Attention! Bot's server is now offline.");
                        }
                    }
                }

                if (Program.isConsole)
                    Console.WriteLine("Attention! Bot Offline");

                Thread.Sleep(1000);
                Environment.Exit(0);
            }
        }
    }
}
