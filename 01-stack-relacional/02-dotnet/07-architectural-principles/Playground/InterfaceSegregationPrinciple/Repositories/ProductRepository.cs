using DependencyInversionPrinciple.Contracts;
using DependencyInversionPrinciple.Entities;
using DependencyInversionPrinciple.Exceptions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DependencyInversionPrinciple.Repositories
{
    internal class ProductRepository : IProductRepository
    {
        private List<Product> _products = new();

        public Task AddProduct(Product product)
        {
            _products.Add(product);
            return Task.CompletedTask;
        }

        public Task EditProduct(Product product)
        {
            var existingProduct = _products.SingleOrDefault(p => p.Id == product.Id);
            if (existingProduct is null)
            {
                throw new EntityNotFoundException($"No se encontró el producto con Id {product.Id}.");
            }
            
            existingProduct.Name = product.Name;
            existingProduct.Description = product.Description;
            existingProduct.Price = product.Price;
            return Task.CompletedTask;
        }

        public Task DeleteProduct(Guid productId)
        {
            var product = _products.SingleOrDefault(p => p.Id == productId);
            if (product is null)
            {
                throw new EntityNotFoundException($"No se encontró el producto con Id {productId}.");
            }
            _products.Remove(product);
            return Task.CompletedTask;
        }

        public Task<IEnumerable<Product>> GetAllProducts()
        {
            return Task.FromResult((IEnumerable<Product>)_products);
        }

        public Task<Product?> GetProduct(Guid productId)
        {
            return Task.FromResult(_products.SingleOrDefault(p => p.Id == productId));
        }
    }
}
