﻿using BusinessLayer.Interface;
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
}