using api.Dtos;
using api.Models;

namespace api.Mappers
{
    public class CommentMappers
    {
        public static CommentResponseDtos MapCommentToCommentResponseDtos(Comment comment)
        {
            return new CommentResponseDtos
            {
                Id = comment.Id,
                Title = comment.Title,
                Content = comment.Content,
                CreatedAt = comment.CreatedAt,
                StockId = comment.StockId
            };
        }

        public static Comment ToCommentFromCreateCommentDtos(CreateCommentDtos createCommentDtos)
        {
            return new Comment
            {
                Title = createCommentDtos.Title,
                Content = createCommentDtos.Content,
                StockId = createCommentDtos.StockId,
                CreatedAt = DateTime.UtcNow
            };
        }

        public static void UpdateCommentFromUpdateCommentDtos(Comment comment, UpdateCommentDtos updateCommentDtos)
        {
            comment.Title = updateCommentDtos.Title;
            comment.Content = updateCommentDtos.Content;
            comment.StockId = updateCommentDtos.StockId;
        }
    }
}
