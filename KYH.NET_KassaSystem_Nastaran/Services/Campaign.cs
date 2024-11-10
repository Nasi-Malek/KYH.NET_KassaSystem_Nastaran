using KYH.NET_KassaSystem_Nastaran.Enum;
using System;

namespace KYH.NET_KassaSystem_Nastaran.Services
{
    public class Campaign
    {
        public CampaignType Type { get; private set; }
        public decimal DiscountValue { get; private set; } // Kan vara procent eller fast rabatt beroende på typ
        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }

        public Campaign(CampaignType type, decimal discountValue, DateTime startDate, DateTime endDate)
        {
            if (discountValue < 0)
                throw new ArgumentException("Rabattvärdet kan inte vara negativt.", nameof(discountValue));

            if (endDate < startDate)
                throw new ArgumentException("Slutdatum kan inte vara tidigare än startdatum.", nameof(endDate));

            Type = type;
            DiscountValue = discountValue;
            StartDate = startDate;
            EndDate = endDate;
        }

        // Kontrollera om kampanjen är aktiv för ett visst datum
        public bool IsActive(DateTime date)
        {
            return date >= StartDate && date <= EndDate;
        }

        // Beräkna priset baserat på kampanjtypen
        public decimal ApplyDiscount(decimal originalPrice)
        {
            if (originalPrice < 0)
                throw new ArgumentException("Pris kan inte vara negativt.", nameof(originalPrice));

            return Type switch
            {
                CampaignType.PercentageDiscount => originalPrice * (1 - DiscountValue / 100),
                CampaignType.FixedDiscount => Math.Max(originalPrice - DiscountValue, 0),
                CampaignType.BuyOneGetOneFree => originalPrice / 2,
                _ => originalPrice
            };
        }
    }
}
