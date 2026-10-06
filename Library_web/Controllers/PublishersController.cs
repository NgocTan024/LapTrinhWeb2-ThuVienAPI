using System.Net.Mime;
using System.Text;
using System.Text.Json;
using Library_web.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace Library_web.Controllers
{
    public class PublishersController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly string _apiBase = "https://localhost:7276/api/Publishers";

        public PublishersController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        // 1. Liệt kê tất cả NXB
        public async Task<IActionResult> Index()
        {
            var list = new List<publisherDTO>();
            try
            {
                var client = _clientFactory.CreateClient();
                var res = await client.GetAsync($"{_apiBase}/get-all-publisher");
                if (!res.IsSuccessStatusCode)
                {
                    res = await client.GetAsync($"{_apiBase}/get-all-publishers");
                }
                res.EnsureSuccessStatusCode();
                var data = await res.Content.ReadFromJsonAsync<IEnumerable<publisherDTO>>();
                if (data != null) list.AddRange(data);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }
            return View(list);
        }

        // 2. Xem chi tiết NXB theo Id
        public async Task<IActionResult> listPublisher(int id)
        {
            var item = new publisherDTO();
            try
            {
                var client = _clientFactory.CreateClient();
                var res = await client.GetAsync($"{_apiBase}/get-publisher-by-id/{id}");
                res.EnsureSuccessStatusCode();
                var data = await res.Content.ReadFromJsonAsync<publisherDTO>();
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

        // 3. Thêm mới NXB (GET & POST)
        [HttpGet]
        public IActionResult addPublisher()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> addPublisher(publisherNoIdDTO model)
        {
            try
            {
                var client = _clientFactory.CreateClient();
                var req = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri($"{_apiBase}/add-publisher"),
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

        // 4. Sửa thông tin NXB (GET & POST)
        [HttpGet]
        public async Task<IActionResult> editPublisher(int id)
        {
            var client = _clientFactory.CreateClient();
            var res = await client.GetAsync($"{_apiBase}/get-publisher-by-id/{id}");
            res.EnsureSuccessStatusCode();
            var data = await res.Content.ReadFromJsonAsync<publisherDTO>();
            if (data != null)
            {
                data.Id = id; // Gán lại ID để form Edit gửi đúng ID cần sửa
            }
            ViewBag.Publisher = data;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> editPublisher([FromRoute] int id, publisherNoIdDTO model)
        {
            try
            {
                var client = _clientFactory.CreateClient();
                var req = new HttpRequestMessage
                {
                    Method = HttpMethod.Put,
                    RequestUri = new Uri($"{_apiBase}/update-publisher-by-id/{id}"),
                    Content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, MediaTypeNames.Application.Json)
                };
                var res = await client.SendAsync(req);
                res.EnsureSuccessStatusCode();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.Publisher = new publisherDTO { Id = id, Name = model.Name };
                return View(model);
            }
        }

        // 5. Xóa NXB
        [HttpGet]
        public async Task<IActionResult> delPublisher([FromRoute] int id)
        {
            try
            {
                var client = _clientFactory.CreateClient();
                var res = await client.DeleteAsync($"{_apiBase}/delete-publisher-by-id/{id}");
                res.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Không thể xóa NXB này (đang có sách thuộc NXB này): " + ex.Message;
            }
            return RedirectToAction("Index");
        }
    }
}