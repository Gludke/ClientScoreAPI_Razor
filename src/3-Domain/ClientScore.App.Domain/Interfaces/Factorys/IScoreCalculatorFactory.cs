namespace ClientScore.App.Domain.Interfaces.Factorys
{
    public interface IScoreCalculatorFactory
    {
        public Task<int> CalcularScore(DateTime dataNascimento, decimal rendimentoAnual, string siglaEstado, string cpf, string email);
    }
}
