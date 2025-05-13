using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PROG7311_PART_TWO.Data;
using PROG7311_PART_TWO.Models;

namespace PROG7311_PART_TWO.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminController(ApplicationDbContext context, UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // GET: Admin/ManageEmployees
        public async Task<IActionResult> ManageEmployees()
        {
            var employeeRoleId = await _roleManager.FindByNameAsync("Employee");
            if (employeeRoleId == null)
                return View(new List<ApplicationUser>());

            var employeesQuery = from user in _context.ApplicationUsers
                                 join userRole in _context.UserRoles on user.Id equals userRole.UserId
                                 where userRole.RoleId == employeeRoleId.Id
                                 select user;

            var employees = await employeesQuery.ToListAsync();
            return View(employees);
        }

        // GET: Admin/RegisterEmployee
        public IActionResult RegisterEmployee()
        {
            return View();
        }

        // POST: Admin/RegisterEmployee
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterEmployee(RegisterEmployeeViewModel model)
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
                    Location = "N/A"
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Employee");
                    return RedirectToAction(nameof(ManageEmployees));
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }
        // GET: Admin/EditEmployee/5
        public async Task<IActionResult> EditEmployee(string id)
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

            // Check if the user is actually an employee
            var isInRole = await _userManager.IsInRoleAsync(user, "Employee");
            if (!isInRole)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Admin/EditEmployee/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditEmployee(string id, ApplicationUser model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            var user = await _context.ApplicationUsers.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            
                    user.Firstname = model.Firstname;
                    user.Lastname = model.Lastname;
                    user.PhoneNumber = model.PhoneNumber;

                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(ManageEmployees));
               
        }

        // GET: Admin/DeleteEmployee/5
        public async Task<IActionResult> DeleteEmployee(string id)
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

            // Check if the user is actually an employee
            var isInRole = await _userManager.IsInRoleAsync(user, "Employee");
            if (!isInRole)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Admin/DeleteEmployee/5
        [HttpPost, ActionName("DeleteEmployee")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteEmployeeConfirmed(string id)
        {
            var user = await _context.ApplicationUsers.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            // Check if the user is actually an employee
            var isInRole = await _userManager.IsInRoleAsync(user, "Employee");
            if (!isInRole)
            {
                return NotFound();
            }

            // Delete the user
            await _userManager.DeleteAsync(user);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(ManageEmployees));
        }
    }
}