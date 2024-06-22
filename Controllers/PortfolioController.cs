using api.Extensions;
using api.interfaces;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/portfolio")]
    [ApiController]
    public class PortfolioController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IStockRepository _StockRepo;
        private readonly IPortfolioRepository _portfolioRepo;
        public PortfolioController(UserManager<User> userManager,
        IStockRepository StockRepo, IPortfolioRepository portfolioRepo)
        {
            _userManager = userManager;
            _StockRepo = StockRepo;
            _portfolioRepo = portfolioRepo;
        }


        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUserPortfolios()
        {
            var username = User.GetUsername();
            var appUser =  await _userManager.FindByNameAsync(username);
            var userPortfolio = await _portfolioRepo.GetUserPortfolio(appUser);
            return Ok(userPortfolio);
        }
    }
}