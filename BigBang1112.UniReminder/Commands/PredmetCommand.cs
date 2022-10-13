using BigBang1112.DiscordBot.Attributes;
using BigBang1112.DiscordBot.Models;
using BigBang1112.DiscordBot;
using Discord.WebSocket;
using Discord;
using System.Net;

namespace BigBang1112.UniReminder.Commands;

[DiscordBotCommand("předmět")]
public partial class PredmetCommand : DiscordBotCommand
{
    public PredmetCommand(DiscordBotService discordBotService) : base(discordBotService)
    {

    }

    public override Task<DiscordBotMessage> ExecuteAsync(SocketInteraction slashCommand)
    {
        return base.ExecuteAsync(slashCommand);
    }

    [DiscordBotSubCommand("úspěšnost")]
    public partial class Uspesnost : DiscordBotCommand
    {
        [DiscordBotCommandOption("kód", ApplicationCommandOptionType.String, "Kód předmětu.")]
        public string? Kod { get; set; }

        public Uspesnost(DiscordBotService discordBotService) : base(discordBotService)
        {

        }

        public override Task<DiscordBotMessage> ExecuteAsync(SocketInteraction slashCommand)
        {
            if (Kod is null)
            {
                return Task.FromResult(new DiscordBotMessage("Chybí kód předmětu."));
            }

            var split = Kod.Split('/');

            if (split.Length != 2)
            {
                return Task.FromResult(new DiscordBotMessage("Kód je nesprávný."));
            }
            
            var grafUrl = $"https://wstag.jcu.cz/StagPortletsJSR168/StagCommonChartServlet?chSHPPRimg=A&chSHPPRPracZkr={split[0]}&chSHPPRZkrPredm={split[1]}";

            return Task.FromResult(new DiscordBotMessage(new EmbedBuilder()
                .WithDescription("Triviální předmět")
                .WithImageUrl(grafUrl)
                .Build()));
        }
    }
}


