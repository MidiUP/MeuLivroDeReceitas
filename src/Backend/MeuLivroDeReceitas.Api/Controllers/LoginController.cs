﻿using MeuLivroDeReceitas.Application.UseCases.Login.FazerLogin;
using MeuLivroDeReceitas.Comunicacao.Requisicoes;
using MeuLivroDeReceitas.Comunicacao.Respostas;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MeuLivroDeReceitas.Api.Controllers
{
    public class LoginController : MeuLivroDeReceitasController
    {
        [HttpPost]
        [ProducesResponseType(typeof(RespostaLoginJson), StatusCodes.Status200OK)]
        public async Task<IActionResult> Login([FromServices] ILoginUseCase usecase, [FromBody] RequisicaoLoginJson request)
        {
            var response = await usecase.Executar(request);
            return Ok(response);
        }
    }
}
