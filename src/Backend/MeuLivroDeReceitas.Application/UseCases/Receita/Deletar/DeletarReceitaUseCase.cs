using AutoMapper;
using MeuLivroDeReceitas.Application.Servicos.UsuarioLogado;
using MeuLivroDeReceitas.Domain.Repositorios.Receita;
using MeuLivroDeReceitas.Domain.Repositorios;
using MeuLivroDeReceitas.Application.UseCases.Receita.Registrar;
using MeuLivroDeReceitas.Comunicacao.Requisicoes;
using MeuLivroDeReceitas.Exceptions.ExceptionsBase;
using MeuLivroDeReceitas.Exceptions;

namespace MeuLivroDeReceitas.Application.UseCases.Receita.Deletar;

public class DeletarReceitaUseCase : IDeletarReceitaUseCase
{
    private readonly IReceitaWriteOnlyRepositorio _repository;
    private readonly IReceitaReadOnlyRepositorio _repositoryRead;
    private readonly IUsuarioLogado _usuarioLogado;
    private readonly IUnidadeDeTrabalho _unidadeDeTrabalho;
    public DeletarReceitaUseCase(IReceitaWriteOnlyRepositorio repository, IUsuarioLogado usuarioLogado, IUnidadeDeTrabalho unidadeDeTrabalho, IReceitaReadOnlyRepositorio repositoryRead)
    {
        _repository = repository;
        _usuarioLogado = usuarioLogado;
        _unidadeDeTrabalho = unidadeDeTrabalho;
        _repositoryRead = repositoryRead;
    }
    public async Task Executar(long id)
    {
        var receita = await _repositoryRead.GetById(id);
        var usuarioLogado = await _usuarioLogado.RecuperarUsuario();

        Validar(receita, usuarioLogado);

        await _repository.Deletar(id);

        await _unidadeDeTrabalho.Commit();

    }

    private static void Validar(Domain.Entidades.Receita receita, Domain.Entidades.Usuario usuarioLogado)
    {
        if (receita == null || usuarioLogado.Id != receita.UsuarioId)
        {
            throw new ErrosDeValidacaoException(new List<string> { ResourceMensagensDeErro.RECEITA_NAO_ENCONTRADA });
        }
    }
}
