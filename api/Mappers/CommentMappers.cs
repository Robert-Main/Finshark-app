using api.Dtos;
using api.Models;

namespace api.Mappers
{
    public class CommentMappers
    {
        public static CommentResponseDtos MapCommentResponseDtos(Comment comment)
        {
            return new CommentResponseDtos
            {
                Id = comment.Id,
                Title = comment.Title,
                Content = comment.Content,
                StockId = comment.StockId,
                CreatedBy = comment.AppUser?.UserName ?? "Unknown",
                CreatedAt = comment.CreatedAt,
            };
        }

        public static Comment ToCommentFromCreateCommentDtos(CreateCommentDtos createCommentDtos, int stockId)
        {
            return new Comment
            {
                Title = createCommentDtos.Title,
                Content = createCommentDtos.Content,
                StockId = stockId,
            };
        }

        public static void UpdateCommentFromUpdateCommentDtos(Comment comment, UpdateCommentDtos updateCommentDtos)
        {
            comment.Title = updateCommentDtos.Title;
            comment.Content = updateCommentDtos.Content;
        }
    }
}
