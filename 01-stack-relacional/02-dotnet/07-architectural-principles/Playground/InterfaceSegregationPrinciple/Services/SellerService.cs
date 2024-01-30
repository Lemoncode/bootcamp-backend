using DependencyInversionPrinciple.Contracts;
using DependencyInversionPrinciple.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DependencyInversionPrinciple.Services
{
    internal class SellerService
    {

        private readonly IProductRepository _productRepository;

        public SellerService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public Task AddProduct(Product product) => _productRepository.AddProduct(product);
        
        public Task EditProduct(Product product)=> _productRepository.EditProduct(product);
        
        public Task DeleteProduct(Guid productId) => _productRepository.DeleteProduct(productId);

    }
}
