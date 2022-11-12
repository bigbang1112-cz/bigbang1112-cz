using BigBang1112.DiscordBot;
using BigBang1112.DiscordBot.Attributes;
using BigBang1112.DiscordBot.Models;
using Discord.WebSocket;

namespace BigBang1112.UniReminder.Commands;

[DiscordBotCommand("dnes", "Jak to dneska bude.")]
public class DnesCommand : DiscordBotCommand
{
    public DnesCommand(DiscordBotService discordBotService) : base(discordBotService)
    {
    }
    
    [DiscordBotSubCommand("zpoždění", "Zeptejte se, jak dlouho na mě budete muset čekat.")]
    public partial class Zpozdeni : DiscordBotCommand
    {
        public Zpozdeni(DiscordBotService discordBotService) : base(discordBotService)
        {

        }

        public override Task<DiscordBotMessage> ExecuteAsync(SocketInteraction slashCommand)
        {
            var random = new Random(DateTime.Today.Ticks.GetHashCode());

            var zpozdeni = random.Next(0, 65);

            var response = zpozdeni switch
            {
                0 => "Dnes přijdu včas",
                > 1 and < 5 => $"Dnes přijdu pozdě o {zpozdeni} minuty",
                > 60 => "Dnes nedorazím",
                _ => $"Dnes přijdu pozdě o {zpozdeni} minut"
            };

            return Task.FromResult(new DiscordBotMessage(response));
        }
    }

    [DiscordBotSubCommand("tabule", "Zeptejte se, jestli dnes půjdete k tabuli.")]
    public partial class Tabule : DiscordBotCommand
    {
        public Tabule(DiscordBotService discordBotService) : base(discordBotService)
        {

        }

        public override Task<DiscordBotMessage> ExecuteAsync(SocketInteraction slashCommand)
        {
            if (DateTime.Today.DayOfWeek != DayOfWeek.Thursday)
            {
                return Task.FromResult(new DiscordBotMessage("Dnes pana doktora Chládka nemáte"));
            }
            
            if (DateTime.Today.Hour >= 11)
            {
                return Task.FromResult(new DiscordBotMessage("Dnes k tabuli již nepůjdete"));
            }

            var random = new Random(((ulong)DateTime.Today.Ticks + slashCommand.User.Id).GetHashCode());

            var pujdete = Convert.ToBoolean(random.Next(0, 2));
            var jakouMinutu = random.Next(0, 90);
            var kdy = DateTime.Today.AddHours(9).AddMinutes(30 + jakouMinutu);

            var response = pujdete switch
            {
                true => $"Dnes půjdete k tabuli, a to pravděpodobně v {kdy:HH:mm}",
                false => "Dnes k tabuli nepůjdete"
            };

            return Task.FromResult(new DiscordBotMessage(response));
        }
    }
}
