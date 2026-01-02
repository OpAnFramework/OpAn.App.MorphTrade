using System.Text.Json;
using Moq;
using OpAn.App.MorphTrade.Abstractions.IngestionDomain;
using OpAn.App.MorphTrade.Abstractions.Tools.JsonConverters;

namespace OpAn.App.MorphTrade.Domain.Ingestion.Tests;

/// <summary>
/// Tests for the contract consistencies.
/// </summary>
[TestClass]
public class IngestionDomainDataVendorContractTests
{
	private IList<OlhcvDatapoint> _mockDatapoints = new List<OlhcvDatapoint>();
	private readonly Mock<IDataVendor> _dataVendorMock = new ();

	/// <summary>
	/// Sets up the test environment.
	/// </summary>
	[TestInitialize]
	public void Setup()
	{
		var data = File.ReadAllText("test_data.json");

		var options = new JsonSerializerOptions
		{
			Converters = { new UnixSecondsToDateTimeConverter() }
		};
		_mockDatapoints = JsonSerializer.Deserialize<IList<OlhcvDatapoint>>(data, options)!;

		_dataVendorMock.Setup(dv => dv
				.GetOlhcvData(
					It.IsAny<string>(),
					It.IsAny<string>(),
					It.IsAny<DateTime>(),
					It.IsAny<TimeSpan>(),
					It.IsAny<TimeSpan>()))
			.Returns(Task.FromResult(_mockDatapoints)!);

		IList<OlhcDatapoint> mockOlhcDatapoints = _mockDatapoints
			.Select(x => new OlhcDatapoint
			{
				Timestamp = x.Timestamp,
				Open = x.Open,
				High = x.High,
				Low = x.Low,
				Close = x.Close
			}).ToList();
		_dataVendorMock.Setup(dv => dv
				.GetOlhcData(
					It.IsAny<string>(),
					It.IsAny<string>(),
					It.IsAny<DateTime>(),
					It.IsAny<TimeSpan>(),
					It.IsAny<TimeSpan>()))
			.Returns(Task.FromResult(mockOlhcDatapoints));
	}

	/// <summary>
	/// Checks for the contract consistency for the Olhcv data.
	/// </summary>
	[TestMethod]
	public void DataVendorContractConsistencyTest_Olhcv()
	{
		IDataVendor dataVendor = _dataVendorMock.Object;
		Assert.AreEqual(_mockDatapoints[0].Timestamp, dataVendor.GetOlhcvData(
			"x",
			"x",
			DateTime.Now,
			TimeSpan.MinValue,
			TimeSpan.MinValue
		).Result![0].Timestamp);
	}
}
