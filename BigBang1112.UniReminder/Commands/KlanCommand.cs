using BigBang1112.DiscordBot;
using BigBang1112.DiscordBot.Attributes;
using BigBang1112.DiscordBot.Models;
using Discord.WebSocket;

namespace BigBang1112.UniReminder.Commands;

[DiscordBotCommand("klán", "Zjistěte existenci pana docenta Klána.")]
public class KlanCommand : DiscordBotCommand
{
    public KlanCommand(DiscordBotService discordBotService) : base(discordBotService)
    {
    }

    public override Task<DiscordBotMessage> ExecuteAsync(SocketInteraction slashCommand)
    {
        return Task.FromResult(new DiscordBotMessage($"Pan docent Klán je nezvěstný již {(int)(DateTime.Now - new DateTime(2021, 9, 30)).TotalDays} dní."));
    }
}
