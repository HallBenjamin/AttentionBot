using Discord.Commands;
using System;
using System.IO;
using System.Linq;
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
            for (int i = 0; i < Program.servIDs.Length; i++)
            {
                if (!(Context.Client.GetGuild(Program.servIDs[i]).IsConnected) || Context.Client.GetGuild(Program.servIDs[i]).GetChannel(Program.chanIDs[i]).Users.Count == 0)
                {
                    Program.servID.Remove(Program.servIDs[i]);
                    Program.chanID.Remove(Program.chanIDs[i]);
                    Program.servIDs = Program.servID.ToArray();
                    Program.chanIDs = Program.chanID.ToArray();

                    BinaryWriter chanWriter = new BinaryWriter(File.Open("channels.txt", FileMode.Truncate));
                    foreach (var value in Program.chanIDs)
                    {
                        chanWriter.Write(value.ToString());
                    }
                    chanWriter.Close();

                    BinaryWriter servWriter = new BinaryWriter(File.Open("servers.txt", FileMode.Truncate));
                    foreach (var value in Program.servIDs)
                    {
                        servWriter.Write(value.ToString());
                    }
                    servWriter.Close();
                }
            }

            for (int i = 0; i < Program.chanIDs.Length; i++)
            {
                await Context.Client.GetGuild(Program.servIDs[i]).GetTextChannel(Program.chanIDs[i]).SendMessageAsync("Attention! Bot is now online.");
            }
        }

        [Command("announcement")]
        [RequireOwner]
        public async Task announcement(string announceMessage)
        {
            for (int i = 0; i < Program.servIDs.Length; i++)
            {
                if (!(Context.Client.GetGuild(Program.servIDs[i]).IsConnected) || Context.Client.GetGuild(Program.servIDs[i]).GetChannel(Program.chanIDs[i]).Users.Count == 0)
                {
                    Program.servID.Remove(Program.servIDs[i]);
                    Program.chanID.Remove(Program.chanIDs[i]);
                    Program.servIDs = Program.servID.ToArray();
                    Program.chanIDs = Program.chanID.ToArray();

                    BinaryWriter chanWriter = new BinaryWriter(File.Open("channels.txt", FileMode.Truncate));
                    foreach (var value in Program.chanIDs)
                    {
                        chanWriter.Write(value.ToString());
                    }
                    chanWriter.Close();

                    BinaryWriter servWriter = new BinaryWriter(File.Open("servers.txt", FileMode.Truncate));
                    foreach (var value in Program.servIDs)
                    {
                        servWriter.Write(value.ToString());
                    }
                    servWriter.Close();
                }
            }

            for (int i = 0; i < Program.chanIDs.Length; i++)
            {
                await Context.Client.GetGuild(Program.servIDs[i]).GetTextChannel(Program.chanIDs[i]).SendMessageAsync("Attention! " + announceMessage);
            }
        }

        [Command("restart")]
        [RequireOwner]
        public async Task restartWarning(string _time = "2", string _botID = "all", string _length = null, string _reason = null)
        {
            for(int i = 0; i < Program.servIDs.Length; i++)
            {
                if(!(Context.Client.GetGuild(Program.servIDs[i]).IsConnected) || Context.Client.GetGuild(Program.servIDs[i]).GetChannel(Program.chanIDs[i]).Users.Count == 0)
                {
                    Program.servID.Remove(Program.servIDs[i]);
                    Program.chanID.Remove(Program.chanIDs[i]);
                    Program.servIDs = Program.servID.ToArray();
                    Program.chanIDs = Program.chanID.ToArray();

                    BinaryWriter chanWriter = new BinaryWriter(File.Open("channels.txt", FileMode.Truncate));
                    foreach (var value in Program.chanIDs)
                    {
                        chanWriter.Write(value.ToString());
                    }
                    chanWriter.Close();

                    BinaryWriter servWriter = new BinaryWriter(File.Open("servers.txt", FileMode.Truncate));
                    foreach (var value in Program.servIDs)
                    {
                        servWriter.Write(value.ToString());
                    }
                    servWriter.Close();
                }
            }

            if (_botID == Program.botID)
            {
                for (int i = 0; i < Program.chanIDs.Length; i++)
                {
                    await Context.Client.GetGuild(Program.servIDs[i]).GetTextChannel(Program.chanIDs[i]).SendMessageAsync("Attention! Bot will go offline for an update in " + _time + " minutes.");
                }
            }
            else if (_botID == "all")
            {
                if (_length == null)
                {
                    for (int i = 0; i < Program.chanIDs.Length; i++)
                    {
                        await Context.Client.GetGuild(Program.servIDs[i]).GetTextChannel(Program.chanIDs[i]).SendMessageAsync("Attention! Bot's server will restart in " + _time + " minutes.");
                    }
                }
                else
                {
                    if (_reason == null)
                    {
                        for (int i = 0; i < Program.chanIDs.Length; i++)
                        {
                            await Context.Client.GetGuild(Program.servIDs[i]).GetTextChannel(Program.chanIDs[i]).SendMessageAsync("Attention! Bot's server will shut down in " + _time + " minutes for " + _length + " hours.");
                        }
                    }
                    else
                    {
                        for (int i = 0; i < Program.chanIDs.Length; i++)
                        {
                            await Context.Client.GetGuild(Program.servIDs[i]).GetTextChannel(Program.chanIDs[i]).SendMessageAsync("Attention! Bot's server will shut down in " + _time + " minutes for " + _length + " hours due to " + _reason + ".");
                        }
                    }
                }
            }
        }

        [Command("exit")]
        [RequireOwner]
        public async Task exitAttentionBot(string _botID = "all", string _length = null, string _reason = null)
        {
            for (int i = 0; i < Program.servIDs.Length; i++)
            {
                if (!(Context.Client.GetGuild(Program.servIDs[i]).IsConnected) || Context.Client.GetGuild(Program.servIDs[i]).GetChannel(Program.chanIDs[i]).Users.Count == 0)
                {
                    Program.servID.Remove(Program.servIDs[i]);
                    Program.chanID.Remove(Program.chanIDs[i]);
                    Program.servIDs = Program.servID.ToArray();
                    Program.chanIDs = Program.chanID.ToArray();

                    BinaryWriter chanWriter = new BinaryWriter(File.Open("channels.txt", FileMode.Truncate));
                    foreach (var value in Program.chanIDs)
                    {
                        chanWriter.Write(value.ToString());
                    }
                    chanWriter.Close();

                    BinaryWriter servWriter = new BinaryWriter(File.Open("servers.txt", FileMode.Truncate));
                    foreach (var value in Program.servIDs)
                    {
                        servWriter.Write(value.ToString());
                    }
                    servWriter.Close();
                }
            }

            if (_botID == Program.botID)
            {
                for (int i = 0; i < Program.chanIDs.Length; i++)
                {
                    await Context.Client.GetGuild(Program.servIDs[i]).GetTextChannel(Program.chanIDs[i]).SendMessageAsync("Attention! Bot is now offline.");
                }
            }
            else if (_botID == "all")
            {
                if (_length == null)
                {
                    for (int i = 0; i < Program.chanIDs.Length; i++)
                    {
                        await Context.Client.GetGuild(Program.servIDs[i]).GetTextChannel(Program.chanIDs[i]).SendMessageAsync("Attention! Bot's server is now restarting.");
                    }
                }
                else
                {
                    if (_reason == null)
                    {
                        for (int i = 0; i < Program.chanIDs.Length; i++)
                        {
                            await Context.Client.GetGuild(Program.servIDs[i]).GetTextChannel(Program.chanIDs[i]).SendMessageAsync("Attention! Bot's server is now offline for " + _length + " hours.");
                        }
                    }
                    else
                    {
                        for (int i = 0; i < Program.chanIDs.Length; i++)
                        {
                            await Context.Client.GetGuild(Program.servIDs[i]).GetTextChannel(Program.chanIDs[i]).SendMessageAsync("Attention! Bot's server is now offline for " + _length + " hours due to " + _reason + ".");
                        }
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
