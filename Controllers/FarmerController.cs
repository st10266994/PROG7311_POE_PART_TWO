using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PROG7311_PART_TWO.Data;
using PROG7311_PART_TWO.Models;
using System.Security.Claims;

namespace PROG7311_PART_TWO.Controllers

{
    [Authorize(Roles = "Farmer")]
    public class FarmerController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly SignInManager<IdentityUser> _signInManager;

        public FarmerController(ApplicationDbContext context, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager,
            IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Farmer/Products
        public async Task<IActionResult> ProductIndex()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var products = await _context.Products
                .Include(p => p.User)
                .Where(p => p.UserId == userId)
                .ToListAsync();

            return View(products);
        }

        // GET: Farmer/CreateProduct
        public IActionResult CreateProduct()
        {
            return View();
        }

        // POST: Farmer/CreateProduct
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProduct(Product product, Microsoft.AspNetCore.Http.IFormFile imageFile)
        {
            try
            {
                ModelState.Remove("UserId");
                ModelState.Remove("User");
                ModelState.Remove("ImageUrlPath");

                if (!ModelState.IsValid)
                {
                    return View(product);
                }

                product.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (imageFile != null && imageFile.Length > 0)
                {
                    var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "products");
                    if (!Directory.Exists(uploadsFolder))
                        Directory.CreateDirectory(uploadsFolder);

                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(fileStream);
                    }

                    product.ImageUrlPath = "/images/products/" + uniqueFileName;
                }
                else
                {
                    product.ImageUrlPath = "/images/products/default-product.jpg";
                }

                _context.Products.Add(product);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Product created successfully!";
                return RedirectToAction(nameof(ProductIndex));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error creating product: {ex.Message}");
                return View(product);
            }
        }
        // GET: Farmer/EditProduct/5
        public async Task<IActionResult> EditProduct(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var product = await _context.Products
                .Where(p => p.ProductId == id && p.UserId == userId)
                .FirstOrDefaultAsync();

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Farmer/EditProduct/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProduct(int id, Product product, Microsoft.AspNetCore.Http.IFormFile imageFile)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (product.UserId != userId)
            {
                return Unauthorized();
            }

            ModelState.Remove("User");
            ModelState.Remove("imageFile");

            if (ModelState.IsValid)
            {
                try
                {
                    var existingProduct = await _context.Products.FindAsync(id);
                    if (existingProduct == null)
                    {
                        return NotFound();
                    }

                    existingProduct.Name = product.Name;
                    existingProduct.Description = product.Description;
                    existingProduct.Category = product.Category;
                    existingProduct.ProductionDate = product.ProductionDate;

                    if (imageFile != null && imageFile.Length > 0)
                    {
                        var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "products");
                        if (!Directory.Exists(uploadsFolder))
                            Directory.CreateDirectory(uploadsFolder);

                        var uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
                        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await imageFile.CopyToAsync(fileStream);
                        }

                        if (!string.IsNullOrEmpty(existingProduct.ImageUrlPath) &&
                            !existingProduct.ImageUrlPath.EndsWith("default-product.jpg"))
                        {
                            var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath,
                                existingProduct.ImageUrlPath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
                            if (System.IO.File.Exists(oldImagePath))
                            {
                                System.IO.File.Delete(oldImagePath);
                            }
                        }

                        existingProduct.ImageUrlPath = "/images/products/" + uniqueFileName;
                    }

                    _context.Update(existingProduct);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Product updated successfully!";
                    return RedirectToAction(nameof(ProductIndex));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, $"Error updating product: {ex.Message}");
                }
            }

            return View(product);
        }
        // GET: Farmer/DeleteProduct/5
        public async Task<IActionResult> DeleteProduct(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var product = await _context.Products
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.ProductId == id && m.UserId == userId);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Farmer/DeleteProduct/5
        [HttpPost, ActionName("DeleteProduct")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteProductConfirmed(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.ProductId == id && m.UserId == userId);

            if (product == null)
            {
                return NotFound();
            }

            // Delete product image if exists
            if (!string.IsNullOrEmpty(product.ImageUrlPath))
            {
                var imagePath = Path.Combine(_webHostEnvironment.WebRootPath,
                    product.ImageUrlPath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ProductIndex));
        }

        // GET: Farmer/Profile
        public async Task<IActionResult> Profile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _context.ApplicationUsers.FindAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            if (TempData["PasswordChangeSuccess"] != null)
            {
                ViewBag.PasswordChangeSuccess = TempData["PasswordChangeSuccess"].ToString();
            }
            if (TempData["PasswordChangeError"] != null)
            {
                ViewBag.PasswordChangeError = TempData["PasswordChangeError"].ToString();
            }

            return View(user);
        }

        // POST: Farmer/UpdateProfile
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProfile(ApplicationUser user)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var currentUser = await _context.ApplicationUsers.FindAsync(userId);

            if (currentUser == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                currentUser.Firstname = user.Firstname;
                currentUser.Lastname = user.Lastname;
                currentUser.PhoneNumber = user.PhoneNumber;
                currentUser.Location = user.Location;


                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Profile));
            }

            return View("Profile", user);
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ProductId == id);
        }

        // POST: Farmer/UpdatePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdatePassword(string newPassword, string confirmPassword)
        {
            if (string.IsNullOrEmpty(newPassword) || string.IsNullOrEmpty(confirmPassword))
            {
                TempData["PasswordChangeError"] = "All fields are required.";
                return RedirectToAction(nameof(Profile));
            }

            if (newPassword != confirmPassword)
            {
                TempData["PasswordChangeError"] = "The new password and confirmation password do not match.";
                return RedirectToAction(nameof(Profile));
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, newPassword);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                TempData["PasswordChangeError"] = $"Failed to update password: {errors}";
                return RedirectToAction(nameof(Profile));
            }

            await _signInManager.SignInAsync(user, isPersistent: false);

            TempData["PasswordChangeSuccess"] = "Your password has been updated successfully.";
            return RedirectToAction(nameof(Profile));
        }
    }
}
