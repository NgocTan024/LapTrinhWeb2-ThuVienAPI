using Web2_NgocTan.Data;
using Web2_NgocTan.Models.Domain;

namespace Web2_NgocTan.Repositories
{
    public class LocalImageRepository : IImageRepository
    {
        private readonly IWebHostEnvironment _hostEnv;
        private readonly IHttpContextAccessor _httpAccessor;
        private readonly AppDbContext _db;

        public LocalImageRepository(
            IWebHostEnvironment hostEnv,
            IHttpContextAccessor httpAccessor,
            AppDbContext db)
        {
            _hostEnv = hostEnv;
            _httpAccessor = httpAccessor;
            _db = db;
        }

        public Image Upload(Image img)
        {
            var fullFileName = $"{img.FileName}{img.FileExtension}";
            var localPath = Path.Combine(_hostEnv.ContentRootPath, "Images", fullFileName);

            using (var fileStream = new FileStream(localPath, FileMode.Create))
            {
                img.File.CopyTo(fileStream);
            }

            var req = _httpAccessor.HttpContext!.Request;
            img.FilePath = $"{req.Scheme}://{req.Host}{req.PathBase}/Images/{fullFileName}";

            _db.Images.Add(img);
            _db.SaveChanges();

            return img;
        }

        public List<Image> GetAllInfoImages()
        {
            return _db.Images.ToList();
        }

        public (byte[], string, string) DownloadFile(int Id)
        {
            var fileById = _db.Images.FirstOrDefault(x => x.Id == Id);
            if (fileById == null)
            {
                throw new FileNotFoundException($"Không tìm thấy ảnh có ID = {Id}");
            }

            var fullFileName = $"{fileById.FileName}{fileById.FileExtension}";
            var path = Path.Combine(_hostEnv.ContentRootPath, "Images", fullFileName);
            var stream = File.ReadAllBytes(path);

            return (stream, "application/octet-stream", fullFileName);
        }
    }
}