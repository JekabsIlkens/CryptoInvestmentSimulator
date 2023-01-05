namespace CryptoInvestmentSimulator.Models.ViewModels
{
	public class PositionModel
	{
		public int Id { get; set; }
		public int BoughtCrypto { get; set; }
		public DateTime DateTime { get; set; }
		public decimal FiatAmount { get; set; }
		public decimal CryptoAmount { get; set; }
		public decimal Margin { get; set; }
		public int Leverage { get; set; }
		public int Status { get; set; }
		public int Wallet { get; set; }
		public int Data { get; set; }
	}
}
