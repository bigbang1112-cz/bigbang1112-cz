using BigBang1112.DiscordBot;
using BigBang1112.DiscordBot.Attributes;
using BigBang1112.DiscordBot.Models;
using Discord.WebSocket;

namespace BigBang1112.UniReminder.Commands;

[DiscordBotCommand("dobrý")]
public partial class DobryCommand : DiscordBotCommand
{
    public DobryCommand(DiscordBotService discordBotService) : base(discordBotService)
    {
        
    }

    public override Task<DiscordBotMessage> ExecuteAsync(SocketInteraction slashCommand)
    {
        return base.ExecuteAsync(slashCommand);
    }

    [DiscordBotSubCommand("den")]
    public partial class Den : DiscordBotCommand
    {
        public Den(DiscordBotService discordBotService) : base(discordBotService)
        {

        }

        public override Task<DiscordBotMessage> ExecuteAsync(SocketInteraction slashCommand)
        {
            return Task.FromResult(new DiscordBotMessage("Dobrý den, děkuji že jste vydrželi"));
        }
    }
}
