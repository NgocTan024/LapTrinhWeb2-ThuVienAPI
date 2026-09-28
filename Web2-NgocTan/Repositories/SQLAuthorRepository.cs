using Web2_NgocTan.Data;
using Web2_NgocTan.Models.Domain;
using Web2_NgocTan.Models.DTO;

namespace Web2_NgocTan.Repositories
{
    public class SQLAuthorRepository : IAuthorRepository
    {
        private readonly AppDbContext _dbContext;
        public SQLAuthorRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<AuthorDTO> GetAllAuthors()
        {
            return _dbContext.Authors.Select(a => new AuthorDTO
            {
                Id = a.Id,
                FullName = a.FullName
            }).ToList();
        }

        public AuthorNoIdDTO GetAuthorById(int id)
        {
            var author = _dbContext.Authors.FirstOrDefault(x => x.Id == id);
            if (author == null) return null;
            return new AuthorNoIdDTO { FullName = author.FullName };
        }

        public AddAuthorRequestDTO AddAuthor(AddAuthorRequestDTO dto)
        {
            var authorDomainModel = new Author { FullName = dto.FullName };
            _dbContext.Authors.Add(authorDomainModel);
            _dbContext.SaveChanges();
            return dto;
        }

        public AuthorNoIdDTO UpdateAuthorById(int id, AuthorNoIdDTO dto)
        {
            var author = _dbContext.Authors.FirstOrDefault(n => n.Id == id);
            if (author != null)
            {
                author.FullName = dto.FullName;
                _dbContext.SaveChanges();
            }
            return dto;
        }

        public Author? DeleteAuthorById(int id)
        {
            var author = _dbContext.Authors.FirstOrDefault(n => n.Id == id);
            if (author != null)
            {
                _dbContext.Authors.Remove(author);
                _dbContext.SaveChanges();
                return author;
            }
            return null;
        }
    }
}