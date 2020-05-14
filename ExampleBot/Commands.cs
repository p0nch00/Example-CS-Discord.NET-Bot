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
            await ReplyAsync("Pong! 🏓 **" + Program._client.Latency + "ms**");
        }
    }
}
