using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpAn.App.MorphTrade.Abstractions.Finance;
using OpAn.App.MorphTrade.Console.Extensions;
using OpAn.App.MorphTrade.Domain.Finance.Bank;
using OpAn.App.MorphTrade.Domain.Finance.Trader;
using OpAn.App.MorphTrade.Domain.Flows;
using OpAn.App.MorphTrade.Domain.Ingestion.Extensions;
using OpAn.App.MorphTrade.Infrastructure.Persistence.Extension;

namespace OpAn.App.MorphTrade.Console;

/// <summary>
/// Public Start-point for the program
/// </summary>
[PublicAPI]
internal class Program
{
    /// <summary>
    /// Main method to call the MorphTrade Console program
    /// </summary>
    /// <param name="args">arguments pass through</param>
    private static void Main(string[] args)
    {
        HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

        // Add Configurations
        builder.AddConfigurations();
        // Add Logging
        builder.Services.AddLogging();

        // Exit if no DataVendor is configured
        if (builder.Configuration.GetValue<string>("DataVendor") is null)
        {
	        System.Console.WriteLine("DataVendor not configured");
	        return;
        }

        // Add the Section for fyers data vendor
        if (builder.Configuration.GetValue<string>("DataVendor")! == "Fyers")
        {
	        // Checks if an auth cycle is ran through.
	        var fyersBuilder = builder.AddFyersDataVendor();
	        if (fyersBuilder is null) return;
        }

        // Add the section for the Alpaca data vendor
        if (builder.Configuration.GetValue<string>("DataVendor")! == "Alpaca")
        {
	        builder.AddAlpacaExtensions();
        }

        // TODO: Add a working trader
        builder.Services.AddScoped<ITrader, NullTrader>();

	    // Add Persistent database
	    builder.Services
		    .AddPersistenceDb(builder.Configuration)
		    .AddBankingDomain()
		    .AddFlowsDomain(builder.Configuration);	// Banking domain needs specific dependencies for persistence.

        // Run the hosted application
        var app = builder.Build();

        // Perform DB migrations
        using (var scope = app.Services.CreateScope())
        {
	        var bankingDbContext = scope.ServiceProvider.GetRequiredService<BankDbContext>();
	        bankingDbContext.Database.Migrate();

	        var flowsDbContext = scope.ServiceProvider.GetRequiredService<FlowsDbContext>();
	        flowsDbContext.Database.Migrate();
        }

	    app.Run();
    }
}
