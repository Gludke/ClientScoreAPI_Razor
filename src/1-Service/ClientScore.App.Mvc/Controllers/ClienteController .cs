using ClientScore.App.Mvc.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.Json;

namespace ClientScore.App.Mvc.Controllers
{
    public class ClienteController : Controller
    {
        private readonly ILogger<ClienteController> _logger;
        private readonly HttpClient _httpClient;

        public ClienteController(ILogger<ClienteController> logger,
                                 IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClient = httpClientFactory.CreateClient("ApiClientScore");
        }

        public async Task<IActionResult> Index()
        {
            var clientes = await _httpClient.GetFromJsonAsync<List<ClienteViewModel>>("Cliente/get/list-all");
            return View(clientes);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ClienteRegisterViewModel model)
        {
            var response = await _httpClient.PostAsJsonAsync("Cliente/post/create", model);

            if (!response.IsSuccessStatusCode)
            {
                await AddModelErrorsFromApi(response);
                return View(model);
            }

            return RedirectToAction("Index");
        }




        #region METHODS

        private async Task AddModelErrorsFromApi(HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync();

            try
            {
                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    // Tenta desserializar como ValidationProblemDetails
                    var problem = JsonSerializer.Deserialize<ValidationProblemDetails>(content, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    if (problem?.Errors is not null && problem.Errors.Any())
                    {
                        foreach (var entry in problem.Errors)
                        {
                            foreach (var msg in entry.Value)
                            {
                                ModelState.AddModelError(string.Empty, msg);
                            }
                        }

                        return;
                    }
                }

                // Se não for validação ou estrutura diferente, exibe o conteúdo como mensagem simples
                ModelState.AddModelError(string.Empty, content);
            }
            catch
            {
                ModelState.AddModelError(string.Empty, $"Erro inesperado na comunicação com a API: {content}");
            }
        }



        #endregion
    }
}
