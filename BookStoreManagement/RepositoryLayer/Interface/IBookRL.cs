using ModelLayer.Dto;
using RepositoryLayer.Entity;

namespace RepositoryLayer.Interface;

public interface IBookRL
{
    //public Task<BooksEntity> GetBookById(int bookId);
    public Task<IEnumerable<BooksEntity>> GetAllBooks();
    public Task<BooksEntity> AddBook(AddBookDto addBookDto);
    public Task<BooksEntity> UpdateBook(int bookId, UpdateBookDto updateBookDto);
    //public Task<bool> DeleteBookAsync(long bookId);
    //public Task<BooksEntity> UpdateBookAsync(BooksEntity updateBook, int bookId);

}
