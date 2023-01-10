# **Cryptocurrency Investment Simulator**

**Goal:** College degree qualification project for University of Latvia <br/>
**Framework:** ASP.NET Core 6 <br/>
**Pattern:** Model-View-Controller <br/>
**Services:** Auth0 and CoinMarketCap API

---

**Description** <br/>
The purpose of this qualification project is to develop a useful website for investors who make their investments in the cryptocurrency market.
The goal is to provide an investor with a convenient, reliable and easy to-use environment to practice their market skills through simulated investments.
Investors can view real-time price charts and buy cryptocurrencies using pre-allocated, artificial money in their wallet. 
With the help of CoinMarketCap API charts reflect real market data, but investments are just imitations. 
In addition, it is possible to add leverage to transactions, which gives you the opportunity to test its benefits and risks. 
Immediately after making the first transaction, the investor starts receiving a transparent analysis of the contents of his 
currency wallets, capital changes and other successes.

---

**Naming conventions for test projects** <br/>
Test classes are named ClassNameTests.cs (for example: MarketControllerTests.cs). <br/>
Test methods are named MethodName_TestScenario_ExpectedResult (for example: InserUser_MissingData_ThrowsException).

---

**Other information** <br/>
Database table scripts and specific data insertion scripts can be found under ExternalCode. <br/>
None of the branches have been deleted, so they can be pulled and used to view the historical state of the project. <br/>
Unit tests use xUnit and functional tests use Selenium WebDriver.