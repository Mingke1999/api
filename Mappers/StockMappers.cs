
using api.DTOs.Stock;
using api.Models;

namespace api.Mappers
{
    public static class StockMappers
    {
        public static StockDto ToStockDto(this Stock stockModel)
        {
            return new StockDto
            {
                stockId = stockModel.StockId,
                Symbol = stockModel.Symbol,
                CompanyName = stockModel.CompanyName,
                Purchase = stockModel.Purchase,
                Industry = stockModel.Industry,
                Divdend = stockModel.Divdend,
                MarketCap = stockModel.MarketCap,
                Comments = stockModel.Commnents
                    .Select(s => s.ToCommentDto()).ToList()
            };
        }
        public static Stock ToStockRequestDto(this CreateStockRequestDto stockDto)
        {
            return new Stock
            {
                Symbol = stockDto.Symbol,
                CompanyName = stockDto.CompanyName,
                Purchase = stockDto.Purchase,
                Industry = stockDto.Industry,
                Divdend = stockDto.Divdend,
                MarketCap = stockDto.MarketCap
            };
        }
    }
}