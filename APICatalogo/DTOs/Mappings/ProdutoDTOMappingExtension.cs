using APICatalogo.Models;

namespace APICatalogo.DTOs.Mappings
{
    public static class ProdutoDTOMappingExtensions
    {
        public static ProdutoDTO? ToProdutoDTO(this Produto produto)
        {
            if (produto is null)
                return null;

            return new ProdutoDTO
            {
                ProdutoId = produto.ProdutoId,
                Nome = produto.Nome,
                Descricao = produto.Descricao,
                Preco = produto.Preco,
                ImagemUrl = produto.ImagemUrl,
                CategoriaId = produto.CategoriaId
            };
        }

        public static Produto? ToProduto(this ProdutoDTO produtoDTO)
        {
            if (produtoDTO is null)
                return null;

            return new Produto
            {
                ProdutoId = produtoDTO.ProdutoId,
                Nome = produtoDTO.Nome,
                Descricao = produtoDTO.Descricao,
                Preco = produtoDTO.Preco,
                ImagemUrl = produtoDTO.ImagemUrl,
                CategoriaId = produtoDTO.CategoriaId
            };
        }

        public static IEnumerable<ProdutoDTO> ToProdutoDTOList(this IEnumerable<Produto> produtos)
        {
            if (produtos is null || !produtos.Any())
                return new List<ProdutoDTO>();

            return produtos.Select(produto => new ProdutoDTO
            {
                ProdutoId = produto.ProdutoId,
                Nome = produto.Nome,
                Descricao = produto.Descricao,
                Preco = produto.Preco,
                ImagemUrl = produto.ImagemUrl,
                CategoriaId = produto.CategoriaId
            }).ToList();
        }
    }
}
