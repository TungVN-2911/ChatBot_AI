using ChatBot_API.Data;
using ChatBot_API.Services;
using ChatBot_Shared.Clients;
using ChatBot_Shared.LLM;
using ChatBot_Shared.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.Google;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.Configure<AppOptions>(builder.Configuration.GetSection(AppOptions.SectionName));

builder.Services.AddHttpClient<OllamaEmbeddingClient>();
builder.Services.AddHttpClient<PineconeVectorStore>();
var appOptions = builder.Configuration.GetSection(AppOptions.SectionName).Get<AppOptions>() ?? new AppOptions();
builder.Services.AddKernel().AddGoogleAIGeminiChatCompletion(appOptions.GeminiModelId, appOptions.GeminiApiKey, GoogleAIVersion.V1_Beta);

builder.Services.AddSingleton<PineconeTextSearch>();
builder.Services.AddScoped<FootballChatAgentFactory>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
