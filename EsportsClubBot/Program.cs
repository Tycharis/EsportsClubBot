using System;
using System.Threading.Tasks;

using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace EsportsClubBot
{
    internal class Program
    {
        private DiscordSocketClient _client;

        // ReSharper disable once ArrangeTypeMemberModifiers
        static void Main(string[] args) => new Program().MainAsync().GetAwaiter().GetResult();

        public async Task MainAsync()
        {
            Console.CancelKeyPress += OnSigInt;

            DiscordSocketConfig config = new DiscordSocketConfig
            {
                MessageCacheSize = 100,
                ExclusiveBulkDelete = true
            };

            _client = new DiscordSocketClient(config);

            _client.Log += Log;

            CommandServiceConfig commandConfig = new CommandServiceConfig
            {
                SeparatorChar = ' ',
                DefaultRunMode = RunMode.Async
            };

            CommandService service = new CommandService(commandConfig);
            CommandHandler handler = new CommandHandler(_client, service);

            await handler.InstallCommandsAsync();

            await _client.LoginAsync(TokenType.Bot, Environment.GetEnvironmentVariable("DiscordToken"));
            await _client.StartAsync();

            _client.Ready += () =>
            {
                Console.WriteLine("Connected to Discord");
                return Task.CompletedTask;
            };

            await Task.Delay(-1);
        }

        protected async void OnSigInt(object sender, ConsoleCancelEventArgs args)
        {
            Console.WriteLine("Bot shutting down.");
            await _client.StopAsync();
            Console.WriteLine("Bot shut down.");
        }

        private static Task Log(LogMessage message)
        {
            Console.WriteLine(message.ToString());
            return Task.CompletedTask;
        }
    }
}
