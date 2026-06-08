using StorageApi.DTOs;
using StorageApi.Models;

namespace StorageApi.Mappers
{
    public static class ProductMapper
    {
        public static ProductDto ToProductDto(this Product product)
        {
            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Count = product.Count
            };
        }

        public static Product ToProductFromCreateProductDto(this CreateProductDto createProd)
        {
            return new Product
            {
                Name = createProd.Name,
                Price = createProd.Price,
                Category = createProd.Category,
                Shelf = createProd.Shelf,
                Count = createProd.Count,
                Description = createProd.Description
            };
        }
    }
}