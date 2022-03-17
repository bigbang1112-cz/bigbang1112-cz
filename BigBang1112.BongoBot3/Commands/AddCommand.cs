namespace BigBang1112.BongoBot3.Commands;

[DiscordBotCommand("add")]
public partial class AddCommand : DiscordBotCommand
{
    public AddCommand(DiscordBotService discordBotService) : base(discordBotService)
    {

    }

    public override Task<DiscordBotMessage> ExecuteAsync(SocketSlashCommand slashCommand)
    {
        throw new NotImplementedException();
    }
}