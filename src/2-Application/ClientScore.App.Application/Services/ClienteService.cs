using AutoMapper;
using ClientScore.App.Domain.Interfaces.Factorys;
using ClientScore.App.Domain.Interfaces.Repositories;
using ClientScore.App.Domain.Interfaces.Services;
using ClientScore.App.Domain.Models;
using ClientScore.App.Domain.ViewModels;
using Microsoft.Extensions.Logging;

namespace ClientScore.App.Application.Services;

public class ClienteService : IClienteService
{
    private readonly ILogger<ClienteService> _logger;
    private readonly IClienteRepository _clienteRepository;
    private readonly IScoreCalculatorFactory _scoreCalculator;
    private readonly IMapper _mapper;

    public ClienteService(ILogger<ClienteService> logger,
                          IClienteRepository clienteRepository,
                          IScoreCalculatorFactory scoreCalculator,
                          IMapper mapper)
    {
        _logger = logger;
        _clienteRepository = clienteRepository;
        _scoreCalculator = scoreCalculator;
        _mapper = mapper;
    }

    public async Task<ResponseService<IEnumerable<Cliente>>> GetListAsync()
    {
        var response = new ResponseService<IEnumerable<Cliente>>();
		
        try
		{
            response.Resposta = await _clienteRepository.GetAllAsync();

            return response;
		}
		catch (Exception ex)
		{
            _logger.LogError($"Erro [ClienteService][GetListAsync] {ex.Message}");
            response.Sucesso = false;
            response.MensagemErro = ex.Message;
        }

        return response;
    }

    public async Task<ResponseService<object>> InsertAsync(ClienteViewModel clienteModel)
    {
        var response = new ResponseService<object>();

        try
        {
            var cliente = _mapper.Map<Cliente>(clienteModel);

            cliente.Score = _scoreCalculator.CalcularScore(cliente.DataNascimento, cliente.RendimentoAnual);

            await _clienteRepository.InsertAsync(cliente);

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Erro [ClienteService][InsertAsync] {ex.Message}");
            response.Sucesso = false;
            response.MensagemErro = ex.Message;
        }

        return response;
    }





    #region METHODS




    #endregion
}
