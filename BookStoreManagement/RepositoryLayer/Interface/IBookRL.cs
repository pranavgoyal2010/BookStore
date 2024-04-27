using ModelLayer.Dto;
using RepositoryLayer.Entity;

namespace RepositoryLayer.Interface;

public interface IBookRL
{
    public Task<IEnumerable<BooksEntity>> GetAllBooks();
    public Task<BooksEntity> AddBook(AddBookDto addBookDto);
    public Task<BooksEntity> UpdateBook(int bookId, UpdateBookDto updateBookDto);
    public Task<bool> DeleteBook(int bookId);
    public Task<BooksEntity> GetBookById(int bookId);
}
