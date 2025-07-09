namespace ClientScore.App.Mvc.Models
{
    public class ClienteViewModel
    {
        public string Nome { get; set; } = string.Empty;
        public DateTime DataNascimento { get; set; }
        public string CPF { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public decimal RendimentoAnual { get; set; }
        public string DDD { get; set; } = string.Empty;
        public string Telefone { get; set; } = string.Empty;
        public int Score { get; set; }
        public EnderecoViewModel Endereco { get; set; } = new();
    }
}
