using api.DTOs.Comment;
using api.Extensions;
using api.Helpers;
using api.interfaces;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/comment")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository _commentRepo;
        private readonly IStockRepository _stockRepo;
        private readonly UserManager<User> _userManager;
        
       public CommentController(ICommentRepository commentRepo, IStockRepository stockRepo, UserManager<User> userManager)
       {
        _commentRepo = commentRepo;
        _stockRepo = stockRepo;
        _userManager = userManager;
       }
       [HttpGet]
       [Authorize]
       public async Task<IActionResult> GetAll([FromQuery]CommentQueryObject queryObject)
       {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var comments = await _commentRepo.GetAllAsync(queryObject);
            var commentDto = comments.Select(s => s.ToCommentDto());
            return Ok(commentDto);
       }
       [HttpGet("{id:int}")]
       public async Task<IActionResult> GetById([FromRoute] int id)
       {
        if(!ModelState.IsValid)
            return BadRequest(ModelState);
            var comment = await _commentRepo.GetByIdAsync(id);
            if(comment == null)
            {
                return NotFound();
            }

            return Ok(comment.ToCommentDto());
       }
       [HttpPost("{stockId:int}")]
       public async Task<IActionResult> Create([FromRoute] int stockId,CreateCommentDto commentDto)
       {
        if(!ModelState.IsValid)
                return BadRequest(ModelState);
        if(!await _stockRepo.StockExist(stockId)){
            return BadRequest("Stock does not exist");
        }

        var username = User.GetUsername();
        var appUser = await _userManager.FindByNameAsync(username);
        var commentModel = commentDto.ToCommentFromCreate(stockId);
        commentModel.AppUserId = appUser.Id;
        await _commentRepo.CreateAsync(commentModel);

        return CreatedAtAction(nameof(GetById), new {id = commentModel.Id}, commentModel.ToCommentDto());
       }
       [HttpPut]
       [Route("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateCommentDto updateDto)
        {
            if(!ModelState.IsValid)
            return BadRequest(ModelState);
            var comment = await _commentRepo.UpdateAsync(id,updateDto.ToCommentFromUpdate());
            if(comment == null)
            {
                return NotFound("Comment Not Found");
            }
            return Ok(comment.ToCommentDto());
        }
        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if(!ModelState.IsValid)
            return BadRequest(ModelState);
            var commentModel = await _commentRepo.DeleteAsync(id);
            if(commentModel == null){
                return NotFound("Comment Not Found");
            }

             return NoContent();
        }
    }
}