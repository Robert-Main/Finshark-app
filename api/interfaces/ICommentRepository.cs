using api.Dtos;
using api.Models;

namespace api.interfaces
{
    public interface ICommentRepository
    {
        Task<List<Comment>> GetAllCommentsAsync();
        Task<Comment?> GetCommentByIdAsync(int id);
        Task<List<Comment>> GetCommentsByStockIdAsync(int stockId);
        Task<Comment> CreateCommentAsync(CreateCommentDtos comment);
        Task<Comment?> UpdateCommentAsync(int id, UpdateCommentDtos updateCommentDtos);
        Task<Comment?> DeleteCommentAsync(int id);
    }
}
