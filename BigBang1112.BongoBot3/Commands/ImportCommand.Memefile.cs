using BigBang1112.DiscordBot.Data;
using BigBang1112.DiscordBot.Models.Db;
using Discord;
using System.Text;
using System.Xml.Linq;

namespace BigBang1112.BongoBot3.Commands;

public partial class ImportCommand
{
    [DiscordBotSubCommand("memefile")]
    public class Memefile : GuildCommand
    {
        private readonly HttpClient _http;
        private readonly IDiscordBotUnitOfWork _discordBotUnitOfWork;

        [DiscordBotCommandOption("file", ApplicationCommandOptionType.Attachment, "Memefile XML.", IsRequired = true)]
        public Attachment File { get; set; } = default!;

        public Memefile(DiscordBotService discordBotService, IDiscordBotUnitOfWork discordBotUnitOfWork, HttpClient http) : base(discordBotService, discordBotUnitOfWork)
        {
            _http = http;
            _discordBotUnitOfWork = discordBotUnitOfWork;
        }

        public override async Task<DiscordBotMessage> ExecuteWithJoinedGuildAsync(SocketInteraction slashCommand, Deferer deferer, DiscordBotJoinedGuildModel joinedGuild, SocketTextChannel textChannel)
        {
            if (!File.ContentType.StartsWith("application/xml"))
            {
                return new DiscordBotMessage("File is not an XML file.", ephemeral: true);
            }

            using var stream = await _http.GetStreamAsync(File.Url);

            var xml = await XDocument.LoadAsync(stream, LoadOptions.None, default);

            if (xml.Root is null)
            {
                return new DiscordBotMessage("XML has an error.", ephemeral: true);
            }

            if (slashCommand.User is SocketGuildUser guildUser)
            {
                if (!guildUser.GuildPermissions.Administrator)
                {
                    return new DiscordBotMessage("You don't have permissions to import a memefile.", ephemeral: true);
                }
            }

            await deferer.DeferAsync(ephemeral: true);

            var existingMemes = await _discordBotUnitOfWork.Memes.GetFromGuildAsync(joinedGuild);
            var existingMemesHashSet = existingMemes.Select(x => x.Content).ToHashSet();

            var memes = xml.Root.Descendants().Select(x =>
            {
                var authorSnowflake = default(ulong?);
                var authorSnowflakeStr = x.Attribute("author")?.Value;

                if (authorSnowflakeStr is not null)
                {
                    authorSnowflake = ulong.Parse(authorSnowflakeStr);
                }

                var date = default(DateTime?);
                var dateStr = x.Attribute("date")?.Value;

                if (dateStr is not null)
                {
                    var dateLong = long.Parse(dateStr);
                    date = DateTime.FromBinary(dateLong);
                }

                return new MemeModel
                {
                    Guid = Guid.NewGuid(),
                    JoinedGuild = joinedGuild,
                    Content = x.Value,
                    AuthorSnowflake = authorSnowflake,
                    AddedOn = date,
                    Attachment = x.Attribute("attach")?.Value
                };
            }).Where(x => !string.IsNullOrEmpty(x.Content) && !existingMemesHashSet.Contains(x.Content));

            await _discordBotUnitOfWork.Memes.AddRangeAsync(memes);

            await _discordBotUnitOfWork.SaveAsync();

            return new DiscordBotMessage("XML imported.", ephemeral: true);
        }
    }
}
