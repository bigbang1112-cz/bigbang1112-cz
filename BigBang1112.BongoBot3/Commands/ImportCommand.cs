namespace BigBang1112.BongoBot3.Commands;

[DiscordBotCommand("import")]
public partial class ImportCommand : DiscordBotCommand
{
    public ImportCommand(DiscordBotService discordBotService) : base(discordBotService)
    {

    }

    public override Task<DiscordBotMessage> ExecuteAsync(SocketSlashCommand slashCommand)
    {
        throw new NotImplementedException();
    }
}
