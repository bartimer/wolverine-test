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
        for (int i = 0; i < 5; i++)
        {
            await bus.SendAsync(new MyCommand() { Id = id + i});
        
        }
    })
    .WithName("Test")
    .WithOpenApi();

await app.RunOaktonCommands(args);
app.Run();

