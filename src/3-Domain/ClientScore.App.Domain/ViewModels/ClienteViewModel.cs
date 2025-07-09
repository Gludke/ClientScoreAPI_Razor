using System.ComponentModel.DataAnnotations;

namespace ClientScore.App.Domain.ViewModels;

public class ClienteViewModel
{
    [Required(ErrorMessage = "O nome é obrigatório.")]
    [StringLength(150)]
    public string Nome { get; set; } = string.Empty;

    [Required(ErrorMessage = "A data de nascimento é obrigatória.")]
    public DateTime DataNascimento { get; set; }

    [Required(ErrorMessage = "O CPF é obrigatório.")]
    [RegularExpression(@"^\d{11}$", ErrorMessage = "O CPF deve conter 11 dígitos numéricos.")]
    public string CPF { get; set; } = string.Empty;

    [Required(ErrorMessage = "O email é obrigatório.")]
    [EmailAddress(ErrorMessage = "Email inválido.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "O rendimento anual é obrigatório.")]
    [Range(0.01, 999999999.99, ErrorMessage = "O rendimento anual deve ser positivo.")]
    public decimal RendimentoAnual { get; set; }

    [Required(ErrorMessage = "O DDD é obrigatório.")]
    [RegularExpression(@"^\d{2}$", ErrorMessage = "O DDD deve conter 2 dígitos.")]
    public string DDD { get; set; } = string.Empty;

    [Required(ErrorMessage = "O telefone é obrigatório.")]
    public string Telefone { get; set; } = string.Empty;

    [Required(ErrorMessage = "O endereço é obrigatório.")]
    public EnderecoViewModel Endereco { get; set; } = new();
}