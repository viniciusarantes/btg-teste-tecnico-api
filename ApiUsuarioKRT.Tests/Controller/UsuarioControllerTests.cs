using ApiUsuarioKRT.Controllers;
using ApiUsuarioKRT.ModelView.Request;
using ApiUsuarioKRT.ModelView.Response;
using ApiUsuarioKRT.Services.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace ApiUsuarioKRT.Tests.Controller
{
    public class UsuarioControllerTests
    {
        private readonly Mock<IUsuarioService> _serviceMock;
        private readonly UsuarioController _controller;

        public UsuarioControllerTests()
        {
            _serviceMock = new Mock<IUsuarioService>();
            _controller = new UsuarioController(Mock.Of<ILogger<UsuarioController>>(), _serviceMock.Object);
        }

        [Fact]
        public async Task GetUsuarioListAsync_UsuarioExists_ReturnsOk()
        {
            // Arrange
            var usuarioList = new List<UsuarioResponse>();
            _serviceMock.Setup(s => s.GetUsuarioListAsync()).ReturnsAsync(usuarioList);

            // Act
            var result = await _controller.GetUsuarioList();

            // Assert
            result.Result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task GetUsuarioListAsync_ThrowsException_Returns500()
        {
            // Arrange
            _serviceMock.Setup(s => s.GetUsuarioListAsync()).ThrowsAsync(new Exception("Teste"));

            // Act
            var result = await _controller.GetUsuarioList();

            // Assert
            var statusCodeResult = result.Result.Should().BeOfType<ObjectResult>().Subject;
            statusCodeResult.StatusCode.Should().Be(500);
        }

        [Fact]
        public async Task GetUsuarioByIdAsync_UsuarioExists_ReturnsOk()
        {
            // Arrange
            var usuario = new UsuarioResponse();
            _serviceMock.Setup(s => s.GetUsuarioAsync("123")).ReturnsAsync(usuario);

            // Act
            var result = await _controller.GetUsuarioDetail("123");

            // Assert
            result.Result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task GetUsuarioByIdAsync_UsuarioNotFound_ReturnsNotFound()
        {
            // Arrange
            _serviceMock.Setup(s => s.GetUsuarioAsync("123")).ReturnsAsync((UsuarioResponse?)null);

            // Act
            var result = await _controller.GetUsuarioDetail("123");

            // Assert
            result.Result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task GetUsuarioByIdAsync_ThrowsException_Returns500()
        {
            // Arrange
            _serviceMock.Setup(s => s.GetUsuarioAsync("123")).ThrowsAsync(new Exception("Teste"));

            // Act
            var result = await _controller.GetUsuarioDetail("123");

            // Assert
            var statusCodeResult = result.Result.Should().BeOfType<ObjectResult>().Subject;
            statusCodeResult.StatusCode.Should().Be(500);
        }

        [Fact]
        public async Task PostUsuarioAsync_CreateUsuario_Returns201()
        {
            // Arrange
            var usuario = new UsuarioResponse();
            _serviceMock.Setup(s => s.CreateUsuarioAsync(It.IsAny<CreateUsuarioRequest>())).ReturnsAsync(usuario);

            // Act
            var result = await _controller.CreateUsuario(new CreateUsuarioRequest());

            // Assert
            var createdResult = result.Result.Should().BeOfType<CreatedAtActionResult>().Subject;
            createdResult.StatusCode.Should().Be(201);
            createdResult.ActionName.Should().Be("GetUsuarioDetail");
        }

        [Fact]
        public async Task PostUsuarioAsync_ThrowsException_Returns500()
        {
            // Arrange
            var usuario = new UsuarioResponse();
            _serviceMock.Setup(s => s.CreateUsuarioAsync(It.IsAny<CreateUsuarioRequest>())).ThrowsAsync(new Exception("Teste"));

            // Act
            var result = await _controller.CreateUsuario(new CreateUsuarioRequest());

            // Assert
            var statusCodeResult = result.Result.Should().BeOfType<ObjectResult>().Subject;
            statusCodeResult.StatusCode.Should().Be(500);
        }


        [Fact]
        public async Task PatchUsuarioAsync_UsuarioChanged_ReturnsOk()
        {
            // Arrange
            var usuario = new UsuarioResponse();
            _serviceMock.Setup(s => s.UpdateUsuarioAsync("123", It.IsAny<UpdateUsuarioRequest>())).ReturnsAsync(usuario);

            // Act
            var result = await _controller.UpdateUsuario("123", new UpdateUsuarioRequest());

            // Assert
            result.Result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task PatchUsuarioAsync_UsuarioNotFound_ReturnsNotFound()
        {
            // Arrange
            _serviceMock.Setup(s => s.UpdateUsuarioAsync("123", It.IsAny<UpdateUsuarioRequest>())).ReturnsAsync((UsuarioResponse?)null);

            // Act
            var result = await _controller.UpdateUsuario("123", new UpdateUsuarioRequest());

            // Assert
            result.Result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task PatchUsuarioAsync_ThrowsException_Returns500()
        {
            // Arrange
            _serviceMock.Setup(s => s.UpdateUsuarioAsync("123", It.IsAny<UpdateUsuarioRequest>())).ThrowsAsync(new Exception("Teste"));

            // Act
            var result = await _controller.UpdateUsuario("123", new UpdateUsuarioRequest());

            // Assert
            var statusCodeResult = result.Result.Should().BeOfType<ObjectResult>().Subject;
            statusCodeResult.StatusCode.Should().Be(500);
        }

        [Fact]
        public async Task DeleteUsuarioAsync_UsuarioDeleted_ReturnsNoContent()
        {
            // Arrange
            var usuario = new UsuarioResponse();
            _serviceMock.Setup(s => s.DeleteUsuarioAsync("123")).ReturnsAsync(true);

            // Act
            var result = await _controller.DeleteUsuario("123");

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task DeleteUsuarioAsync_UsuarioNotFound_ReturnsNotFound()
        {
            // Arrange
            _serviceMock.Setup(s => s.DeleteUsuarioAsync("123")).ReturnsAsync(false);

            // Act
            var result = await _controller.DeleteUsuario("123");

            // Assert
            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task DeleteUsuarioAsync_ThrowsException_Returns500()
        {
            // Arrange
            _serviceMock.Setup(s => s.DeleteUsuarioAsync("123")).ThrowsAsync(new Exception("Teste"));

            // Act
            var result = await _controller.DeleteUsuario("123");

            // Assert
            var statusCodeResult = result.Should().BeOfType<ObjectResult>().Subject;
            statusCodeResult.StatusCode.Should().Be(500);
        }
    }
}
