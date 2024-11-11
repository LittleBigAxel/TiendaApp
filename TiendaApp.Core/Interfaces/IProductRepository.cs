using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiendaApp.Core.Entities;

namespace TiendaApp.Core.Interfaces
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAllProductsAsync();
        Task<Product> GetProductByIdAsync(int id);
        Task AddProductAsync(Product product);
        Task UpdateProductAsync(Product product);
        Task DeleteProductAsync(int id);
    }
}
