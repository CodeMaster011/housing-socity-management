using System;
using System.Linq;
using System.Threading.Tasks;
using HSM.WebApp.Data;
using HSM.WebApp.Data.Models;
using HSM.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HSM.WebApp.Controllers
{
    public class ChargesController : Controller
    {
        private readonly ILogger<ChargesController> _logger;
        private readonly HsmDbContext _dbContext;

        public ChargesController(ILogger<ChargesController> logger, HsmDbContext dbContext)
        {
            _logger = logger;
            this._dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            var model = await _dbContext.Charges
            .AsNoTracking()
                .OrderBy(m => m.Name)
            .ToArrayAsync();
            
            return View(model);
        }

        public IActionResult Create()
        {
            return View(model: new Charges {  });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] Charges model)
        {
            var status = new StatusModel<Charges>
            {
                Model = model,
                Errors = new System.Collections.Generic.List<string>()
            };

            // data validation to be done here
            if (string.IsNullOrEmpty(model.Name))
                status.Errors.Add("Name of charges is not specified.");
            
            
            if (status.Errors.Count != 0)
                return View("Status", status);

            model.Id = Guid.NewGuid().ToString();
            _dbContext.Charges.Add(model);
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
            var unit = await _dbContext.Charges.AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);
            
            return View(unit);
        }

        public async Task<IActionResult> Payment()
        {
            var model = new ChargesPaymentModel();
            var units = await _dbContext.Units.AsNoTracking()
                .Include(u => u.Owner)
                .OrderBy(u => u.Name)
                .ToArrayAsync();
            model.Units = units.Select(u => new SelectedUnit{ Unit = u, UnitId = u.Id, IsSelected = true }).ToList();
            model.Charges = await _dbContext.Charges.AsNoTracking()
                .OrderBy(c => c.Name)
                .ToListAsync();
            model.Date = DateTime.Now;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Payment([FromForm] ChargesPaymentModel model)
        {
            var charge = await _dbContext.Charges.AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == model.SelectedChargesId);
            if(charge == null) return NotFound("Charge not found");
            var selectedUnitIds = model.Units.Where(u => u.IsSelected).Select(u => u.UnitId).ToArray();
            var transactions = new Transaction[selectedUnitIds.Length];
            for (int i = 0; i < selectedUnitIds.Length; i++)
            {
                var unitId = selectedUnitIds[i];
                var unit = await _dbContext.Units.Include(u => u.Owner).ThenInclude(u => u.Account).FirstOrDefaultAsync(u => u.Id == unitId);
                if (unit == null)
                    return NotFound($"{unitId} does not exits.");
                transactions[i] = new Transaction
                {
                    Id = Guid.NewGuid().ToString(),
                    Date = model.Date,
                    AccountId = unit.Owner.Account.Id,
                    UnitId = unit.Id,
                    LedgerId = charge.LedgerId,
                    IsDue = true,
                    // PassthroughId
                    Description = $"{model.Particulars} {charge.Name} Charge {unit.Name} on {unit.Owner.Name}",
                    CreatedByUserOn = DateTime.Now,
                    Amount = CalculateChargeAmount(charge, unit)
                };
            }

            return Json(transactions);
        }

        protected double CalculateChargeAmount(Charges charge, Unit unit)
        {
            var finalAmount = 0d;
            if (charge.OnArea.HasValue) finalAmount += charge.OnArea.Value * unit.Area.Value;
            if (charge.OnFlatAmount.HasValue) finalAmount += charge.OnFlatAmount.Value;
            if (charge.MaxAmount.HasValue && finalAmount > charge.MaxAmount.Value)
                finalAmount = charge.MaxAmount.Value;
            if (charge.MinAmount.HasValue && finalAmount < charge.MinAmount.Value)
                finalAmount = charge.MinAmount.Value;
            
            finalAmount = Math.Floor(finalAmount / 10) * 10;
            return finalAmount;
        }
    }
}
