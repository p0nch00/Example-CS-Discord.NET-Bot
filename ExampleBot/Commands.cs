using Discord;
using Discord.Commands;
using System.Threading.Tasks;

namespace ExampleBot
{
    public class Commands : ModuleBase<SocketCommandContext>
    {
        [Command("ping")]
        private async Task Ping()
        {
            var eb = new EmbedBuilder(); // Creates the embed

            eb.WithColor(Color.Blue); // Adds the blue sidebar to the embed
            eb.WithCurrentTimestamp(); // Adds the current timestamp to the footer
            eb.WithDescription("Pong! " + Program._client.Latency); // Adds a description to the embed to say Pong! and then put the latency

            await ReplyAsync("", false, eb.Build());// Responds with the embed
        }
    }
}
