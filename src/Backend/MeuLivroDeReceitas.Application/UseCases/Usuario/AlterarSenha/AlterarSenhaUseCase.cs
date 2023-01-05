using MeuLivroDeReceitas.Application.Servicos.Criptografia;
using MeuLivroDeReceitas.Application.Servicos.UsuarioLogado;
using MeuLivroDeReceitas.Comunicacao.Requisicoes;
using MeuLivroDeReceitas.Domain.Repositorios;
using MeuLivroDeReceitas.Domain.Repositorios.Usuario;
using MeuLivroDeReceitas.Exceptions;
using MeuLivroDeReceitas.Exceptions.ExceptionsBase;

namespace MeuLivroDeReceitas.Application.UseCases.Usuario.AlterarSenha;

public class AlterarSenhaUseCase : IAlterarSenhaUseCase
{
    private readonly IUsuarioUpdateOnlyRepositorio _usuarioUpdateOnlyRepositorio;
    private readonly IUsuarioLogado _usuarioLogado;
    private readonly EncriptadorDeSenha _encriptadorDeSenha;
    private readonly IUnidadeDeTrabalho _unidadeDeTrabalho;
    public AlterarSenhaUseCase(IUsuarioUpdateOnlyRepositorio usuarioUpdateOnlyRepositorio, IUsuarioLogado usuarioLogado, EncriptadorDeSenha encriptadorDeSenha, IUnidadeDeTrabalho unidadeDeTrabalho) 
    {
        _usuarioUpdateOnlyRepositorio = usuarioUpdateOnlyRepositorio;
        _usuarioLogado = usuarioLogado;
        _encriptadorDeSenha = encriptadorDeSenha;
        _unidadeDeTrabalho = unidadeDeTrabalho;
    }
    public async Task Executar(RequisicaoAlterarSenhaJson requisicao)
    {
        var usuarioLogado = await _usuarioLogado.RecuperarUsuario();
        var usuarioEditar = await _usuarioUpdateOnlyRepositorio.RecuperarPorId(usuarioLogado.Id);
        
        Validar(requisicao, usuarioEditar);
        
        var novaSenhaCriptografada = _encriptadorDeSenha.Criptografia(requisicao.NovaSenha);
        usuarioEditar.Senha = novaSenhaCriptografada;
        _usuarioUpdateOnlyRepositorio.Update(usuarioEditar);
        await _unidadeDeTrabalho.Commit();

    }

    private void Validar(RequisicaoAlterarSenhaJson requisicao, Domain.Entidades.Usuario usuario)
    {
        var validator = new AlterarSenhaValidator();
        var resultado = validator.Validate(requisicao);
 
        var senhaAtualCriptografada = _encriptadorDeSenha.Criptografia(requisicao.SenhaAtual);
        if (!usuario.Senha.Equals(senhaAtualCriptografada))
        {
            resultado.Errors.Add(new FluentValidation.Results.ValidationFailure("senhaAtual", ResourceMensagensDeErro.SENHA_ATUAL_INVALIDA));
        }
        if (!resultado.IsValid)
        {
            var mensagens = resultado.Errors.Select(x => x.ErrorMessage).ToList();
            throw new ErrosDeValidacaoException(mensagens);
        }
    }
}
