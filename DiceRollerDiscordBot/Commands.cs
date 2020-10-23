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
        public async Task Cor(CommandContext ctx, [Description("Quantity of D6's to roll.")] int amount)
        {
            Random rnd = new Random();
            string output = "\r";
            for (int i = 0; i < amount; i++)
            {
                int temp = rnd.Next(1, 7);
                if (temp >= 6) { output += DSharpPlus.Formatter.Bold(temp.ToString()) + " "; }
                else { output += temp.ToString() + " "; }
            }
            await ctx.Channel.SendMessageAsync(ctx.Member.Mention + output).ConfigureAwait(false);
        }

        [Command("com")]
        [Description("City of Mist")]
        public async Task Com(CommandContext ctx, [Description("Modifier i.e. \'+3\' or \'-1\'. Left blank if you want to roll without it.")] params string[] args)
        {
            string input = string.Join("", args);
            Random rnd = new Random();
            string output = "\r";
            int d1 = rnd.Next(1, 7);
            int d2 = rnd.Next(1, 7);
            int sum = d1 + d2;
            output += d1 + " + " + d2 + " = " + DSharpPlus.Formatter.Bold(sum.ToString());
            if (input != null && input != "")
            {
                sum += Int32.Parse(input);
                if (input.Substring(0, 1) != "-" && input.Substring(0, 1) != "+") { input.Insert(0, "+"); }
                output += " (" + input.Substring(0, 1) + input.Substring(1) + " = " + DSharpPlus.Formatter.Bold(sum.ToString()) + ")";
            }
            await ctx.Channel.SendMessageAsync(ctx.Member.Mention + output).ConfigureAwait(false);
        }

        [Command("dnd")]
        [Description("Dungeons & Dragons 5e")]
        public async Task Dnd(CommandContext ctx, [Description("Advantage/Disadvantage and/or inputifier i.e. \'+3\', \'A\', \'D-1\'. Left blank if you want to roll without it.")] params string[] args)
        {
            string input = string.Join("", args).ToLower();
            Random rnd = new Random();
            int roll = 0;
            string output = "\r";
            if (input != null && input != "")
            {
                if (input.Contains('a')) { 
                    int d1 =  rnd.Next(1, 21);
                    int d2 = rnd.Next(1, 21);
                    roll = Math.Max(d1,d2);

                    if (d1 == d2) { output += DSharpPlus.Formatter.Bold(d1.ToString()) + " " + DSharpPlus.Formatter.Bold(d1.ToString()); }
                    else if (d1 == roll) { output += DSharpPlus.Formatter.Bold(d1.ToString()) + " " + d2; }
                    else { output += d1 + " " + DSharpPlus.Formatter.Bold(d2.ToString()); }

                    if (input.Length > 1)
                    {
                        if (input.Substring(1, 1) != "-" && input.Substring(1, 1) != "+") { input.Insert(1, "+"); }
                        int sum = roll + Int32.Parse(input.Substring(1));
                        output += "\r" + roll + " " + input.Substring(1, 1) + " " + input.Substring(2) + " = " + DSharpPlus.Formatter.Bold(sum.ToString());

                    }
                }
                else if (input.Contains('d'))
                {
                    int d1 = rnd.Next(1, 21);
                    int d2 = rnd.Next(1, 21);
                    roll = Math.Min(d1, d2);

                    if (d1 == d2) { output += DSharpPlus.Formatter.Bold(d1.ToString()) + " " + DSharpPlus.Formatter.Bold(d1.ToString()); }
                    else if (d1 == roll) { output += DSharpPlus.Formatter.Bold(d1.ToString()) + " " + d2; }
                    else { output += d1 + " " + DSharpPlus.Formatter.Bold(d2.ToString()); }

                    if (input.Length > 1)
                    {
                        if (input.Substring(1, 1) != "-" && input.Substring(1, 1) != "+") { input.Insert(1, "+"); }
                        int sum = roll + Int32.Parse(input.Substring(1));
                        output += "\r" + roll + " " + input.Substring(1, 1) + " " + input.Substring(2) + " = " + DSharpPlus.Formatter.Bold(sum.ToString());

                    }
                }
                else if ("+-0123456789".Contains(input.Substring(0, 1)))
                {
                    int d = rnd.Next(1, 21);
                    if (input.Substring(0, 1) != "-" && input.Substring(0, 1) != "+") { input.Insert(0, "+"); };
                    int sum = roll + Int32.Parse(input.Substring(1));
                    output += d + " " + input.Substring(0, 1) + " " + input.Substring(1) + " = " + DSharpPlus.Formatter.Bold(sum.ToString());
                }
                
            }
            else { 
                int d = rnd.Next(1, 21);
                output += DSharpPlus.Formatter.Bold(d.ToString());
                }
            await ctx.Channel.SendMessageAsync(ctx.Member.Mention + output).ConfigureAwait(false);

        }

        [Command("wh")]
        [Description("Warhammer d100 based systems")]
        public async Task Wh(CommandContext ctx)
        {
            Random rnd = new Random();
            int d = rnd.Next(1, 101);
            await ctx.Channel.SendMessageAsync(ctx.Member.Mention + "\r" + DSharpPlus.Formatter.Bold(d.ToString())).ConfigureAwait(false);
        }


        //TO DO: make it priettier
        [Command("roll")]
        [Description("Any roll based on your input.")]
        public async Task Roll(CommandContext ctx, params string[] args)
        {
            Random rnd = new Random();
            string input = string.Join("", args).ToUpper();
            input = input.Replace(" ","");
            string output = "\r";

            List<string> elements = new List<string>();
            List<int> rolls = new List<int>();

            string tempStr = "";
            string[] charTypes = new string[] { "NUMBER", "OPERATOR", "DICE" };
            string lastChar = "";
            if ("0123456789".Contains(input[0])) { lastChar = "NUMBER"; }
            else if ("+-/*".Contains(input[0])) { lastChar = "OPERATOR"; }
            else if ("Dd".Contains(input[0])) { lastChar = "DICE"; }

            for (int i = 0; i < input.Length; i++) 
            {
                if (GetTypeName(input[i]) == lastChar) { tempStr += input[i]; }
                else {
                    elements.Add(tempStr.Substring(0));
                    tempStr = "" + input[i];
                    lastChar = GetTypeName(input[i]); ;
                }
                if (i == input.Length - 1) { elements.Add(tempStr.Substring(0)); }
            }

            for (int i = 0; i < elements.Count; i++)
            { 
                if (elements[i] == "D")
                {
                    int loopLength;
                    if (i == 0 || !Char.IsNumber(elements[i - 1][0]))
                    {
                        output += DSharpPlus.Formatter.Bold(elements[i] + elements[i + 1] + ":");
                        loopLength = 1;
                    }
                    else
                    {
                        output += DSharpPlus.Formatter.Bold(elements[i - 1] + elements[i] + elements[i + 1] + ":");
                        loopLength = Int32.Parse(elements[i - 1]);
                    }
                    int dice = Int32.Parse(elements[i + 1]) + 1;
                    int currentRoll;
                    int sum = 0;
                    for (int j = 0; j < loopLength; j++)
                    {
                        currentRoll = rnd.Next(1, dice);
                        output += " " + currentRoll;
                        sum += currentRoll;
                    }

                    elements.RemoveAt(i+1);
                    if (loopLength > 1)
                    {
                        elements.RemoveAt(i - 1);
                        output += " (=" + sum + ")";
                    }
                    output += "\r";
                    rolls.Add(sum);

                }
            }


            if (elements.Count > 1) {
                int rollIndex = 0;
                string lastLineEquasion = "";
                for (int i = 0; i < elements.Count; i++)
                {
                    if (elements[i] == "D")
                    {
                        output += DSharpPlus.Formatter.Bold(rolls[rollIndex].ToString()) + " ";
                        lastLineEquasion += rolls[rollIndex];
                        rollIndex++;
                    }
                    else
                    {
                        lastLineEquasion += elements[i];
                        output += elements[i] + " ";
                    }
                }
                string suma = new DataTable().Compute(lastLineEquasion, null).ToString();
                output += "= " + DSharpPlus.Formatter.Bold(suma);
            }
            
            await ctx.Channel.SendMessageAsync(ctx.Member.Mention + output).ConfigureAwait(false);
        }

        //TAKE A LOOK AT THIW WHEN YOU WONT BE SLEEPY!
        public string GetTypeName(char input) {
            if ("0123456789".Contains(input)) { return "NUMBER"; }
            else if ("+-/*".Contains(input)) { return "OPERATOR"; }
            else if ("Dd".Contains(input)) { return "DICE"; }
            else return null;
        }

    }
}
