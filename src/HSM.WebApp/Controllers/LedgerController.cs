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
    public class LedgerController : Controller
    {
        private readonly ILogger<LedgerController> _logger;
        private readonly HsmDbContext _dbContext;

        public LedgerController(ILogger<LedgerController> logger, HsmDbContext dbContext)
        {
            _logger = logger;
            this._dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            var model = await _dbContext.Ledgers
            .AsNoTracking()
                .OrderBy(m => m.Name)
            .ToArrayAsync();
            
            return View(model);
        }

        public IActionResult Create()
        {
            return View(model: new Ledger {  });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] Ledger model)
        {
            var status = new StatusModel<Ledger>
            {
                Model = model,
                Errors = new System.Collections.Generic.List<string>()
            };

            // data validation to be done here
            if (string.IsNullOrEmpty(model.Name))
                status.Errors.Add("Name of ledger is not specified.");
            
            

            if (status.Errors.Count != 0)
                return View("Status", status);

            model.Id = Guid.NewGuid().ToString();
            _dbContext.Ledgers.Add(model);
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
            var unit = await _dbContext.Ledgers.AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);
            
           
            var transactions = await _dbContext.Transactions.AsNoTracking()
            .Where(t => t.LedgerId == id)
                .OrderByDescending(t => t.Date)
                .Take(30)
                .ToArrayAsync();
            unit.Transactions = transactions;
            
            return View(unit);
        }
    }
}
