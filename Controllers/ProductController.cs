using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Challenge.Api.Data.Entities;
using Challenge.Api.Models;
using System.IO;

namespace Challenge.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly DataContext _context;

        public ProductController(DataContext context)
        {
            _context = context;
        }

        // GET: api/Product
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            return await _context.Products.ToListAsync();
        }

        // GET: api/Product/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        // PUT: api/Product/5
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

        // POST: api/Product
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Consumes("multipart/form-data")]
        [HttpPost]
        public async Task<IActionResult> PostProduct([FromForm] ProductRequest request)
        {
            var fileName = "";
            var fileNamePath = string.Empty;
            if (!ModelState.IsValid)
            {
                var response = new Response<object>
                {
                    IsSuccess = false,
                    Message = "Modelo no valido"
                };
                return (IActionResult)response;
            }

            foreach (var item in request.MyFile)
            {
                var guid = Guid.NewGuid().ToString();
                fileName = $"{guid}.png";
                fileNamePath = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "Uploads",
                    fileName);
                using (var strem = new FileStream(fileNamePath, FileMode.Create))
                {
                    await item.CopyToAsync(strem);
                }
                fileNamePath = $"~/Uploads/{fileName}";
            }

            var product = new Product()
            {
                Name = request.Name,
                Description = request.Description,
                FileImageName = fileNamePath
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return Ok(new Response<object>
            {
                IsSuccess = true,
                Message = "Product Save"
            });
        }

        // DELETE: api/Product/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
