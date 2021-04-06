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
    public class UnitsController : Controller
    {
        private readonly ILogger<UnitsController> _logger;
        private readonly HsmDbContext _dbContext;
        private readonly ISmallIdService smallIdService;

        public UnitsController(ILogger<UnitsController> logger, HsmDbContext dbContext, ISmallIdService smallIdService)
        {
            _logger = logger;
            this._dbContext = dbContext;
            this.smallIdService = smallIdService;
        }

        public async Task<IActionResult> Index()
        {
            var model = await _dbContext.Units
            .AsNoTracking()
                .OrderBy(m => m.Name)
            .ToArrayAsync();
            
            return View(model);
        }
        public IActionResult Create([FromQuery]string ownerId)
        {
            return View(model: new Unit { OwnerId = ownerId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] Unit model)
        {
            var status = new StatusModel<Unit>
            {
                Model = model,
                Errors = new System.Collections.Generic.List<string>()
            };

            // data validation to be done here
            if (string.IsNullOrEmpty(model.Name))
                status.Errors.Add("Name of unit is not specified.");
            if (!model.Area.HasValue)
                status.Errors.Add("Area of unit is not specified.");
            if(string.IsNullOrWhiteSpace(model.OwnerId))
            {
                status.Errors.Add("Owner of unit is not specified.");
            }
            else
            {
                var owner = await _dbContext.Members.AsNoTracking().FirstOrDefaultAsync(m => m.Id == model.OwnerId);
                if (owner == null)
                {
                    var expandedId = smallIdService.GetFullId<Member>(model.OwnerId);
                    if(expandedId == null
                        || await _dbContext.Members.AsNoTracking().FirstOrDefaultAsync(m => m.Id == expandedId) == null)
                    {
                        status.Errors.Add("Owner of unit is not valid.");
                    }
                    else
                    {
                        model.OwnerId = expandedId;
                    }
                }
            }
            model.Owner = null;

            if (status.Errors.Count != 0)
                return View("Status", status);

            model.Id = Guid.NewGuid().ToString();
            _dbContext.Units.Add(model);
            try
            {
                if (await _dbContext.SaveChangesAsync() <= 0)
                    status.Errors.Add("Failed to update Database. There may be connection error. Contact administrator now.");

                status.ModelId = model.Id;
                model.Owner = await _dbContext.Members.AsNoTracking().FirstOrDefaultAsync(m => m.Id == model.OwnerId);
            }
            catch (System.Exception e)
            {
                status.Errors.Add(e.Message);
            }

            return View("Status", status);
        }

        public async Task<IActionResult> Details(string id)
        {
            var unit = await _dbContext.Units.AsNoTracking()
                .Include(m => m.Owner)
                    .ThenInclude(m => m.Account)
                .FirstOrDefaultAsync(m => m.Id == id);
            
            return View(unit);
        }
    }
}
