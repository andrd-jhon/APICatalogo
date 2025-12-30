namespace APICatalogo.Pagination
{
    public class ProdutosFIltroPreco : QueryStringParameters
    {
        public decimal? Preco { get; set; }
        public string? PrecoCriterio { get; set; }
    }
}
