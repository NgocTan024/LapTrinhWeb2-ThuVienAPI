using Web2_NgocTan.Models.Domain;
using Web2_NgocTan.Models.DTO;
using WebAPI_simple.Models.Domain;
using WebAPI_simple.Models.DTO;

namespace Web2_NgocTan.Repositories
{
    public interface IBookRepository
    {
        List<BookWithAuthorAndPublisherDTO> GetAllBooks();
        BookWithAuthorAndPublisherDTO GetBookById(int id);
        AddBookRequestDTO AddBook(AddBookRequestDTO addBookRequestDTO);
        AddBookRequestDTO? UpdateBookById(int id, AddBookRequestDTO bookDTO);
        Book? DeleteBookById(int id);
    }
}