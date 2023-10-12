using backend.Context;
using backend.Dtos;
using backend.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] CreateUpdateProductDto dto)
        {
            var newproduct = new ProductEntity()
            {
                Brand = dto.Brand,
                Title = dto.Title,
            };
            await _context.Products.AddAsync(newproduct);
            await _context.SaveChangesAsync();

            return Ok("Product Saved!");
        }

        [HttpGet]
        public async Task<ActionResult<List<ProductEntity>>> GetAllProducts()
        {
            var products = await _context.Products.ToListAsync();
            return Ok(products);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<ProductEntity>> GetProductById([FromRoute] long id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(q => q.Id == id);
            if (product is null)
            {
                return NotFound("Product Not Found");
            }
            return Ok(product);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateProduct([FromRoute] long id, [FromBody] CreateUpdateProductDto dto)
        {
            var product = await _context.Products.FirstOrDefaultAsync(q => q.Id == id);
            if (product is null)
            {
                return NotFound("Product Not Found");
            }

            product.Id = id;
            product.Title = dto.Title;
            product.Brand = dto.Brand;
            product.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();
            return Ok("Product Updated!");
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteProduct([FromRoute] long id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(q => q.Id == id);
            if (product is null)
            {
                return NotFound("Product Not Found");
            }
            _context.Products.Remove(product);

            await _context.SaveChangesAsync();
            return Ok("Product Deleted!");
        }
    }
}
