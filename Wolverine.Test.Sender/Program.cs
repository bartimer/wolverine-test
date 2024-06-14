using Oakton;
using Wolverine;
using Wolverine.Test.Domain;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Host.UseWolverine("Sender", false);
builder.Host.ApplyOaktonExtensions();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapPost("/test/{id}", async (int id, IMessageBus bus) =>
    {
        await bus.SendAsync(new MyCommand() { Id = id });
    })
    .WithName("Test")
    .WithOpenApi();

await app.RunOaktonCommands(args);
app.Run();

