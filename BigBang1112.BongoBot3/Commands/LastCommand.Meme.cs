using BigBang1112.DiscordBot.Data;
using BigBang1112.DiscordBot.Models.Db;

namespace BigBang1112.BongoBot3.Commands;

public partial class LastCommand
{
    [DiscordBotSubCommand("meme", "Gets the last meme.")]
    public class Meme : GuildCommand
    {
        private readonly IDiscordBotRepo _repo;

        public Meme(DiscordBotService discordBotService, IDiscordBotRepo repo) : base(discordBotService, repo)
        {
            _repo = repo;
        }

        public override async Task<DiscordBotMessage> ExecuteWithJoinedGuildAsync(SocketSlashCommand slashCommand, Deferer deferer, DiscordBotJoinedGuildModel joinedGuild, SocketTextChannel textChannel)
        {
            var meme = await _repo.GetLastMemeAsync(joinedGuild);

            if (meme is null)
            {
                return new DiscordBotMessage("There are no memes at the moment.", ephemeral: true);
            }

            return new DiscordBotMessage(meme.Content);
        }
    }
}
