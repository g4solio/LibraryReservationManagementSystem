using LibraryReservationManagementSystem.DbContexts;
using LibraryReservationManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryReservationManagementSystem.Repositories;

public class BookRepository(NpgApplicationContext applicationContext) : IRepository<Book>
{
    public IOperationResult<Book> Add(Book entity)
    {
        var book = applicationContext.Books.Add(entity);
        applicationContext.SaveChanges();

        if (book.State != EntityState.Added)
            "Book could not be added".AsFailedOperation<Book>();


        return book.Entity.AsSuccessfulOperation();
    }

    public IOperationResult<Book> Update(Book entity)
    {
        var book = applicationContext.Books.Update(entity);
        applicationContext.SaveChanges();

        if (book.State != EntityState.Modified)
            "Book could not be updated".AsFailedOperation<Book>();

        return book.Entity.AsSuccessfulOperation();
    }

    public IOperationResult<Book> Delete(Book entity)
    {
        var book = applicationContext.Books.Remove(entity);
        applicationContext.SaveChanges();
        
        if (book.State != EntityState.Deleted)
            "Book could not be deleted".AsFailedOperation<Book>();
        
        return book.Entity.AsSuccessfulOperation();
    }

    public IOperationResult<Book> GetById(int id)
    {
        var book = applicationContext.Books.Find(id);

        if (book == null)
            "Book could not be found".AsFailedOperation<Book>();
        
        return book!.AsSuccessfulOperation();
    }

    public IOperationResult<IList<Book>> GetAll()
    {
        var books = applicationContext.Books.ToList();
        if (books.Count == 0)
            "No books found".AsFailedOperation<IEnumerable<Book>>();

        return books.AsSuccessfulOperation<IList<Book>>();
    }

    public IOperationResult<IList<Book>> GetByCondition(Func<Book, bool> condition)
    {
        var books = applicationContext.Books.Where(condition).ToList();

        if (books.Count == 0)
            "No books found".AsFailedOperation<IEnumerable<Book>>();

        return books.AsSuccessfulOperation<IList<Book>>();
    }
}