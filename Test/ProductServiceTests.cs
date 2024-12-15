using Application.Contracts;
using Application.Services;
using AutoMapper;
using DTOs;
using Models;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Test
{
    public class ProductServiceTests
    {
        private readonly ProductService _service;
        private readonly Mock<IProductRepository> _mockRepository;
        private readonly Mock<IMapper> _mockMapper;

        public ProductServiceTests()
        {
            _mockRepository = new Mock<IProductRepository>();
            _mockMapper = new Mock<IMapper>();

            _service = new ProductService(_mockRepository.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task GetAllProductsAsync_ShouldReturnProducts()
        {
            var products = new List<Product>
            {
                new Product { Id = 1, Name = "Product 1", Price = 100 },
                new Product { Id = 2, Name = "Product 2", Price = 200 }
            };

            _mockRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(products);

            _mockMapper
                .Setup(mapper => mapper.Map<IEnumerable<ProductDTO>>(It.IsAny<List<Product>>()))
                .Returns(new List<ProductDTO>
                {
                    new ProductDTO { Name = "Product 1", Price = 100 },
                    new ProductDTO { Name = "Product 2", Price = 200 }
                });

            var result = await _service.GetAllProductsAsync();

            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task AddProductAsync_ShouldCallRepository()
        {
            var productDto = new ProductDTO
            {
                Name = "New Product",
                Price = 150
            };

            var product = new Product
            {
                Name = "New Product",
                Price = 150
            };

            _mockMapper.Setup(mapper => mapper.Map<Product>(productDto)).Returns(product);

            _mockRepository.Setup(repo => repo.AddAsync(It.IsAny<Product>())).Returns(Task.CompletedTask);

            await _service.AddProductAsync(productDto);

            _mockRepository.Verify(repo => repo.AddAsync(It.IsAny<Product>()), Times.Once);
        }
    }
}
