using Microsoft.EntityFrameworkCore;
using Npgsql;
using VideoDownloaderApi.Abstractions;
using VideoDownloaderApi.Abstractions.Command;
using VideoDownloaderApi.Abstractions.Query;
using VideoDownloaderApi.Database;
using VideoDownloaderApi.Database.Models;
using VideoDownloaderApi.Enums;
using VideoDownloaderApi.Handlers.CommandHandlers;
using VideoDownloaderApi.Handlers.QueryHandlers;
using VideoDownloaderApi.Mediators;
using VideoDownloaderApi.Models.Commands;
using VideoDownloaderApi.Models.Queries;
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

var app = builder.Build();
app.UseRouting();
app.UseCors();
app.MapControllers();
await app.RunAsync();

