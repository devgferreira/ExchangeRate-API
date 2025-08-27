namespace ExchangeRate.Application.DTO.Currency
{
    public class CurrencyPriceBidVariationDTO
    {
        public decimal FirstBidPrice { get; set; }
        public decimal LastBidPrice { get; set; }
        public decimal VariationPercentage { get; set; }
        public decimal VariationPrice { get; set; }
    }
}
