using BigBang1112.DiscordBot.Data;

namespace BigBang1112.BongoBot3.Commands;

[DiscordBotCommand("memes")]
public partial class MemesCommand : GuildCommand
{
    private readonly IDiscordBotRepo _repo;

    public MemesCommand(DiscordBotService discordBotService, IDiscordBotRepo repo) : base(discordBotService, repo)
    {
        _repo = repo;
    }

    public override async Task<DiscordBotMessage> ExecuteWithJoinedGuildAsync(SocketSlashCommand slashCommand, Deferer deferer, DiscordBot.Models.Db.DiscordBotJoinedGuildModel joinedGuild, SocketTextChannel textChannel)
    {
        var meme = await _repo.GetRandomMemeAsync(joinedGuild);

        if (meme is null)
        {
            return new DiscordBotMessage("There are no memes stored.");
        }

        return new DiscordBotMessage(meme.Content);
    }
}