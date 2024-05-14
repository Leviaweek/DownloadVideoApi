using System.Threading.Channels;
using Microsoft.EntityFrameworkCore;
using VideoDownloaderApi.Abstractions;
using VideoDownloaderApi.Abstractions.Command;
using VideoDownloaderApi.Abstractions.Query;
using VideoDownloaderApi.Database;
using VideoDownloaderApi.Handlers.CommandHandlers;
using VideoDownloaderApi.Handlers.QueryHandlers;
using VideoDownloaderApi.Mediators;
using VideoDownloaderApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        corsPolicyBuilder =>
        {
            corsPolicyBuilder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
builder.Services.AddHttpClient();
builder.Services.AddDbContextFactory<MediaDbContext>(optionsBuilder =>
    optionsBuilder.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddOptions<VideoDownloaderOptions>()
    .Bind(builder.Configuration.GetSection(VideoDownloaderOptions.OptionName)).ValidateOnStart();
builder.Services.AddSingleton<IQueryMediator<IQuery<IResponse<IResult, IError>>>, QueryMediator>();
builder.Services.AddSingleton<ICommandMediator<ICommand<IResponse<IResult, IError>>>, CommandMediator>();
builder.Services.AddTransient<YoutubeVideoDownloader>();
builder.Services.AddTransient<IFetchFormatsQueryHandler, FetchYoutubeFormatsQueryHandler>();
builder.Services.AddTransient<IDownloadMediaCommandHandler, DownloadYoutubeMediaCommandHandler>();
builder.Services.AddHostedService<DownloadMediaQueueService>();
builder.Services.AddHostedService<DownloadMediaCleaner>();
builder.Services.AddSingleton<DownloadMediaQueue>();
builder.Services.AddSingleton(
    Channel.CreateBounded<DownloadTask>(new BoundedChannelOptions(10) {SingleWriter = true, SingleReader = true}));
builder.Services.AddSingleton(svc => svc.GetRequiredService<Channel<DownloadTask>>().Reader);
builder.Services.AddSingleton(svc => svc.GetRequiredService<Channel<DownloadTask>>().Writer);


var app = builder.Build();
app.UseRouting();
app.UseCors();
app.MapControllers();
await app.RunAsync();

