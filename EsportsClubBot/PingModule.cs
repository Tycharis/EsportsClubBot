using System.Threading.Tasks;

using Discord.Commands;

namespace EsportsClubBot
{
    // $ping
    public class PingModule : ModuleBase<SocketCommandContext>
    {
        [Command("ping")]
        [Summary("Pongs a ping.")]
        public Task PongAsync() => ReplyAsync("Pong!");
    }
}
