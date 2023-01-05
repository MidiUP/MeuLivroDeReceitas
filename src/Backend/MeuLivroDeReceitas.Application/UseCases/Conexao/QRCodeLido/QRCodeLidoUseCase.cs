using MeuLivroDeReceitas.Application.Servicos.UsuarioLogado;
using MeuLivroDeReceitas.Comunicacao.Respostas;
using MeuLivroDeReceitas.Domain.Repositorios.Codigo;
using MeuLivroDeReceitas.Domain.Repositorios.Conexao;
using MeuLivroDeReceitas.Exceptions.ExceptionsBase;

namespace MeuLivroDeReceitas.Application.UseCases.Conexao.QRCodeLido;

public class QRCodeLidoUseCase : IQRCodeLidoUseCase
{

    private readonly IConexaoReadOnlyRepositorio _repositorioConexao;
    private readonly ICodigoReadOnlyRepository _repository;
    private readonly IUsuarioLogado _usuarioLogado;

    public QRCodeLidoUseCase(ICodigoReadOnlyRepository repository, IUsuarioLogado usuarioLogado, IConexaoReadOnlyRepositorio repositorioConexao)
    {
        _repository = repository;
        _usuarioLogado = usuarioLogado;
        _repositorioConexao = repositorioConexao;
    }

    public async Task<(RespostaUsuarioConexaoJson usuarioParaSeConectar, long idUsuarioQueGerouQRCode)> Executar(string codigoConexao)
    {
        var usuarioLogado = await _usuarioLogado.RecuperarUsuario();
        var codigo = await _repository.RecuperarEntidadeCodigo(codigoConexao);

        await Validar(codigo, usuarioLogado);

        return (new RespostaUsuarioConexaoJson { Nome = usuarioLogado.Nome, IdUsuarioSolicitante = usuarioLogado.Id }, codigo.UsuarioId);
    }

    private async Task Validar(Domain.Entidades.Codigos codigo, Domain.Entidades.Usuario usuarioLogado)
    {
        if(codigo is null)
        {
            throw new MeuLivroDeReceitasException("");
        }

        if (codigo.UsuarioId.Equals(usuarioLogado.Id))
        {
            throw new MeuLivroDeReceitasException("");
        }

        var existsConexao = await _repositorioConexao.ExisteConexao(codigo.UsuarioId, usuarioLogado.Id);

        if (existsConexao)
        {
            throw new MeuLivroDeReceitasException("");
        }

    }
}
