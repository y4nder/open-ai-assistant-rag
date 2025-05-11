using Carter;
using RagOpenAiTestApp;
using RagOpenAiTestApp.ChatAgents;
using RagOpenAiTestApp.Database;
using RagOpenAiTestApp.FunctionTools;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<Database>();

#pragma warning disable OPENAI001

builder.Services.AddOpenAi(builder.Configuration);
builder.Services.AddChatAgent();

#pragma warning restore OPENAI001

builder.Services.AddAgentTools();

builder.Services.AddCarter();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapCarter();
app.UseHttpsRedirection();
app.Run();
