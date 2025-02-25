using FluentAssertions;
using LibraryReservationManagementSystem.Models;
using LibraryReservationManagementSystem.OperationResults;
using LibraryReservationManagementSystem.Repositories;
using LibraryReservationManagementSystem.Services;
using Microsoft.Extensions.Logging;
using Moq;

namespace LibraryReservationManagementSystem.Tests.UnitTests;

[TestFixture]
public class LibraryReservationManagementServiceTests
{
    public ILibraryReservationManagementService Setup(
        Action<Mock<IRepository<Book>>>? bookRepoMoqOptions = null,
        Action<Mock<IRepository<Customer>>>? customerRepoMoqOptions = null,
        Action<Mock<IRepository<Reservation>>>? reservationRepoMoqOptions = null)
    {
        var bookRepositoryMoq = new Mock<IRepository<Book>>();
        var customerRepositoryMoq = new Mock<IRepository<Customer>>();
        var reservationRepositoryMoq = new Mock<IRepository<Reservation>>();

        if (bookRepoMoqOptions != null)
            bookRepoMoqOptions(bookRepositoryMoq);

        if (customerRepoMoqOptions != null)
            customerRepoMoqOptions(customerRepositoryMoq);

        if (reservationRepoMoqOptions != null)
            reservationRepoMoqOptions(reservationRepositoryMoq);

        var repositoryFactoryMoq = new Mock<IRepositoryFactory>();

        repositoryFactoryMoq.Setup(f => f.GetRepository<Book>()).Returns(bookRepositoryMoq.Object);
        repositoryFactoryMoq.Setup(f => f.GetRepository<Customer>()).Returns(customerRepositoryMoq.Object);
        repositoryFactoryMoq.Setup(f => f.GetRepository<Reservation>()).Returns(reservationRepositoryMoq.Object);

        var loggerMoq = new Mock<ILogger<LibraryReservationManagementService>>();
        return new LibraryReservationManagementService(repositoryFactoryMoq.Object, loggerMoq.Object);
    }


    #region RentABook

    [Test]
    public void RentABook_BookNotFound_ReturnsFailedOperation()
    {
        // Arrange
        var service = Setup(
            bookRepoMoqOptions: moq =>
            {
                moq.Setup(r => r.GetById(It.IsAny<int>())).Returns("Book not found".AsFailedOperation<Book>());
            },
            customerRepoMoqOptions: moq =>
            {
                moq.Setup(r => r.GetById(It.IsAny<int>())).Returns((new Customer
                {
                    FirstName = null,
                    LastName = null,
                    Email = null,
                    RegistrationDate = default
                }).AsSuccessfulOperation());
            });
        // Act
        var result = service.RentABook(1, 1);
        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Be("Book not found");
    }

    [Test]
    public void RentABook_CustomerNotFound_ReturnsFailedOperation()
    {
        // Arrange
        var service = Setup(
            bookRepoMoqOptions: moq => moq.Setup(r => r.GetById(It.IsAny<int>())).Returns((new Book
            {
                Title = null,
                Author = null,
                ISBN = null,
                Status = Book.StatusEnum.Available
            }).AsSuccessfulOperation()),
            customerRepoMoqOptions: moq => moq.Setup(r => r.GetById(It.IsAny<int>())).Returns("Customer not found".AsFailedOperation<Customer>()));
        // Act
        var result = service.RentABook(1, 1);
        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Be("Customer not found");
    }

    [Test]
    public void RentABook_BookNotAvailable_ReturnsFailedOperation()
    {
        // Arrange
        var service = Setup(
            bookRepoMoqOptions: moq => moq.Setup(r => r.GetById(It.IsAny<int>())).Returns((new Book
            {
                Title = null,
                Author = null,
                ISBN = null,
                Status = Book.StatusEnum.Unavailable
            }).AsSuccessfulOperation()),
            customerRepoMoqOptions: moq => moq.Setup(r => r.GetById(It.IsAny<int>())).Returns((new Customer
            {
                FirstName = null,
                LastName = null,
                Email = null,
                RegistrationDate = default
            }).AsSuccessfulOperation()));
        // Act
        var result = service.RentABook(1, 1);
        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Be("Book is not available");
    }

