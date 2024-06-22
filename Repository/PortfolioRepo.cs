using api.Data;
using api.interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class PortfolioRepo : IPortfolioRepository
    {
        private readonly ApplicationDBContext _context;
        public PortfolioRepo(ApplicationDBContext context)
        {
            _context = context;
        }
        public async Task<List<Stock>> GetUserPortfolio(User user)
        {
            return await _context.Portfolios.Where(u => u.AppUserId == user.Id )
            .Select(stock => new Stock{
                StockId = stock.StockId,
                Symbol = stock.Stock.Symbol,
                CompanyName = stock.Stock.CompanyName,
                Purchase = stock.Stock.Purchase,
                Divdend = stock.Stock.Divdend,
                Industry = stock.Stock.Industry,
                MarketCap = stock.Stock.MarketCap

            }).ToListAsync();
        }
    }
}