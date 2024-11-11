using Microsoft.AspNetCore.Mvc;
using TiendaApp.Core.Entities;
using TiendaApp.Core.Interfaces;
using TiendaApp.Infrastructure.Data;
using Microsoft.Extensions.Logging;

public class ProductsController : Controller
{
    private readonly IProductRepository _productRepository;
    private readonly ILogger<ProductsController> _logger;

    public ProductsController(IProductRepository productRepository, ILogger<ProductsController> logger)
    {
        _productRepository = productRepository;
        _logger = logger;
    }

    // GET: Products
    public async Task<IActionResult> Index()
    {
        var products = await _productRepository.GetAllProductsAsync();
        return View(products);
    }

    // GET: Products/Details/
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var product = await _productRepository.GetProductByIdAsync(id.Value);
        if (product == null)
        {
            return NotFound();
        }

        return View(product);
    }

    // GET: Products/Create
    public IActionResult Create()
    {
        return View(new Product());
    }

    // POST: Products/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Name,ImageURL,Category,Price,Stock")] Product product)
    {
        _logger.LogInformation("Attempting to create product: {@Product}", product);

        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Invalid model state when creating product. Errors: {Errors}",
                string.Join("; ", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)));

            return View(product);
        }

        try
        {
            await _productRepository.AddProductAsync(product);
            _logger.LogInformation("Successfully created product with Name: {ProductName}", product.Name);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while creating product: {@Product}", product);
            ModelState.AddModelError("", $"Unable to save product: {ex.Message}");
            return View(product);
        }
    }

    // GET: Products/Edit/
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var product = await _productRepository.GetProductByIdAsync(id.Value);
        if (product == null)
        {
            return NotFound();
        }

        return View(product);
    }

    // POST: Products/Edit/
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Name,ImageURL,Category,Price,Stock")] Product product)
    {
        if (id != product.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            await _productRepository.UpdateProductAsync(product);
            return RedirectToAction(nameof(Index));
        }

        return View(product);
    }

    // GET: Products/Delete/
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var product = await _productRepository.GetProductByIdAsync(id.Value);
        if (product == null)
        {
            return NotFound();
        }

        return View(product);
    }

    // POST: Products/Delete/
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var product = await _productRepository.GetProductByIdAsync(id);
        if (product != null)
        {
            await _productRepository.DeleteProductAsync(id);
        }

        return RedirectToAction(nameof(Index));
    }
}
