using BigBang1112;
using BigBang1112.BongoBot3;
using BigBang1112.DiscordBot.Data;
using BigBang1112.Extensions;

var assembly = typeof(Program).Assembly;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseEssentials();

var config = builder.Configuration;

var options = new EssentialsOptions
{
    Title = "BigBang1112",
    Assembly = assembly,
    Config = config
};

// Add services to the container.
builder.Services.AddEssentials(options);

builder.Services.AddDbContext2<DiscordBotContext>(options.Config, "DiscordBotDb");
builder.Services.AddScoped<IDiscordBotUnitOfWork, DiscordBotUnitOfWork>();

builder.Services.AddSingleton<BongoBot3DiscordBotService>();
builder.Services.AddHostedService(x => x.GetRequiredService<BongoBot3DiscordBotService>());

var app = builder.Build();

app.UseEssentials(options);

app.Run();
