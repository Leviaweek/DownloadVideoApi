using VideoDownloaderApi.Abstractions;
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
builder.Services.AddOptions<VideoDownloaderOptions>()
    .Bind(builder.Configuration.GetSection(VideoDownloaderOptions.OptionName)).ValidateOnStart();
builder.Services
    .AddSingleton<IQueryMediator<IQuery<IQueryResponse<IResult, IError>>, IResult, IError>, QueryMediator>();
builder.Services.AddTransient<IVideoDownload, YoutubeVideoDownloader>();
builder.Services.AddSingleton<IQueryHandler<IQuery<IQueryResponse<IResult, IError>>, IResult, IError>, FetchFormatsQueryHandler>();

var app = builder.Build();
app.UseRouting();
app.UseCors();
app.MapControllers();
await app.RunAsync();

