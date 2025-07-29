using ClientScore.App.Domain.Models;

namespace ClientScore.App.Domain.Interfaces.Repositories
{
    public interface IClienteRepository
    {
        public Task<Cliente?> GetByIdAsync(long id);
        public Task<int?> GetScoreByCpfAsync(string cpf);
        public Task<bool> ExistsEmailAsync(string email);
        public Task<List<Cliente>> GetAllAsync();
        public Task InsertAsync(Cliente cliente);
    }
}
