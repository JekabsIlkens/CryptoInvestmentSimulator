using CryptoInvestmentSimulator.Enums;
using CryptoInvestmentSimulator.Models.ViewModels;

namespace UnitTests.Mocks
{
    public class ModelMock
    {
        /// <summary>
        /// Creates a <see cref="UserModel"/> filled with valid mock data.
        /// User is verified.
        /// </summary>
        /// <returns>Filled <see cref="UserModel"/></returns>
        public static UserModel GetValidVerifiedUserModel()
        {
            return new UserModel()
            {
                Id = 1,
                Email = "mock@mail.com",
                Verified = 1,
                Username = "mock-name",
                Avatar = "mock-avatar",
                TimeZone = "GMT+02:00"
            };
        }

        /// <summary>
        /// Creates a <see cref="UserModel"/> filled with invalid data.
        /// </summary>
        /// <returns>Filled <see cref="UserModel"/></returns>
        public static UserModel GetInvalidUserModel()
        {
            return new UserModel()
            {
                Id = -1,
                Email = "",
                Verified = -1,
                Username = "",
                Avatar = "",
                TimeZone = ""
            };
        }

        /// <summary>
        /// Creates a <see cref="MarketDataModel"/> filled with valid data.
        /// Sets collection to date time in past.
        /// </summary>
        /// <returns>Filled <see cref="MarketDataModel"/></returns>
        public static MarketDataModel GetValidOldMarketDataModel()
        {
            return new MarketDataModel()
            {
                Id = 1,
                CollectionTime = DateTime.Now.AddDays(-10),
                UnitValue = 0.025M,
                Change24h = 25.00M,
                Change7d = 45.00M,
                CryptoSymbol = CryptoEnum.BTC.ToString(),
                FiatSymbol = FiatEnum.EUR.ToString()
            };
        }

        /// <summary>
        /// Creates a <see cref="MarketDataModel"/> filled with valid values.
        /// Sets collection date to current date time.
        /// </summary>
        /// <returns>Filled <see cref="MarketDataModel"/></returns>
        public static MarketDataModel GetValidNewMarketDataModel()
        {
            return new MarketDataModel()
            {
                Id = 2,
                CollectionTime = DateTime.Now,
                UnitValue = 0.015M,
                Change24h = 15.00M,
                Change7d = 85.00M,
                CryptoSymbol = CryptoEnum.ETH.ToString(),
                FiatSymbol = FiatEnum.EUR.ToString()
            };
        }

        /// <summary>
        /// Creates a <see cref="PositionModel"/> filled with valid values.
        /// Sets buy date time to past date time.
        /// Has no leverage selected.
        /// </summary>
        /// <returns>Filled <see cref="PositionModel"/></returns>
        public static PositionModel GetOldBitcoinPositionWithoutLeverage()
        {
            return new PositionModel()
            {
                Id = 1,
                DateTime = DateTime.Now.AddDays(-10),
                FiatAmount = 300M,
                CryptoAmount = 150M,
                Margin = 0M,
                Leverage = (int)LeverageEnum.None,
                Status = (int)StatusEnum.Open,
                Wallet = 1,
                Data = 1
            };
        }

        /// <summary>
        /// Creates a <see cref="PositionModel"/> filled with valid values.
        /// Sets buy date time to current date time.
        /// Has 5x leverage selected.
        /// </summary>
        /// <returns>Filled <see cref="PositionModel"/></returns>
        public static PositionModel GetNewEtheriumPositionWithLeverage()
        {
            return new PositionModel()
            {
                Id = 2,
                DateTime = DateTime.Now,
                FiatAmount = 500M,
                CryptoAmount = 150M,
                Margin = 100M,
                Leverage = (int)LeverageEnum.Five,
                Status = (int)StatusEnum.Open,
                Wallet = 1,
                Data = 2
            };
        }
    }
}
