using CryptoInvestmentSimulator.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Xunit;

namespace UnitTests.ControllerTests
{
	public class MarketControllerTests
	{
		/// <summary>
		/// Tests if calling Index view as an authorized user returns Index page.
		/// </summary>
		[Fact]
		public void Index_AuthorizedUser_ReturnsIndexView()
		{
			// Arrange
			var marketController = new MarketController();			

			var claims = new List<Claim> { new Claim(ClaimTypes.Email, "test-mail") };
			var identity = new ClaimsIdentity(claims, "TestAuth");
			var claimsPrincipal = new ClaimsPrincipal(identity);

			var httpContext = new DefaultHttpContext();
			httpContext.User = claimsPrincipal;
			marketController.ControllerContext = new ControllerContext { HttpContext = httpContext };

			// Act
			var result = marketController.Index() as ViewResult;

			// Assert
			Assert.Equal("200", marketController.HttpContext.Response.StatusCode.ToString());
			Assert.Equal("Index", result.ViewName);
		}

		/// <summary>
		/// Tests if calling Bitcoin view as an authorized user returns Bitcoin page.
		/// </summary>
		[Fact]
		public void Bitcoin_AuthorizedUser_ReturnsBitcoinView()
		{
			// Arrange
			var marketController = new MarketController();

			var claims = new List<Claim> { new Claim(ClaimTypes.Email, "test-mail") };
			var identity = new ClaimsIdentity(claims, "TestAuth");
			var claimsPrincipal = new ClaimsPrincipal(identity);

			var httpContext = new DefaultHttpContext();
			httpContext.User = claimsPrincipal;
			marketController.ControllerContext = new ControllerContext { HttpContext = httpContext };

			// Act
			var result = marketController.Bitcoin() as ViewResult;

			// Assert
			Assert.Equal("200", marketController.HttpContext.Response.StatusCode.ToString());
			Assert.Equal("Bitcoin", result.ViewName);
		}

		/// <summary>
		/// Tests if calling Etherium view as an authorized user returns Etherium page.
		/// </summary>
		[Fact]
		public void Etherium_AuthorizedUser_ReturnsEtheriumView()
		{
			// Arrange
			var marketController = new MarketController();

			var claims = new List<Claim> { new Claim(ClaimTypes.Email, "test-mail") };
			var identity = new ClaimsIdentity(claims, "TestAuth");
			var claimsPrincipal = new ClaimsPrincipal(identity);

			var httpContext = new DefaultHttpContext();
			httpContext.User = claimsPrincipal;
			marketController.ControllerContext = new ControllerContext { HttpContext = httpContext };

			// Act
			var result = marketController.Etherium() as ViewResult;

			// Assert
			Assert.Equal("200", marketController.HttpContext.Response.StatusCode.ToString());
			Assert.Equal("Etherium", result.ViewName);
		}

		/// <summary>
		/// Tests if calling Cardano view as an authorized user returns Cardano page.
		/// </summary>
		[Fact]
		public void Cardano_AuthorizedUser_ReturnsCardanoView()
		{
			// Arrange
			var marketController = new MarketController();

			var claims = new List<Claim> { new Claim(ClaimTypes.Email, "test-mail") };
			var identity = new ClaimsIdentity(claims, "TestAuth");
			var claimsPrincipal = new ClaimsPrincipal(identity);

			var httpContext = new DefaultHttpContext();
			httpContext.User = claimsPrincipal;
			marketController.ControllerContext = new ControllerContext { HttpContext = httpContext };

			// Act
			var result = marketController.Cardano() as ViewResult;

			// Assert
			Assert.Equal("200", marketController.HttpContext.Response.StatusCode.ToString());
			Assert.Equal("Cardano", result.ViewName);
		}

		/// <summary>
		/// Tests if calling Cosmos view as an authorized user returns Cosmos page.
		/// </summary>
		[Fact]
		public void Cosmos_AuthorizedUser_ReturnsCosmosView()
		{
			// Arrange
			var marketController = new MarketController();

			var claims = new List<Claim> { new Claim(ClaimTypes.Email, "test-mail") };
			var identity = new ClaimsIdentity(claims, "TestAuth");
			var claimsPrincipal = new ClaimsPrincipal(identity);

			var httpContext = new DefaultHttpContext();
			httpContext.User = claimsPrincipal;
			marketController.ControllerContext = new ControllerContext { HttpContext = httpContext };

			// Act
			var result = marketController.Cosmos() as ViewResult;

			// Assert
			Assert.Equal("200", marketController.HttpContext.Response.StatusCode.ToString());
			Assert.Equal("Cosmos", result.ViewName);
		}

