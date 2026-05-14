using api.Data;
using api.Dtos;
using api.interfaces;
using api.Mappers;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ApplicationDBContext _context;

        public CommentRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<List<Comment>> GetAllCommentsAsync()
        {
            return await _context.Comments.ToListAsync();
        }

        public async Task<Comment?> GetCommentByIdAsync(int id)
        {
            return await _context.Comments.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<List<Comment>> GetCommentsByStockIdAsync(int stockId)
        {
            return await _context.Comments.Where(c => c.StockId == stockId).ToListAsync();
        }

        public async Task<Comment> CreateCommentAsync(int stockId, CreateCommentDtos comment)
        {
            var commentModel = CommentMappers.ToCommentFromCreateCommentDtos(comment, stockId);
            _context.Comments.Add(commentModel);
            await _context.SaveChangesAsync();
            return commentModel;
        }

        public async Task<Comment?> UpdateCommentAsync(int id, UpdateCommentDtos updateCommentDtos)
        {
            var existing = await _context.Comments.FirstOrDefaultAsync(c => c.Id == id);
            if (existing == null) return null;

            CommentMappers.UpdateCommentFromUpdateCommentDtos(existing, updateCommentDtos);

            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<Comment?> DeleteCommentAsync(int id)
        {
            var comment = await _context.Comments.FirstOrDefaultAsync(c => c.Id == id);
            if (comment == null) return null;

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
            return comment;
        }
    }
}
