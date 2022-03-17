using BigBang1112.DiscordBot.Data;

namespace BigBang1112.BongoBot3.Commands;

[DiscordBotCommand("memes")]
public partial class MemesCommand : DiscordBotCommand
{
    private readonly IDiscordBotRepo _repo;

    public MemesCommand(DiscordBotService discordBotService, IDiscordBotRepo repo) : base(discordBotService)
    {
        _repo = repo;
    }

    public override async Task<DiscordBotMessage> ExecuteAsync(SocketSlashCommand slashCommand)
    {
        var discordBotGuid = GetDiscordBotGuid();

        if (discordBotGuid is null)
        {
            return new DiscordBotMessage("I cannot store anything into a database.", ephemeral: true);
        }

        if (slashCommand.Channel is not SocketTextChannel textChannel)
        {
            return new DiscordBotMessage("Not a text channel, server cannot be detected.", ephemeral: true);
        }

        var joinedGuild = await _repo.GetJoinedDiscordGuildAsync(discordBotGuid.Value, textChannel);

        if (joinedGuild is null)
        {
            return new DiscordBotMessage("Guild couldn't be detected for unknown reason.", ephemeral: true);
        }

        var meme = await _repo.GetRandomMemeAsync(joinedGuild);

        if (meme is null)
        {
            return new DiscordBotMessage("There are no memes stored.");
        }

        return new DiscordBotMessage(meme.Content);
    }
}