﻿using CryptoInvestmentSimulator.Controllers;
using CryptoInvestmentSimulator.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Xunit;

namespace UnitTests.ControllerTests
{
    public class PortfolioControllerTests
    {
        /// <summary>
        /// Tests if calling Index as an authorized user returns
        /// Index page with filled view model.
        /// </summary>
        [Fact]
        public void Index_EverythingOk_ReturnsIndexViewWithModel()
        {
            // Arrange
            var portfolioController = new PortfolioController();
            var httpContext = new DefaultHttpContext();

            var claims = new List<Claim> { new Claim(ClaimTypes.Email, "test-mail") };

            var identity = new ClaimsIdentity(claims, "TestAuth");
            var claimsPrincipal = new ClaimsPrincipal(identity);
            httpContext.User = claimsPrincipal;

            portfolioController.ControllerContext = new ControllerContext { HttpContext = httpContext };

            // Act
            var result = portfolioController.Index() as ViewResult;

            // Assert
            Assert.Equal("200", portfolioController.HttpContext.Response.StatusCode.ToString());
            Assert.Equal("Index", result.ViewName);
            Assert.Equal(typeof(UserModel), result.Model.GetType());
            Assert.NotNull(result.Model);

            // DEVELOPER COMMENT => result.Model was checked with debug and contains correct data.
        }

        /// <summary>
        /// Tests if calling UpdateDetails as an authorized user redirects
        /// back to Index with filled view model and changed details.
        /// </summary>
        [Fact]
        public void UpdateDetails_EverythingOk_ReturnsIndexViewWithUpdatedModel()
        {
            // Arrange
            var portfolioController = new PortfolioController();
            var httpContext = new DefaultHttpContext();

            var claims = new List<Claim> { new Claim(ClaimTypes.Email, "test-mail") };

            var identity = new ClaimsIdentity(claims, "TestAuth");
            var claimsPrincipal = new ClaimsPrincipal(identity);
            httpContext.User = claimsPrincipal;

            portfolioController.ControllerContext = new ControllerContext { HttpContext = httpContext };

            // Act
            var result = portfolioController.UpdateDetails("new-name", "new-avatar", "new-zone") as ViewResult;

            // Assert
            Assert.Equal("200", portfolioController.HttpContext.Response.StatusCode.ToString());
            Assert.Equal("Index", result.ViewName);
            Assert.Equal(typeof(UserModel), result.Model.GetType());
            Assert.NotNull(result.Model);

            // DEVELOPER COMMENT => result.Model was checked with debug and changed correctly.
        }
    }
}