using Web2_NgocTan.Data;
using Web2_NgocTan.Models.Domain;
using Web2_NgocTan.Models.DTO;

namespace Web2_NgocTan.Repositories
{
    public class SQLPublisherRepository : IPublisherRepository
    {
        private readonly AppDbContext _dbContext;
        public SQLPublisherRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<PublisherDTO> GetAllPublishers()
        {
            return _dbContext.Publishers.Select(p => new PublisherDTO
            {
                Id = p.Id,
                Name = p.Name
            }).ToList();
        }

        public PublisherNoIdDTO GetPublisherById(int id)
        {
            var publisher = _dbContext.Publishers.FirstOrDefault(x => x.Id == id);
            if (publisher != null) return new PublisherNoIdDTO { Name = publisher.Name };
            return null;
        }

        public AddPublisherRequestDTO AddPublisher(AddPublisherRequestDTO dto)
        {
            var publisherDomainModel = new Publisher { Name = dto.Name };
            _dbContext.Publishers.Add(publisherDomainModel);
            _dbContext.SaveChanges();
            return dto;
        }

        public PublisherNoIdDTO UpdatePublisherById(int id, PublisherNoIdDTO dto)
        {
            var publisher = _dbContext.Publishers.FirstOrDefault(n => n.Id == id);
            if (publisher != null)
            {
                publisher.Name = dto.Name;
                _dbContext.SaveChanges();
            }
            return dto;
        }

        public Publisher? DeletePublisherById(int id)
        {
            var publisher = _dbContext.Publishers.FirstOrDefault(n => n.Id == id);
            if (publisher != null)
            {
                _dbContext.Publishers.Remove(publisher);
                _dbContext.SaveChanges();
                return publisher;
            }
            return null;
        }
    }
}