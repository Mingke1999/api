using api.Data;
using api.DTOs.Stock;
using api.interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class StockRepo : IStockRepository
    {
        private readonly ApplicationDBContext _context;
        //dependency injection
        public StockRepo(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<Stock> CreateAsync(Stock stockModel)
        {
            await _context.Stocks.AddAsync(stockModel);
            await _context.SaveChangesAsync();
            return stockModel;
        }

        public async Task<Stock?> DeleteAsync(int id)
        {
            var stockModel = await _context.Stocks.FirstOrDefaultAsync(x=> x.stockId == id);
            if(stockModel == null){
                return null;
            }
            _context.Stocks.Remove(stockModel);
            await _context.SaveChangesAsync();
            return stockModel;
        }

        public async Task<List<Stock>> GetAllAsync()
        {
            return await _context.Stocks.ToListAsync();
        }

        public async Task<Stock?> GetByIdAsync(int id)
        {
            return await _context.Stocks.FindAsync(id);
        }

        public async Task<Stock?> UpdateAsync(int id, UpdateStockRequestDto updateDto)
        {
            var existingStock = await _context.Stocks.FirstOrDefaultAsync(x => x.stockId == id);
            if(existingStock == null){
                return null;
            }
                existingStock.Symbol = updateDto.Symbol;
                existingStock.CompanyName = updateDto.CompanyName;
                existingStock.Purchase = updateDto.Purchase;
                existingStock.Industry = updateDto.Industry;
                existingStock.Divdend = updateDto.Divdend;
                existingStock.MarketCap = updateDto.MarketCap;

                await _context.SaveChangesAsync();
                return existingStock;
        }
    }
}