using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APICatalogo.Models
{
    [Table("Categorias")]
    public class Categoria
    {
        public Categoria()
        {
            Produtos = new Collection<Produto>();
        }

        [Key]
        public int CategoriaId { get; set; }
        
        [Required(ErrorMessage = "O nome é obrigatório")]
        [StringLength(80, ErrorMessage = "O nome deve ter no máximo {1} e no mínimo {2} caracteres", MinimumLength = 5)]
        public string Nome { get; set; }
        [Required]
        [StringLength(300, ErrorMessage = "A url da imagem excede o limite de {1} caracteres")]
        public string ImagemUrl { get; set; }
        public ICollection<Produto> Produtos { get; set; }
    }
}
