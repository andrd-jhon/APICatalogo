using APICatalogo.Controllers;
using APICatalogo.DTOs;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APICatalogoXUnit.UnitTests
{
    public class PostProdutosUnitTest : IClassFixture<ProdutosUnitTestController>
    {
        private readonly ProdutosController _controller;

        public PostProdutosUnitTest(ProdutosUnitTestController controller)
        {
            _controller = new ProdutosController(controller.repository, controller.mapper);
        }

        [Fact]
        public async Task CadastrarProdutos_Return_CreatedStatusCode()
        {
            // Arrange  
            var novoProdutoDto = new ProdutoDTO
            {
                Nome = "Novo Produto",
                Descricao = "Descrição do Novo Produto",
                Preco = 10.99,
                ImagemUrl = "imagemfake1.jpg",
                CategoriaId = 2
            };

            // Act
            var data = await _controller.Post(novoProdutoDto);

            // Assert
            var createsResult = data.Result.Should().BeOfType<CreatedAtRouteResult>();
            createsResult.Subject.StatusCode.Should().Be(201);
        }

        [Fact]
        public async Task CadastrarProdutos_Return_BadRequest()
        {
            // Arrange  
            ProdutoDTO produto = null;

            // Act
            var data = await _controller.Post(produto);

            // Assert
            var createsResult = data.Result.Should().BeOfType<BadRequestResult>();
            createsResult.Subject.StatusCode.Should().Be(400);
        }
    }
}
