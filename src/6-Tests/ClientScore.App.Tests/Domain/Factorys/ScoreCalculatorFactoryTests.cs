using ClientScore.App.Domain.Factorys;

namespace ClientScore.App.Tests.Domain.Factorys
{
    public class ScoreCalculatorFactoryTests
    {
        private readonly ScoreCalculatorFactory _factory;

        public ScoreCalculatorFactoryTests()
        {
            _factory = new ScoreCalculatorFactory();
        }

        [Fact(DisplayName = "Score deve ser 400 (idade > 40 e rendimento entre 60k e 120k)")]
        public void CalcularScore_DeveRetornar400()
        {
            // Arrange
            var dataNascimento = new DateTime(1975, 1, 1); // > 40 anos
            var rendimentoAnual = 90000m;

            // Act
            var score = _factory.CalcularScore(dataNascimento, rendimentoAnual);

            // Assert
            Assert.Equal(400, score); // 200 (renda) + 200 (idade)
        }

        [Fact(DisplayName = "Score deve ser 250 (idade entre 25-40 e rendimento < 60k)")]
        public void CalcularScore_DeveRetornar250()
        {
            // Arrange
            var dataNascimento = new DateTime(1995, 1, 1); // idade ~30
            var rendimentoAnual = 50000m;

            // Act
            var score = _factory.CalcularScore(dataNascimento, rendimentoAnual);

            // Assert
            Assert.Equal(250, score); // 100 (renda) + 150 (idade)
        }
    }
}
