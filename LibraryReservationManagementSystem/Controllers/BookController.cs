﻿using LibraryReservationManagementSystem.Models;
using LibraryReservationManagementSystem.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace LibraryReservationManagementSystem.Controllers;

[ApiController]
[Route("[controller]")]
public class BookController(RepositoryFactory repositoryFactory, ILogger<BookController> logger) : ControllerBase
{
    private readonly ILogger<BookController> _logger = logger;

    [HttpGet(Name = "GetBooks")]
    public IActionResult Get()
    {
        using var repository = repositoryFactory.GetRepository<Book>();
        var books = repository.GetAll();
        return books.IsSuccess
            ? Ok(books.Data)
            : NotFound(books.Message);
    }

    [HttpGet("{id}", Name = "GetBook")]
    public IActionResult Get(int id)
    {
        using var repository = repositoryFactory.GetRepository<Book>();
        var book = repository.GetById(id);
        return book.IsSuccess
            ? Ok(book.Data)
            : NotFound(book.Message);
    }

    #region Context

    public class CreateBookRequest
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string ISBN { get; set; }
        public Book.StatusEnum Status { get; set; }

        public Book ToBook()
        {
            return new Book
            {
                Title = Title,
                Author = Author,
                ISBN = ISBN,
                Status = Status
            };
        }
    }

    #endregion

    [HttpPost(Name = "CreateBook")]
    public IActionResult Post(CreateBookRequest book)
    {
        using var repository = repositoryFactory.GetRepository<Book>();
        var result = repository.Add(book.ToBook());
        return result.IsSuccess
            ? CreatedAtRoute("GetBook", new { id = result.Data.Id }, result.Data)
            : BadRequest(result.Message);
    }

    #region Context

    public class UpdateBookRequest
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string ISBN { get; set; }
        public Book.StatusEnum Status { get; set; }
        public Book ToBook()
        {
            return new Book
            {
                Id = Id,
                Title = Title,
                Author = Author,
                ISBN = ISBN,
                Status = Status
            };
        }
    }

    #endregion

    [HttpPut("{id}", Name = "UpdateBook")]
    public IActionResult Put(int id, UpdateBookRequest book)
    {
        using var repository = repositoryFactory.GetRepository<Book>();
        var result = repository.Update(book.ToBook());
        return result.IsSuccess
            ? Ok(result.Data)
            : BadRequest(result.Message);
    }

    [HttpDelete("{id}", Name = "DeleteBook")]
    public IActionResult Delete(int id)
    {
        using var repository = repositoryFactory.GetRepository<Book>();
        var entityToDelete = repository.GetById(id);

        if (!entityToDelete.IsSuccess || entityToDelete.Data == null)
            return NotFound(entityToDelete.Message);

        var result = repository.Delete(entityToDelete.Data);
        return result.IsSuccess
            ? Ok(result.Data)
            : BadRequest(result.Message);
    }
}