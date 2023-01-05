using MeuLivroDeReceitas.Api.Filtros.UsuarioLogado;
using MeuLivroDeReceitas.Application.UseCases.Dashboard;
using MeuLivroDeReceitas.Application.UseCases.Receita.Recuperar;
using MeuLivroDeReceitas.Application.UseCases.Receita.Registrar;
using MeuLivroDeReceitas.Comunicacao.Enum;
using MeuLivroDeReceitas.Comunicacao.Requisicoes;
using MeuLivroDeReceitas.Comunicacao.Respostas;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace MeuLivroDeReceitas.Api.Controllers;

[ServiceFilter(typeof(UsuarioAutenticadoAttribute))]
public class DashboardController : MeuLivroDeReceitasController
{
    [HttpGet]
    [ProducesResponseType(typeof(RespostaDashboardJson),StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> RecuperarDashboard([FromServices] IDashboardUseCase useCase, 
        [FromQuery(Name = "categoria")] string? categoria, [FromQuery(Name = "tituloOuIngrediente")] string? tituloOuIngrediente)
    {
        var bodyRequest = new RequisicaoDashboardJson
        {
            Categoria = !string.IsNullOrEmpty(categoria) ? (Comunicacao.Enum.Categoria) int.Parse(categoria) : null ,
            TituloOuIngrediente = !string.IsNullOrEmpty(tituloOuIngrediente) ? tituloOuIngrediente : null
        };
        var resposta = await useCase.Executar(bodyRequest);

        if(resposta.Receitas.Any())
        {
            return Ok(resposta);
        }

        return NoContent();
    }

}
