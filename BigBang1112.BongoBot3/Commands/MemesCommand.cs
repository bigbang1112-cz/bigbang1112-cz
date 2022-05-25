using BigBang1112.DiscordBot.Data;

namespace BigBang1112.BongoBot3.Commands;

[DiscordBotCommand("memes")]
public partial class MemesCommand : GuildCommand
{
    private readonly IDiscordBotUnitOfWork _discordBotUnitOfWork;

    public MemesCommand(DiscordBotService discordBotService, IDiscordBotUnitOfWork discordBotUnitOfWork) : base(discordBotService, discordBotUnitOfWork)
    {
        _discordBotUnitOfWork = discordBotUnitOfWork;
    }

    public override async Task<DiscordBotMessage> ExecuteWithJoinedGuildAsync(SocketInteraction slashCommand, Deferer deferer, DiscordBot.Models.Db.DiscordBotJoinedGuildModel joinedGuild, SocketTextChannel textChannel)
    {
        var meme = await _discordBotUnitOfWork.Memes.GetRandomAsync(joinedGuild);

        if (meme is null)
        {
            return new DiscordBotMessage("There are no memes stored.");
        }

        return new DiscordBotMessage(meme.Content);
    }
}