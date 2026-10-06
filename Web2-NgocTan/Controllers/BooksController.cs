using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web2_NgocTan.Models.DTO;
using Web2_NgocTan.Repositories;

namespace Web2_NgocTan.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    // [Authorize]
    public class BooksController : ControllerBase
    {
        private readonly IBookRepository _bookRepository;
        private readonly ILogger<BooksController> _logger;

        public BooksController(IBookRepository bookRepository, ILogger<BooksController> logger)
        {
            _bookRepository = bookRepository;
            _logger = logger;
        }

        [HttpGet("get-all-books")]
        // [Authorize(Roles = "Read")]
        public IActionResult GetAll([FromQuery] string? filterOn, [FromQuery] string? filterQuery, [FromQuery] string? sortBy, [FromQuery] bool isAscending = true, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 100)
        {
            _logger.LogInformation("GetAll Book Action method was invoked");
            _logger.LogWarning("This is a warning log");
            _logger.LogError("This is a error log");

            var allBooks = _bookRepository.GetAllBooks(filterOn, filterQuery, sortBy, isAscending, pageNumber, pageSize);

            _logger.LogInformation($"Finished GetAllBook request with data {JsonSerializer.Serialize(allBooks)}");
            return Ok(allBooks);
        }

        [HttpGet("get-book-by-id/{id}")]
        // [Authorize(Roles = "Read")]
        public IActionResult GetBookById([FromRoute] int id)
        {
            return Ok(_bookRepository.GetBookById(id));
        }

        [HttpPost("add-book")]
        // [Authorize(Roles = "Write")]
        public IActionResult AddBook([FromBody] AddBookRequestDTO addBookRequestDTO)
        {
            if (ValidateAddBook(addBookRequestDTO))
            {
                try
                {
                    var bookAdd = _bookRepository.AddBook(addBookRequestDTO);
                    return Ok(bookAdd);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            return BadRequest(ModelState);
        }

        [HttpPut("update-book-by-id/{id}")]
        // [Authorize(Roles = "Write")]
        public IActionResult UpdateBookById(int id, [FromBody] AddBookRequestDTO dto)
        {
            try
            {
                var updatedBook = _bookRepository.UpdateBookById(id, dto);
                if (updatedBook == null)
                {
                    return NotFound($"Không tìm thấy sách có ID = {id}");
                }
                return Ok(updatedBook);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("delete-book-by-id/{id}")]
        // [Authorize(Roles = "Write")]
        public IActionResult DeleteBookById(int id)
        {
            return Ok(_bookRepository.DeleteBookById(id));
        }

        #region Private methods
        private bool ValidateAddBook(AddBookRequestDTO addBookRequestDTO)
        {
            if (addBookRequestDTO == null)
            {
                ModelState.AddModelError(nameof(addBookRequestDTO), $"Please add book data");
                return false;
            }

            if (string.IsNullOrEmpty(addBookRequestDTO.Description))
            {
                ModelState.AddModelError(nameof(addBookRequestDTO.Description), $"{nameof(addBookRequestDTO.Description)} cannot be null");
            }

            if (addBookRequestDTO.Rate < 0 || addBookRequestDTO.Rate > 5)
            {
                ModelState.AddModelError(nameof(addBookRequestDTO.Rate), $"{nameof(addBookRequestDTO.Rate)} cannot be less than 0 and more than 5");
            }

            if (ModelState.ErrorCount > 0)
            {
                return false;
            }
            return true;
        }
        #endregion
    }
}