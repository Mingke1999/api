using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using api.interfaces;
using api.Mappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace api.Controllers
{
    [Route("api/comment")]
    [ApiController]
    public class CommnetController : ControllerBase
    {
        private readonly ICommentRepository _commentRepo;
        
       public CommnetController(ICommentRepository commentRepo)
       {
        _commentRepo = commentRepo;
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
    }
}