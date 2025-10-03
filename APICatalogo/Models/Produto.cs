using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace APICatalogo.Models
{
    [Table("Produtos")]
    public class Produto
    {
        [Key]
        public int ProdutoId { get; set; }
        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(20, ErrorMessage = "O nome deve ter entre 5 e 20 carácteres.", MinimumLength = 5)]
        public string? Nome { get; set; }
        [Required]
        [StringLength(10, ErrorMessage = "A descrição deve conter no mínimo {1} caracteres.")]
        public string? Descricao { get; set; }
        [Required]
        [Column(TypeName = "decimal(10,2)")]
        [Range(1, 1000, ErrorMessage = "O Preço deve estar entre o valor {1} e {2}.")]
        public decimal Preco { get; set; }
        [Required]
        [StringLength(300, MinimumLength = 10)]
        public string? ImagemUrl { get; set; }
        public float Estoque { get; set; }
        public DateTime DataCadastro { get; set; }
        public int CategoriaId { get; set; }

        [JsonIgnore]
        public Categoria? Categoria { get; set; }
    }
}
