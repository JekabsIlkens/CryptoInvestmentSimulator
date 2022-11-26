namespace CryptoInvestmentSimulator.Models
{
    public class UserModel
    {
        public int UserId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string EmailAddress { get; set; }

        public string AvatarUrl { get; set; }

        public bool IsVerified { get; set; }
    }
}
