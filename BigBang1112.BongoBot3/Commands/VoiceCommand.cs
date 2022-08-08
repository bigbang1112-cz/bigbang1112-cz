using BigBang1112.DiscordBot;
using BigBang1112.DiscordBot.Data;

namespace BigBang1112.BongoBot3.Commands;

[DiscordBotCommand("voice")]
public partial class VoiceCommand : DiscordBotCommand
{
    public VoiceCommand(DiscordBotService discordBotService) : base(discordBotService)
    {
        
    }

    public override async Task<DiscordBotMessage> ExecuteAsync(SocketInteraction slashCommand)
    {
        if (slashCommand.User is not SocketGuildUser guildUser)
        {
            return new DiscordBotMessage("You're not executing this command on a server", ephemeral: true);
        }

        if (guildUser.VoiceChannel is null)
        {
            return new DiscordBotMessage("You're not in a voice channel", ephemeral: true);
        }

        await guildUser.VoiceChannel.ConnectAsync(external: true);

        return new DiscordBotMessage("I joined");
    }
}