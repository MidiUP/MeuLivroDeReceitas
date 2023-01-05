using MeuLivroDeReceitas.Api.Filtros.UsuarioLogado;
using MeuLivroDeReceitas.Application.UseCases.Usuario.AlterarSenha;
using MeuLivroDeReceitas.Application.UseCases.Usuario.Registrar;
using MeuLivroDeReceitas.Comunicacao.Requisicoes;
using MeuLivroDeReceitas.Comunicacao.Respostas;
using MeuLivroDeReceitas.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MeuLivroDeReceitas.Api.Controllers;


public class UsuarioController : MeuLivroDeReceitasController
{

    [HttpPost]
    [ProducesResponseType(typeof(RespostaUsuarioRegistradoJson), StatusCodes.Status201Created)]
    public async Task<IActionResult> RegistrarUsuario([FromServices] IRegistrarUsuarioUseCase usecase, [FromBody] RequisicaoRegistrarUsuarioJson request)
    {

        var resposta = await usecase.Executar(request);
        return Created(string.Empty, resposta);
    }

    [HttpPut("alterar-senha")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ServiceFilter(typeof(UsuarioAutenticadoAttribute))]
    public async Task<IActionResult> AlterarSenha([FromServices] IAlterarSenhaUseCase usecase, [FromBody] RequisicaoAlterarSenhaJson request)
    {
        await usecase.Executar(request);
        return NoContent();
    }
}