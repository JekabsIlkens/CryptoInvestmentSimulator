using CryptoInvestmentSimulator.Constants;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System.Diagnostics;

namespace FunctionalTests
{
    public class PageLoadPerformanceTests
    {
        public IWebDriver webDriver;
        public WebDriverWait webDriverWait;
        public Stopwatch stopWatch;

        [SetUp]
        public void Setup()
        {
            webDriver = new ChromeDriver();
            webDriverWait = new WebDriverWait(webDriver, TimeSpan.FromSeconds(20));
            stopWatch = new Stopwatch();
        }

        /// <summary>
        /// Tests portfolio page load performance. Must be equal or under 1 second.
        /// </summary>
        [Test]
        public void PortfolioPage_NavigationButtonClick_LoadsUnder1Second()
        {
            // Goes trough login procedure (requires test runner to manually fill captcha and click continue).
            webDriver.Navigate().GoToUrl(AuthenticationConstants.LocalURL);
            webDriver.FindElement(By.Id("login")).Click();
            webDriver.FindElement(By.Id("username")).SendKeys("CisSelenium@gmail.com");
            webDriver.FindElement(By.Id("password")).SendKeys("Testingpass123!");
            webDriverWait.Until(webDriver => webDriver.FindElement(By.Id("homeSubtitle")).Displayed);          

            // Measures time of target page load.
            stopWatch.Start();
            webDriver.FindElement(By.Id("portfolio")).Click();
            webDriverWait.Until(webDriver => webDriver.FindElement(By.Id("reset")).Displayed);
            stopWatch.Stop();

            // Collects elapsed time and asserts if it doesn't exceed 1 second limit.
            TimeSpan timeElapsed = stopWatch.Elapsed;
            Assert.GreaterOrEqual(1000, timeElapsed.Milliseconds);
        }

        /// <summary>
        /// Tests market page load performance. Must be equal or under 1 second.
        /// </summary>
        [Test]
        public void MarketPage_NavigationButtonClick_LoadsUnder1Second()
        {
            // Goes trough login procedure (requires test runner to manually fill captcha and click continue).
            webDriver.Navigate().GoToUrl(AuthenticationConstants.LocalURL);
            webDriver.FindElement(By.Id("login")).Click();
            webDriver.FindElement(By.Id("username")).SendKeys("CisSelenium@gmail.com");
            webDriver.FindElement(By.Id("password")).SendKeys("Testingpass123!");
            webDriverWait.Until(webDriver => webDriver.FindElement(By.Id("homeSubtitle")).Displayed);

            // Measures time of target page load.
            stopWatch.Start();
            webDriver.FindElement(By.Id("market")).Click();
            webDriverWait.Until(webDriver => webDriver.FindElement(By.Id("tableZone")).Displayed);
            stopWatch.Stop();

            // Collects elapsed time and asserts if it doesn't exceed 1 second limit.
            TimeSpan timeElapsed = stopWatch.Elapsed;
            Assert.GreaterOrEqual(1000, timeElapsed.Milliseconds);
        }

        /// <summary>
        /// Tests bitcoin chart page load performance. Must be equal or under 1 second.
        /// </summary>
        [Test]
        public void BitcoinPage_ChartButtonClick_LoadsUnder1Second()
        {
            // Goes trough login procedure (requires test runner to manually fill captcha and click continue).
            webDriver.Navigate().GoToUrl(AuthenticationConstants.LocalURL);
            webDriver.FindElement(By.Id("login")).Click();
            webDriver.FindElement(By.Id("username")).SendKeys("CisSelenium@gmail.com");
            webDriver.FindElement(By.Id("password")).SendKeys("Testingpass123!");
            webDriverWait.Until(webDriver => webDriver.FindElement(By.Id("homeSubtitle")).Displayed);
            webDriver.FindElement(By.Id("market")).Click();

            // Measures time of target page load.
            stopWatch.Start();
            webDriver.Navigate().GoToUrl(AuthenticationConstants.LocalURL + "/market/bitcoin");
            webDriverWait.Until(webDriver => webDriver.FindElement(By.Id("chart-container")).Displayed);
            stopWatch.Stop();

            // Collects elapsed time and asserts if it doesn't exceed 1 second limit.
            TimeSpan timeElapsed = stopWatch.Elapsed;
            Assert.GreaterOrEqual(1000, timeElapsed.Milliseconds);
        }

