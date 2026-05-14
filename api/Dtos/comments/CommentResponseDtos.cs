using api.Models;

namespace api.Dtos
{
    public class CommentResponseDtos
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public int? StockId { get; set; }
    }
}
