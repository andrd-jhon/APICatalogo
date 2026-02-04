using APICatalogo.Controllers;
using APICatalogo.DTOs;
using APICatalogoXUnit.UnitTests;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;

namespace APICatalogoXUnit
{
    public class GetProdutosUnitTest : IClassFixture<ProdutosUnitTestController>
    {
        private readonly ProdutosController _controller;

        public GetProdutosUnitTest(ProdutosUnitTestController controller)
        {
            _controller = new ProdutosController(controller.repository, controller.mapper);
        }

        [Fact]
        public async Task GetProdutosById_OkResult()
        {
            //arrange
            var prodId = 2;
            //act
            var data = await _controller.Get(prodId);
            //assert
            //var okResult = Assert.IsType<ObjectResult>(data.Result);
            //Assert.Equal(200, okResult.StatusCode);

            //assert (fluentassertions)
            data.Result.Should().BeOfType<OkObjectResult>().Which.StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task GetProdutosById_Return_NotFound()
        {
            //arrange
            var prodId = 10000000;
            //act
            var data = await _controller.Get(prodId);

            //assert (fluentassertions)
            data.Result.Should().BeOfType<NotFoundObjectResult>().Which.StatusCode.Should().Be(404);
        }

        [Fact]
        public async Task GetProdutosById_Return_BadRequest()
        {
            //arrange
            var prodId = -1;
            //act
            var data = await _controller.Get(prodId);

            //assert (fluentassertions)
            data.Result.Should().BeOfType<BadRequestResult>().Which.StatusCode.Should().Be(400);
        }
    }
}
