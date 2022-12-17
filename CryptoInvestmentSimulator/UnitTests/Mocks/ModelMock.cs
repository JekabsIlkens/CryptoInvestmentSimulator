using CryptoInvestmentSimulator.Enums;
using CryptoInvestmentSimulator.Models.ViewModels;

namespace UnitTests.Mocks
{
    public class ModelMock
    {
        /// <summary>
        /// Creates a <see cref="UserModel"/> filled with valid mock data.
        /// </summary>
        /// <returns>Filled <see cref="UserModel"/></returns>
        public static UserModel GetValidUserModel()
        {
            return new UserModel()
            {
                Id = 1,
                Username = "mock-name",
                Email = "mock-email",
                Avatar = "mock-url",
                Verified = 0,
                TimeZone = "mock-zone"
            };
        }

        /// <summary>
        /// Creates a <see cref="UserModel"/> filled with invalid values.
        /// </summary>
        /// <returns>Filled <see cref="UserModel"/></returns>
        public static UserModel GetInvalidUserModel()
        {
            return new UserModel()
            {
                Id = -15,
                Username = "",
                Email = "",
                Avatar = "",
                Verified = -4,
                TimeZone = ""
            };
        }

        /// <summary>
        /// Creates a <see cref="MarketDataModel"/> filled with valid values.
        /// Sets collection to date time in past.
        /// </summary>
        /// <returns>Filled <see cref="MarketDataModel"/></returns>
        public static MarketDataModel GetValidMarketDataModelOld()
        {
            return new MarketDataModel()
            {
                CryptoSymbol = CryptoEnum.BTC.ToString(),
                FiatSymbol = FiatEnum.EUR.ToString(),
                CollectionTime = DateTime.Now.AddDays(-1),
                UnitValue = 0.021599M,
                Change24h = 25.00M,
                Change7d = 45.00M
            };
        }

        /// <summary>
        /// Creates a <see cref="MarketDataModel"/> filled with valid values.
        /// Sets collection date to current date time.
        /// </summary>
        /// <returns>Filled <see cref="MarketDataModel"/></returns>
        public static MarketDataModel GetValidMarketDataModelNew()
        {
            return new MarketDataModel()
            {
                CryptoSymbol = CryptoEnum.BTC.ToString(),
                FiatSymbol = FiatEnum.EUR.ToString(),
                CollectionTime = DateTime.Now,
                UnitValue = 0.091599M,
                Change24h = 15.00M,
                Change7d = 85.00M
            };
        }
    }
}
