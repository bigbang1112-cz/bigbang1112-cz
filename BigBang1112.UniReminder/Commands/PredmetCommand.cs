using BigBang1112.DiscordBot.Attributes;
using BigBang1112.DiscordBot.Models;
using BigBang1112.DiscordBot;
using Discord.WebSocket;
using Discord;
using System.Net;
using BigBang1112.DiscordBot.Repos;
using BigBang1112.UniReminder.Models;
using System.Linq;

namespace BigBang1112.UniReminder.Commands;

[DiscordBotCommand("předmět", "Odpovím vám na to nejpodstatnější z daného předmětu.")]
public partial class PredmetCommand : DiscordBotCommand
{
    private readonly IPredmetRepo _predmetRepo;

    public PredmetCommand(DiscordBotService discordBotService, IPredmetRepo predmetRepo) : base(discordBotService)
    {
        _predmetRepo = predmetRepo;
    }

    [DiscordBotCommandOption("název", ApplicationCommandOptionType.String, "Název předmětu.")]
    public string? Nazev { get; set; }

    public async Task<IEnumerable<string>> AutocompleteNazevAsync(string value)
    {
        return await _predmetRepo.GetAllNazvyLikeAsync(value);
    }

    [DiscordBotCommandOption("kód", ApplicationCommandOptionType.String, "Kód předmětu.")]
    public string? Kod { get; set; }

    public async Task<IEnumerable<string>> AutocompleteKodAsync(string value)
    {
        var split = value.Split('/');
        
        if (split.Length <= 0 || split.Length > 2)
        {
            return Enumerable.Empty<string>();
        }

        var pracoviste = split[0];

        var kod = split.Length == 2 ? split[1] : null;

        return (await _predmetRepo.GetAllLikePracovisteAndKodAsync(pracoviste, kod)).Select(x => $"{x.Pracoviste}/{x.Predmet}");
    }

    public override async Task<DiscordBotMessage> ExecuteAsync(SocketInteraction slashCommand)
    {
        if (Nazev is null && Kod is null)
        {
            return new DiscordBotMessage("Prosím, zadejte buď název nebo kód předmětu. Teď Vám nemám co říct.", ephemeral: true);
        }

        if (Kod is not null)
        {
            var split = Kod.Split('/');

            if (split.Length != 2)
            {
                return new DiscordBotMessage("Kód je nesprávný.");
            }

            var predmet = await _predmetRepo.GetByPracovisteAndPredmetAsync(split[0], split[1]);
                
            if (predmet is null)
            {
                return new DiscordBotMessage("Kód neexistuje.");
            }

            return GetFinalMessage(predmet);
        }

        if (Nazev is null)
        {
            throw new Exception();
        }

        var predmety = await _predmetRepo.GetAllByNameAsync(Nazev);

        if (!predmety.Any())
        {
            return new DiscordBotMessage($"Předmět '{Nazev}' neexistuje.");
        }
        
        return GetFinalMessage(predmety.First(), predmety.Skip(1).ToArray());
    }

    private DiscordBotMessage GetFinalMessage(PredmetModel predmet, params PredmetModel[] dalsiPredmety)
    {
        var grafUrl = $"https://wstag.jcu.cz/StagPortletsJSR168/StagCommonChartServlet?chSHPPRimg=A&chSHPPRPracZkr={predmet.Pracoviste}&chSHPPRZkrPredm={predmet.Predmet}";

        var zsLs = "";

        if (predmet.ZS)
        {
            zsLs += "(ZS";

            if (predmet.LS)
            {
                zsLs += " i LS)";
            }
            else
            {
                zsLs += ")";
            }
        }
        else if (predmet.LS)
        {
            zsLs = "(LS)";
        }

        var messageComponent = default(MessageComponent);

        if (dalsiPredmety.Length > 0)
        {
            var dalsiPredmet = dalsiPredmety.First();
            
            messageComponent = new ComponentBuilder()
                .WithButton($"Přepnout na {dalsiPredmet.Pracoviste}/{dalsiPredmet.Predmet}", $"předmět-{dalsiPredmet.Pracoviste}/{dalsiPredmet.Predmet}", ButtonStyle.Secondary)
                .Build();
        }

        return new DiscordBotMessage(new EmbedBuilder()
            .WithTitle($"{predmet.Pracoviste}/{predmet.Predmet} - {predmet.Nazev} {zsLs}")
            .WithColor(Color.Purple)
            .WithDescription("Triviální předmět, při kterém budete psát zápočty až do aleluja.")
            .AddField("Typ zkoušky", predmet.TypZkousky, inline: true)
            .AddField("Kredity", predmet.Kredity, inline: true)
            .WithImageUrl(grafUrl)
            .Build(), component: messageComponent);
    }

    public override async Task<DiscordBotMessage?> ExecuteButtonAsync(SocketMessageComponent messageComponent, Deferer deferer)
    {
        var kod = messageComponent.Data.CustomId.Substring("předmět".Length + 1);
        var split = kod.Split('/');

        var predmet = await _predmetRepo.GetByPracovisteAndPredmetAsync(split[0], split[1]);

        if (predmet is null)
        {
            return new DiscordBotMessage("Předmět neexistuje.", ephemeral: true);
        }

        var predmety = await _predmetRepo.GetAllByNameAsync(predmet.Nazev);

        var predmetyAfter = predmety.TakeWhile(x => x.Guid != predmet.Guid);
        var predmetyBefore = predmety.Skip(predmetyAfter.Count() + 1);

        return GetFinalMessage(predmet, predmetyBefore.Concat(predmetyAfter).ToArray());
    }
}


