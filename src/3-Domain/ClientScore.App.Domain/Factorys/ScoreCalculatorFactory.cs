using ClientScore.App.Domain.Interfaces.Factorys;

namespace ClientScore.App.Domain.Factorys;

public class ScoreCalculatorFactory : IScoreCalculatorFactory
{
    public int CalcularScore(DateTime dataNascimento, decimal rendimentoAnual)
    {
        var hoje = DateTime.Today;
        var idade = hoje.Year - dataNascimento.Year;

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

        return score;
    }
}
