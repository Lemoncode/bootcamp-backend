using DependencyInversionPrinciple.Contracts;

using Microsoft.AspNetCore.Mvc;

namespace DependencyInversionPrinciple.Controllers
{
    [ApiController]
    [Route("/api")]
    public class BuyerController : Controller
    {
        private readonly IBuyerService _buyerService;

        public BuyerController(IBuyerService buyerService)
        {
            _buyerService = buyerService;
        }

        public async Task<IActionResult> Index()
        {
            return Ok(await _buyerService.GetAllProducts());
        }

        [HttpGet("{productId}")]
        public async Task<IActionResult> GetProduct([FromRoute] Guid productId)
        {
            return Ok(await _buyerService.GetProduct(productId));
        }
    }
}
