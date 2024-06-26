using api.Data;
using api.DTOs.Stock;
using api.interfaces;
using api.Mappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [Route("api/stock")]
    [ApiController]
    public class StockController : ControllerBase
    {
        //why _context not context,
        //what ControllerBase doing
        private readonly ApplicationDBContext _context;
        private readonly IStockRepository _stockRepo;
        public StockController(ApplicationDBContext context, IStockRepository stockRepo)
        {
            _stockRepo = stockRepo;
            _context = context;
        } //what context receiving

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            //var stocks = await _context.Stocks.ToListAsync();
            var stocks = await _stockRepo.GetAllAsync();
            var stockDto = stocks.Select(    s => s.ToStockDto());

            return Ok(stocks);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var stocks = await _stockRepo.GetByIdAsync(id);
            if(stocks == null){
                return NotFound();
            }
            return Ok(stocks.ToStockDto());
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateStockRequestDto requestDto)
        {
            var stockModel = requestDto.ToStockRequestDto();
            await _stockRepo.CreateAsync(stockModel);
            return CreatedAtAction(nameof(GetById), 
                    new {id = stockModel.stockId}, 
                    stockModel.ToStockDto());
        }
        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockRequestDto updateDto)
        {
            var stockModel = await  _stockRepo.UpdateAsync(id,updateDto);
            if(stockModel == null){
                return NotFound();
            }
          
            return Ok(stockModel.ToStockDto());
        }
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var stockModel = await _stockRepo.DeleteAsync(id);
            if(stockModel == null){
                return NotFound();
            }
         
             return NoContent();
        }
    }
}