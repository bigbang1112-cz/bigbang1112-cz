namespace BigBang1112.BongoBot3.Commands;

public partial class AddCommand
{
    [DiscordBotSubCommand("meme")]
    public class Meme : DiscordBotCommand
    {
        public Meme(DiscordBotService discordBotService) : base(discordBotService)
        {

        }

        public override Task<DiscordBotMessage> ExecuteAsync(SocketSlashCommand slashCommand)
        {
            throw new NotImplementedException();
        }
    }
}