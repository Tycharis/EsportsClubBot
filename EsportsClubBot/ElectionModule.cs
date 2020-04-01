using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace EsportsClubBot
{
    [Group("election")]
    public class ElectionModule : ModuleBase<SocketCommandContext>
    {
        // $election distro Member 04APR2020 2359 https://forms.google.com/... true
        [Command("distro")]
        [RequireOwner] // Requires that the President must call an election in this manner.
        [Summary("Sends election information to all users in a specified role.")]
        public async Task DistroAsync([Summary("The specified role to send to.")] string role,
            [Summary("The end date of the vote, formatted as DDMMYYYY.")] string date,
            [Summary("The end time of the vote, formatted as HH:MM (24 hour).")] string time,
            [Summary("The Google Forms ballot link, generic with respect to ID.")] string url,
            [Summary("Whether the election is a special election.")] bool isSpecialElection)
        {
            List<int> voterIds = new List<int>();

            IEnumerable<SocketGuildUser> roleMembers = Context.Guild.Users.Where(u => u.Roles.Any(r => r.Name == role));

            foreach (SocketGuildUser user in roleMembers)
            {
                Random random = new Random();
                int idNumber;

                do
                {
                    idNumber = random.Next(10000, 99999);
                }
                while (voterIds.Any(i => i == idNumber));

                voterIds.Add(idNumber);

                StringBuilder messageBuilder = new StringBuilder();

                messageBuilder.Append($"The Esports Club at Kansas State University is having a{(isSpecialElection ? " special election" : "n election")}. ")
                    .Append($"You are receiving this message because you are a{("aeiouAEIOU".IndexOf(role[0]) >= 0 ? "n " : " ") + role}. \n\n")
                    .Append($"__Election Information__\n\n")
                    .Append($"Respond by: **{date} at {time} CDT (America/Chicago)**\n")
                    .Append($"Response URL: {url}{idNumber}\n\n")
                    .Append($"If you do not see your voter ID number autofilled, your number is **{idNumber}**. ")
                    .Append("This number is private and will not identify you. It will only be used to validate your vote. ")
                    .Append("All votes are confidential, even to the reviewing Officers. ")
                    .Append("The Esports Club at Kansas State University thanks you for your membership.\n\n")
                    .Append("_I am a bot, and this action was performed automatically. Please contact the President if you have any questions or concerns._");

                await user.SendMessageAsync(messageBuilder.ToString());

                voterIds.Sort();

                string list = string.Join(", \n", voterIds);
                List<string> messages = Paginate(list);

                foreach (string message in messages)
                {
                    await Context.Channel.SendMessageAsync(message);
                }
            }
        }

        [Command("distro")]
        [RequireOwner]
        [Summary("Sends election information to all users in a specified role.")]
        public async Task DistroAsync([Summary("The specified role to send to.")] string role,
            [Summary("The end date of the vote, formatted as DDMMYYYY.")] string date,
            [Summary("The end time of the vote, formatted as HH:MM (24 hour).")] string time,
            [Summary("The Google Forms ballot link, generic with respect to ID.")] string url)
        {
            await DistroAsync(role, date, time, url, false);
        }

        private List<string> Paginate(string list)
        {
            List<string> messages = new List<string>();

            if (list.Length > 2000)
            {
                int index = 2000;

                while (list[index] != '\n')
                {
                    index--;
                }

                messages.Add(list.Substring(0, index));
                messages.AddRange(Paginate(list.Substring(index + 1)));
            }
            else
            {
                messages.Add(list);
            }

            return messages;
        }
    }
}
