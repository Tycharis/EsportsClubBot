using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace EsportsClubBot
{
    [Group("admin")]
    public class AdminModule : ModuleBase<SocketCommandContext>
    {
        [Group("broadcast")]
        public class BroadcastModule : ModuleBase<SocketCommandContext>
        {
            // $admin broadcast @Officer This is a test message to all Officers.
            [Command]
            [RequireRole("Officer")]
            [Summary("Broadcasts a message to a select role.")]
            public async Task BroadcastAsync([Summary("The role to send to.")] IRole role,
                [Summary("The message to send.")] [Remainder] string message)
            {
                IReadOnlyCollection<SocketGuildUser> users = Context.Guild.Users;

                if (role != null)
                {
                    users = users.Where(u => u.Roles.Any(r => r.Name == role.Name)).ToList();
                }

                foreach (SocketGuildUser user in users)
                {
                    await user.SendMessageAsync(message);
                }
            }

            // $admin broadcast This is a test message to all users.
            [Command]
            [RequireRole("Officer")]
            [Summary("Broadcasts a message to everyone on the server.")]
            public async Task BroadcastAsync([Summary("The message to send.")] [Remainder] string message)
            {
                await BroadcastAsync(null, message);
            }
        }
        
        [Group("purge")]
        public class PurgeModule : ModuleBase<SocketCommandContext>
        {
            // $admin purge
            [Command]
            [RequireUserPermission(ChannelPermission.ManageMessages, ErrorMessage = "You do not have manage messages permissions in this channel. Contact an Officer for support.")]
            [Summary("Purges the last message from the channel.")]
            public async Task PurgeAsync()
            {
                await PurgeAsync(1);
            }

            // $admin purge 5
            [Command]
            [RequireUserPermission(ChannelPermission.ManageMessages, ErrorMessage = "You do not have manage messages permissions in this channel. Contact an Officer for support.")]
            [Summary("Purges a specified number of messages from the channel.")]
            public async Task PurgeAsync([Summary("The number of messages to purge.")] int count)
            {
                IEnumerable<IMessage> messages = await Context.Channel.GetMessagesAsync(Context.Message, Direction.Before, count).FlattenAsync();

                foreach (IMessage message in messages)
                {
                    await message.DeleteAsync();
                }

                await Context.Message.DeleteAsync();
            }
        }
    }
}
