using BigBang1112.DiscordBot.Data;
using BigBang1112.DiscordBot.Models.Db;
using Discord;

namespace BigBang1112.BongoBot3.Commands;

public partial class AddCommand
{
    [DiscordBotSubCommand("meme")]
    public class Meme : GuildCommand
    {
        private readonly IDiscordBotUnitOfWork _discordBotUnitOfWork;

        [DiscordBotCommandOption("content", ApplicationCommandOptionType.String, "Content of the meme.", IsRequired = true)]
        public string? Content { get; set; }

        public Meme(DiscordBotService discordBotService, IDiscordBotUnitOfWork discordBotUnitOfWork) : base(discordBotService, discordBotUnitOfWork)
        {
            _discordBotUnitOfWork = discordBotUnitOfWork;
        }

        public override async Task<DiscordBotMessage> ExecuteWithJoinedGuildAsync(SocketInteraction slashCommand, Deferer deferer, DiscordBotJoinedGuildModel joinedGuild, SocketTextChannel textChannel)
        {
            if (Content is null)
            {
                return new DiscordBotMessage("Please give me something to add.", ephemeral: true);
            }

            if (await _discordBotUnitOfWork.Memes.ExistsAsync(Content))
            {
                return new DiscordBotMessage("This meme already exists.", ephemeral: true);
            }

            var meme = new MemeModel
            {
                Guid = Guid.NewGuid(),
                JoinedGuild = joinedGuild,
                AddedOn = DateTime.UtcNow,
                AuthorSnowflake = slashCommand.User.Id,
                Content = Content
            };

            await _discordBotUnitOfWork.Memes.AddAsync(meme);

            await _discordBotUnitOfWork.SaveAsync();

            return new DiscordBotMessage
            {
                Message = $"Added a meme: ```{meme.Content}```"
            };
        }
    }
}