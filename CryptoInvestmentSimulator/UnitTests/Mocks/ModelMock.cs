using CryptoInvestmentSimulator.Models;

namespace UnitTests.Mocks
{
    public class ModelMock
    {
        /// <summary>
        /// Creates a user model with valid data
        /// </summary>
        /// <returns>UserModel</returns>
        public static UserModel GetValidUserModel()
        {
            return new UserModel()
            {
                UserId = 1,
                FirstName = "John",
                LastName = "Doe",
                EmailAddress = "JohnDoe@gmail.com",
                AvatarUrl = "https://mockurl.com/avatar",
                IsVerified = true
            };
        }
    }
}
