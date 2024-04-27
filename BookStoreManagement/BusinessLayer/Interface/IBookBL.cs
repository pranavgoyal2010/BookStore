using ModelLayer.Dto;
using RepositoryLayer.Entity;

namespace BusinessLayer.Interface;

public interface IBookBL
{
    public Task<BooksEntity> AddBook(AddBookDto addBookDto);
    public Task<IEnumerable<BooksEntity>> GetAllBooks();
    public Task<BooksEntity> UpdateBook(int bookId, UpdateBookDto updateBookDto);
    public Task<bool> DeleteBook(int bookId);
}
