namespace ClientScore.App.Domain.Models
{
    public class Endereco
    {
        public long Id { get; set; }
        public long ClienteId { get; set; }
        public string Estado { get; set; } = string.Empty;
        public string Cidade { get; set; } = string.Empty;
        public string Rua { get; set; } = string.Empty;
        public string? Numero { get; set; }
        public string? Complemento { get; set; }
        public string CEP { get; set; } = string.Empty;
    }
}
