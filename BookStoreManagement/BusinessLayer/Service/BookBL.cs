using BusinessLayer.Interface;
using ModelLayer.Dto;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;

namespace BusinessLayer.Service;

public class BookBL : IBookBL
{
    private readonly IBookRL _bookRL;

    public BookBL(IBookRL bookRL)
    {
        _bookRL = bookRL;
    }

    public Task<BooksEntity> GetBookById(int bookId)
    {
        return _bookRL.GetBookById(bookId);
    }

    public Task<BooksEntity> AddBook(AddBookDto addBookDto)
    {
        return _bookRL.AddBook(addBookDto);
    }

    public Task<IEnumerable<BooksEntity>> GetAllBooks()
    {
        return _bookRL.GetAllBooks();
    }

    public Task<BooksEntity> UpdateBook(int bookId, UpdateBookDto updateBookDto)
    {
        return _bookRL.UpdateBook(bookId, updateBookDto);
    }

    public Task<bool> DeleteBook(int bookId)
    {
        return _bookRL.DeleteBook(bookId);
    }
}
