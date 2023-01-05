using AspNetCore.Hashids.Mvc;
using MeuLivroDeReceitas.Api.Filtros.UsuarioLogado;
using MeuLivroDeReceitas.Application.UseCases.Receita.Atualizar;
using MeuLivroDeReceitas.Application.UseCases.Receita.Deletar;
using MeuLivroDeReceitas.Application.UseCases.Receita.Recuperar;
using MeuLivroDeReceitas.Application.UseCases.Receita.RecuperarPorId;
using MeuLivroDeReceitas.Application.UseCases.Receita.Registrar;
using MeuLivroDeReceitas.Comunicacao.Requisicoes;
using MeuLivroDeReceitas.Comunicacao.Respostas;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MeuLivroDeReceitas.Api.Controllers;

[ServiceFilter(typeof(UsuarioAutenticadoAttribute))]
public class ReceitaController : MeuLivroDeReceitasController
{
    [HttpPost]
    [ProducesResponseType(typeof(RespostaReceitaJson),StatusCodes.Status201Created)]
    public async Task<IActionResult> AdicionarReceita([FromServices] IRegistrarReceitaUseCase useCase, [FromBody] RequisicaoReceitaJson request)
    {
        var resposta = await useCase.Executar(request);
        return Created(string.Empty, resposta);
    }

    [HttpGet("usuario")]
    [ProducesResponseType(typeof(RespostaReceitaJson), StatusCodes.Status200OK)]
    public async Task<IActionResult> RecuperarReceitasUsuario([FromServices] IRecuperarPorUsuarioUseCase usecase)
    {
        var resultado = await usecase.Executar();
        return Ok(resultado);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(RespostaReceitaJson), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> RecuperarPorId(
        [FromServices] IRecuperarPorIdUsecase usecase,
        [FromRoute] long id)
    {
        var resultado = await usecase.Executar(id);
        if (resultado != null)
        {
            return Ok(resultado);
        }
        return NoContent();
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Update(
        [FromServices] IAtualizarReceitaUseCase usecase,
        [FromRoute] long id,
        [FromBody] RequisicaoReceitaJson receita
        )
    {
        await usecase.Executar(id, receita);
        return NoContent();
    }
    
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete(
        [FromServices] IDeletarReceitaUseCase usecase,
        [FromRoute] long id
        )
    {
        await usecase.Executar(id);
        return NoContent();
    }
}
