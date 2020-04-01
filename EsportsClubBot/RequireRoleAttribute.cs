using System;
using System.Linq;
using System.Threading.Tasks;

using Discord.Commands;
using Discord.WebSocket;

namespace EsportsClubBot
{
    public class RequireRoleAttribute : PreconditionAttribute
    {
        private readonly string _name;

        public RequireRoleAttribute(string name) => _name = name;

        public override Task<PreconditionResult> CheckPermissionsAsync(ICommandContext context, CommandInfo command, IServiceProvider services)
        {
            if (context.User is SocketGuildUser user)
            {
                if (user.Roles.Any(r => r.Name == _name))
                {
                    return Task.FromResult(PreconditionResult.FromSuccess());
                }
                else return Task.FromResult(PreconditionResult.FromError($"You must have the {_name} role to run this command."));
            }
            else return Task.FromResult(PreconditionResult.FromError("You must be in a server to run this command."));
        }
    }
}
