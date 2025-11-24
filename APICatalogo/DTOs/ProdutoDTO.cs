using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APICatalogo.DTOs
{
    public class ProdutoDTO
    {
        public int ProdutoId { get; set; }
        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(20, ErrorMessage = "O nome deve ter entre 5 e 20 carácteres.", MinimumLength = 5)]
        //[PrimeiraLetraMaiuscula]
        public string? Nome { get; set; }
        [Required]
        [StringLength(10, ErrorMessage = "A descrição deve conter no mínimo {1} caracteres.")]
        public string? Descricao { get; set; }
        [Required]
        public decimal Preco { get; set; }
        [Required]
        [StringLength(300, MinimumLength = 10)]
        public string? ImagemUrl { get; set; }
        public int CategoriaId { get; set; }
    }
}
