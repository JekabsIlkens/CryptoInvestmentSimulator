namespace CryptoInvestmentSimulator.Models.ViewModels
{
    public class LiquidationModel
    {
        public int positionId { get; set; }

        public decimal FiatAmount { get; set; }

        public decimal CryptoAmount { get; set; }

        public decimal MarginAmount { get; set; }

        public int RatioId { get; set; }

        public decimal UnitValue { get; set; }

        public int CryptoId { get; set; }
    }
}