    [Test]
    public void RentABook_BookIsAvailable_ReturnsSuccessfulOperation()
    {
        // Arrange
        var customer = new Customer
        {
            FirstName = null,
            LastName = null,
            Email = null,
            RegistrationDate = default
        };
        var book = new Book
        {
            Title = null,
            Author = null,
            ISBN = null,
            Status = Book.StatusEnum.Available
        };
        var service = Setup(
            bookRepoMoqOptions: moq => moq.Setup(r => r.GetById(It.Is<int>(id => id == 1))).Returns(book.AsSuccessfulOperation()),
            customerRepoMoqOptions: moq => moq.Setup(r => r.GetById(It.Is<int>(id => id == 1))).Returns(customer.AsSuccessfulOperation()),
            reservationRepoMoqOptions: moq => moq.Setup(r => r.Add(It.Is<Reservation>(r =>
                    r.Book.Id == book.Id && r.Customer.Id == customer.Id && r.ReservationDate.Day == DateTime.Now.Day && r.ExpirationDate.Day == r.ReservationDate.AddDays(7).Day
                )
            )).Returns(new Reservation
            {
                Customer = customer,
                Book = book,
                ExpirationDate = default,
                ReservationDate = default
            }.AsSuccessfulOperation()));
        // Act
        var result = service.RentABook(1, 1);
        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().NotBeNull();
        book.Status.Should().Be(Book.StatusEnum.Unavailable);
    }

    #endregion

    #region ExtendReservation

    [Test]
    public void ExtendReservation_ReservationNotFound_ReturnsFailedOperation()
    {
        // Arrange
        var service = Setup(
            reservationRepoMoqOptions: moq => moq.Setup(r => r.GetById(It.IsAny<int>())).Returns("Reservation not found".AsFailedOperation<Reservation>()));
        // Act
        var result = service.ExtendReservation(1, DateTime.Now.AddDays(7));
        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Be("Reservation not found");
    }

    [Test]
    public void ExtendReservation_ReservationFound_ReturnsSuccessfulOperation()
    {
        // Arrange
        var reservation = new Reservation
        {
            Customer = new Customer
            {
                FirstName = null,
                LastName = null,
                Email = null,
                RegistrationDate = default
            },
            Book = new Book
            {
                Title = null,
                Author = null,
                ISBN = null,
                Status = Book.StatusEnum.Available
            },
            ExpirationDate = DateTime.Now.AddDays(7),
            ReservationDate = DateTime.Now
        };
        var service = Setup(
            reservationRepoMoqOptions: moq => moq.Setup(r => r.GetById(It.Is<int>(id => id == 1))).Returns(reservation.AsSuccessfulOperation()));
        // Act
        var result = service.ExtendReservation(1, DateTime.Now.AddDays(8));
        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().NotBeNull();
        reservation.ExpirationDate.Day.Should().Be(DateTime.Now.AddDays(8).Day);
    }

    #endregion

    #region GetReservations

    [Test]
    public void GetReservationsByCustomer_CustomerNotFound_ReturnsFailedOperation()
    {
        // Arrange
        var service = Setup(
            customerRepoMoqOptions: moq => moq.Setup(r => r.GetById(It.IsAny<int>())).Returns("Customer not found".AsFailedOperation<Customer>()));
        // Act
        var result = service.GetReservationsByCustomer(1);
        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Be("Customer not found");
    }

    [Test]
    public void GetReservationsByCustomer_CustomerFound_ReturnsSuccessfulOperation()
    {
        // Arrange
        var customer = new Customer
        {
            FirstName = null,
            LastName = null,
            Email = null,
            RegistrationDate = default
        };
        var reservations = new List<Reservation>
        {
            new Reservation
            {
                Customer = customer,
                Book = new Book
                {
                    Title = null,
                    Author = null,
                    ISBN = null,
                    Status = Book.StatusEnum.Available
                },
                ExpirationDate = DateTime.Now.AddDays(7),
                ReservationDate = DateTime.Now
            }
        };
        var service = Setup(
            customerRepoMoqOptions: moq => moq.Setup(r => r.GetById(It.Is<int>(id => id == 1))).Returns(customer.AsSuccessfulOperation()),
            reservationRepoMoqOptions: moq => moq.Setup(r => r.GetAll()).Returns(reservations.AsSuccessfulOperation<IList<Reservation>>()));
        // Act
        var result = service.GetReservationsByCustomer(1);
        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data.Should().HaveCount(1);
    }

    #endregion

}