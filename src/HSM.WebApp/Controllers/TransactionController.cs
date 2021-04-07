using System;
using System.Linq;
using System.Threading.Tasks;
using HSM.WebApp.Data;
using HSM.WebApp.Data.Models;
using HSM.WebApp.Models;
using HSM.WebApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HSM.WebApp.Controllers
{
    public class TransactionController : Controller
    {
        private readonly ILogger<TransactionController> _logger;
        private readonly HsmDbContext _dbContext;
        private readonly ISmallIdService smallIdService;

        public TransactionController(ILogger<TransactionController> logger, HsmDbContext dbContext, ISmallIdService smallIdService)
        {
            _logger = logger;
            this._dbContext = dbContext;
            this.smallIdService = smallIdService;
        }

        public async Task<IActionResult> Index([FromQuery]int fromNum)
        {
            var model = await _dbContext.Transactions
            .AsNoTracking()
                .OrderByDescending(m => m.Date)
                .Skip(fromNum)
                .Take(100)
            .ToArrayAsync();
            
            return View(model);
        }
        public IActionResult Create([FromQuery]string mAccountId, [FromQuery]string unitId)
        {
            return View(model: new Transaction { AccountId = mAccountId, UnitId = unitId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] Transaction model)
        {
            var status = new StatusModel<Transaction>
            {
                Model = model,
                Errors = new System.Collections.Generic.List<string>()
            };

            // data validation to be done here
            if (string.IsNullOrEmpty(model.Description))
                status.Errors.Add("Description of transaction is not specified.");
            if (!model.Date.HasValue)
                status.Errors.Add("Date of transaction is not specified.");
            
            if(string.IsNullOrWhiteSpace(model.LedgerId))
            {
                status.Errors.Add("Ledger of transaction is not specified.");
            }
            else
            {
                var ledger = await _dbContext.Ledgers.AsNoTracking().FirstOrDefaultAsync(m => m.Id == model.LedgerId);
                if (ledger == null)
                {
                    status.Errors.Add("Ledger of transaction is not valid.");
                }
            }
            if(string.IsNullOrWhiteSpace(model.PassthroughId))
            {
                status.Errors.Add("Passthrough of transaction is not specified.");
            }
            else
            {
                var ledger = await _dbContext.TransactionPassthrough.AsNoTracking().FirstOrDefaultAsync(m => m.Id == model.PassthroughId);
                if (ledger == null)
                {
                    status.Errors.Add("Passthrough of transaction is not valid.");
                }
            }
            if(!string.IsNullOrWhiteSpace(model.AccountId))
            {
                var ownerAccount = await _dbContext.MemberAccounts.AsNoTracking().FirstOrDefaultAsync(m => m.Id == model.AccountId);
                if (ownerAccount == null)
                {
                    status.Errors.Add("Owner of transaction is not valid.");
                }
            }
            if(!string.IsNullOrWhiteSpace(model.UnitId))
            {
                var unit = await _dbContext.Units.AsNoTracking().FirstOrDefaultAsync(m => m.Id == model.UnitId);
                if (unit == null)
                {
                    status.Errors.Add("Unit of transaction is not valid.");
                }
            }
            
            model.Account = null;
            model.Passthrough = null;
            model.Unit = null;
            model.Ledger = null;
            model.Passthrough = null;

            if (status.Errors.Count != 0)
                return View("Status", status);

            model.Id = Guid.NewGuid().ToString();
            _dbContext.Transactions.Add(model);
            try
            {
                if (await _dbContext.SaveChangesAsync() <= 0)
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
            var transaction = await _dbContext.Transactions.AsNoTracking()
                .Include(m => m.Unit)
                .Include(m => m.Ledger)
                .Include(m => m.Passthrough)
                .Include(m => m.Account)
                    .ThenInclude(m => m.Member)
                .FirstOrDefaultAsync(m => m.Id == id);
            
            return View(transaction);
        }
    }
}
