using ClientScore.App.Domain.Models;
using ClientScore.App.Domain.Interfaces.Repositories;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace ClientScore.App.Data.Repositories;

public class ClienteRepository : IClienteRepository
{
    private readonly IConfiguration _configuration;

    private readonly string _connectionString;

    public ClienteRepository(IConfiguration configuration)
    {
        _configuration = configuration;

        _connectionString = _configuration["ConnectionStrings:SqlServer"] ?? throw new ArgumentException("'ConnectionStrings:SqlServer' esta null");
    }

    public async Task<Cliente?> GetByIdAsync(long id)
    {
        const string query = @"
        SELECT 
            c.Id, c.Nome, c.DataNascimento, c.CPF, c.Email, c.RendimentoAnual, c.Telefone, c.DDD, c.Score
            e.Id AS EnderecoId, e.Estado, e.Cidade, e.Rua, e.Numero, e.Complemento, e.CEP
        FROM Cliente c
        LEFT JOIN Endereco e ON c.Id = e.ClienteId
        WHERE c.Id = @Id";

        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand(query, connection);

        command.Parameters.AddWithValue("@Id", id);

        await connection.OpenAsync();
        using var reader = await command.ExecuteReaderAsync();

        if (!reader.HasRows)
            return null;

        await reader.ReadAsync();

        var cliente = new Cliente
        {
            Id = reader.GetInt64(0),
            Nome = reader.GetString(1),
            DataNascimento = reader.GetDateTime(2),
            CPF = reader.GetString(3),
            Email = reader.GetString(4),
            RendimentoAnual = reader.GetDecimal(5),
            Telefone = reader.GetString(6),
            DDD = reader.GetString(7),
            Score = reader.GetInt32(8),
            Endereco = new Endereco
            {
                Id = reader.GetInt64(9),
                ClienteId = reader.GetInt64(0),
                Estado = reader.GetString(10),
                Cidade = reader.GetString(11),
                Rua = reader.GetString(12),
                Numero = reader.IsDBNull(13) ? string.Empty : reader.GetString(13),
                Complemento = reader.IsDBNull(14) ? string.Empty : reader.GetString(14),
                CEP = reader.GetString(15)
            }
        };

        return cliente;
    }

    public async Task<List<Cliente>> GetAllAsync()
    {
        const string query = @"
        SELECT 
            c.Id, c.Nome, c.DataNascimento, c.CPF, c.Email, c.RendimentoAnual, c.Telefone, c.DDD, c.Score,
            e.Id AS EnderecoId, e.Estado, e.Cidade, e.Rua, e.Numero, e.Complemento, e.CEP
        FROM Cliente c
        INNER JOIN Endereco e ON c.Id = e.ClienteId";

        var clientes = new List<Cliente>();

        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand(query, connection);

        await connection.OpenAsync();

        using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            var cliente = new Cliente
            {
                Id = reader.GetInt64(0),
                Nome = reader.GetString(1),
                DataNascimento = reader.GetDateTime(2),
                CPF = reader.GetString(3),
                Email = reader.GetString(4),
                RendimentoAnual = reader.GetDecimal(5),
                Telefone = reader.GetString(6),
                DDD = reader.GetString(7),
                Score = reader.GetInt32(8),
                Endereco = new Endereco
                {
                    Id = reader.GetInt64(9),
                    ClienteId = reader.GetInt64(0),
                    Estado = reader.GetString(10),
                    Cidade = reader.GetString(11),
                    Rua = reader.GetString(12),
                    Numero = reader.IsDBNull(13) ? string.Empty : reader.GetString(13),
                    Complemento = reader.IsDBNull(14) ? string.Empty : reader.GetString(14),
                    CEP = reader.GetString(15)
                }
            };

            clientes.Add(cliente);
        }

        return clientes;
    }

    public async Task InsertAsync(Cliente cliente)
    {
        const string queryInsertCliente = @"
        INSERT INTO Cliente (Nome, DataNascimento, CPF, Email, RendimentoAnual, Telefone, DDD, Score)
        VALUES (@Nome, @DataNascimento, @CPF, @Email, @RendimentoAnual, @Telefone, @DDD, @Score);
        SELECT CAST(SCOPE_IDENTITY() AS BIGINT);";

        const string queryInsertEndereco = @"
        INSERT INTO Endereco (ClienteId, Estado, Cidade, Rua, Numero, Complemento, CEP)
        VALUES (@ClienteId, @Estado, @Cidade, @Rua, @Numero, @Complemento, @CEP);";

        using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();

        using var transaction = connection.BeginTransaction();

        try
        {
            //Inserindo cliente no DB
            using var commandCliente = new SqlCommand(queryInsertCliente, connection, transaction);
            commandCliente.Parameters.AddWithValue("@Nome", cliente.Nome);
            commandCliente.Parameters.AddWithValue("@DataNascimento", cliente.DataNascimento);
            commandCliente.Parameters.AddWithValue("@CPF", cliente.CPF);
            commandCliente.Parameters.AddWithValue("@Email", cliente.Email);
            commandCliente.Parameters.AddWithValue("@RendimentoAnual", cliente.RendimentoAnual);
            commandCliente.Parameters.AddWithValue("@DDD", cliente.DDD);
            commandCliente.Parameters.AddWithValue("@Telefone", cliente.Telefone);
            commandCliente.Parameters.AddWithValue("@Score", cliente.Score);

            var idCliente = (long)await commandCliente.ExecuteScalarAsync();
            cliente.Id = idCliente;

            //Inserindo endereço no DB
            using var commandEndereco = new SqlCommand(queryInsertEndereco, connection, transaction);
            commandEndereco.Parameters.AddWithValue("@ClienteId", idCliente);
            commandEndereco.Parameters.AddWithValue("@Estado", cliente.Endereco.Estado);
            commandEndereco.Parameters.AddWithValue("@Cidade", cliente.Endereco.Cidade);
            commandEndereco.Parameters.AddWithValue("@Rua", cliente.Endereco.Rua);
            commandEndereco.Parameters.AddWithValue("@Numero", string.IsNullOrWhiteSpace(cliente.Endereco.Numero) ? DBNull.Value : cliente.Endereco.Numero);
            commandEndereco.Parameters.AddWithValue("@Complemento", string.IsNullOrWhiteSpace(cliente.Endereco.Numero) ? DBNull.Value : cliente.Endereco.Numero);
            commandEndereco.Parameters.AddWithValue("@CEP", cliente.Endereco.CEP);

            await commandEndereco.ExecuteNonQueryAsync();

            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }


}
