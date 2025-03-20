using System.ComponentModel.DataAnnotations;

namespace APICatalogo.DTO
{
    public class ProdutoDTO
    {
        public int ProdutoId { get; set; }

        [Required]
        [StringLength(80, ErrorMessage = "O nome deve ter no máximo {1} caracteres.")]
        public string? Nome { get; set; }

        [Required]
        [StringLength(300, ErrorMessage = "A descrição deve ter no máximo {1} caracteres.")]
        public string? Descricao { get; set; }

        [Required]
        [Range(1, 10000, ErrorMessage = "O preço deve estar entre {1} e {2}.")]
        public decimal Preco { get; set; }

        public string? ImagemUrl { get; set; }

        public int CategoriaId { get; set; }
    }
}
