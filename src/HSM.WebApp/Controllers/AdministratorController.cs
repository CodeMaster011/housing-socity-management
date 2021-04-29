using System.Linq;
using System.Threading.Tasks;
using HSM.WebApp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HSM.WebApp.Controllers
{
    public class AdministratorController : Controller
    {
        private readonly ILogger<AdministratorController> _logger;
        private readonly HsmDbContext _dbContext;

        public AdministratorController(ILogger<AdministratorController> logger, HsmDbContext dbContext)
        {
            _logger = logger;
            this._dbContext = dbContext;
        }

        public IActionResult Index()
        {
            // var model = await _dbContext.Transactions
            // .AsNoTracking()
            //     .Include(t => t.Unit)
            //     .Include(t => t.Account)
            //         .ThenInclude(t => t.Member)
            //     .OrderByDescending(m => m.Date)
            //     .Skip(fromNum)
            //     .Take(100)
            // .ToArrayAsync();
            
            return View();
        }
    }
}
