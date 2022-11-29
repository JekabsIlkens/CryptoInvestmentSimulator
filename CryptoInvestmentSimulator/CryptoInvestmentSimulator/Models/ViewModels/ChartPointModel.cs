using System.Runtime.Serialization;

namespace CryptoInvestmentSimulator.Models.ViewModels
{
    [DataContract]
    public class ChartPointModel
    {
        public ChartPointModel(double timePoint, double pricePoint)
        {
            TimePoint = timePoint;
            PricePoint = pricePoint;
        }

        [DataMember(Name = "x")]
        public double? TimePoint = null;

        [DataMember(Name = "y")]
        public double? PricePoint = null;
    }
}