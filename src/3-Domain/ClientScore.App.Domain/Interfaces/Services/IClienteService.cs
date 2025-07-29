using ClientScore.App.Domain.Models;
using ClientScore.App.Domain.ViewModels;

namespace ClientScore.App.Domain.Interfaces.Services
{
    public interface IClienteService
    {
        public Task<ResponseService<int?>> GetScoreByCpfAsync(string cpf);
        public Task<ResponseService<IEnumerable<Cliente>>> GetListAsync();
        public Task<ResponseService<object>> InsertAsync(ClienteViewModel cliente);
    }
}
