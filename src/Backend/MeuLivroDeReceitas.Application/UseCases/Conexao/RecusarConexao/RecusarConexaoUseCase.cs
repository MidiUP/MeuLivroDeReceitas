using MeuLivroDeReceitas.Application.Servicos.UsuarioLogado;
using MeuLivroDeReceitas.Domain.Repositorios.Codigo;
using MeuLivroDeReceitas.Domain.Repositorios;

namespace MeuLivroDeReceitas.Application.UseCases.Conexao.RecusarConexao;

public class RecusarConexaoUseCase : IRecusarConexaoUseCase
{
    private readonly ICodigoWriteOnlyRepository _repository;
    private readonly IUsuarioLogado _usuarioLogado;
    private readonly IUnidadeDeTrabalho _unidadeDeTrabalho;
    public RecusarConexaoUseCase(ICodigoWriteOnlyRepository repository, IUsuarioLogado usuarioLogado, IUnidadeDeTrabalho unidadeDeTrabalho)
    {
        _repository = repository;
        _usuarioLogado = usuarioLogado;
        _unidadeDeTrabalho = unidadeDeTrabalho;
    }

    public async Task<long> Executar()
    {
        var usuarioLogado = await _usuarioLogado.RecuperarUsuario();
        await _repository.Deletar(usuarioLogado.Id);

        await _unidadeDeTrabalho.Commit();

        return usuarioLogado.Id;
    }
}
