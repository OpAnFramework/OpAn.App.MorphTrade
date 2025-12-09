using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using OpAn.App.MorphTrade.Console.Extensions;
using OpAn.App.MorphTrade.Domain.Ingestion.Extensions;

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

        // Run the hosted application

        var app = builder.Build();

	    app.Run();
    }
}
