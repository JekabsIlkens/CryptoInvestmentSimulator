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
                Email = "mock-email",
                Verified = 0,
                Username = "mock-name",
                Avatar = "mock-url",
                TimeZone = "GMT-09:00"
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
                Email = "",
                Verified = -4,
                Username = "",
                Avatar = "",
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
                Id = 1,
                CollectionTime = DateTime.Now.AddDays(-1),
                UnitValue = 0.021599M,
                Change24h = 25.00M,
                Change7d = 45.00M,
                CryptoSymbol = CryptoEnum.BTC.ToString(),
                FiatSymbol = FiatEnum.EUR.ToString(),
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
                Id = 2,
                CollectionTime = DateTime.Now,
                UnitValue = 0.091599M,
                Change24h = 15.00M,
                Change7d = 85.00M,
                CryptoSymbol = CryptoEnum.BTC.ToString(),
                FiatSymbol = FiatEnum.EUR.ToString(),
            };
        }
    }
}
