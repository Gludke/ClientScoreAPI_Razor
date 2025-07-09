using Moq;
using AutoMapper;
using Microsoft.Extensions.Logging;
using ClientScore.App.Application.Services;
using ClientScore.App.Domain.Interfaces.Repositories;
using ClientScore.App.Domain.Models;
using ClientScore.App.Domain.Interfaces.Factorys;
using ClientScore.App.Domain.ViewModels;

namespace ClientScore.App.Tests.Application.Services
{
    public class ClienteServiceTests
    {
        private readonly Mock<IClienteRepository> _clienteRepositoryMock;
        private readonly Mock<ILogger<ClienteService>> _loggerMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IScoreCalculatorFactory> _scoreCalculatorMock;

        private readonly ClienteService _clienteService;

        public ClienteServiceTests()
        {
            _clienteRepositoryMock = new Mock<IClienteRepository>();
            _loggerMock = new Mock<ILogger<ClienteService>>();
            _mapperMock = new Mock<IMapper>();
            _scoreCalculatorMock = new Mock<IScoreCalculatorFactory>();

            _clienteService = new ClienteService(
                _loggerMock.Object,
                _clienteRepositoryMock.Object,
                _scoreCalculatorMock.Object,
                _mapperMock.Object
            );
        }




        #region GetListAsync()

        [Fact(DisplayName = "Deve retornar lista de clientes com sucesso")]
        public async Task GetListAsync_DeveRetornarSucesso()
        {
            // Arrange
            var clientesMock = new List<Cliente>
            {
                new Cliente { Id = 1, Nome = "Cliente A" },
                new Cliente { Id = 2, Nome = "Cliente B" }
            };

            _clienteRepositoryMock
                .Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(clientesMock);

            // Act
            var resultado = await _clienteService.GetListAsync();

            // Assert
            Assert.True(resultado.Sucesso);
            Assert.NotNull(resultado.Resposta);
            Assert.Equal(2, resultado.Resposta.Count());
            Assert.Null(resultado.MensagemErro);
        }

        [Fact(DisplayName = "Deve retornar resposta com sucesso (repository retornando nulo)")]
        public async Task GetListAsync_DeveRetornarSucessoQuandoRepositoryRetornaNulo()
        {
            // Arrange
            _clienteRepositoryMock
                .Setup(repo => repo.GetAllAsync())
                .ReturnsAsync((List<Cliente>?)null);

            // Act
            var resultado = await _clienteService.GetListAsync();

            // Assert
            Assert.True(resultado.Sucesso);
            Assert.Null(resultado.Resposta);
            Assert.Null(resultado.MensagemErro);
        }

        [Fact(DisplayName = "Deve capturar exceção e retornar falha no response")]
        public async Task GetListAsync_DeveRetornalErroAoCapturarExcecao()
        {
            // Arrange
            _clienteRepositoryMock
                .Setup(repo => repo.GetAllAsync())
                .ThrowsAsync(new Exception("Erro de banco"));

            // Act
            var resultado = await _clienteService.GetListAsync();

            // Assert
            Assert.False(resultado.Sucesso);
            Assert.Null(resultado.Resposta);
            Assert.Equal("Erro de banco", resultado.MensagemErro);
        }

        #endregion


        [Fact(DisplayName = "Deve inserir cliente com sucesso")]
        public async Task InsertAsync_DeveRetornarSucesso()
        {
            // Arrange
            var clienteViewModel = new ClienteViewModel
            {
                Nome = "João",
                DataNascimento = new DateTime(1990, 1, 1),
                CPF = "12345678901",
                Email = "joao@example.com",
                RendimentoAnual = 90000,
                Telefone = "999999999",
                DDD = "11",
                Endereco = new EnderecoViewModel
                {
                    Estado = "SP",
                    Cidade = "São Paulo",
                    Rua = "Rua A",
                    CEP = "01001000",
                    Numero = "123",
                    Complemento = "Apto"
                }
            };

            var clienteMapeado = new Cliente();

            _mapperMock.Setup(m => m.Map<Cliente>(clienteViewModel)).Returns(clienteMapeado);
            _scoreCalculatorMock.Setup(c => c.CalcularScore(It.IsAny<DateTime>(), It.IsAny<decimal>())).Returns(300);
            _clienteRepositoryMock.Setup(r => r.InsertAsync(clienteMapeado)).Returns(Task.CompletedTask);

            // Act
            var result = await _clienteService.InsertAsync(clienteViewModel);

            // Assert
            Assert.True(result.Sucesso);
            Assert.Null(result.MensagemErro);
        }

        [Fact(DisplayName = "Deve retornar erro quando ocorrer exceção")]
        public async Task InsertAsync_DeveRetornarErro()
        {
            // Arrange
            var clienteViewModel = new ClienteViewModel();

            _mapperMock.Setup(m => m.Map<Cliente>(It.IsAny<ClienteViewModel>())).Throws(new Exception("Erro de mapeamento"));

            // Act
            var result = await _clienteService.InsertAsync(clienteViewModel);

            // Assert
            Assert.False(result.Sucesso);
            Assert.Equal("Erro de mapeamento", result.MensagemErro);
        }

    }
}
