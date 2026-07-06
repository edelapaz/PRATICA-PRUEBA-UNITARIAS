using Microsoft.AspNetCore.Mvc;
using Moq;
using PruebasUnitarias.Api.Application;
using PruebasUnitarias.Api.Controllers;
using PruebasUnitarias.Api.Domain;

namespace PruebasUnitarias.Tests;

public sealed class UsersControllerTests
{
    [Fact]
    public async Task GetAsync_CuandoExistenUsuarios_RetornaOkConTresUsuarios()
    {
        // Arrange
        var users = new List<User>
        {
            new() { Id = 1, Name = "John", Email = "john@test.com" },
            new() { Id = 2, Name = "Jane", Email = "jane@test.com" },
            new() { Id = 3, Name = "Bob", Email = "bob@test.com" }
        };

        var userServiceMock = new Mock<IUserService>();
        userServiceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(users);

        var controller = new UsersController(userServiceMock.Object);

        // Act
        var result = await controller.GetAsync();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedUsers = Assert.IsAssignableFrom<IReadOnlyList<User>>(okResult.Value);
        Assert.Equal(3, returnedUsers.Count);
        userServiceMock.Verify(s => s.GetAllAsync(), Times.Once());
    }

    [Fact]
    public async Task GetByIdAsync_CuandoUsuarioExiste_RetornaOkConUsuarioIdUno()
    {
        // Arrange
        var user = new User { Id = 1, Name = "John", Email = "john@test.com" };
        var userServiceMock = new Mock<IUserService>();
        userServiceMock.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(user);

        var controller = new UsersController(userServiceMock.Object);

        // Act
        var result = await controller.GetByIdAsync(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedUser = Assert.IsType<User>(okResult.Value);
        Assert.Equal(1, returnedUser.Id);
        userServiceMock.Verify(s => s.GetByIdAsync(It.Is<int>(id => id == 1)), Times.Once());
    }

    [Fact]
    public async Task GetByIdAsync_CuandoUsuarioNoExiste_RetornaNotFound()
    {
        // Arrange
        var userServiceMock = new Mock<IUserService>();
        userServiceMock.Setup(s => s.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((User?)null);

        var controller = new UsersController(userServiceMock.Object);

        // Act
        var result = await controller.GetByIdAsync(10);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task CreateAsync_CuandoDatosValidos_RetornaCreatedAtActionYLlamaServicioUnaVez()
    {
        // Arrange
        var inputUser = new User { Name = "John", Email = "john@test.com" };
        var createdUser = new User { Id = 1, Name = "John", Email = "john@test.com" };

        var userServiceMock = new Mock<IUserService>();
        userServiceMock.Setup(s => s.CreateAsync(It.IsAny<User>())).ReturnsAsync(createdUser);

        var controller = new UsersController(userServiceMock.Object);

        // Act
        var result = await controller.CreateAsync(inputUser);

        // Assert
        var createdAtResult = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal(nameof(UsersController.GetByIdAsync), createdAtResult.ActionName);
        userServiceMock.Verify(s => s.CreateAsync(It.IsAny<User>()), Times.Once());
    }

    [Fact]
    public async Task CreateAsync_CuandoEmailEsJohnTestCom_VerificaParametroConItIs()
    {
        // Arrange
        var inputUser = new User { Name = "John", Email = "john@test.com" };
        var createdUser = new User { Id = 1, Name = "John", Email = "john@test.com" };

        var userServiceMock = new Mock<IUserService>();
        userServiceMock.Setup(s => s.CreateAsync(It.IsAny<User>())).ReturnsAsync(createdUser);

        var controller = new UsersController(userServiceMock.Object);

        // Act
        _ = await controller.CreateAsync(inputUser);

        // Assert
        userServiceMock.Verify(
            s => s.CreateAsync(It.Is<User>(u => u.Email == "john@test.com")),
            Times.Once());
    }

    [Fact]
    public async Task CreateAsync_CuandoSeEjecuta_CallbackCuentaUnaSolaLlamada()
    {
        // Arrange
        var inputUser = new User { Name = "John", Email = "john@test.com" };
        var createdUser = new User { Id = 1, Name = "John", Email = "john@test.com" };
        var callCount = 0;

        var userServiceMock = new Mock<IUserService>();
        userServiceMock
            .Setup(s => s.CreateAsync(It.IsAny<User>()))
            .Callback(() => callCount++)
            .ReturnsAsync(createdUser);

        var controller = new UsersController(userServiceMock.Object);

        // Act
        _ = await controller.CreateAsync(inputUser);

        // Assert
        Assert.Equal(1, callCount);
    }

    [Fact]
    public async Task DeleteAsync_CuandoUsuarioExiste_RetornaNoContentYEjecutaDelete()
    {
        // Arrange
        var existingUser = new User { Id = 1, Name = "John", Email = "john@test.com" };
        var userServiceMock = new Mock<IUserService>();
        userServiceMock.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(existingUser);
        userServiceMock.Setup(s => s.DeleteAsync(1)).Returns(Task.CompletedTask);

        var controller = new UsersController(userServiceMock.Object);

        // Act
        var result = await controller.DeleteAsync(1);

        // Assert
        Assert.IsType<NoContentResult>(result);
        userServiceMock.Verify(s => s.DeleteAsync(It.Is<int>(id => id == 1)), Times.Once());
    }

    [Fact]
    public async Task DeleteAsync_CuandoUsuarioNoExiste_RetornaNotFoundYNoEjecutaDelete()
    {
        // Arrange
        var userServiceMock = new Mock<IUserService>();
        userServiceMock.Setup(s => s.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((User?)null);

        var controller = new UsersController(userServiceMock.Object);

        // Act
        var result = await controller.DeleteAsync(404);

        // Assert
        Assert.IsType<NotFoundResult>(result);
        userServiceMock.Verify(s => s.DeleteAsync(It.IsAny<int>()), Times.Never());
    }

    [Fact]
    public async Task FlujoCompleto_GetByIdCreateGetAll_CadaMetodoSeInvocaUnaVez()
    {
        // Arrange
        var existingUser = new User { Id = 1, Name = "Eliazar de la paz", Email = "eliazar.delapaz@micm.gob.do" };
        var inputUser = new User { Name = "Juan Miguel Jiménez Torres", Email = "miguel.jimenez@micm.gob.do" };
        var createdUser = new User { Id = 2, Name = "Jabin Beriguete Medina", Email = "jabin.beriguete@micm.gob.do" };
        var allUsers = new List<User> { existingUser, createdUser };

        var userServiceMock = new Mock<IUserService>();
        userServiceMock.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(existingUser);
        userServiceMock.Setup(s => s.CreateAsync(It.IsAny<User>())).ReturnsAsync(createdUser);
        userServiceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(allUsers);

        var controller = new UsersController(userServiceMock.Object);

        // Act
        var byIdResult = await controller.GetByIdAsync(1);
        var createResult = await controller.CreateAsync(inputUser);
        var getAllResult = await controller.GetAsync();

        // Assert
        _ = Assert.IsType<OkObjectResult>(byIdResult);
        _ = Assert.IsType<CreatedAtActionResult>(createResult);
        var okAllResult = Assert.IsType<OkObjectResult>(getAllResult);
        var users = Assert.IsAssignableFrom<IReadOnlyList<User>>(okAllResult.Value);
        Assert.Equal(2, users.Count);

        userServiceMock.Verify(s => s.GetByIdAsync(It.Is<int>(id => id == 1)), Times.Once());
        userServiceMock.Verify(s => s.CreateAsync(It.IsAny<User>()), Times.Once());
        userServiceMock.Verify(s => s.GetAllAsync(), Times.Once());
    }
}
