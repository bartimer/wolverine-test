using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Wolverine.EntityFrameworkCore;
using Wolverine.SqlServer;

namespace Wolverine.Test.Domain;

public static class WolverineExtensions
{
    public static IHostBuilder UseWolverine(this IHostBuilder builder, string serviceName, bool listen,
        string connnectionStringKey = "Test")
    {
        builder.UseWolverine((context, opts) =>
        {
            opts.ServiceName = serviceName;
            var connectionString = context.Configuration.GetConnectionString(connnectionStringKey);
            opts.UseSqlServerPersistenceAndTransport(connectionString, "background");

            opts.OptimizeArtifactWorkflow();

            opts.ApplicationAssembly = typeof(TheHandler).Assembly;

            opts.UseEntityFrameworkCoreTransactions();

            opts.Policies.LogMessageStarting(LogLevel.Debug);

            opts.Durability.Mode = DurabilityMode.Solo;
            
            if (!listen)
            {
                opts.Discovery.DisableConventionalDiscovery();
                opts.Policies.DisableConventionalLocalRouting();

                opts.PublishAllMessages().ToSqlServerQueue("inbound").TelemetryEnabled(true);
            }
            else
            {


                opts.PublishAllMessages().ToSqlServerQueue("inbound");
                opts.Policies.UseDurableLocalQueues();
                opts.Discovery
                    .DisableConventionalDiscovery()
                    .IncludeType<TheHandler>();

                opts.ListenToSqlServerQueue("inbound")
                    //.MaximumParallelMessages(5)
                    .MaximumMessagesToReceive(50);
            }
        });
        return builder;
    }
}