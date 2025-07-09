using System.ComponentModel.DataAnnotations;

namespace ClientScore.App.Domain.ViewModels;

public class EnderecoViewModel
{
    [Required(ErrorMessage = "O estado é obrigatório.")]
    [StringLength(50)]
    public string Estado { get; set; } = string.Empty;

    [Required(ErrorMessage = "A cidade é obrigatória.")]
    [StringLength(100)]
    public string Cidade { get; set; } = string.Empty;

    [Required(ErrorMessage = "A rua é obrigatória.")]
    [StringLength(200)]
    public string Rua { get; set; } = string.Empty;

    public string? Numero { get; set; }

    public string? Complemento { get; set; }

    [Required(ErrorMessage = "O CEP é obrigatório.")]
    [RegularExpression(@"^\d{8}$", ErrorMessage = "O CEP deve conter exatamente 8 dígitos numéricos.")]
    public string CEP { get; set; } = string.Empty;
}