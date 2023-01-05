using MeuLivroDeReceitas.Application.Servicos.Token;
using MeuLivroDeReceitas.Comunicacao.Respostas;
using MeuLivroDeReceitas.Domain.Repositorios.Usuario;
using MeuLivroDeReceitas.Exceptions;
using MeuLivroDeReceitas.Exceptions.ExceptionsBase;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using System.Threading.Tasks;

namespace MeuLivroDeReceitas.Api.Filtros.UsuarioLogado;

public class UsuarioAutenticadoAttribute : AuthorizeAttribute, IAsyncAuthorizationFilter
{
    private readonly TokenContoller _tokenContoller;
    private readonly IUsuarioReadOnlyRepositorio _repository;

    public UsuarioAutenticadoAttribute(TokenContoller tokenContoller, IUsuarioReadOnlyRepositorio repository)
    {
        _tokenContoller = tokenContoller;
        _repository = repository;
    }

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        try
        {
            var token = TokenNaRequisicao(context);
            var email = _tokenContoller.RecuperarEmail(token);
            var usuario = await _repository.RecuperarPorEmail(email);

            if (usuario is null)
            {
                throw new MeuLivroDeReceitasException(string.Empty);
            }
        }
        catch (SecurityTokenExpiredException)
        {
            TokenExpirado(context);
        }
        catch
        {
            UsuarioSemPermissao(context);
        }
    }

    private static string TokenNaRequisicao(AuthorizationFilterContext context)
    {
        var authorization = context.HttpContext.Request.Headers["Authorization"].ToString();
        if (string.IsNullOrEmpty(authorization))
        {
            throw new MeuLivroDeReceitasException(string.Empty);
        }
        return authorization["Bearer".Length..].Trim();
    }

    private static void TokenExpirado(AuthorizationFilterContext context)
    {
        context.Result = new UnauthorizedObjectResult(new RespostaErroJson(ResourceMensagensDeErro.TOKEN_EXPIRADO));
    }

    private static void UsuarioSemPermissao(AuthorizationFilterContext context)
    {
        context.Result = new UnauthorizedObjectResult(new RespostaErroJson(ResourceMensagensDeErro.USUARIO_SEM_PERMISSAO));
    }
}
