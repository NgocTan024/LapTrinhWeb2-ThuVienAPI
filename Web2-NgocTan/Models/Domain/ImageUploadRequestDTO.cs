using System.ComponentModel.DataAnnotations;

namespace Web2_NgocTan.Models.DTO
{
    public class ImageUploadRequestDTO
    {
        [Required]
        public IFormFile File { get; set; }

        [Required]
        public string FileName { get; set; } = string.Empty;

        public string? FileDescription { get; set; }
    }
}