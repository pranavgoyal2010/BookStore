using ModelLayer.Dto;
using RepositoryLayer.Entity;

namespace BusinessLayer.Interface;

public interface IBookBL
{
    public Task<BooksEntity> AddBook(AddBookDto addBookDto);
}
