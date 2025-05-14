using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PROG7311_PART_TWO.Data;
using PROG7311_PART_TWO.Models;

namespace PROG7311_PART_TWO.Controllers
{
    //CODE ATTRIBUTION
    // CREATING AND DELETING USERS 
    //AUTHOR: YogiHosting
    //SOURCE: https://www.yogihosting.com/aspnet-core-identity-create-read-update-delete-users/
    //DATE ACCESSED: 1 May 2025

    //CODE ATTRIBUTION
    // QUERY FILTERS 
    //AUTHOR: Microsoft 
    //SOURCE: https://learn.microsoft.com/en-us/ef/core/querying/filters
    //DATE ACCESSED: 4 May 2025
    [Authorize(Roles = "Employee")]
    public class EmployeeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public EmployeeController(ApplicationDbContext context, UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // GET: Employee/Farmers
        public async Task<IActionResult> FarmerIndex()
        {
            var farmerRoleId = await _roleManager.FindByNameAsync("Farmer");
            if (farmerRoleId == null)
                return View(new List<ApplicationUser>());

            var farmersQuery = from user in _context.ApplicationUsers
                               join userRole in _context.UserRoles on user.Id equals userRole.UserId
                               where userRole.RoleId == farmerRoleId.Id
                               select user;

            var farmers = await farmersQuery.ToListAsync();
            return View(farmers);
        }

        // GET: Employee/CreateFarmer
        public IActionResult CreateFarmer()
        {
            return View();
        }

        // POST: Employee/CreateFarmer
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateFarmer(RegisterFarmerViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    Firstname = model.Firstname,
                    Lastname = model.Lastname,
                    PhoneNumber = model.PhoneNumber,
                    Location = model.Location
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Farmer");
                    return RedirectToAction(nameof(FarmerIndex));
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }

        // GET: Employee/DeleteFarmer/5
        public async Task<IActionResult> DeleteFarmer(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.ApplicationUsers.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            // Check if the user is actually a farmer
            var isInRole = await _userManager.IsInRoleAsync(user, "Farmer");
            if (!isInRole)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Employee/DeleteFarmer/5
        [HttpPost, ActionName("DeleteFarmer")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteFarmerConfirmed(string id)
        {
            var user = await _context.ApplicationUsers.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            // Check if the user is actually a farmer
            var isInRole = await _userManager.IsInRoleAsync(user, "Farmer");
            if (!isInRole)
            {
                return NotFound();
            }

            // Delete all products from this farmer
            var products = await _context.Products.Where(p => p.UserId == id).ToListAsync();
            _context.Products.RemoveRange(products);

            // Delete the user
            await _userManager.DeleteAsync(user);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(FarmerIndex));
        }

        // GET: Employee/Products
        public async Task<IActionResult> Products(DateTime? fromDate, DateTime? toDate, string category, string searchString)
        {
            var productsQuery = _context.Products.Include(p => p.User).AsQueryable();

            // Apply date range filter
            if (fromDate.HasValue)
            {
                productsQuery = productsQuery.Where(p => p.ProductionDate >= fromDate.Value);
            }

            if (toDate.HasValue)
            {
                productsQuery = productsQuery.Where(p => p.ProductionDate <= toDate.Value);
            }

            // Apply category filter
            if (!string.IsNullOrEmpty(category))
            {
                productsQuery = productsQuery.Where(p => p.Category == category);
            }

            // Apply search filter
            if (!string.IsNullOrEmpty(searchString))
            {
                productsQuery = productsQuery.Where(p => p.Name.Contains(searchString) || p.Description.Contains(searchString));
            }

            var products = await productsQuery.ToListAsync();

            // Get list of categories for the filter dropdown
            ViewBag.Categories = await _context.Products.Select(p => p.Category).Distinct().ToListAsync();

            ViewBag.FromDate = fromDate;
            ViewBag.ToDate = toDate;
            ViewBag.Category = category;
            ViewBag.SearchString = searchString;

            return View(products);
        }

        // GET: Employee/ProductDetails/5
        public async Task<IActionResult> ProductDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.ProductId == id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        public async Task<IActionResult> FarmerProducts(string id, DateTime? fromDate, DateTime? toDate, string category, string searchString)
        {
            if (id == null)
            {
                return NotFound();
            }

            var farmer = await _context.ApplicationUsers.FindAsync(id);
            if (farmer == null)
            {
                return NotFound();
            }

            // Check if the user is actually a farmer
            var isInRole = await _userManager.IsInRoleAsync(farmer, "Farmer");
            if (!isInRole)
            {
                return NotFound();
            }

            // Start with products from this farmer
            var productsQuery = _context.Products
                .Include(p => p.User)
                .Where(p => p.UserId == id)
                .AsQueryable();

            // Apply date range filter
            if (fromDate.HasValue)
            {
                productsQuery = productsQuery.Where(p => p.ProductionDate >= fromDate.Value);
            }

            if (toDate.HasValue)
            {
                productsQuery = productsQuery.Where(p => p.ProductionDate <= toDate.Value);
            }

            // Apply category filter
            if (!string.IsNullOrEmpty(category))
            {
                productsQuery = productsQuery.Where(p => p.Category == category);
            }

            // Apply search filter
            if (!string.IsNullOrEmpty(searchString))
            {
                productsQuery = productsQuery.Where(p => p.Name.Contains(searchString) || p.Description.Contains(searchString));
            }

            var products = await productsQuery.ToListAsync();

            // Get list of categories for this farmer's products for the filter dropdown
            ViewBag.Categories = await _context.Products
                .Where(p => p.UserId == id)
                .Select(p => p.Category)
                .Distinct()
                .ToListAsync();

            ViewBag.Farmer = farmer;
            ViewBag.FromDate = fromDate;
            ViewBag.ToDate = toDate;
            ViewBag.Category = category;
            ViewBag.SearchString = searchString;

            return View(products);
        }


    }
}