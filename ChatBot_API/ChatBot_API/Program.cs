using ChatBot_API.Components;
using ChatBot_API.Data;
using ChatBot_API.Services;
using ChatBot_Shared.Clients;
using ChatBot_Shared.Search;
using ChatBot_Shared.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.Google;
using Neo4j.Driver;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.Configure<AppOptions>(builder.Configuration.GetSection(AppOptions.SectionName));

builder.Services.AddHttpClient<OllamaEmbeddingClient>();
builder.Services.AddHttpClient<PineconeVectorStore>();
var appOptions = builder.Configuration.GetSection(AppOptions.SectionName).Get<AppOptions>() ?? new AppOptions();
builder.Services.AddKernel().AddGoogleAIGeminiChatCompletion(appOptions.GeminiModelId, appOptions.GeminiApiKey, GoogleAIVersion.V1_Beta);

builder.Services.AddSingleton<PineconeTextSearch>();
builder.Services.AddScoped<FootballChatAgentFactory>();
builder.Services.AddScoped<ChatHistoryService>();

builder.Services.AddSingleton<IDriver>(p =>
{
    var options = p.GetRequiredService<IOptions<AppOptions>>().Value;
    return GraphDatabase.Driver(options.Neo4jUri, AuthTokens.Basic(options.Neo4jUsername, options.Neo4jPassword));
});

builder.Services.AddSingleton<GraphQueryPlugin>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseAuthorization();

app.UseAntiforgery();

app.MapControllers();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
