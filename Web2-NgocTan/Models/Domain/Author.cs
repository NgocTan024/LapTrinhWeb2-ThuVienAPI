using Web2_NgocTan.Models.Domain;

namespace Web2_NgocTan.Models.Domain
{
    public class Author
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public List<Book_Author> Book_Authors { get; set; }
    }
}