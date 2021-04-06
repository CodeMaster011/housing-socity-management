using System;
using System.Linq;
using System.Globalization;
using System.Threading.Tasks;
using HSM.WebApp.Data;
using HSM.WebApp.Data.Models;
using HSM.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace HSM.WebApp.Controllers
{
    public class MemberController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HsmDbContext _dbContext;

        public MemberController(ILogger<HomeController> logger, HsmDbContext dbContext)
        {
            _logger = logger;
            this._dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            var model = await _dbContext.Members
            .AsNoTracking()
                .OrderBy(m => m.Name)
            .ToArrayAsync();
            
            return View(model);
        }

        public IActionResult Create()
        {
            return View(model: new Member());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm]Member model)
        {
            var status = new StatusModel<Member>
            {
                Model = model,
                Errors = new System.Collections.Generic.List<string>()
            };

            // data validation to be done here
            if(string.IsNullOrEmpty(model.Name))
                status.Errors.Add("Name of member is not specified.");
            if(!string.IsNullOrEmpty(model.PhoneNumber) && model.PhoneNumber.Length == 10 && !long.TryParse(model.PhoneNumber, NumberStyles.Integer, null, out _))
                status.Errors.Add("Specified phone number is not number");

            if(status.Errors.Count != 0)
                return View("Status", status);
             
            model.Id = Guid.NewGuid().ToString();
            model.OwnedUnits = null;
            model.Account = null;
            model.LastUpdatedOn = DateTime.Now;

            model.Account = new MemberAccount { Member = model };
            _dbContext.Members.Add(model);
            try
            {
                if(await _dbContext.SaveChangesAsync() <= 0)
                    status.Errors.Add("Failed to update Database. There may be connection error. Contact administrator now.");
                
                status.ModelId = model.Id;
            }
            catch (System.Exception e)
            {
                status.Errors.Add(e.Message);
            }

            return View("Status", status);
        }

        public async Task<IActionResult> Details(string id)
        {
            var member = await _dbContext.Members.AsNoTracking()
                .Include(m => m.Account)
                .Include(m => m.OwnedUnits)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (member.Account != null)
            {
                var transactions = await _dbContext.Transactions.AsNoTracking()
                .Where(t => t.AccountId == member.Account.Id)
                .OrderByDescending(t => t.Date)
                .Take(5)
                .ToArrayAsync();
                member.Account.Transactions = transactions;
            }
            return View(member);
        }

        public async Task<IActionResult> AddMemberTransactionAccount(string memberId)
        {
            var member = await _dbContext.Members.AsNoTracking()
                .Include(m => m.Account)
                .FirstOrDefaultAsync(m => m.Id == memberId);
            if (member.Account == null)
            {
                _dbContext.MemberAccounts.Add(new MemberAccount
                {
                    ActivatedOn = DateTime.Now,
                    Balance = 0d,
                    CalculatedOn = DateTime.Now,
                    MemberId = memberId
                });
                if (await _dbContext.SaveChangesAsync() > 0)
                    return RedirectToActionPermanent(nameof(Details),new { id = memberId });
            }
            return RedirectToActionPermanent(nameof(Index));
        }
    }
}
