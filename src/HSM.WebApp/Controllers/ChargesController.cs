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
    }
}
