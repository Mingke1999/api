using api.Data;
using api.DTOs.Stock;
using api.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/stock")]
    [ApiController]
    public class StockController : ControllerBase
    {
        //why _context not context,
        //what ControllerBase doing
        private readonly ApplicationDBContext _context;
        public StockController(ApplicationDBContext context)
        {
            _context = context;
        } //what context receiving

        [HttpGet]
        public IActionResult GetAll()
        {
            var stocks =  _context.Stocks.ToList()
            .Select(
                s => s.ToStockDto()
            );
            return Ok(stocks);
        }

        [HttpGet("{id}")]
        public IActionResult GetById([FromRoute] int id)
        {
            var stocks = _context.Stocks.Find(id);
            if(stocks == null){
                return NotFound();
            }
            return Ok(stocks.ToStockDto());
        }

        [HttpPost]
        public IActionResult Create([FromBody] CreateStockRequestDto requestDto)
        {
            var stockModel = requestDto.ToStockRequestDto();
            _context.Stocks.Add(stockModel);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetById), 
                    new {id = stockModel.stockId}, 
                    stockModel.ToStockDto());
        }
        [HttpPut]
        [Route("{id}")]
        public IActionResult Update([FromRoute] int id, [FromBody] UpdateStockRequestDto updateDto)
        {
            var stockModel = _context.Stocks.FirstOrDefault(x => x.stockId == id);
            if(stockModel == null){
                return NotFound();
            }

            stockModel.Symbol = updateDto.Symbol;
            stockModel.CompanyName = updateDto.CompanyName;
            stockModel.Industry = updateDto.Industry;
            stockModel.Divdend = updateDto.Divdend;
            stockModel.Purchase = updateDto.Purchase;
            stockModel.MarketCap = updateDto.MarketCap;

            _context.SaveChanges();
            return Ok(stockModel.ToStockDto());
        }
        [HttpDelete]
        [Route("{id}")]
        public IActionResult Delete([FromRoute] int id)
        {
            var stockModel = _context.Stocks.FirstOrDefault(x => id == x.stockId);
            if(stockModel == null){
                return NotFound();
            }
            _context.Stocks.Remove(stockModel);
             _context.SaveChanges();

             return NoContent();
        }
    }
}