		/// <summary>
		/// Tests if calling Dogecoin view as an authorized user returns Dogecoin page.
		/// </summary>
		[Fact]
		public void Dogecoin_AuthorizedUser_ReturnsDogecoinView()
		{
			// Arrange
			var marketController = new MarketController();

			var claims = new List<Claim> { new Claim(ClaimTypes.Email, "test-mail") };
			var identity = new ClaimsIdentity(claims, "TestAuth");
			var claimsPrincipal = new ClaimsPrincipal(identity);

			var httpContext = new DefaultHttpContext();
			httpContext.User = claimsPrincipal;
			marketController.ControllerContext = new ControllerContext { HttpContext = httpContext };

			// Act
			var result = marketController.Dogecoin() as ViewResult;

			// Assert
			Assert.Equal("200", marketController.HttpContext.Response.StatusCode.ToString());
			Assert.Equal("Dogecoin", result.ViewName);
		}

		/// <summary>
		/// Tests if calling _Chart1h partial view for Bitcoin returns
		/// _Chart1h with two filled ViewBags.
		/// </summary>
		[Fact]
		public void BTC1hChart_AuthorizedUser_ReturnsChart1hPartialView()
		{
			// Arrange
			var marketController = new MarketController();

			var claims = new List<Claim> { new Claim(ClaimTypes.Email, "test-mail") };
			var identity = new ClaimsIdentity(claims, "TestAuth");
			var claimsPrincipal = new ClaimsPrincipal(identity);

			var httpContext = new DefaultHttpContext();
			httpContext.User = claimsPrincipal;
			marketController.ControllerContext = new ControllerContext { HttpContext = httpContext };

			// Act
			var result = marketController.BTC1hChart() as PartialViewResult;

			// Assert
			Assert.Equal("200", marketController.HttpContext.Response.StatusCode.ToString());
			Assert.Equal("_Chart1h", result.ViewName);
			Assert.Equal(2, result.ViewData.Count);

			// DEVELOPER COMMENT => ViewBags were checked with debug and contain correct data.
			// Too much use of recursion would be required to test ViewBags automatically.
		}

		/// <summary>
		/// Tests if calling _Chart4h partial view for Bitcoin returns
		/// _Chart4h with two filled ViewBags.
		/// </summary>
		[Fact]
		public void BTC4hChart_AuthorizedUser_ReturnsChart4hPartialView()
		{
			// Arrange
			var marketController = new MarketController();

			var claims = new List<Claim> { new Claim(ClaimTypes.Email, "test-mail") };
			var identity = new ClaimsIdentity(claims, "TestAuth");
			var claimsPrincipal = new ClaimsPrincipal(identity);

			var httpContext = new DefaultHttpContext();
			httpContext.User = claimsPrincipal;
			marketController.ControllerContext = new ControllerContext { HttpContext = httpContext };

			// Act
			var result = marketController.BTC4hChart() as PartialViewResult;

			// Assert
			Assert.Equal("200", marketController.HttpContext.Response.StatusCode.ToString());
			Assert.Equal("_Chart4h", result.ViewName);
			Assert.Equal(2, result.ViewData.Count);

			// DEVELOPER COMMENT => ViewBags were checked with debug and contain correct data.
			// Too much use of recursion would be required to test ViewBags automatically.
		}

		/// <summary>
		/// Tests if calling _Chart8h partial view for Bitcoin returns
		/// _Chart8h with two filled ViewBags.
		/// </summary>
		[Fact]
		public void BTC8hChart_AuthorizedUser_ReturnsChart8hPartialView()
		{
			// Arrange
			var marketController = new MarketController();

			var claims = new List<Claim> { new Claim(ClaimTypes.Email, "test-mail") };
			var identity = new ClaimsIdentity(claims, "TestAuth");
			var claimsPrincipal = new ClaimsPrincipal(identity);

			var httpContext = new DefaultHttpContext();
			httpContext.User = claimsPrincipal;
			marketController.ControllerContext = new ControllerContext { HttpContext = httpContext };

			// Act
			var result = marketController.BTC8hChart() as PartialViewResult;

			// Assert
			Assert.Equal("200", marketController.HttpContext.Response.StatusCode.ToString());
			Assert.Equal("_Chart8h", result.ViewName);
			Assert.Equal(2, result.ViewData.Count);

			// DEVELOPER COMMENT => ViewBags were checked with debug and contain correct data.
			// Too much use of recursion would be required to test ViewBags automatically.
		}

