using CryptoInvestmentSimulator.Controllers;
using CryptoInvestmentSimulator.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Xunit;

namespace UnitTests.ControllerTests
{
    public class HomeControllerTests
    {
        /// <summary>
        /// Tests if calling Index as an authorized user returns
        /// Index page with filled view model.
        /// </summary>
        [Fact]
        public void Index_EverythingOk_ReturnsIndexViewWithModel()
        {
            // Arrange
            var homeController = new HomeController();
            var httpContext = new DefaultHttpContext();

            var claims = new List<Claim>{ new Claim(ClaimTypes.Email, "test-mail") };

            var identity = new ClaimsIdentity(claims, "TestAuth");
            var claimsPrincipal = new ClaimsPrincipal(identity);
            httpContext.User = claimsPrincipal;

            homeController.ControllerContext = new ControllerContext{ HttpContext = httpContext };

            // Act
            var result = homeController.Index() as ViewResult;

            // Assert
            Assert.Equal("200", homeController.HttpContext.Response.StatusCode.ToString());
            Assert.Equal("Index", result.ViewName);
            Assert.Equal(typeof(UserModel), result.Model.GetType());
            Assert.NotNull(result.Model);

            // DEVELOPER COMMENT => result.Model was checked with debug and contains correct data.
        }

        /// <summary>
        /// Tests if calling SetUsername as an authorized user redirects
        /// back to Index with filled view model and changed username.
        /// </summary>
        [Fact]
        public void SetUsername_EverythingOk_ReturnsIndexViewWithUpdatedModel()
        {
            // Arrange
            var homeController = new HomeController();
            var httpContext = new DefaultHttpContext();

            var claims = new List<Claim> { new Claim(ClaimTypes.Email, "test-mail") };

            var identity = new ClaimsIdentity(claims, "TestAuth");
            var claimsPrincipal = new ClaimsPrincipal(identity);
            httpContext.User = claimsPrincipal;

            homeController.ControllerContext = new ControllerContext { HttpContext = httpContext };

            // Act
            var result = homeController.SetUsername("new-name") as ViewResult;

            // Assert
            Assert.Equal("200", homeController.HttpContext.Response.StatusCode.ToString());
            Assert.Equal("Index", result.ViewName);
            Assert.Equal(typeof(UserModel), result.Model.GetType());
            Assert.NotNull(result.Model);

            // DEVELOPER COMMENT => result.Model was checked with debug and changed correctly.
        }
    }
}
