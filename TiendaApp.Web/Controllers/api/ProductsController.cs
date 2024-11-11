using Microsoft.AspNetCore.Mvc;
using TiendaApp.Core.Entities;
using TiendaApp.Core.Interfaces;

namespace TiendaApp.Web.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(IProductRepository productRepository, ILogger<ProductsController> logger)
        {
            _productRepository = productRepository;
            _logger = logger;
        }

        // GET: api/products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            var products = await _productRepository.GetAllProductsAsync();
            return Ok(products);
        }

        // POST: api/products/{id}/purchase
        [HttpPost("{id}/purchase")]
        public async Task<IActionResult> PurchaseProduct(int id)
        {
            try
            {
                var product = await _productRepository.GetProductByIdAsync(id);

                if (product == null)
                    return NotFound();

                if (product.Stock <= 0)
                    return BadRequest(new { message = "Product is out of stock" });

                product.Stock -= 1;
                await _productRepository.UpdateProductAsync(product);

                return Ok(new
                {
                    message = "Purchase successful",
                    newStock = product.Stock
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error purchasing product {ProductId}", id);
                return StatusCode(500, new { message = "An error occurred while processing your purchase" });
            }
        }
    }
}