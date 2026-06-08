using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StorageApi.Data;
using StorageApi.DTOs;
using StorageApi.Mappers;
using StorageApi.Models;

namespace StorageApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly StorageContext _context;

        public ProductsController(StorageContext context)
        {
            _context = context;
        }

        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProduct(string? name, string? category)
        {
            var products = _context.Product as IQueryable<Product>;

            if (!string.IsNullOrWhiteSpace(name))
            {
                name = name.Trim();
                products = products.Where(p => p.Name.ToLower() == name.ToLower());
            }

            if (!string.IsNullOrWhiteSpace(category))
            {
                category = category.Trim();
                products = products.Where(p => p.Category.ToLower() == category.ToLower());
            }

            if (!string.IsNullOrWhiteSpace(category) && !string.IsNullOrWhiteSpace(name))
            {
                category = category.Trim();
                products = products.Where(p => p.Category.ToLower() == category.ToLower() && p.Name.ToLower() == name.ToLower());
            }

            if (products.IsNullOrEmpty())
            {
                return NotFound();
            }

            var prodsToReturn = await products.OrderBy(p => p.Name).ToListAsync();
            return Ok(prodsToReturn);
        }

        [HttpGet("stats")]
        public async Task<ActionResult> GetStats()
        {
            var allProducts = await _context.Product.Select(p => p.ToProductDto()).ToListAsync();
            var totalCountOfProducts = allProducts.Count();
            int totalInventoryValue = allProducts.Sum(p => p.InventoryValue);
            var averagePrice = totalInventoryValue / totalCountOfProducts;
            var response = new StatsDto
            {
                TotalCount = totalCountOfProducts,
                TotalInventoryValue = totalInventoryValue,
                AvergePrice = averagePrice
            };
            return Ok(response);
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetProduct(int id)
        {
            var product = await _context.Product.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return product.ToProductDto();
        }

        // PUT: api/Products/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, Product product)
        {
            if (id != product.Id)
            {
                return BadRequest();
            }

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Products
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(CreateProductDto product)
        {
            var prodToSave = new Product
            {
                Name = product.Name,
                Price = product.Price,
                Category = product.Category,
                Shelf = product.Shelf,
                Count = product.Count,
                Description = product.Description
            };

            _context.Product.Add(prodToSave);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProduct", new { id = prodToSave.Id }, product);
        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Product.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductExists(int id)
        {
            return _context.Product.Any(e => e.Id == id);
        }
    }
}
