using Dapper;
using ModelLayer.Dto;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;

namespace RepositoryLayer.Service;

public class BookRL : IBookRL
{
    private readonly BookStoreContext _bookStoreContext;

    public BookRL(BookStoreContext bookStoreContext)
    {
        _bookStoreContext = bookStoreContext;
    }

    public async Task<BooksEntity> AddBook(AddBookDto addBookDto)
    {
        try
        {
            var book = new BooksEntity
            {
                BookName = addBookDto.BookName,
                Description = addBookDto.Description,
                Author = addBookDto.Author,
                Price = addBookDto.Price,
                BookImage = addBookDto.BookImage,
                Quantity = addBookDto.Quantity,
                AddedOn = DateTime.Now,
                UpdatedOn = DateTime.Now
            };

            using (var connection = _bookStoreContext.CreateConnection())
            {
                var query = @"
                INSERT INTO Books (BookName, Description, Author, Price, BookImage, Quantity, AddedOn, UpdatedOn)
                VALUES (@BookName, @Description, @Author, @Price, @BookImage, @Quantity, @AddedOn, @UpdatedOn);
                SELECT * FROM Books WHERE BookId = SCOPE_IDENTITY();";

                var insertedBook = await connection.QueryFirstOrDefaultAsync<BooksEntity>(query, book);
                return insertedBook;
            }
        }
        catch (Exception ex)
        {

            throw new Exception("Error occurred while adding book", ex);
        }
    }

    public async Task<IEnumerable<BooksEntity>> GetAllBooks()
    {
        try
        {
            string query = "SELECT * FROM Books";

            using (var connection = _bookStoreContext.CreateConnection())
            {
                IEnumerable<BooksEntity> books = await connection.QueryAsync<BooksEntity>(query);
                return books;
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Error occurred while retrieving all books", ex);
        }
    }
}