        /// <summary>
        /// Tests etherium chart page load performance. Must be equal or under 1 second.
        /// </summary>
        [Test]
        public void EtheriumPage_ChartButtonClick_LoadsUnder1Second()
        {
            // Goes trough login procedure (requires test runner to manually fill captcha and click continue).
            webDriver.Navigate().GoToUrl(AuthenticationConstants.LocalURL);
            webDriver.FindElement(By.Id("login")).Click();
            webDriver.FindElement(By.Id("username")).SendKeys("CisSelenium@gmail.com");
            webDriver.FindElement(By.Id("password")).SendKeys("Testingpass123!");
            webDriverWait.Until(webDriver => webDriver.FindElement(By.Id("homeSubtitle")).Displayed);
            webDriver.FindElement(By.Id("market")).Click();

            // Measures time of target page load.
            stopWatch.Start();
            webDriver.Navigate().GoToUrl(AuthenticationConstants.LocalURL + "/market/etherium");
            webDriverWait.Until(webDriver => webDriver.FindElement(By.Id("chart-container")).Displayed);
            stopWatch.Stop();

            // Collects elapsed time and asserts if it doesn't exceed 1 second limit.
            TimeSpan timeElapsed = stopWatch.Elapsed;
            Assert.GreaterOrEqual(1000, timeElapsed.Milliseconds);
        }

        /// <summary>
        /// Tests cardano chart page load performance. Must be equal or under 1 second.
        /// </summary>
        [Test]
        public void CardanoPage_ChartButtonClick_LoadsUnder1Second()
        {
            // Goes trough login procedure (requires test runner to manually fill captcha and click continue).
            webDriver.Navigate().GoToUrl(AuthenticationConstants.LocalURL);
            webDriver.FindElement(By.Id("login")).Click();
            webDriver.FindElement(By.Id("username")).SendKeys("CisSelenium@gmail.com");
            webDriver.FindElement(By.Id("password")).SendKeys("Testingpass123!");
            webDriverWait.Until(webDriver => webDriver.FindElement(By.Id("homeSubtitle")).Displayed);
            webDriver.FindElement(By.Id("market")).Click();

            // Measures time of target page load.
            stopWatch.Start();
            webDriver.Navigate().GoToUrl(AuthenticationConstants.LocalURL + "/market/cardano");
            webDriverWait.Until(webDriver => webDriver.FindElement(By.Id("chart-container")).Displayed);
            stopWatch.Stop();

            // Collects elapsed time and asserts if it doesn't exceed 1 second limit.
            TimeSpan timeElapsed = stopWatch.Elapsed;
            Assert.GreaterOrEqual(1000, timeElapsed.Milliseconds);
        }


        /// <summary>
        /// Tests cosmos chart page load performance. Must be equal or under 1 second.
        /// </summary>
        [Test]
        public void CosmosPage_ChartButtonClick_LoadsUnder1Second()
        {
            // Goes trough login procedure (requires test runner to manually fill captcha and click continue).
            webDriver.Navigate().GoToUrl(AuthenticationConstants.LocalURL);
            webDriver.FindElement(By.Id("login")).Click();
            webDriver.FindElement(By.Id("username")).SendKeys("CisSelenium@gmail.com");
            webDriver.FindElement(By.Id("password")).SendKeys("Testingpass123!");
            webDriverWait.Until(webDriver => webDriver.FindElement(By.Id("homeSubtitle")).Displayed);
            webDriver.FindElement(By.Id("market")).Click();

            // Measures time of target page load.
            stopWatch.Start();
            webDriver.Navigate().GoToUrl(AuthenticationConstants.LocalURL + "/market/cosmos");
            webDriverWait.Until(webDriver => webDriver.FindElement(By.Id("chart-container")).Displayed);
            stopWatch.Stop();

            // Collects elapsed time and asserts if it doesn't exceed 1 second limit.
            TimeSpan timeElapsed = stopWatch.Elapsed;
            Assert.GreaterOrEqual(1000, timeElapsed.Milliseconds);
        }


        /// <summary>
        /// Tests dogecoin chart page load performance. Must be equal or under 1 second.
        /// </summary>
        [Test]
        public void DogecoinPage_ChartButtonClick_LoadsUnder1Second()
        {
            // Goes trough login procedure (requires test runner to manually fill captcha and click continue).
            webDriver.Navigate().GoToUrl(AuthenticationConstants.LocalURL);
            webDriver.FindElement(By.Id("login")).Click();
            webDriver.FindElement(By.Id("username")).SendKeys("CisSelenium@gmail.com");
            webDriver.FindElement(By.Id("password")).SendKeys("Testingpass123!");
            webDriverWait.Until(webDriver => webDriver.FindElement(By.Id("homeSubtitle")).Displayed);
            webDriver.FindElement(By.Id("market")).Click();

            // Measures time of target page load.
            stopWatch.Start();
            webDriver.Navigate().GoToUrl(AuthenticationConstants.LocalURL + "/market/dogecoin");
            webDriverWait.Until(webDriver => webDriver.FindElement(By.Id("chart-container")).Displayed);
            stopWatch.Stop();

            // Collects elapsed time and asserts if it doesn't exceed 1 second limit.
            TimeSpan timeElapsed = stopWatch.Elapsed;
            Assert.GreaterOrEqual(1000, timeElapsed.Milliseconds);
        }
    }
}