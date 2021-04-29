using System.Collections.Generic;
using HSM.WebApp.Data.Models;

namespace HSM.WebApp.Models
{
    public class TransactionCreationModel : StatusModel<Transaction>
    {
        public List<Ledger> Ledgers { get; set; }
        public List<TransactionPassthrough> TransactionPassthroughs { get; set; }
        public List<Member> Members { get; set; }
        public List<Unit> Units { get; set; }
    }
}
