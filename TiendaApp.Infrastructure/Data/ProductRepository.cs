using System;
using TiendaApp.Core.Entities;
using TiendaApp.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace TiendaApp.Infrastructure.Data
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ProductRepository> _logger;

        public ProductRepository(ApplicationDbContext context, ILogger<ProductRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<Product>> GetAllProductsAsync()
        {
            try
            {
                return await _context.Products.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all products");
                throw;
            }
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            try
            {
                var product = await _context.Products.FindAsync(id);
                if (product == null)
                {
                    _logger.LogWarning("Product with ID {ProductId} not found", id);
                }
                return product;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving product with ID {ProductId}", id);
                throw;
            }
        }

        public async Task AddProductAsync(Product product)
        {
            try
            {
                if (product == null)
                {
                    throw new ArgumentNullException(nameof(product));
                }

                _logger.LogInformation("Adding new product: {@Product}", product);
                await _context.Products.AddAsync(product);

                var saveResult = await _context.SaveChangesAsync();
                _logger.LogInformation("Successfully added product. SaveChanges result: {SaveResult}", saveResult);

                if (saveResult <= 0)
                {
                    throw new Exception("Failed to save the product to the database.");
                }
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database error while adding product: {@Product}", product);
                throw new Exception("A database error occurred while saving the product.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding product: {@Product}", product);
                throw;
            }
        }

        public async Task UpdateProductAsync(Product product)
        {
            try
            {
                if (product == null)
                {
                    throw new ArgumentNullException(nameof(product));
                }

                _context.Products.Update(product);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating product: {@Product}", product);
                throw;
            }
        }

        public async Task DeleteProductAsync(int id)
        {
            try
            {
                var product = await _context.Products.FindAsync(id);
                if (product != null)
                {
                    _context.Products.Remove(product);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("Product with ID {ProductId} successfully deleted", id);
                }
                else
                {
                    _logger.LogWarning("Attempted to delete non-existent product with ID {ProductId}", id);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting product with ID {ProductId}", id);
                throw;
            }
        }
    }
}
