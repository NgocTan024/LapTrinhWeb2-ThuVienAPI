using Web2_NgocTan.Models.Domain;
using Web2_NgocTan.Models.DTO;

namespace Web2_NgocTan.Repositories
{
    public interface IAuthorRepository
    {
        List<AuthorDTO> GetAllAuthors();
        AuthorNoIdDTO GetAuthorById(int id);
        AddAuthorRequestDTO AddAuthor(AddAuthorRequestDTO addAuthorRequestDTO);
        AuthorNoIdDTO UpdateAuthorById(int id, AuthorNoIdDTO authorNoIdDTO);
        Author? DeleteAuthorById(int id);
    }
}