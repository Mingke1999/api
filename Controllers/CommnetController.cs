using api.DTOs.Comment;
using api.interfaces;
using api.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/comment")]
    [ApiController]
    public class CommnetController : ControllerBase
    {
        private readonly ICommentRepository _commentRepo;
        private readonly IStockRepository _stockRepo;
        
       public CommnetController(ICommentRepository commentRepo, IStockRepository stockRepo)
       {
        _commentRepo = commentRepo;
        _stockRepo = stockRepo;
       }
       [HttpGet]
       public async Task<IActionResult> GetAll()
       {
            var comments = await _commentRepo.GetAllAsync();
            var commentDto = comments.Select(s => s.ToCommentDto());
            return Ok(commentDto);
       }
       [HttpGet("{id}")]
       public async Task<IActionResult> GetById([FromRoute] int id)
       {
            var comment = await _commentRepo.GetByIdAsync(id);
            if(comment == null)
            {
                return NotFound();
            }

            return Ok(comment.ToCommentDto());
       }
       [HttpPost("{stockId}")]
       public async Task<IActionResult> Create([FromRoute] int stockId,CreateCommentDto commentDto)
       {
        if(!await _stockRepo.StockExist(stockId)){
            return BadRequest("Stock does not exist");
        }
        var commentModel = commentDto.ToCommentFromCreate(stockId);
        await _commentRepo.CreateAsync(commentModel);

        return CreatedAtAction(nameof(GetById), new {id = commentModel.Id}, commentModel.ToCommentDto());
       }
       [HttpPut]
       [Route("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateCommentDto updateDto)
        {
            var comment = await _commentRepo.UpdateAsync(id,updateDto.ToCommentFromUpdate());
            if(comment == null)
            {
                NotFound("Comment Not Found");
            }
            return Ok(comment.ToCommentDto());
        }
    }
}