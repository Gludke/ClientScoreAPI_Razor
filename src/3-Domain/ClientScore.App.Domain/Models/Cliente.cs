namespace ClientScore.App.Domain.Models
{
    public class Cliente
    {
        public long Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public DateTime DataNascimento { get; set; }
        public string CPF { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public decimal RendimentoAnual { get; set; }
        public string DDD { get; set; } = string.Empty;
        public string Telefone { get; set; } = string.Empty;
        public int Score { get; set; }

        //Relação 1:1
        public Endereco Endereco { get; set; } = new();
    }
}
