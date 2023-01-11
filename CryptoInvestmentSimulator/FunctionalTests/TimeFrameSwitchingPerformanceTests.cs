using NUnit.Framework;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using System.Diagnostics;
using CryptoInvestmentSimulator.Constants;

namespace FunctionalTests
{
    public class TimeFrameSwitchingPerformanceTests
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
        /// Tests bitcoin switch to 4h chart load performance. Must be equal or under 1 second.
        /// </summary>
        [Test]
        public void BitcoinPage_SwitchTo4hChart_LoadsUnder1Second()
        {
            // Goes trough login procedure (requires test runner to manually fill captcha and click continue).
            webDriver.Navigate().GoToUrl(AuthenticationConstants.LocalURL);
            webDriver.FindElement(By.Id("login")).Click();
            webDriver.FindElement(By.Id("username")).SendKeys("CisSelenium@gmail.com");
            webDriver.FindElement(By.Id("password")).SendKeys("Testingpass123!");
            webDriverWait.Until(webDriver => webDriver.FindElement(By.Id("homeSubtitle")).Displayed);

            // Navigates to bitcoin page and waits for it to load.
            webDriver.Navigate().GoToUrl(AuthenticationConstants.LocalURL + "/market/bitcoin");
            webDriverWait.Until(webDriver => webDriver.FindElement(By.Id("chart-container")).Displayed);

            // Measures time of target page load.
            stopWatch.Start();
            webDriver.FindElement(By.Id("4h")).Click();
            webDriverWait.Until(webDriver => webDriver.FindElement(By.Id("chartZone4h")).Displayed);
            stopWatch.Stop();

            // Collects elapsed time and asserts if it doesn't exceed 1 second limit.
            TimeSpan timeElapsed = stopWatch.Elapsed;
            Assert.GreaterOrEqual(1000, timeElapsed.Milliseconds);
        }

        /// <summary>
        /// Tests bitcoin switch to 8h chart load performance. Must be equal or under 1 second.
        /// </summary>
        [Test]
        public void BitcoinPage_SwitchTo8hChart_LoadsUnder1Second()
        {
            // Goes trough login procedure (requires test runner to manually fill captcha and click continue).
            webDriver.Navigate().GoToUrl(AuthenticationConstants.LocalURL);
            webDriver.FindElement(By.Id("login")).Click();
            webDriver.FindElement(By.Id("username")).SendKeys("CisSelenium@gmail.com");
            webDriver.FindElement(By.Id("password")).SendKeys("Testingpass123!");
            webDriverWait.Until(webDriver => webDriver.FindElement(By.Id("homeSubtitle")).Displayed);

            // Navigates to bitcoin page and waits for it to load.
            webDriver.Navigate().GoToUrl(AuthenticationConstants.LocalURL + "/market/bitcoin");
            webDriverWait.Until(webDriver => webDriver.FindElement(By.Id("chart-container")).Displayed);

            // Measures time of target page load.
            stopWatch.Start();
            webDriver.FindElement(By.Id("8h")).Click();
            webDriverWait.Until(webDriver => webDriver.FindElement(By.Id("chartZone8h")).Displayed);
            stopWatch.Stop();

            // Collects elapsed time and asserts if it doesn't exceed 1 second limit.
            TimeSpan timeElapsed = stopWatch.Elapsed;
            Assert.GreaterOrEqual(1000, timeElapsed.Milliseconds);
        }

        /// <summary>
        /// Tests bitcoin switch to 24h chart load performance. Must be equal or under 1 second.
        /// </summary>
        [Test]
        public void BitcoinPage_SwitchTo24hChart_LoadsUnder1Second()
        {
            // Goes trough login procedure (requires test runner to manually fill captcha and click continue).
            webDriver.Navigate().GoToUrl(AuthenticationConstants.LocalURL);
            webDriver.FindElement(By.Id("login")).Click();
            webDriver.FindElement(By.Id("username")).SendKeys("CisSelenium@gmail.com");
            webDriver.FindElement(By.Id("password")).SendKeys("Testingpass123!");
            webDriverWait.Until(webDriver => webDriver.FindElement(By.Id("homeSubtitle")).Displayed);

            // Navigates to bitcoin page and waits for it to load.
            webDriver.Navigate().GoToUrl(AuthenticationConstants.LocalURL + "/market/bitcoin");
            webDriverWait.Until(webDriver => webDriver.FindElement(By.Id("chart-container")).Displayed);

            // Measures time of target page load.
            stopWatch.Start();
            webDriver.FindElement(By.Id("24h")).Click();
            webDriverWait.Until(webDriver => webDriver.FindElement(By.Id("chartZone24h")).Displayed);
            stopWatch.Stop();

            // Collects elapsed time and asserts if it doesn't exceed 1 second limit.
            TimeSpan timeElapsed = stopWatch.Elapsed;
            Assert.GreaterOrEqual(1000, timeElapsed.Milliseconds);
        }
    }
}
