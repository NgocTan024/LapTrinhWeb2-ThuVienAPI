using System.Net.Mime;
using System.Text;
using System.Text.Json;
using Library_web.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace Library_web.Controllers
{
    public class BooksController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly string _apiBase = "https://localhost:7276/api";

        public BooksController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        // 1. Lấy danh sách sách kèm Tìm kiếm & Sắp xếp
        public async Task<IActionResult> Index([FromQuery] string? filterOn = null, string? filterQuery = null, string? sortBy = null, bool isAscending = true)
        {
            var bookList = new List<BookDTO>();
            try
            {
                var httpClient = _clientFactory.CreateClient();
                var queryUrl = $"{_apiBase}/Books/get-all-books?filterOn={filterOn}&filterQuery={filterQuery}&sortBy={sortBy}&isAscending={isAscending}";

                var apiRes = await httpClient.GetAsync(queryUrl);
                apiRes.EnsureSuccessStatusCode();

                var items = await apiRes.Content.ReadFromJsonAsync<IEnumerable<BookDTO>>();
                if (items != null)
                {
                    bookList.AddRange(items);
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }
            return View(bookList);
        }

        // 2. Xem chi tiết một cuốn sách theo ID
        public async Task<IActionResult> listBook(int id)
        {
            var bookDetail = new BookDTO();
            try
            {
                var httpClient = _clientFactory.CreateClient();
                var apiRes = await httpClient.GetAsync($"{_apiBase}/Books/get-book-by-id/{id}");
                apiRes.EnsureSuccessStatusCode();

                var item = await apiRes.Content.ReadFromJsonAsync<BookDTO>();
                if (item != null) bookDetail = item;
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }
            return View(bookDetail);
        }

        // 3. Mở trang Thêm mới sách (GET)
        [HttpGet]
        public async Task<IActionResult> addBook()
        {
            await PopulateDropdownData();
            return View();
        }

        // 3. Gửi dữ liệu Thêm mới sách lên API (POST)
        [HttpPost]
        public async Task<IActionResult> addBook(addBookDTO newBook)
        {
            try
            {
                var httpClient = _clientFactory.CreateClient();
                var reqMsg = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri($"{_apiBase}/Books/add-book"),
                    Content = new StringContent(JsonSerializer.Serialize(newBook), Encoding.UTF8, MediaTypeNames.Application.Json)
                };

                var apiRes = await httpClient.SendAsync(reqMsg);
                apiRes.EnsureSuccessStatusCode();

                var created = await apiRes.Content.ReadFromJsonAsync<addBookDTO>();
                if (created != null)
                {
                    return RedirectToAction("Index", "Books");
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }

            await PopulateDropdownData();
            return View(newBook);
        }

        // 4. Mở trang Chỉnh sửa sách (GET)
        [HttpGet]
        public async Task<IActionResult> editBook(int id)
        {
            var httpClient = _clientFactory.CreateClient();
            var apiRes = await httpClient.GetAsync($"{_apiBase}/Books/get-book-by-id/{id}");
            apiRes.EnsureSuccessStatusCode();

            ViewBag.Book = await apiRes.Content.ReadFromJsonAsync<BookDTO>();
            await PopulateDropdownData();
            return View();
        }

        // 4. Gửi dữ liệu Cập nhật sách lên API (POST -> PUT)
        [HttpPost]
        public async Task<IActionResult> editBook([FromRoute] int id, editBookDTO updatedBook)
        {
            try
            {
                var httpClient = _clientFactory.CreateClient();
                var reqMsg = new HttpRequestMessage
                {
                    Method = HttpMethod.Put,
                    RequestUri = new Uri($"{_apiBase}/Books/update-book-by-id/{id}"),
                    Content = new StringContent(JsonSerializer.Serialize(updatedBook), Encoding.UTF8, MediaTypeNames.Application.Json)
                };

                var apiRes = await httpClient.SendAsync(reqMsg);
                apiRes.EnsureSuccessStatusCode();

                var result = await apiRes.Content.ReadFromJsonAsync<addBookDTO>();
                if (result != null)
                {
                    return RedirectToAction("Index", "Books");
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }

            await PopulateDropdownData();
            return View(updatedBook);
        }

        // 5. Xóa sách theo ID
        [HttpGet]
        public async Task<IActionResult> delBook([FromRoute] int id)
        {
            try
            {
                var httpClient = _clientFactory.CreateClient();
                var apiRes = await httpClient.DeleteAsync($"{_apiBase}/Books/delete-book-by-id/{id}");
                apiRes.EnsureSuccessStatusCode();
                return RedirectToAction("Index", "Books");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }
            return RedirectToAction("Index", "Books");
        }

        // Hàm hỗ trợ tải danh sách Tác giả và NXB vào ViewBag
        private async Task PopulateDropdownData()
        {
            var httpClient = _clientFactory.CreateClient();

            var authorRes = await httpClient.GetAsync($"{_apiBase}/Authors/get-all-author");
            if (!authorRes.IsSuccessStatusCode)
            {
                authorRes = await httpClient.GetAsync($"{_apiBase}/Authors/get-all-authors");
            }
            ViewBag.listAuthor = authorRes.IsSuccessStatusCode
                ? await authorRes.Content.ReadFromJsonAsync<List<authorDTO>>()
                : new List<authorDTO>();

            var pubRes = await httpClient.GetAsync($"{_apiBase}/Publishers/get-all-publisher");
            if (!pubRes.IsSuccessStatusCode)
            {
                pubRes = await httpClient.GetAsync($"{_apiBase}/Publishers/get-all-publishers");
            }
            ViewBag.listPublisher = pubRes.IsSuccessStatusCode
                ? await pubRes.Content.ReadFromJsonAsync<List<publisherDTO>>()
                : new List<publisherDTO>();
        }
    }
}