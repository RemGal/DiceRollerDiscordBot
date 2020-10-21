using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace DiceRollerDiscordBot
{
    class Commands : BaseCommandModule
    {
        [Command("ping")]
        [Description("Returns \"Pong!\"")]
        public async Task Ping(CommandContext ctx)
        {
            await ctx.Channel.SendMessageAsync(ctx.Member.Mention + " Pong! ").ConfigureAwait(false);
        }

        [Command("cor")]
        [Description("Coriolis")]
        public async Task Cor(CommandContext ctx, int ile)
        {
            Random rnd = new Random();
            string output = "\r";
            for (int i = 0; i < ile; i++)
            {
                int temp = rnd.Next(1, 7);
                if (temp >= 6) { output += DSharpPlus.Formatter.Bold(temp.ToString()) + " "; }
                else { output += temp.ToString() + " "; }
            }
            await ctx.Channel.SendMessageAsync(ctx.Member.Mention + output).ConfigureAwait(false);
        }

        [Command("com")]
        [Description("City of Mist")]
        public async Task Com(CommandContext ctx, params string[] args)
        {
            string mod = string.Join("", args);
            Random rnd = new Random();
            string output = "\r";
            int d1 = rnd.Next(1, 7);
            int d2 = rnd.Next(1, 7);
            int sum = d1 + d2;
            output += d1 + " + " + d2 + " = " + DSharpPlus.Formatter.Bold(sum.ToString());
            if (mod != null && mod != "")
            {
                sum += Int32.Parse(mod);
                if (mod.Substring(0, 1) != "-" && mod.Substring(0, 1) != "+") { mod.Insert(0, "+"); }
                output += " (" + mod.Substring(0, 1) + mod.Substring(1, mod.Length - 1) + " = " + DSharpPlus.Formatter.Bold(sum.ToString()) + ")";
            }
            await ctx.Channel.SendMessageAsync(ctx.Member.Mention + output).ConfigureAwait(false);
        }

        [Command("dnd")]
        [Description("Dungeons & Dragons")]
        public async Task Dnd(CommandContext ctx, params string[] args)
        {
            string mod = string.Join("", args);
            Random rnd = new Random();
            int d = rnd.Next(1, 21);
            string output = "\r";
            if (mod != null && mod != "")
            {
                int sum = d + Int32.Parse(mod);
                if (mod.Substring(0, 1) != "-" && mod.Substring(0, 1) != "+") { mod.Insert(0, "+"); }
                output += d + " " + mod.Substring(0, 1) + " " + mod.Substring(1, mod.Length - 1) + " = " + DSharpPlus.Formatter.Bold(sum.ToString());
            }
            else { output += DSharpPlus.Formatter.Bold(d.ToString()); }
            await ctx.Channel.SendMessageAsync(ctx.Member.Mention + output).ConfigureAwait(false);

        }

        [Command("wh")]
        [Description("Warhammer")]
        public async Task Wh(CommandContext ctx)
        {
            Random rnd = new Random();
            int d = rnd.Next(1, 101);
            await ctx.Channel.SendMessageAsync(ctx.Member.Mention + "\r" + DSharpPlus.Formatter.Bold(d.ToString())).ConfigureAwait(false);
        }
    }
}
