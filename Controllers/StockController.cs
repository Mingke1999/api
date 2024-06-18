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
    }
}