using BigBang1112.Attributes;

namespace BigBang1112.BongoBot3;

[DiscordBot("18a800a1-5275-4efb-bd46-41ccfbee1f2e", name: "BongoBot3", version: "1.0.0")]
[SecretAppsettingsPath("DiscordBots:BongoBot3:Secret")]
public class BongoBot3DiscordBotService : DiscordBotService
{
    public BongoBot3DiscordBotService(IServiceProvider serviceProvider) : base(serviceProvider)
    {

    }
}
