using MeuLivroDeReceitas.Application.Servicos.Token;
using MeuLivroDeReceitas.Domain.Entidades;
using MeuLivroDeReceitas.Domain.Repositorios.Usuario;
using MeuLivroDeReceitas.Exceptions.ExceptionsBase;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;

namespace MeuLivroDeReceitas.Api.Filtros.UsuarioLogado;

public class UsuarioLogadoHandler : AuthorizationHandler<UsuarioLogadoRequirement>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly TokenContoller _tokenContoller;
    private readonly IUsuarioReadOnlyRepositorio _repository;

    public UsuarioLogadoHandler(IHttpContextAccessor httpContextAccessor, TokenContoller tokenContoller, IUsuarioReadOnlyRepositorio repository)
    {
        _httpContextAccessor = httpContextAccessor;
        _repository = repository;
        _tokenContoller = tokenContoller;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, UsuarioLogadoRequirement requirement)
    {
        try
        {
            var authorization = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString();

            if (string.IsNullOrEmpty(authorization))
            {
                context.Fail();
                return;
            }

            var token = authorization.Split(" ")[1];

            var emailUsuario = _tokenContoller.RecuperarEmail(token);
            var usuario = await _repository.RecuperarPorEmail(emailUsuario);

            if (usuario is null)
            {
                context.Fail();
                return;
            }

            context.Succeed(requirement);
        }
        catch
        {
            context.Fail();
            return;
        }
    }

}
