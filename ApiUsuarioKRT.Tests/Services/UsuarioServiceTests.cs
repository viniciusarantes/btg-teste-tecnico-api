using ApiUsuarioKRT.Data.Interfaces;
using ApiUsuarioKRT.Models;
using ApiUsuarioKRT.ModelView.Request;
using ApiUsuarioKRT.ModelView.Response;
using ApiUsuarioKRT.Services;
using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using Moq;

namespace ApiUsuarioKRT.Tests.Services
{
    public class UsuarioServiceTests
    {
        private readonly Mock<IUsuarioRepository> _repositoryMock;
        private readonly IMemoryCache _cache;
        private readonly UsuarioService _service;

        public UsuarioServiceTests()
        {
            _repositoryMock = new Mock<IUsuarioRepository>();
            var options = new MemoryCacheOptions();
            _cache = new MemoryCache(options);

            _service = new UsuarioService(_repositoryMock.Object, _cache);
        }

        [Fact]
        public async Task GetUsuarioAsync_ExistingId_ReturnsUsuarioResponse()
        {
            // Arrange
            var usuarioId = Guid.NewGuid().ToString();
            var cacheKey = $"usuario_{usuarioId}";
            var usuarioMock = new Usuario
            {
                Id = usuarioId,
                NomeTitular = "Teste",
                Cpf = "123",
                StatusConta = StatusConta.Ativo,
            };
            _repositoryMock.Setup(r => r.GetUsuarioAsync(usuarioId)).ReturnsAsync(usuarioMock);

            // Act
            var result = await _service.GetUsuarioAsync(usuarioId);

            // Assert            
            result.Should().NotBeNull();
            result!.NomeTitular.Should().Be("Teste");
            _repositoryMock.Verify(r => r.GetUsuarioAsync(usuarioId), Times.Once);

            var isCached = _cache.TryGetValue(cacheKey, out UsuarioResponse? cacheValue);
            isCached.Should().BeTrue();

        }

