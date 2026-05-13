using api.Dtos;
using api.interfaces;
using api.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository _commentRepository;

        public CommentController(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetComments()
        {
            var comments = await _commentRepository.GetAllCommentsAsync();
            var commentData = comments.Select(c => CommentMappers.MapCommentToCommentResponseDtos(c)).ToList();
            return Ok(new { success = true, message = "Comments retrieved successfully", data = commentData });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetComment(int id)
        {
            var comment = await _commentRepository.GetCommentByIdAsync(id);
            if (comment == null)
                return NotFound(new { success = false, message = "Comment with that id not found" });

            return Ok(new { success = true, message = "Comment retrieved successfully", data = CommentMappers.MapCommentToCommentResponseDtos(comment) });
        }

        [HttpGet("stock/{stockId}")]
        public async Task<IActionResult> GetCommentsByStockId(int stockId)
        {
            var comments = await _commentRepository.GetCommentsByStockIdAsync(stockId);
            var commentData = comments.Select(c => CommentMappers.MapCommentToCommentResponseDtos(c)).ToList();
            return Ok(new { success = true, message = "Comments retrieved successfully", data = commentData });
        }

        [HttpPost]
        public async Task<IActionResult> CreateComment([FromBody] CreateCommentDtos comment)
        {
            if (comment == null)
                return BadRequest(new { success = false, message = "Comment data is required" });

            var commentModel = await _commentRepository.CreateCommentAsync(comment);
            return CreatedAtAction(nameof(GetComment), new { id = commentModel.Id }, new { success = true, message = "Comment created successfully", data = CommentMappers.MapCommentToCommentResponseDtos(commentModel) });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateComment([FromRoute] int id, [FromBody] UpdateCommentDtos comment)
        {
            if (comment == null)
                return BadRequest(new { success = false, message = "Comment data is required" });

            var commentModel = await _commentRepository.UpdateCommentAsync(id, comment);
            if (commentModel == null)
                return NotFound(new { success = false, message = "Comment with that id not found" });

            return Ok(new { success = true, message = "Comment updated successfully", data = CommentMappers.MapCommentToCommentResponseDtos(commentModel) });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComment(int id)
        {
            var comment = await _commentRepository.DeleteCommentAsync(id);
            if (comment == null)
                return NotFound(new { success = false, message = "Comment with that id not found" });

            return Ok(new { success = true, message = "Comment deleted successfully" });
        }
    }
}
