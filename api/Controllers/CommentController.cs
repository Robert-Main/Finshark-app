using api.Dtos;
using api.interfaces;
using api.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IstockRepository _stockRepository;

        public CommentController(ICommentRepository commentRepository, IstockRepository stockRepository)
        {
            _commentRepository = commentRepository;
            _stockRepository = stockRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetComments([FromQuery(Name = "Symbol")] string? symbol)
        {
            if (!string.IsNullOrWhiteSpace(symbol))
            {
                var stock = await _stockRepository.GetStockBySymbolAsync(symbol);
                if (stock == null)
                    return NotFound(new
                    {
                        success = false,
                        message = "Stock with that symbol not found"
                    });

                var comments = await _commentRepository.GetCommentsByStockIdAsync(stock.Id);
                var commentData = comments.Select(c => CommentMappers.MapCommentResponseDtos(c)).ToList();
                return Ok(new
                {
                    success = true,
                    message = "Comments retrieved successfully",
                    data = commentData
                });
            }

            var commentsAll = await _commentRepository.GetAllCommentsAsync();
            var commentDataAll = commentsAll.Select(c => CommentMappers.MapCommentResponseDtos(c)).ToList();
            return Ok(new
            {
                success = true,
                message = "Comments retrieved successfully",
                data = commentDataAll
            });
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetComment(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { success = false, message = "Invalid model state" });
            var comment = await _commentRepository.GetCommentByIdAsync(id);
            if (comment == null)
                return NotFound(new
                {
                    success = false,
                    message = "Comment with that id not found"
                });

            return Ok(new
            {
                success = true,
                message = "Comment retrieved successfully",
                data = CommentMappers.MapCommentResponseDtos(comment)
            });
        }

        [HttpGet("stock/{stockId:int}")]
        public async Task<IActionResult> GetCommentsByStockId(int stockId)
        {
            if (!ModelState.IsValid)
                return BadRequest(new
                {
                    success = false,
                    message = "Invalid model state"
                });

            var comments = await _commentRepository.GetCommentsByStockIdAsync(stockId);
            var commentData = comments.Select(c => CommentMappers.MapCommentResponseDtos(c)).ToList();
            return Ok(new
            {
                success = true,
                message = "Comments retrieved successfully",
                data = commentData
            });
        }

        [HttpPost("{stockId:int}")]
        public async Task<IActionResult> CreateComment([FromRoute] int stockId, [FromBody] CreateCommentDtos comment)
        {
            if (!ModelState.IsValid)
                return BadRequest(new
                {
                    success = false,
                    message = "Invalid model state"
                });

            if (!await _stockRepository.StockExistsAsync(stockId))
                return NotFound(new
                {
                    success = false,
                    message = "Stock with that id not found"
                });

            if (comment == null)
                return BadRequest(new
                {
                    success = false,
                    message = "Comment data is required"
                });

            var commentModel = await _commentRepository.CreateCommentAsync(stockId, comment);
            return CreatedAtAction(nameof(GetComment), new { id = commentModel.Id }, new
            {
                success = true,
                message = "Comment created successfully",
                data = CommentMappers.MapCommentResponseDtos(commentModel)
            });
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateComment([FromRoute] int id, [FromBody] UpdateCommentDtos comment)
        {
            if (!ModelState.IsValid)
                return BadRequest(new

                {
                    success = false,
                    message = "Invalid model state"
                });

            if (comment == null)
                return BadRequest(new
                {
                    success = false,
                    message = "Comment data is required"
                });

            var commentModel = await _commentRepository.UpdateCommentAsync(id, comment);
            if (commentModel == null)
                return NotFound(new
                {
                    success = false,
                    message = "Comment with that id not found"
                });

            return Ok(new
            {
                success = true,
                message = "Comment updated successfully",
                data = CommentMappers.MapCommentResponseDtos(commentModel)
            });
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteComment(int id)
        {
            var comment = await _commentRepository.DeleteCommentAsync(id);
            if (comment == null)
                return NotFound(new
                {
                    success = false,
                    message = "Comment with that id not found"
                });

            return Ok(new
            {
                success = true,
                message = "Comment deleted successfully"
            });
        }
    }
}
