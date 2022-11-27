namespace CryptoInvestmentSimulator.Models
{
    public class UserModel
    {
        public int UserId { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public string AvatarUrl { get; set; }

        public bool IsVerified { get; set; }
    }
}
