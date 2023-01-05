using MeuLivroDeReceitas.Application.Servicos.UsuarioLogado;
using MeuLivroDeReceitas.Domain.Repositorios.Codigo;
using MeuLivroDeReceitas.Domain.Repositorios;
using MeuLivroDeReceitas.Domain.Repositorios.Conexao;

namespace MeuLivroDeReceitas.Application.UseCases.Conexao.AceitarConexao;

public class AceitarConexaoUseCase : IAceitarConexaoUseCase
{
    private readonly ICodigoWriteOnlyRepository _repository;
    private readonly IConexaoWriteOnlyRepositorio _codigoWriteOnlyRepository;
    private readonly IUsuarioLogado _usuarioLogado;
    private readonly IUnidadeDeTrabalho _unidadeDeTrabalho;
    public AceitarConexaoUseCase(ICodigoWriteOnlyRepository repository, IUsuarioLogado usuarioLogado, IUnidadeDeTrabalho unidadeDeTrabalho, IConexaoWriteOnlyRepositorio codigoWriteOnlyRepository)
    {
        _repository = repository;
        _usuarioLogado = usuarioLogado;
        _unidadeDeTrabalho = unidadeDeTrabalho;
        _codigoWriteOnlyRepository = codigoWriteOnlyRepository;
    }

    public async Task<long> Executar(long usuarioParaSeConectar)
    {
        var usuarioLogado = await _usuarioLogado.RecuperarUsuario();

        await _repository.Deletar(usuarioLogado.Id);

        await _codigoWriteOnlyRepository.Registrar(new Domain.Entidades.Conexao
        {
            UsuarioId = usuarioLogado.Id,
            ConectadoComUsuarioId = usuarioParaSeConectar
        });
        
        await _codigoWriteOnlyRepository.Registrar(new Domain.Entidades.Conexao
        {
            UsuarioId = usuarioParaSeConectar,
            ConectadoComUsuarioId = usuarioLogado.Id
        });

        await _unidadeDeTrabalho.Commit();

        return usuarioLogado.Id;
    }
}
