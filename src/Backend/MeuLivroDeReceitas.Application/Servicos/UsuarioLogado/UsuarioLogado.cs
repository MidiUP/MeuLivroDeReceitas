using MeuLivroDeReceitas.Application.Servicos.Token;
using MeuLivroDeReceitas.Domain.Entidades;
using MeuLivroDeReceitas.Domain.Repositorios.Usuario;
using Microsoft.AspNetCore.Http;

namespace MeuLivroDeReceitas.Application.Servicos.UsuarioLogado;

public class UsuarioLogado : IUsuarioLogado
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly TokenContoller _tokenContoller;
    private readonly IUsuarioReadOnlyRepositorio _repository;
    public UsuarioLogado(IHttpContextAccessor httpContextAccessor, TokenContoller tokenContoller, IUsuarioReadOnlyRepositorio repository) 
    {
        _httpContextAccessor = httpContextAccessor;
        _tokenContoller= tokenContoller;
        _repository = repository;
    }
    public async Task<Usuario> RecuperarUsuario()
    {
        var authorization = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString();
        var token = authorization.Split(" ")[1];
        //var token = authorization["Bearer".Length..].Trim();
        var emailUsuario = _tokenContoller.RecuperarEmail(token);
        var usuario = await _repository.RecuperarPorEmail(emailUsuario);
        return usuario;
    }
}
