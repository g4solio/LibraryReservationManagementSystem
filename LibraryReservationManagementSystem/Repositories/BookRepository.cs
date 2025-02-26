using LibraryReservationManagementSystem.DbContexts;
using LibraryReservationManagementSystem.Models;
using LibraryReservationManagementSystem.OperationResults;
using Microsoft.EntityFrameworkCore;

namespace LibraryReservationManagementSystem.Repositories;

public class BookRepository(NpgApplicationContext applicationContext) : IRepository<Book>, IAsyncDisposable
{
    public IOperationResult<Book> Add(Book entity)
    {
        var book = applicationContext.Books.Add(entity);

        if (book.State != EntityState.Added)
            return "Book could not be added".AsFailedOperation<Book>();

        return book.Entity.AsSuccessfulOperation();
    }

    public IOperationResult<Book> Update(Book entity)
    {
        var oldBook = GetById(entity.Id);

        if (!oldBook.IsSuccess)
            return oldBook.Message.AsFailedOperation<Book>();

        oldBook.Data!.Title = entity.Title;
        oldBook.Data!.Author = entity.Author;
        oldBook.Data!.ISBN = entity.ISBN;
        oldBook.Data!.Status = entity.Status;

        var book = applicationContext.Books.Update(oldBook.Data!);

        if (book.State != EntityState.Modified)
            return "Book could not be updated".AsFailedOperation<Book>();

        return book.Entity.AsSuccessfulOperation();
    }

    public IOperationResult<Book> Delete(Book entity)
    {
        var book = applicationContext.Books.Remove(entity);

        if (book.State != EntityState.Deleted)
            return "Book could not be deleted".AsFailedOperation<Book>();

        return book.Entity.AsSuccessfulOperation();
    }

    public IOperationResult<Book> GetById(int id)
    {
        var book = applicationContext.Books.Find(id);

        if (book == null)
            return "Book could not be found".AsFailedOperation<Book>();

        return book!.AsSuccessfulOperation();
    }

    public IOperationResult<IList<Book>> GetAll()
    {
        var books = applicationContext.Books.ToList();
        if (books.Count == 0)
            return "No books found".AsFailedOperation<IList<Book>>();

        return books.AsSuccessfulOperation<IList<Book>>();
    }

    public IOperationResult<IList<Book>> GetByCondition(Func<Book, bool> condition)
    {
        var books = applicationContext.Books.Where(condition).ToList();

        if (books.Count == 0)
            return "No books found".AsFailedOperation<IList<Book>>();

        return books.AsSuccessfulOperation<IList<Book>>();
    }

    public void Dispose() => applicationContext.SaveChanges();

    public async ValueTask DisposeAsync() => await applicationContext.SaveChangesAsync();
}