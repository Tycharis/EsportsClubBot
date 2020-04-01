using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using Discord.Commands;

namespace EsportsClubBot
{
    public class RngModule : ModuleBase<SocketCommandContext>
    {
        // $roll 2d4
        [Command("roll")]
        [Summary("Rolls a die. Valid sizes are D&D standards.")]
        public async Task RollAsync([Summary("The number of times and type of die to role, formatted as 1d4, 2d6, etc.")] string param)
        {
            Random random = new Random();
            StringBuilder rolls = new StringBuilder();

            List<int> dieSizes = new List<int> { 4, 6, 8, 10, 12, 20 };

            string[] values = param.Split("d");

            int rollCount;
            int rollMax;

            if (!int.TryParse(values[0], out rollCount))
            {
                rollCount = 1;
            }

            if (!int.TryParse(values[1], out rollMax))
            {
                await Context.Channel.SendMessageAsync("Malformed input. Expected d4, 1d4, 2d6, etc.");
                return;
            }

            if (!dieSizes.Contains(rollMax))
            {
                await Context.Channel.SendMessageAsync("Valid die sizes are d4, d8, d10, d12, and d20");
                return;
            }

            if (rollCount > 100)
            {
                rollCount = 100;
                rolls.Append("Generating only 100 rolls.\n");
            }

            rolls.Append($"{Context.User.Username} rolled: ");

            for (int i = 0; i < rollCount; i++)
            {
                rolls.Append(random.Next(1, rollMax + 1));

                if (i != rollCount - 1)
                {
                    rolls.Append(", ");
                }
            }
            
            await Context.Channel.SendMessageAsync(rolls.ToString());
        }

        [Group("flip")]
        public class FlipModule : ModuleBase<SocketCommandContext>
        {
            // $flip
            [Command]
            [Summary("Flips a coin.")]
            public async Task FlipAsync()
            {
                Random random = new Random();
                string flip = random.Next(0, 2) == 1 ? "Heads" : "Tails";

                await Context.Channel.SendMessageAsync($"{Context.User.Username} flipped: {flip}");
            }

            // $flip 10
            [Command]
            [Summary("Flips a coin a specified number of times.")]
            public async Task FlipAsync(int count)
            {
                Random random = new Random();
                StringBuilder flips = new StringBuilder();

                if (count > 20)
                {
                    count = 20;
                    flips.Append("Generating only 20 flips.\n");
                }

                flips.Append($"{Context.User.Username} flipped: \n\n");

                int headsCount = 0;
                int tailsCount = 0;

                for (int i = 0; i < count; i++)
                {
                    int flip = random.Next(0, 2);

                    if (flip == 1)
                    {
                        flips.Append($"{i + 1}: Heads\n");
                        headsCount++;
                    }
                    else
                    {
                        flips.Append($"{i + 1}: Tails\n");
                        tailsCount++;
                    }
                }

                flips.Append($"\nTotal heads: {headsCount}\nTotal tails: {tailsCount}");

                await Context.Channel.SendMessageAsync(flips.ToString());
            }
        }
    }
}
