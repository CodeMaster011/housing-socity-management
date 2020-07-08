using System;

namespace HSM.WebApp.Data.Models
{
    public class Unit
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
    }
}