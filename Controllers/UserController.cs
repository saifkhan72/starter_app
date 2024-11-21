using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Starter_app.Data;
using Starter_app.Models;

namespace Starter_app.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _db;
        public UserController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, ApplicationDbContext db)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _db = db;
        }
        public IActionResult Index()
        {
            return View();
        }

        #region API CALLS
        [HttpGet]
        public IActionResult Users()
        {
            var user = _db.ApplicationUser.ToList();
            return Json(user);
        }
        [HttpGet]
        public IActionResult GetUsers()
        {
            // Get query parameters sent by DataTables
            var draw = HttpContext.Request.Query["draw"].FirstOrDefault();
            var start = HttpContext.Request.Query["start"].FirstOrDefault();
            var length = HttpContext.Request.Query["length"].FirstOrDefault();
            var sortColumnIndex = HttpContext.Request.Query["order[0][column]"].FirstOrDefault();
            var sortColumnDirection = HttpContext.Request.Query["order[0][dir]"].FirstOrDefault();
            var searchValue = HttpContext.Request.Query["search[value]"].FirstOrDefault();

            // Paging parameters
            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;

            // Query the database and include the related CurrencyModel
            var users = _db.ApplicationUser.ToList();
            Console.WriteLine($"Users Count: {users.Count}");


            // Total number of records before filtering
            var recordsTotal = users.Count();

            // Paging
            var data = users.Skip(skip).Take(pageSize).ToList();

            // Returning data in JSON format with draw, recordsTotal, recordsFiltered, and actual data
            return Json(new
            {
                draw = draw,
                recordsFiltered = recordsTotal,
                recordsTotal = recordsTotal,
                data = data.Select(e => new
                {
                    e.Id,
                    e.Name,
                    e.UserName,
                    e.Email,
                }).ToList()
            });
        }
        #endregion
    }
}
