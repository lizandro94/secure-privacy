using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using webapi.Models;
using webapi.Services;

namespace webapi.Controllers
{
    [Authorize]
    [Route("users/{userId}/[controller]")]
    [ApiController]
    public class ProductsController(UserService service) : Controller
    {
        private readonly UserService service = service;

        [HttpGet]
        public async Task<List<Product>> GetProducts(string userId) => await service.GetProducts(userId);

        [HttpPost]
        public async Task<IActionResult> CreateProduct(string userId, Product product)
        {
            await service.CreateProduct(userId, product);
            return Ok(product);
        }
    }
}