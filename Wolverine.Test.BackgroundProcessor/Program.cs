using Microsoft.Extensions.Hosting;
using Oakton;
using Wolverine.Test.Domain;

var builder = Host.CreateDefaultBuilder(args);

builder.UseWolverine("BackgroundProcessor", true);
return await builder.RunOaktonCommands(args);;