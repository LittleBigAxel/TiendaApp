using Microsoft.AspNetCore.Mvc;
using TiendaApp.Core.Interfaces;
using System.Net.Http;
using System.Text.Json;

namespace TiendaApp.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<HomeController> _logger;

        public HomeController(
            IProductRepository productRepository,
            IHttpClientFactory httpClientFactory,
            ILogger<HomeController> logger)
        {
            _productRepository = productRepository;
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _productRepository.GetAllProductsAsync();
            return View(products);
        }

        [HttpPost]
        public async Task<IActionResult> Buy(int id)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                var response = await client.PostAsync(
                    $"https://localhost:7025/api/products/{id}/purchase",
                    null);

                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Purchase successful!";
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    TempData["ErrorMessage"] = $"Purchase failed: {error}";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error purchasing product {ProductId}", id);
                TempData["ErrorMessage"] = "An error occurred while processing your purchase.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}