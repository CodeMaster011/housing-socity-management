using System;
using System.Collections.Generic;

namespace HSM.WebApp.Data.Models
{
    /*
        Idea: 
            Due in each month to be charged to every unit and booked as liability for a member account.
            Making IsDue = true.
            Amount = (ve-).
            Once payment is received from member, liability for that member account will be reduced.
    */
    public class Transaction : IIdentifiable
    {
        public string Id { get; set; }
        public DateTime? Date { get; set; } // required

        public string UnitId { get; set; }
        public Unit Unit { get; set; }

        public string AccountId { get; set; }
        public MemberAccount Account { get; set; }

        public string LedgerId { get; set; } // required
        public Ledger Ledger { get; set; }

        public string PassthroughId { get; set; } // required
        public TransactionPassthrough Passthrough { get; set; }

        public string Description { get; set; } // required
        public string Tags { get; set; }
        public double Amount { get; set; } // Inflow = Ve+, Outflow = Ve-
        public bool IsDue { get; set; } // once true, no actual CFs occurred
        public bool IsContra { get; set; }

        // ====== System Audit Data ======
        public DateTime? CreatedByUserOn { get; set; }
        public string CreatedByUser { get; set; }
    }

    public class TransactionPassthrough : IIdentifiable
    {
        public string Id { get; set; }
        public string Name { get; set; } // Cash, Bank
        public bool IsLocked { get; set; }

        public double Balance { get; set; }
        public DateTime? CalculatedOn { get; set; }

        public ICollection<Transaction> Transactions { get; set; }
    }

    public class Ledger : IIdentifiable
    {
        public string Id { get; set; }
        public string Name { get; set; } // Maintenance, Maintenance-Due (L), Expense, Other Income, Deposit, Deposit-Due (L)
        public bool IsDue { get; set; }
        public bool IsLocked { get; set; }

        public double Balance { get; set; }
        public DateTime? CalculatedOn { get; set; }

        public ICollection<Transaction> Transactions { get; set; }
    }

    public class MemberAccount : IIdentifiable
    {
        public string Id { get; set; }
        public string MemberId { get; set; }
        public Member Member { get; set; }

        public DateTime? ActivatedOn { get; set; }
        public double Balance { get; set; }
        public DateTime? CalculatedOn { get; set; }

        public ICollection<Transaction> Transactions { get; set; }
    }
}