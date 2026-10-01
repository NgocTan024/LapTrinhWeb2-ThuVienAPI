using System.ComponentModel.DataAnnotations;

namespace Web2_NgocTan.Models.DTO
{
    public class AddBookRequestDTO
    {
        [Required]
        [MinLength(1)]
        public string Title { get; set; }
        [MinLength(1)]
        public string Description { get; set; }
        public bool IsRead { get; set; }
        public DateTime? DateRead { get; set; }

        [Range(0, 5, ErrorMessage = "From 0 to 5")]
        public int? Rate { get; set; }

        public string Genre { get; set; }
        public string CoverUrl { get; set; }
        public DateTime DateAdded { get; set; }

        public int PublisherID { get; set; }
        public List<int> AuthorIds { get; set; }
    }
}