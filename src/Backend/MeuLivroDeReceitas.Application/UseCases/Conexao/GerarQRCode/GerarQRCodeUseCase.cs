using MeuLivroDeReceitas.Application.Servicos.UsuarioLogado;
using MeuLivroDeReceitas.Domain.Entidades;
using MeuLivroDeReceitas.Domain.Repositorios;
using MeuLivroDeReceitas.Domain.Repositorios.Codigo;

namespace MeuLivroDeReceitas.Application.UseCases.Conexao.GerarQRCode;

public class GerarQRCodeUseCase : IGerarQRCodeUseCase
{
    private readonly ICodigoWriteOnlyRepository _repository;
    private readonly IUsuarioLogado _usuarioLogado;
    private readonly IUnidadeDeTrabalho _unidadeDeTrabalho;
    public GerarQRCodeUseCase(ICodigoWriteOnlyRepository repository, IUsuarioLogado usuarioLogado, IUnidadeDeTrabalho unidadeDeTrabalho)
    {
        _repository = repository;
        _usuarioLogado = usuarioLogado;
        _unidadeDeTrabalho = unidadeDeTrabalho;
    }
    public async Task<(string qrCode, long idUsuario)> Executar()
    {
       var usuarioLogado = await _usuarioLogado.RecuperarUsuario();

        var codigo = new Codigos
        {
            Codigo = Guid.NewGuid().ToString(),
            UsuarioId = usuarioLogado.Id
        };

        await _repository.Registrar(codigo);

        await _unidadeDeTrabalho.Commit();

        return (codigo.Codigo, usuarioLogado.Id);

    }
}
