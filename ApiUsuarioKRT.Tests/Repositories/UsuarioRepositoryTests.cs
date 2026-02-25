using ApiUsuarioKRT.Data.Interfaces;
using ApiUsuarioKRT.Data.Repositories;
using ApiUsuarioKRT.Models;
using Dapper;
using FluentAssertions;
using Microsoft.Data.Sqlite;
using Moq;
using System.Data;

namespace ApiUsuarioKRT.Tests.Repositories
{
    public class UsuarioRepositoryTests
    {
        private readonly IDbConnection _connection;
        private readonly Mock<IDbConnectionFactory> _factoryMock;
        private readonly UsuarioRepository _repository;

        public UsuarioRepositoryTests()
        {
            _connection = new SqliteConnection("Data Source=:memory:");
            _connection.Open();

            var sql = @"
                CREATE TABLE usuario (
                    id TEXT PRIMARY KEY NOT NULL,
                    nomeTitular TEXT NOT NULL,
                    cpf TEXT NOT NULL,
                    statusConta INTEGER NOT NULL,
                    isDeleted INTEGER NOT NULL DEFAULT 0
                )";
            _connection.Execute(sql);

            _factoryMock = new Mock<IDbConnectionFactory>();
            _factoryMock.Setup(f => f.CreateConnection()).Returns(_connection);

            _repository = new UsuarioRepository(_factoryMock.Object);
        }

        [Fact]
        public async Task CreateUsuarioAsync_InsertUsuario_ReturnsUser()
        {
            // Arrange
            var usuarioId = Guid.NewGuid().ToString();
            var usuario = new Usuario
            {
                Id = usuarioId,
                NomeTitular = "Teste",
                Cpf = "123",
                StatusConta = StatusConta.Ativo,
            };

            // Act
            var result = await _repository.CreateUsuarioAsync(usuario);

            // Assert
            result.Should().NotBeNull();
            result!.NomeTitular.Should().Be("Teste");
        }

        [Fact]
        public async Task SoftDeleteUsuarioAsync_ChangeDeleteFlag_ReturnsTrue()
        {
            // Arrange
            var usuarioId = Guid.NewGuid().ToString();
            await _connection.ExecuteAsync("INSERT INTO usuario (id, nomeTitular, cpf, statusConta) VALUES (@usuarioId, 'Teste', '123', 1)", new { usuarioId });

            // Act
            var result = await _repository.SoftDeleteUsuarioAsync(usuarioId);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task DeleteUsuarioAsync_DeleteRegister_ReturnsTrue()
        {
            // Arrange
            var usuarioId = Guid.NewGuid().ToString();
            await _connection.ExecuteAsync("INSERT INTO usuario (id, nomeTitular, cpf, statusConta) VALUES (@usuarioId, 'Teste', '123', 1)", new { usuarioId });

            // Act
            var result = await _repository.DeleteUsuarioAsync(usuarioId);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task GetUsuarioAsync_ExistingId_ReturnsUsuario()
        {
            // Arrange
            var usuarioId = Guid.NewGuid().ToString();
            await _connection.ExecuteAsync("INSERT INTO usuario (id, nomeTitular, cpf, statusConta) VALUES (@usuarioId, 'Teste', '123', 1)", new { usuarioId });

            // Act
            var result = await _repository.GetUsuarioAsync(usuarioId);

            // Assert
            result.Should().NotBeNull();
            result!.Id.Should().Be(usuarioId);
        }

        [Fact]
        public async Task GetUsuarioAsync_NonExistingId_ReturnsNull()
        {
            // Arrange
            var usuarioId = Guid.NewGuid().ToString();

            // Act
            var result = await _repository.GetUsuarioAsync(usuarioId);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetUsuarioListAsync_ExistingData_ReturnsUsuario()
        {
            // Arrange
            var usuarioId = Guid.NewGuid().ToString();
            await _connection.ExecuteAsync("INSERT INTO usuario (id, nomeTitular, cpf, statusConta) VALUES (@usuarioId, 'Teste', '123', 1)", new { usuarioId });

            // Act
            var result = await _repository.GetUsuarioListAsync();

            // Assert
            result.Should().NotBeNull();
            result!.Should().HaveCount(1);
        }

        [Fact]
        public async Task GetUsuarioListAsync_NonExistingData_ReturnsEmptyList()
        {
            // Arrange
            await _connection.ExecuteAsync("DELETE FROM usuario");

            // Act
            var result = await _repository.GetUsuarioListAsync();

            // Assert
            result.Should().HaveCount(0);
        }
    }
}
