using BigBang1112.Attributes;
using BigBang1112.DiscordBot;
using BigBang1112.DiscordBot.Attributes;

namespace BigBang1112.UniReminder;

[DiscordBot("1a548f40-c91a-4a5a-8f87-6672ff15d061", name: "UniReminder", "1.0.0")]
[SecretAppsettingsPath("DiscordBots:UniReminder:Secret")]
public class UniReminderDiscordBotService : DiscordBotService
{
    public UniReminderDiscordBotService(IServiceProvider serviceProvider) : base(serviceProvider)
    {

    }

    protected override async Task ReadyAsync()
    {
        await base.ReadyAsync();
        await OverwriteGuildApplicationCommandsAsync();
    }
}
