using ClientScore.App.Domain.Interfaces.Factorys;
using ClientScore.App.Domain.Interfaces.Repositories;
using System.Text.RegularExpressions;

namespace ClientScore.App.Domain.Factorys;

public class ScoreCalculatorFactory : IScoreCalculatorFactory
{
    public Dictionary<string, int> EstadosEmissoresCPF { get; set; } = new();

    private readonly IClienteRepository _clienteRepository;

    public ScoreCalculatorFactory(IClienteRepository clienteRepository)
    {
        _clienteRepository = clienteRepository;

        EstadosEmissoresCPF = new Dictionary<string, int>
        {
            { "DF", 1 },
            { "GO", 1 },
            { "MT", 1 },
            { "MS", 1 },
            { "TO", 1 },
            { "AC", 2 },
            { "AM", 2 },
            { "AP", 2 },
            { "PA", 2 },
            { "RO", 2 },
            { "RR", 2 },
            { "CE", 3 },
            { "MA", 3 },
            { "PI", 3 },
            { "AL", 4 },
            { "PB", 4 },
            { "PE", 4 },
            { "RN", 4 },
            { "BA", 5 },
            { "SE", 5 },
            { "MG", 6 },
            { "ES", 7 },
            { "RJ", 7 },
            { "SP", 8 },
            { "PR", 9 },
            { "SC", 9 },
            { "RS", 0 }
        };
    }

    public async Task<int> CalcularScore(DateTime dataNascimento, decimal rendimentoAnual, string siglaEstado, string cpf, string email)
    {
        var hoje = DateTime.Today;
        var idade = hoje.Year - dataNascimento.Year;

        //obter 9° digito CPF
        var nonoDigCpf = cpf[9];

        if (dataNascimento.Date > hoje.AddYears(-idade))
            idade--;

        int score = 0;

        //Cálculo baseado no Rendimento
        if (rendimentoAnual > 120000)
            score += 300;
        else if (rendimentoAnual >= 60000)
            score += 200;
        else
            score += 100;

        //Cálculo baseado na Idade
        if (idade > 40)
            score += 200;
        else if (idade >= 25)
            score += 150;
        else
            score += 50;

        //Cálculo baseado no estado de origem do CPF
        if (EstadosEmissoresCPF.Any(e => e.Key == siglaEstado && e.Value == nonoDigCpf))
            score += 100;

        //Análise de email:
        //-80 pontos se o email conter 5 ou mais numeros na string em sequencia(ex rodrigo9726346@gmail.com)
        if(HaCincoDigitosConsecutivos(email))
            score -= 80;

        //-100 pontos caso o email já exista no DB
        if (await _clienteRepository.ExistsEmailAsync(email))
            score -= 100;

        return score;
    }

    bool HaCincoDigitosConsecutivos(string input)
    {
        return Regex.IsMatch(input, @"\d{5}");
    }
}
