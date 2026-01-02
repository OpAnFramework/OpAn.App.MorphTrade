using System.Net;
using System.Net.Sockets;

namespace OpAn.App.MorphTrade.Domain.Flows.Tests.Fixtures;

/// <summary>
/// A static class of Network helper methods.
/// </summary>
public static class StaticNetworkHelper
{
	/// <summary>
	/// Provides a free listening port.
	/// </summary>
	/// <returns></returns>
	public static int GetFreePort()
	{
		var listener = new TcpListener(IPAddress.Loopback, 0);
		listener.Start();
		int port = ((IPEndPoint)listener.LocalEndpoint).Port;
		listener.Stop();
		return port;
	}
}
