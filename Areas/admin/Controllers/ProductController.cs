using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System.Reflection.Metadata.Ecma335;
using Wolmart.MVC.DAL;
using Wolmart.MVC.Extension;
using Wolmart.MVC.Interface;
using Wolmart.MVC.Models;
using Wolmart.MVC.ViewModels;

namespace Wolmart.MVC.Areas.admin.Controllers
{
    [Area("admin")]
    [Authorize(Roles = "Admin")]
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly IEmailRepository _emailRepository;

        public ProductController(AppDbContext context, IWebHostEnvironment env,IEmailRepository emailRepository)
        {
            _context = context;
            _env = env;
            _emailRepository = emailRepository;
        }
        public IActionResult Index(int page)
        {
            IQueryable<Product> product = _context.Products.OrderByDescending(x => x.ID);

            return View(PagiNationList<Product>.Create(product, page, 5));
        }

        public async Task<IActionResult> GetLabelCount()
        {
            ViewBag.Colors = await _context.Colors.ToListAsync();

            return PartialView("_ProductColorPartial");
        }

        public async Task<IActionResult> GetSizeCount()
        {
            ViewBag.Sizes = await _context.Sizes.ToListAsync();

            return PartialView("_ProductSizePartial");
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Brands = await _context.Brands.ToListAsync();
            ViewBag.Categories = await _context.Categories.ToListAsync();
            ViewBag.Colors = await _context.Colors.ToListAsync();
            ViewBag.Sizes = await _context.Sizes.ToListAsync();
            ViewBag.Description = await _context.ProductDescriptions.Select(x => x.Text).ToListAsync();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            ViewBag.Brands = await _context.Brands.ToListAsync();
            ViewBag.Categories = await _context.Categories.ToListAsync();
            ViewBag.Colors = await _context.Colors.ToListAsync();
            ViewBag.Sizes = await _context.Sizes.ToListAsync();
            ViewBag.Description = await _context.ProductDescriptions.ToListAsync();

            foreach (var sizeCount in product.SizeCounts)
            {
                if (sizeCount == null || sizeCount <= 0)
                {
                    ModelState.AddModelError("SizeCounts", "Count must be a positive number!");
                }
            }

            foreach (var price in product.Prices)
            {
                if (price == null || price <= 0)
                {
                    ModelState.AddModelError("Prices", "Price must be a positive number!");
                }
            }

            foreach (var count in product.Counts)
            {
                if (count == null || count <= 0)
                {
                    ModelState.AddModelError("Counts", "Count must be a positive number!");
                }
            }

            if (!ModelState.IsValid)
            {
                return View();
            }


            if (!await _context.Brands.AnyAsync(x => x.ID == product.BrandID))
            {
                ModelState.AddModelError("BrandID", "Select a correct brand");

                return View();
            }

            if (product?.CategoryID == null)
            {
                ModelState.AddModelError("CategoryID", "Select a correct category");

                return View();
            }

            if (!await _context.Categories.AnyAsync(x => x.ID == product.CategoryID))
            {
                ModelState.AddModelError("CategoryID", "Select a correct category");

                return View();
            }

            if (product.ColorIDs.Count() != product.Counts.Count())
            {
                ModelState.AddModelError("", "Select a correct list");

                return View();
            }

            List<ProductSize> productSizes = new List<ProductSize>();

            for (int i = 0; i < product.SizeIDs.Count(); i++)
            {
                if (!await _context.Sizes.AnyAsync(x => x.ID == product.SizeIDs[i]))
                {
                    ModelState.AddModelError("", $"This size id {product.SizeIDs[i]} is incorrect`");

                    return View();
                }

                if (product.SizeCounts[i] <= 0)
                {
                    ModelState.AddModelError("", $"Count is incorrect");

                    return View();
                }

                ProductSize productSize = new ProductSize
                {
                    SizeID = product.SizeIDs[i],
                    Count = (int)product.SizeCounts[i],
                    CreatedAt = DateTime.Now
                };

                productSizes.Add(productSize);
            }

            product.ProductSizes = productSizes;

            List<ProductColor> productColors = new List<ProductColor>();

            for (int i = 0; i < product.ColorIDs.Count(); i++)
            {
                if (!await _context.Colors.AnyAsync(c => c.ID == product.ColorIDs[i]))
                {
                    ModelState.AddModelError("", $"This color id {product.ColorIDs[i]} is incorrect`");

                    return View();
                }

                if (product.Counts[i] <= 0)
                {
                    ModelState.AddModelError("", $"Count is incorrect");

                    return View();
                }


                if (product.Prices[i] <= 0)
                {
                    ModelState.AddModelError("", $"Price is incorrect");

                    return View();
                }

                if (product.DiscountedPrices[i] <= 0)
                {
                    ModelState.AddModelError("", $"Discounted price is incorrect");

                    return View();
                }

                ProductColor productColor = new ProductColor
                {
                    ColorID = product.ColorIDs[i],
                    Count = (int)product.Counts[i],
                    Price = (int)product.Prices[i],
                    DiscountedPrice = product.DiscountedPrices[i],
                    CreatedAt = DateTime.Now
                };

                productColors.Add(productColor);
            }
            product.ProductColors = productColors;

            if (product.Counts.Count() == 0)
            {
                ModelState.AddModelError("Count", "Count must be higher than 0!");

                return View();
            }


            if (product.Files != null && product.Files.Count() > 4)
            {
                ModelState.AddModelError("Files ", "You can select a maximum 5 images");

                return View();
            }


            if (product.MainFile != null)
            {
                if (!product.MainFile.CheckFileType())
                {
                    ModelState.AddModelError("File", "File type must be jpg or png.");
                    return View(product);
                }


                if (!product.MainFile.CheckFileSize(20000))
                {
                    ModelState.AddModelError("MainFile", "The maximum size must be 20mb!");
                    return View(product);
                }
                product.MainImage = product.MainFile.CreateImage(_env, "assets", "images", "products");
            }
            else
            {
                ModelState.AddModelError("MainFile", "Images is required");
                return View();
            }
            if (product.HoverFile != null)
            {
                if (!product.HoverFile.CheckFileType())
                {
                    ModelState.AddModelError("HoverFile", "File type must be jpg or png.");
                    return View(product);
                }


                if (!product.HoverFile.CheckFileSize(20000))
                {
                    ModelState.AddModelError("HoverFile", "The maximum size must be 20mb!");
                    return View(product);
                }
                product.HoverImage = product.HoverFile.CreateImage(_env, "assets", "images", "products");
            }
            else
            {
                ModelState.AddModelError("HoverFile", "Images is required");
                return View();
            }

            if (product.Files != null && product.Files.Count() > 0)
            {
                List<ProductImage> productImages = new List<ProductImage>();
                foreach (IFormFile file in product.Files)
                {
                    if (!file.CheckFileType())
                    {
                        ModelState.AddModelError("Files", "File type must be jpg or png.");
                        return View(product);
                    }

                    if (!file.CheckFileSize(20000))
                    {
                        ModelState.AddModelError("Files", "The maximum size must be 20mb!");
                        return View();
                    }

                    ProductImage productImage = new ProductImage
                    {
                        Image = file.CreateImage(_env, "assets", "images", "products"),
                        CreatedAt = DateTime.Now
                    };

                    productImages.Add(productImage);
                }
                product.ProductImages = productImages;
            }

            if (product.ProductDescriptions != null)
            {
                List<ProductDescription> productDescriptions = new List<ProductDescription>();
                for (int i = 0; i < product.ProductDescriptions.Count; i++)
                {
                    ProductDescription ProductDescriptions = new ProductDescription
                    {
                        Text = product.ProductDescriptions[0].Text,
                        CreatedAt = DateTime.Now
                    };
                    productDescriptions.Add(ProductDescriptions);
                }
                product.ProductDescriptions = productDescriptions;
            }

            product.CreatedAt = DateTime.Now;
            product.Name = product.Name.Trim();
            product.Count = productColors.Sum(x => x.Count) + productSizes.Sum(x => x.Count);

            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            await _emailRepository.GenerateLinkForNewProduct(product);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Update(int? ID)
        {
            ViewBag.Brands = await _context.Brands.ToListAsync();
            ViewBag.Categories = await _context.Categories.ToListAsync();
            ViewBag.Colors = await _context.Colors.ToListAsync();
            ViewBag.Sizes = await _context.Sizes.ToListAsync();

            if (ID == null) return BadRequest();

            Product product = await _context.Products
                .Include(x => x.Brand)
                .Include(x => x.Category)
                .Include(x => x.ProductImages)
                .Include(x => x.ProductColors)
                .ThenInclude(x=>x.Color)
                .Include(x => x.ProductSizes)
                .Include(x => x.ProductSpecifications)
                .Include(x => x.ProductDescriptions)
                .FirstOrDefaultAsync(x => x.ID == ID);

            product.ColorIDs = await _context.ProductColors.Where(x => x.ProductID == ID).Select(x => x.ColorID).ToListAsync();
            
            if (product == null) return NotFound();

            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? ID, Product product)
        {
            ViewBag.Brands = await _context.Brands.ToListAsync();
            ViewBag.Categories = await _context.Categories.ToListAsync();
            ViewBag.Colors = await _context.Colors.ToListAsync();
            ViewBag.Sizes = await _context.Sizes.ToListAsync();
            ViewBag.Description = await _context.ProductDescriptions.Select(x => x.Text).ToListAsync();

            if (ID == null) return BadRequest();

            Product products = await _context.Products
                .Include(x => x.Brand)
                .Include(x => x.Category)
                .Include(x => x.ProductImages)
                .Include(x => x.ProductColors)
                .ThenInclude(x=>x.Color)
                .Include(x => x.ProductSizes)
                .ThenInclude(x=>x.Size)
                .Include(x => x.ProductSpecifications)
                .Include(x => x.ProductDescriptions)
                .FirstOrDefaultAsync(x => x.ID == ID);

            if (products == null) return NotFound();

            if (!await _context.Brands.AnyAsync(x=>x.ID == product.BrandID))
            {
                ModelState.AddModelError("BrandID", "Select a correct brand");

                return View();
            }

            if (product?.CategoryID == null)
            {
                ModelState.AddModelError("BrandID", "Select a correct brand");

                return View();
            }

            if (!await _context.Categories.AnyAsync(x => x.ID == product.CategoryID))
            {
                ModelState.AddModelError("CategoryID", "Select a correct category");

                return View();
            }

            if (product.ColorIDs.Count() != product.Counts.Count())
            {
                ModelState.AddModelError("", "Select a correct list");

                return View();
            }

            List<ProductSize> productSizes = new List<ProductSize>();

            if (product.SizeIDs != null)
            {
                for (int i = 0; i < product.SizeIDs.Count(); i++)
                {
                    try
                    {
                        if (!await _context.Sizes.AnyAsync(x => x.ID == product.SizeIDs[i]))
                        {
                            ModelState.AddModelError("", $"This size id {product.SizeIDs[i]} is incorrect");
                            return View();
                        }

                        if (product.SizeCounts[i] <= 0)
                        {
                            ModelState.AddModelError("", $"Count is incorrect");
                            return View();
                        }

                        ProductSize productSize = new ProductSize
                        {
                            SizeID = product.SizeIDs[i],
                            Count = (int)product.SizeCounts[i],
                            UpdatedAt = DateTime.Now
                        };

                        productSizes.Add(productSize);
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", $"An error occurred: {ex.Message}");
                        return View();
                    }
                }

                products.ProductSizes = productSizes;
            }

            List<ProductColor> productColors = new List<ProductColor>();

            if (product.ColorIDs != null)
            {
                for (int i = 0; i < product.ColorIDs.Count(); i++)
                {
                    try
                    {
                        if (!await _context.Colors.AnyAsync(x => x.ID == product.ColorIDs[i]))
                        {
                            ModelState.AddModelError("", $"This color id {product.ColorIDs[i]} is incorrect");
                            return View();
                        }

                        if (product.Counts[i] <= 0)
                        {
                            ModelState.AddModelError("", $"Count is incorrect");
                            return View();
                        }

                        ProductColor productColor = new ProductColor
                        {
                            ColorID = product.ColorIDs[i],
                            Price = (int)product.Prices[i],
                            DiscountedPrice = product.DiscountedPrices?[i],
                            Count = (int)product.Counts[i],
                            UpdatedAt = DateTime.Now
                        };

                        productColors.Add(productColor);
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", $"An error occurred: {ex.Message}");
                        return View();
                    }
                }

                products.ProductColors = productColors;
            }

            if (product.Counts.Count() == 0)
            {
                ModelState.AddModelError("Count", "Count must be higher than 0!");

                return View();
            }

            if (product.Files != null && product.Files.Count() > 4)
            {
                ModelState.AddModelError("Files ", "You can select a maximum 5 images");

                return View();
            }

            if (product.MainFile != null)
            {
                if (!product.MainFile.CheckFileType())
                {
                    ModelState.AddModelError("File", "File type must be jpg or png.");
                    return View(product);
                }


                if (!product.MainFile.CheckFileSize(20000))
                {
                    ModelState.AddModelError("MainFile", "The maximum size must be 20mb!");
                    return View(product);
                }
                products.MainImage = product.MainFile.CreateImage(_env, "assets", "images", "products");
            }

            if (product.HoverFile != null)
            {
                if (!product.HoverFile.CheckFileType())
                {
                    ModelState.AddModelError("HoverFile", "File type must be jpg or png.");
                    return View(product);
                }


                if (!product.HoverFile.CheckFileSize(20000))
                {
                    ModelState.AddModelError("HoverFile", "The maximum size must be 20mb!");
                    return View(product);
                }
                products.HoverImage = product.HoverFile.CreateImage(_env, "assets", "images", "products");
            }

            if (product.Files != null && product.Files.Count() > 0)
            {
                List<ProductImage> productImages = new List<ProductImage>();
                foreach (IFormFile file in product.Files)
                {
                    if (!file.CheckFileType())
                    {
                        ModelState.AddModelError("Files", "File type must be jpg or png.");
                        return View(product);
                    }

                    if (!file.CheckFileSize(20000))
                    {
                        ModelState.AddModelError("Files", "The maximum size must be 20mb!");
                        return View();
                    }

                    ProductImage productImage = new ProductImage
                    {
                        Image = file.CreateImage(_env, "assets", "images", "products"),
                        CreatedAt = DateTime.Now
                    };

                    productImages.Add(productImage);
                }
                products.ProductImages = productImages;
            }

            if (product.ProductDescriptions != null)
            {
                _context.ProductDescriptions.RemoveRange(products.ProductDescriptions);

                List<ProductDescription> productDescriptions = new List<ProductDescription>();

                for (int i = 0; i < product.ProductDescriptions.Count; i++)
                {
                    if (product.ProductDescriptions[i].Text != null)
                    {
                        ProductDescription productDescription = new ProductDescription
                        {
                            Text = product.ProductDescriptions[i].Text,
                            UpdatedAt = DateTime.Now
                        };
                        productDescriptions.Add(productDescription);
                    }
                }
                product.ProductDescriptions = productDescriptions;
                products.ProductDescriptions = product.ProductDescriptions;
            }

            products.UpdatedAt = DateTime.Now;
            products.Name = product.Name.Trim();
            products.CategoryID = product.CategoryID;
            products.BrandID = product.BrandID;
            products.ProductSpecifications[0].Model = product.ProductSpecifications[0].Model.Trim();
            products.ProductSpecifications[0].Guarantee = product.ProductSpecifications[0].Guarantee.Trim();
            products.Count = productColors.Sum(x => x.Count) + productSizes.Sum(x => x.Count);

            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int? ID,int page)
        {
            IQueryable<Product> products = _context.Products.OrderByDescending(x => x.ID == ID);

            if (ID == null) return BadRequest();

            Product product = await _context.Products.FirstOrDefaultAsync(x => x.ID == ID);

            if (product == null) return NotFound();

            _context.Products.Remove(product);

            await _context.SaveChangesAsync();

            return PartialView("_ProductIndexPartial", PagiNationList<Product>.Create(products, page, 5));
        }

        [HttpGet]
        public async Task<IActionResult> RemoveImg(int? ID)
        {
            if (ID == null) return BadRequest();

            ProductImage productImage = await _context.ProductImages.Include(x => x.Product).FirstOrDefaultAsync(x => x.ID == ID);

            if (productImage == null) return NotFound();

            Product product = await _context.Products.Include(x => x.ProductImages).FirstOrDefaultAsync(x => x.ID == productImage.ProductID);

            _context.ProductImages.Remove(productImage);

            await _context.SaveChangesAsync();

            Helpers.FileHelper.DeleteFile(_env, productImage.Image);

            return PartialView("_ProductImagePartial", product.ProductImages);
        }

        public async Task<IActionResult> Read(int? ID)
        {
            ViewBag.Brands = await _context.Brands.ToListAsync();
            ViewBag.Categories = await _context.Categories.ToListAsync();
            ViewBag.Colors = await _context.Colors.ToListAsync();
            ViewBag.Sizes = await _context.Sizes.ToListAsync();
            ViewBag.Description = await _context.ProductDescriptions.Select(x => x.Text).ToListAsync();

            if (ID == null) return BadRequest();

            Product products = await _context.Products
                .Include(x => x.Brand)
                .Include(x => x.Category)
                .Include(x => x.ProductImages)
                .Include(x => x.ProductColors)
                .ThenInclude(x => x.Color)
                .Include(x => x.ProductSizes)
                .ThenInclude(x => x.Size)
                .Include(x => x.ProductSpecifications)
                .Include(x => x.ProductDescriptions)
                .FirstOrDefaultAsync(x => x.ID == ID);

            if (products == null) return NotFound();

            return View(products);
        }
    }
}
