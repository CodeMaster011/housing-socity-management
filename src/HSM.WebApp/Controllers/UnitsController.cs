using System.Linq;
using System.Threading.Tasks;
using HSM.WebApp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HSM.WebApp.Controllers
{
    public class UnitsController : Controller
    {
        private readonly ILogger<UnitsController> _logger;
        private readonly HsmDbContext _dbContext;

        public UnitsController(ILogger<UnitsController> logger, HsmDbContext dbContext)
        {
            _logger = logger;
            this._dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            var model = await _dbContext.Units
            .AsNoTracking()
                .OrderBy(m => m.Name)
            .ToArrayAsync();
            
            return View(model);
        }

    }
}