		/// <summary>
		/// Tests if calling _Chart24h partial view for Bitcoin returns
		/// _Chart24h with two filled ViewBags.
		/// </summary>
		[Fact]
		public void BTC24hChart_AuthorizedUser_ReturnsChart24hPartialView()
		{
			// Arrange
			var marketController = new MarketController();

			var claims = new List<Claim> { new Claim(ClaimTypes.Email, "test-mail") };
			var identity = new ClaimsIdentity(claims, "TestAuth");
			var claimsPrincipal = new ClaimsPrincipal(identity);

			var httpContext = new DefaultHttpContext();
			httpContext.User = claimsPrincipal;
			marketController.ControllerContext = new ControllerContext { HttpContext = httpContext };

			// Act
			var result = marketController.BTC24hChart() as PartialViewResult;

			// Assert
			Assert.Equal("200", marketController.HttpContext.Response.StatusCode.ToString());
			Assert.Equal("_Chart24h", result.ViewName);
			Assert.Equal(2, result.ViewData.Count);

			// DEVELOPER COMMENT => ViewBags were checked with debug and contain correct data.
			// Too much use of recursion would be required to test ViewBags automatically.
		}

		/// <summary>
		/// Tests if GetNewMarketData receives successful API responses and
		/// creates a list of filled market data models for all supported cryptocurrencies.
		/// </summary>
		[Fact]
		public void GetNewMarketData_SuccessfulRequests_ValidListOfFilledModels()
		{
			// Arrange
			var marketController = new MarketController();

			// Act
			var result = marketController.GetNewMarketData();

			// Assert
			Assert.Equal(5, result.Count);
			Assert.NotNull(result[0]);
			Assert.NotNull(result[1]);
			Assert.NotNull(result[2]);
			Assert.NotNull(result[3]);
			Assert.NotNull(result[4]);
		}

		/// <summary>
		/// Tests if Position Table partial view is rendered when user opens Bitcoin page.
		/// DISCLAMER: tests for other crypto Position Tables would be identical, so skipping those tests.
		/// </summary>
		[Fact]
		public void PositionsTableBTC_AuthorizedUserOpensBitcoinPage_PositionTableDisplayed()
		{
			// Arrange
			var marketController = new MarketController();

			var claims = new List<Claim> { new Claim(ClaimTypes.Email, "test-mail") };
			var identity = new ClaimsIdentity(claims, "TestAuth");
			var claimsPrincipal = new ClaimsPrincipal(identity);

			var httpContext = new DefaultHttpContext();
			httpContext.User = claimsPrincipal;
			marketController.ControllerContext = new ControllerContext { HttpContext = httpContext };

			// Act
			var result = marketController.PositionsTableBTC() as PartialViewResult;

			// Assert
			Assert.Equal("200", marketController.HttpContext.Response.StatusCode.ToString());
			Assert.Equal("_PositionsTable", result.ViewName);
			Assert.Equal(5, result.ViewData.Count);

			// DEVELOPER COMMENT => ViewBags were checked with debug and contain correct data.
			// Too much use of recursion would be required to test ViewBags automatically.
		}

		/// <summary>
		/// Tests if position opening process is stopped and error view bag returned if
		/// user tries to make a purchase bigger than his fiat wallet balance.
		/// DISCLAMER: tests for other crypto Position Openings would be identical, so skipping those tests.
		/// </summary>
		[Fact]
		public void OpenBitcoinPosition_UserTriesToBuyTooMuch_ViewBagWithErrorReturned()
		{
			// Arrange
			var marketController = new MarketController();

			var claims = new List<Claim> { new Claim(ClaimTypes.Email, "test-mail") };
			var identity = new ClaimsIdentity(claims, "TestAuth");
			var claimsPrincipal = new ClaimsPrincipal(identity);

			var httpContext = new DefaultHttpContext();
			httpContext.User = claimsPrincipal;
			marketController.ControllerContext = new ControllerContext { HttpContext = httpContext };

			// Act
			var result = marketController.OpenBitcoinPosition("500", "150", "2x", "50") as ViewResult;

			// Assert
			Assert.Equal("200", marketController.HttpContext.Response.StatusCode.ToString());
			Assert.Equal("Bitcoin", result.ViewName);
			Assert.Single(result.ViewData);
		}
	}  
}
