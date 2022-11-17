using BigBang1112.DiscordBot.Data;
using BigBang1112.DiscordBot.Models.Db;

namespace BigBang1112.BongoBot3.Commands;

public partial class MemeCommand
{
    [DiscordBotSubCommand("last", "Gets the last meme.")]
    public class Last : GuildCommand
    {
        private readonly IDiscordBotUnitOfWork _discordBotUnitOfWork;

        public Last(DiscordBotService discordBotService, IDiscordBotUnitOfWork discordBotUnitOfWork) : base(discordBotService, discordBotUnitOfWork)
        {
            _discordBotUnitOfWork = discordBotUnitOfWork;
        }

        public override async Task<DiscordBotMessage> ExecuteWithJoinedGuildAsync(SocketInteraction slashCommand, Deferer deferer, DiscordBotJoinedGuildModel joinedGuild, SocketTextChannel textChannel)
        {
            var meme = await _discordBotUnitOfWork.Memes.GetLastAsync(joinedGuild);

            if (meme is null)
            {
                return new DiscordBotMessage("There are no memes at the moment.", ephemeral: true);
            }

            return new DiscordBotMessage(meme.Content);
        }
    }
}