using MeuLivroDeReceitas.Application.Servicos.Criptografia;
using MeuLivroDeReceitas.Application.Servicos.Token;
using MeuLivroDeReceitas.Comunicacao.Requisicoes;
using MeuLivroDeReceitas.Comunicacao.Respostas;
using MeuLivroDeReceitas.Domain.Repositorios.Usuario;
using MeuLivroDeReceitas.Exceptions.ExceptionsBase;

namespace MeuLivroDeReceitas.Application.UseCases.Login.FazerLogin;

internal class LoginUseCase : ILoginUseCase
{
    private readonly IUsuarioReadOnlyRepositorio _usuarioReadOnlyRepositorio;
    private readonly EncriptadorDeSenha _encriptadorDeSenha;
    private readonly TokenContoller _tokenController;
    public LoginUseCase(EncriptadorDeSenha encriptadorDeSenha, TokenContoller tokenController, IUsuarioReadOnlyRepositorio usuarioReadOnlyRepositorio)
    {
        _encriptadorDeSenha = encriptadorDeSenha;
        _tokenController = tokenController;
        _usuarioReadOnlyRepositorio = usuarioReadOnlyRepositorio;
    }
    public async Task<RespostaLoginJson> Executar(RequisicaoLoginJson input)
    {
        var senhaCriptografada = _encriptadorDeSenha.Criptografia(input.Senha);

        var usuario = await _usuarioReadOnlyRepositorio.RecuperarPorEmailSenha(input.Email, senhaCriptografada);

        if(usuario == null)
        {
            throw new LoginInvalidoException();
        }

        return new RespostaLoginJson
        {
            Nome = usuario.Nome,
            Token = _tokenController.GerarToken(usuario.Email)
        };
    }
}
