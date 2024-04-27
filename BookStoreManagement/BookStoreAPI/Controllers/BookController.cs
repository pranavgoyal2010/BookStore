using BusinessLayer.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.Dto;
using ModelLayer.Response;
using RepositoryLayer.Entity;

namespace BookStoreAPI.Controllers;

[Route("api/book")]
[ApiController]
public class BookController : ControllerBase
{
    private readonly IBookBL _bookBL;

    public BookController(IBookBL bookBL)
    {
        _bookBL = bookBL;
    }


    [Authorize(Roles = "admin")]
    [HttpPost]
    public async Task<IActionResult> AddBook(AddBookDto addBookDto)
    {
        try
        {
            //var userRole = User.FindFirstValue(ClaimTypes.Role);

            if (addBookDto == null)
            {
                var errorResponse = new ResponseModel<string>
                {
                    Success = false,
                    Message = "Book object is null"
                };
                return BadRequest(errorResponse);
            }

            var addedBook = await _bookBL.AddBook(addBookDto);

            var response = new ResponseModel<BooksEntity>
            {
                Message = "Book added successfully",
                Data = addedBook
            };

            return Ok(response);
        }
        catch (Exception ex)
        {
            var errorResponse = new ResponseModel<string>
            {
                Success = false,
                Message = ex.Message,
            };

            return StatusCode(500, errorResponse);
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAllBooks()
    {
        try
        {
            var books = await _bookBL.GetAllBooks();

            var response = new ResponseModel<IEnumerable<BooksEntity>>
            {
                Message = "Books retrieved successfully",
                Data = books
            };

            return Ok(response);
        }
        catch (Exception ex)
        {
            var errorResponse = new ResponseModel<string>
            {
                Success = false,
                Message = ex.Message
            };

            return StatusCode(500, errorResponse);
        }
    }

    [Authorize(Roles = "admin")]
    [HttpPut("{bookId}")]
    public async Task<IActionResult> UpdateBook(int bookId, [FromBody] UpdateBookDto updateBookDto)
    {
        try
        {
            if (updateBookDto == null)
            {
                var errorResponse = new ResponseModel<string>
                {
                    Success = false,
                    Message = "Book object is null"
                };
                return BadRequest(errorResponse);
            }

            var updatedBook = await _bookBL.UpdateBook(bookId, updateBookDto);


            var response = new ResponseModel<BooksEntity>
            {
                Message = "Book updated successfully",
                Data = updatedBook
            };

            return Ok(response);
        }

        catch (Exception ex)
        {
            var errorResponse = new ResponseModel<string>
            {
                Success = false,
                Message = ex.Message
            };

            return StatusCode(500, errorResponse);
        }
    }

    [Authorize(Roles = "admin")]
    [HttpDelete("{bookId}")]
    public async Task<IActionResult> DeleteBook(int bookId)
    {
        try
        {
            var deleted = await _bookBL.DeleteBook(bookId);

            if (deleted)
            {
                var response = new ResponseModel<string>
                {
                    Message = "Book deleted successfully"
                };
                return Ok(response);
            }
            else
            {
                var response = new ResponseModel<string>
                {
                    Success = false,
                    Message = $"Book with ID {bookId} not found"
                };
                return NotFound(response);
            }
        }
        catch (Exception ex)
        {
            var response = new ResponseModel<string>
            {
                Success = false,
                Message = ex.Message
            };
            return StatusCode(500, response);
        }
    }
}
