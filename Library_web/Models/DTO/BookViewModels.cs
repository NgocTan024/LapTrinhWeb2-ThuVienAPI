using System.ComponentModel.DataAnnotations;

namespace Library_web.Models.DTO
{
    public class BookDTO
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public bool IsRead { get; set; }
        public DateTime? DateRead { get; set; }
        public int? Rate { get; set; }
        public string? Genre { get; set; }
        public string? CoverUrl { get; set; }
        public DateTime DateAdded { get; set; }
        public string? PublisherName { get; set; }
        public List<string> AuthorNames { get; set; } = new();
    }

    public class addBookDTO
    {
        public string title { get; set; } = string.Empty;
        public string? description { get; set; }
        public bool isRead { get; set; }
        public DateTime? dateRead { get; set; }

        [Range(0, 5, ErrorMessage = "From 0 to 5")]
        public int? rate { get; set; }

        public string? genre { get; set; }
        public string? coverUrl { get; set; }
        public DateTime dateAdded { get; set; }
        public int publisherID { get; set; }
        public List<int> authorIds { get; set; } = new();
    }

    public class editBookDTO
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsRead { get; set; }
        public DateTime? DateRead { get; set; }
        public int? Rate { get; set; }
        public string? Genre { get; set; }
        public string? CoverUrl { get; set; }
        public DateTime DateAdded { get; set; }
        public int PublisherID { get; set; }
        public List<int> AuthorIds { get; set; } = new();
    }

    public class authorDTO
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
    }
    public class authorNoIdDTO
    {
        public string FullName { get; set; } = string.Empty;
    }

    

    public class publisherNoIdDTO
    {
        public string Name { get; set; } = string.Empty;
    }

    public class publisherDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}