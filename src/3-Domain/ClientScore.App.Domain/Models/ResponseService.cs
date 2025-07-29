namespace ClientScore.App.Domain.Models
{
    public class ResponseService<T>
    {
        public bool Sucesso { get; set; } = true;
        public string? MensagemErro { get; set; }
        public T? Resposta { get; set; }
    }
}
