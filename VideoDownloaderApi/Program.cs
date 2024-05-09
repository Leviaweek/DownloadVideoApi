using Microsoft.EntityFrameworkCore;
using VideoDownloaderApi.Abstractions;
using VideoDownloaderApi.Database;
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
builder.Services.AddDbContextFactory<YoutubeMediaDbContext>(optionsBuilder =>
    optionsBuilder.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddOptions<VideoDownloaderOptions>()
    .Bind(builder.Configuration.GetSection(VideoDownloaderOptions.OptionName)).ValidateOnStart();
builder.Services
    .AddSingleton<IQueryMediator<IQuery<IQueryResponse<IResult, IError>>, IResult, IError>, QueryMediator>();
builder.Services.AddTransient<IVideoDownloader, YoutubeVideoDownloader>();
builder.Services.AddTransient<IVideoService, YoutubeVideoService>();
builder.Services.AddTransient<IMediaRepository<YoutubeMediaRepository>, YoutubeMediaRepository>();
builder.Services.AddSingleton<IQueryHandler<IQuery<IQueryResponse<IResult, IError>>, IResult, IError>, FetchFormatsQueryHandler>();

var app = builder.Build();
app.UseRouting();
app.UseCors();
app.MapControllers();
await app.RunAsync();

