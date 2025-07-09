namespace ClientScore.App.Domain.Interfaces.Factorys
{
    public interface IScoreCalculatorFactory
    {
        public int CalcularScore(DateTime dataNascimento, decimal rendimentoAnual);
    }
}