        [Fact]
        public async Task GetUsuarioAsync_NonExistingId_ReturnsNull()
        {
            // Arrange
            var usuarioId = Guid.NewGuid().ToString();
            _repositoryMock.Setup(r => r.GetUsuarioAsync(usuarioId)).ReturnsAsync((Usuario?)null);

            // Act
            var result = await _service.GetUsuarioAsync(usuarioId);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetUsuarioAsync_ExistingCachedId_ReturnsCachedUsuarioResponse()
        {
            // Arrange
            var usuarioId = Guid.NewGuid().ToString();
            var cacheKey = $"usuario_{usuarioId}";
            _repositoryMock.Setup(r => r.GetUsuarioAsync(usuarioId)).ReturnsAsync((Usuario?)null);

            var outValue = new UsuarioResponse
            {
                Id = usuarioId,
                NomeTitular = "Cached Test",
                Cpf = "123",
                StatusConta = "Ativo",
            };
            _cache.Set(cacheKey, outValue);

            // Act
            var result = await _service.GetUsuarioAsync(usuarioId);

            // Assert
            result.Should().NotBeNull();
            result!.NomeTitular.Should().Be("Cached Test");
            _repositoryMock.Verify(r => r.GetUsuarioAsync(usuarioId), Times.Never);
        }

        [Fact]
        public async Task GetUsuarioListAsync_ExistingData_ReturnsUsuarioList()
        {
            // Arrange
            _cache.Remove("usuario_list");
            var usuarioMock = new Usuario
            {
                Id = Guid.NewGuid().ToString(),
                NomeTitular = "Teste",
                Cpf = "123",
                StatusConta = StatusConta.Ativo,
            };
            var mockList = new List<Usuario> { usuarioMock };
            _repositoryMock.Setup(r => r.GetUsuarioListAsync()).ReturnsAsync(mockList);

            // Act
            var result = await _service.GetUsuarioListAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(1);
            _repositoryMock.Verify(r => r.GetUsuarioListAsync(), Times.Once);

            var isCached = _cache.TryGetValue("usuario_list", out List<UsuarioResponse>? cacheValue);
            isCached.Should().BeTrue();
        }

        [Fact]
        public async Task GetUsuarioListAsync_NonExistingData_ReturnsEmptyList()
        {
            // Arrange
            _cache.Remove("usuario_list");
            var mockList = new List<Usuario>();
            _repositoryMock.Setup(r => r.GetUsuarioListAsync()).ReturnsAsync(mockList);

            // Act
            var result = await _service.GetUsuarioListAsync();

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task GetUsuarioListAsync_ExistingCachedList_ReturnsCachedList()
        {
            // Arrange
            _cache.Remove("usuario_list");
            var emptyList = new List<Usuario>();
            _repositoryMock.Setup(r => r.GetUsuarioListAsync()).ReturnsAsync(emptyList);

            var usuarioMock = new UsuarioResponse
            {
                Id = Guid.NewGuid().ToString(),
                NomeTitular = "Teste",
                Cpf = "123",
                StatusConta = "Ativo",
            };
            var outValue = new List<UsuarioResponse> { usuarioMock };
            _cache.Set("usuario_list", outValue);

            // Act
            var result = await _service.GetUsuarioListAsync();

            // Assert
            result.Should().NotBeNull();
            result!.Should().HaveCount(1);
            _repositoryMock.Verify(r => r.GetUsuarioListAsync(), Times.Never);
        }

        [Fact]
        public async Task UpdateUsuarioAsync_OnlyNameProvided_UpdatesNameOnlyAndClearRequest()
        {
            // Arrange
            var usuarioId = Guid.NewGuid().ToString();
            var cacheKey = $"usuario_{usuarioId}";
            var usuarioMock = new Usuario
            {
                Id = usuarioId,
                NomeTitular = "Teste",
                Cpf = "123",
                StatusConta = StatusConta.Ativo,
            };
            _cache.Set(cacheKey, usuarioMock);
            _cache.Set("usuario_list", new List<Usuario> { usuarioMock });
            var request = new UpdateUsuarioRequest
            {
                NomeTitular = "Renomeado"
            };
            _repositoryMock.Setup(r => r.GetUsuarioAsync(usuarioId)).ReturnsAsync(usuarioMock);
            _repositoryMock.Setup(r => r.UpdateUsuarioAsync(It.IsAny<Usuario>())).ReturnsAsync(true);

            object? outValue = null;

            // Act
            var result = await _service.UpdateUsuarioAsync(usuarioId, request);

            // Assert
            result!.NomeTitular.Should().Be("Renomeado");
            result!.Cpf.Should().Be("123");
            _repositoryMock.Verify(r => r.GetUsuarioAsync(usuarioId), Times.Once);
            _repositoryMock.Verify(r => r.UpdateUsuarioAsync(It.IsAny<Usuario>()), Times.Once);

            var isCached = _cache.TryGetValue(cacheKey, out var _);
            var isCachedList = _cache.TryGetValue("usuario_list", out var _);
            isCached.Should().BeFalse();
            isCachedList.Should().BeFalse();
        }

        [Fact]
        public async Task CreateUsuarioAsync_ValidRequest_ShouldReturnResponseAndClearCache()
        {
            // Arrange
            _cache.Remove("usuario_list");
            var request = new CreateUsuarioRequest
            {
                NomeTitular = "Created Test",
                Cpf = "123",
            };

            var inserted = new Usuario
            {
                Id = Guid.NewGuid().ToString(),
                NomeTitular = request.NomeTitular,
                Cpf = request.Cpf,
                StatusConta = StatusConta.Ativo,
            };

            _repositoryMock.Setup(r => r.CreateUsuarioAsync(It.IsAny<Usuario>())).ReturnsAsync(inserted);
            _cache.Set("usuario_list", new List<UsuarioResponse>());

            // Act
            var result = await _service.CreateUsuarioAsync(request);

            // Assert
            result.Should().NotBeNull();
            result.NomeTitular.Should().Be("Created Test");
            result.Id.Should().NotBeEmpty();
            _repositoryMock.Verify(c => c.CreateUsuarioAsync(It.IsAny<Usuario>()), Times.Once);

            var isCached = _cache.TryGetValue("usuario_list", out var _);
            isCached.Should().BeFalse();
        }

        [Fact]
        public async Task DeleteUsuarioAsync_ExistingId_ShouldReturnTrueAndClearCache()
        {
            // Arrange
            var usuarioId = Guid.NewGuid().ToString();

            _repositoryMock.Setup(r => r.DeleteUsuarioAsync(usuarioId)).ReturnsAsync(true);
            _cache.Set("usuario_list", new List<UsuarioResponse>());

            // Act
            var result = await _service.DeleteUsuarioAsync(usuarioId);

            // Assert
            result.Should().BeTrue();
            _repositoryMock.Verify(c => c.DeleteUsuarioAsync(usuarioId), Times.Once);

            var isCached = _cache.TryGetValue("usuario_list", out var _);
            isCached.Should().BeFalse();
        }
    }
}
