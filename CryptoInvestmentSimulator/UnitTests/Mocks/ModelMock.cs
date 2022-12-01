using CryptoInvestmentSimulator.Models.ViewModels;

namespace UnitTests.Mocks
{
    public class ModelMock
    {
        /// <summary>
        /// Creates a user model filled with valid mock data.
        /// </summary>
        /// <returns>Filled user model</returns>
        public static UserModel GetValidUserModel()
        {
            return new UserModel()
            {
                UserId = 1,
                Username = "mock-name",
                Email = "mock-email",
                AvatarUrl = "mock-url",
                IsVerified = false,
                TimeZone = "mock-zone"
            };
        }

        /// <summary>
        /// Creates a user model filled with invalid values.
        /// </summary>
        /// <returns>Filled user model</returns>
        public static UserModel GetInvalidUserModel()
        {
            return new UserModel()
            {
                UserId = -15,
                Username = "",
                Email = "",
                AvatarUrl = "",
                IsVerified = false,
                TimeZone = ""
            };
        }
    }
}
