using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace ExampleBot
{
    class Program
    {
        static void Main(string[] args) => new Program().RunBot().GetAwaiter().GetResult();

        // Creating the necessary variables
        public static DiscordSocketClient _client; 
        private CommandService _commands;
        private IServiceProvider _services;

        // Runbot task
        public async Task RunBot()
        {
            _client = new DiscordSocketClient(); // Define _client
            _commands = new CommandService(); // Define _commands

            _services = new ServiceCollection() // Define _services
                .AddSingleton(_client)
                .AddSingleton(_commands)
                .BuildServiceProvider();

            string botToken = "TOKENGOESHERE"; // Make a string for the token

            _client.Log += Log; // Logging

            await RegisterCommandsAsync(); // Call registercommands

            await _client.LoginAsync(TokenType.Bot, botToken); // Log into the bot user

            await _client.StartAsync(); // Start the bot user

            await _client.SetGameAsync("GAMENAME"); // Set the game the bot is playing

            await Task.Delay(-1); // Delay for -1 to keep the console window opened
        }

        private async Task RegisterCommandsAsync()
        {
            _client.MessageReceived += HandleCommandAsync; // Messagerecieved

            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), null); // Add module to _commands
        }

        private Task Log(LogMessage arg) // Logging
        {
            Console.WriteLine(arg); // Print the log to Console
            return Task.CompletedTask; // Return with completedtask
        }

        private async Task HandleCommandAsync(SocketMessage arg)
        {
            string messageLower = arg.Content.ToLower(); // Convert the message to a Lower
            var message = arg as SocketUserMessage; // Create a variable with the message as SocketUserMessage
            if (message is null || message.Author.IsBot) return; // Checks if the message is empty or sent by a bot
            int argumentPos = 0; // Sets the argpos to 0 (the start of the message)
            if (message.HasStringPrefix("PREFIXGOESHERE", ref argumentPos) || message.HasMentionPrefix(_client.CurrentUser, ref argumentPos)) // If the message has the prefix at the start or starts with someone mentioning the bot
            {
                var context = new SocketCommandContext(_client, message); // Create a variable called context
                var result = await _commands.ExecuteAsync(context, argumentPos, _services); // Create a veriable called result
                if (!result.IsSuccess) // If the result is unsuccessful
                {
                    Console.WriteLine(result.ErrorReason); // Print the error to console
                    await message.Channel.SendMessageAsync(result.ErrorReason); // Print the error to the channel where the error was caused (e.g "Unknown Command")
                }
            }
        }
    }
}
