using ClientScore.App.Domain.ViewModels;
using ClientScore.App.Domain.Interfaces.Services;
using ClientScore.App.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ClientScore.App.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ClienteController : ControllerBase
{
    private readonly ILogger<ClienteController> _logger;
    private readonly IClienteService _clienteService;

    public ClienteController(ILogger<ClienteController> logger, IClienteService clienteService)
    {
        _logger = logger;
        _clienteService = clienteService;
    }

    [SwaggerOperation(Summary = "Obtém todos os clientes e seus endereços", Description = "Retorna um IEnumerable<Cliente>'")]
    [SwaggerResponse(StatusCodes.Status200OK, "Obtido com sucesso", typeof(IEnumerable<Cliente>))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Erro no processamento da requisição")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Erro interno no servidor")]
    [HttpGet("get/list-all")]
    public async Task<IActionResult> GetListAsync()
    {
        try
        {
            var result = await _clienteService.GetListAsync();

            if(result.Sucesso == false)
                return BadRequest(result.MensagemErro);

            return Ok(result.Resposta);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return BadRequest(ex.Message);
        }
    }


    [SwaggerOperation(Summary = "Obtém o score do cliente pelo seu CPF", Description = "Retorna um Score (int)'")]
    [SwaggerResponse(StatusCodes.Status200OK, "Obtido com sucesso", typeof(int))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Erro no processamento da requisição")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Erro interno no servidor")]
    [HttpGet("get/score/{cpf}")]
    public async Task<IActionResult> GetScoreByCpfAsync([FromRoute] string cpf)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _clienteService.GetScoreByCpfAsync(cpf);

            if (result.Sucesso == false)
                return BadRequest(result.MensagemErro);

            return Ok(result.Resposta);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return BadRequest(ex.Message);
        }
    }

    [SwaggerOperation(Summary = "Cria um novo cliente")]
    [SwaggerResponse(StatusCodes.Status200OK, "Criado com sucesso")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Erro de validação ou falha no processamento da requisição")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Erro interno no servidor")]
    [HttpPost("post/create")]
    public async Task<IActionResult> InsertAsync([FromBody] ClienteViewModel model)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _clienteService.InsertAsync(model);

            if (result.Sucesso == false)
                return BadRequest(result.MensagemErro);

            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return BadRequest(ex.Message);
        }
    }


}
