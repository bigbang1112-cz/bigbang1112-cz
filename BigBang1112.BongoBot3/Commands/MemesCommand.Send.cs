using BigBang1112.DiscordBot.Data;
using BigBang1112.DiscordBot.Models.Db;
using Discord;

namespace BigBang1112.BongoBot3.Commands;

public partial class MemesCommand
{
    [DiscordBotSubCommand("send", "Send a meme to someone.")]
    public class Send : GuildCommand
    {
        private readonly IDiscordBotUnitOfWork _discordBotUnitOfWork;
        
        [DiscordBotCommandOption("who", ApplicationCommandOptionType.Mentionable, "User to send the meme to.", IsRequired = true)]
        public IMentionable? Who { get; set; }

        public Send(DiscordBotService discordBotService, IDiscordBotUnitOfWork discordBotUnitOfWork) : base(discordBotService, discordBotUnitOfWork)
        {
            _discordBotUnitOfWork = discordBotUnitOfWork;
        }

        public override async Task<DiscordBotMessage> ExecuteWithJoinedGuildAsync(SocketInteraction slashCommand, Deferer deferer, DiscordBotJoinedGuildModel joinedGuild, SocketTextChannel textChannel)
        {
            if (Who is null)
            {
                return new DiscordBotMessage("Nobody was mentioned.", ephemeral: true);
            }
            
            var meme = await _discordBotUnitOfWork.Memes.GetRandomAsync(joinedGuild);

            if (meme is null)
            {
                return new DiscordBotMessage("There are no memes at the moment.", ephemeral: true);
            }

            return new DiscordBotMessage($"{meme.Content} {Who.Mention}");
        }
    }
}