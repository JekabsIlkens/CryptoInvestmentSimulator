namespace CryptoInvestmentSimulator.Models.ViewModels
{
    public class UserModel
    {
        public int UserId { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public string AvatarUrl { get; set; }

        public bool IsVerified { get; set; }

        public string TimeZone { get; set; }
    }
}
