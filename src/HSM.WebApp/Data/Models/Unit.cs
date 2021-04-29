using System;

namespace HSM.WebApp.Data.Models
{
    public class Unit : IIdentifiable
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public long? Area { get; set; }

        public string OwnerId { get; set; }
        public Member Owner { get; set; }

        // Rented information
        public bool IsRented { get; set; }
        public DateTime? RentedFrom { get; set; }
        public string RentedPersonName { get; set; }
        public string RentedPersonPhone { get; set; }
        public string MeterNumber { get; set; }
    }

    public class Charges : IIdentifiable
    {
        public string Id { get; set; }
        public string Name { get; set; } // Maintenance, Deposit
        public DateTime? EffectiveFrom { get; set; }
        public double? OnArea { get; set; }
        public double? OnFlatAmount { get; set; }
        public double? MaxAmount { get; set; }
        public double? MinAmount { get; set; }
        
        public Ledger Ledger { get; set; }
        public string LedgerId { get; set; }
    }
}