using System;
using System.Threading.Tasks;

using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace EsportsClubBot
{
    class Program
    {
        private DiscordSocketClient _client;

        static void Main(string[] args) => new Program().MainAsync().GetAwaiter().GetResult();

        public async Task MainAsync()
        {
            Console.CancelKeyPress += new ConsoleCancelEventHandler(OnSigInt);

            DiscordSocketConfig _config = new DiscordSocketConfig
            {
                MessageCacheSize = 100,
                ExclusiveBulkDelete = true
            };

            _client = new DiscordSocketClient(_config);

            _client.Log += Log;

            CommandServiceConfig _commandConfig = new CommandServiceConfig
            {
                SeparatorChar = ' '
            };

            CommandService service = new CommandService(_commandConfig);
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
            await _client.StopAsync();
            Console.WriteLine("Bot shutting down.");

            return;
        }

        private Task Log(LogMessage message)
        {
            Console.WriteLine(message.ToString());
            return Task.CompletedTask;
        }
    }
}
