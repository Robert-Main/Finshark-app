using System.ComponentModel.DataAnnotations;

namespace api.Dtos
{
    public class UpdateCommentDtos
    {
        [Required(ErrorMessage = "Title is required.")]
        [MinLength(5, ErrorMessage = "Title cannot be less than 5 characters.")]
        [MaxLength(100, ErrorMessage = "Title cannot be longer than 100 characters.")]
        public string Title { get; set; } = string.Empty;
        [Required(ErrorMessage = "Content is required.")]
        [MinLength(1, ErrorMessage = "Content cannot be empty.")]
        [MaxLength(1000, ErrorMessage = "Content cannot be longer than 1000 characters.")]
        public string Content { get; set; } = string.Empty;
    }
}
