namespace APICatalogo.Pagination
{
    public class ProdutosFIltroPreco : QueryStringParameters
    {
        public double? Preco { get; set; }
        public string? PrecoCriterio { get; set; }
    }
}
