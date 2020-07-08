using System.Collections.Generic;
using HSM.WebApp.Data.Models;

namespace HSM.WebApp.Models
{
    public class MemberCreationStatusModel
    {
        public string MemberId { get; set; }
        public Member Member { get; set; }
        public List<string> Errors { get; set; }
    }
}
