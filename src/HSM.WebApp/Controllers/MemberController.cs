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
        public async Task<IActionResult> Create([FromForm]Member model)
        {
            var status = new MemberCreationStatusModel
            {
                Member = model,
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
            _dbContext.Members.Add(model);
            try
            {
                if(await _dbContext.SaveChangesAsync() <= 0)
                    status.Errors.Add("Failed to update Database. There may be connection error. Contact administrator now.");
                
                status.MemberId = model.Id;
            }
            catch (System.Exception e)
            {
                status.Errors.Add(e.Message);
            }

            return View("Status", status);
        }

        public async Task<IActionResult> Details(string id)
        {
            var member = await _dbContext.Members.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);
            return View(member);
        }
    }
}
