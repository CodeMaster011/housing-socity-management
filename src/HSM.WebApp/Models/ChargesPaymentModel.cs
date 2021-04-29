using System;
using System.Collections.Generic;
using HSM.WebApp.Data.Models;

namespace HSM.WebApp.Models
{
    public class ChargesPaymentModel
    {
        public List<Charges> Charges { get; set; }
        public List<SelectedUnit> Units { get; set; }
        // public List<Member> Members { get; set; }

        public string SelectedChargesId { get; set; }
        public string Particulars { get; set; }
        public DateTime Date { get; set; }
        // public string[] SelectedUnits { get; set; }
    }

    public class SelectedUnit
    {
        public Unit Unit { get; set; }
        public string UnitId { get; set; }
        public bool IsSelected { get; set; }
    }
}
