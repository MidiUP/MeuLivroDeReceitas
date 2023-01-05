using AutoMapper;
using MeuLivroDeReceitas.Application.Servicos.UsuarioLogado;
using MeuLivroDeReceitas.Comunicacao.Requisicoes;
using MeuLivroDeReceitas.Comunicacao.Respostas;
using MeuLivroDeReceitas.Domain.Repositorios.Receita;
using MeuLivroDeReceitas.Exceptions.ExceptionsBase;
using MeuLivroDeReceitas.Exceptions;
using MeuLivroDeReceitas.Domain.Repositorios;
using MeuLivroDeReceitas.Application.UseCases.Receita.Registrar;
using MeuLivroDeReceitas.Domain.Entidades;

namespace MeuLivroDeReceitas.Application.UseCases.Receita.Atualizar;

public class AtualizarReceitaUseCase : IAtualizarReceitaUseCase
{
    private readonly IReceitaUpdateOnlyRepositorio _repository;
    private readonly IMapper _mapper;
    private readonly IUsuarioLogado _usuarioLogado;
    private readonly IUnidadeDeTrabalho _unidadeDeTrabalho;
    public AtualizarReceitaUseCase(IReceitaUpdateOnlyRepositorio repository, IMapper mapper, IUsuarioLogado usuarioLogado, IUnidadeDeTrabalho unidadeDeTrabalho)
    {
        _mapper = mapper;
        _repository = repository;
        _usuarioLogado = usuarioLogado;
        _unidadeDeTrabalho = unidadeDeTrabalho;
    }
    public async Task Executar(long id, RequisicaoReceitaJson request)
    {
        var receita = await _repository.GetById(id);
        var usuarioLogado = await _usuarioLogado.RecuperarUsuario();

        Validar(request, receita, usuarioLogado);

        _mapper.Map(request, receita);

        _repository.Update(receita);

        await _unidadeDeTrabalho.Commit();

    }

    private static void Validar(RequisicaoReceitaJson request, Domain.Entidades.Receita receita, Domain.Entidades.Usuario usuarioLogado)
    {
        var validator = new RegistrarReceitaValidator();
        var result = validator.Validate(request);

        if (receita == null || usuarioLogado.Id != receita.UsuarioId)
        {
            throw new ErrosDeValidacaoException(new List<string> { ResourceMensagensDeErro.RECEITA_NAO_ENCONTRADA });
        }

        if (!result.IsValid)
        {
            var mensagens = result.Errors.Select(erro => erro.ErrorMessage).ToList();
            throw new ErrosDeValidacaoException(mensagens);
        }
    }
}
