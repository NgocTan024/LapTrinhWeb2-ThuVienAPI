using System.Net.Mime;
using System.Text;
using System.Text.Json;
using Library_web.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace Library_web.Controllers
{
    public class AuthorsController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly string _apiBase = "https://localhost:7276/api/Authors";

        public AuthorsController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        // 1. Liệt kê tất cả tác giả
        public async Task<IActionResult> Index()
        {
            var list = new List<authorDTO>();
            try
            {
                var client = _clientFactory.CreateClient();
                var res = await client.GetAsync($"{_apiBase}/get-all-author");
                res.EnsureSuccessStatusCode();
                var data = await res.Content.ReadFromJsonAsync<IEnumerable<authorDTO>>();
                if (data != null) list.AddRange(data);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }
            return View(list);
        }

        // 2. Xem chi tiết tác giả theo Id
        public async Task<IActionResult> listAuthor(int id)
        {
            var item = new authorDTO();
            try
            {
                var client = _clientFactory.CreateClient();
                var res = await client.GetAsync($"{_apiBase}/get-author-by-id/{id}");
                res.EnsureSuccessStatusCode();
                var data = await res.Content.ReadFromJsonAsync<authorDTO>();
                if (data != null)
                {
                    data.Id = id; // Gán lại ID chuẩn từ thanh địa chỉ URL
                    item = data;
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }
            return View(item);
        }

        // 3. Thêm mới tác giả (GET & POST)
        [HttpGet]
        public IActionResult addAuthor()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> addAuthor(authorNoIdDTO model)
        {
            try
            {
                var client = _clientFactory.CreateClient();
                var req = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri($"{_apiBase}/add-author"),
                    Content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, MediaTypeNames.Application.Json)
                };
                var res = await client.SendAsync(req);
                res.EnsureSuccessStatusCode();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View(model);
            }
        }

        // 4. Sửa thông tin tác giả (GET & POST)
        [HttpGet]
        public async Task<IActionResult> editAuthor(int id)
        {
            var client = _clientFactory.CreateClient();
            var res = await client.GetAsync($"{_apiBase}/get-author-by-id/{id}");
            res.EnsureSuccessStatusCode();
            var data = await res.Content.ReadFromJsonAsync<authorDTO>();
            if (data != null)
            {
                data.Id = id; // Gán lại ID để form Edit gửi đúng ID cần sửa
            }
            ViewBag.Author = data;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> editAuthor([FromRoute] int id, authorNoIdDTO model)
        {
            try
            {
                var client = _clientFactory.CreateClient();
                var req = new HttpRequestMessage
                {
                    Method = HttpMethod.Put,
                    RequestUri = new Uri($"{_apiBase}/update-author-by-id/{id}"),
                    Content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, MediaTypeNames.Application.Json)
                };
                var res = await client.SendAsync(req);
                res.EnsureSuccessStatusCode();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.Author = new authorDTO { Id = id, FullName = model.FullName };
                return View(model);
            }
        }

        // 5. Xóa tác giả
        [HttpGet]
        public async Task<IActionResult> delAuthor([FromRoute] int id)
        {
            try
            {
                var client = _clientFactory.CreateClient();
                var res = await client.DeleteAsync($"{_apiBase}/delete-author-by-id/{id}");
                res.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Không thể xóa tác giả này (có thể đang gắn với sách): " + ex.Message;
            }
            return RedirectToAction("Index");
        }
    }
}