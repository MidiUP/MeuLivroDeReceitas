using AutoMapper;
using MeuLivroDeReceitas.Application.Servicos.UsuarioLogado;
using MeuLivroDeReceitas.Comunicacao.Requisicoes;
using MeuLivroDeReceitas.Comunicacao.Respostas;
using MeuLivroDeReceitas.Domain.Entidades;
using MeuLivroDeReceitas.Domain.Repositorios;
using MeuLivroDeReceitas.Domain.Repositorios.Ingrediente;
using MeuLivroDeReceitas.Domain.Repositorios.Receita;
using MeuLivroDeReceitas.Exceptions.ExceptionsBase;

namespace MeuLivroDeReceitas.Application.UseCases.Receita.Registrar;

public class RegistrarReceitaUseCase : IRegistrarReceitaUseCase
{
    private readonly IReceitaWriteOnlyRepositorio _repository;
    private readonly IMapper _mapper;
    private readonly IUnidadeDeTrabalho _unidadeDeTrabalho;
    private readonly IUsuarioLogado _usuarioLogado;
    private readonly IIngredienteWriteOnlyRepository _repositoryIngrediente;
    public RegistrarReceitaUseCase(IReceitaWriteOnlyRepositorio repository, IMapper mapper, IUnidadeDeTrabalho unidadeDeTrabalho, IUsuarioLogado usuarioLogado, IIngredienteWriteOnlyRepository repositoryIngrediente)
    {
        _repository = repository;
        _mapper = mapper;
        _unidadeDeTrabalho = unidadeDeTrabalho;
        _usuarioLogado = usuarioLogado;
        _repositoryIngrediente = repositoryIngrediente;
    }
    public async Task<RespostaReceitaJson> Executar(RequisicaoReceitaJson request)
    {
        Validar(request);

        var usuarioLogado = await _usuarioLogado.RecuperarUsuario();

        var receita = _mapper.Map<Domain.Entidades.Receita>(request);
        receita.UsuarioId = usuarioLogado.Id;

        receita = await _repository.Adicionar(receita);
        await _unidadeDeTrabalho.Commit();

        return _mapper.Map<RespostaReceitaJson>(receita);

        var idReceita = receita.Id;

        var ingredientes = request.Ingredientes.Select((i, index) =>
        {
            var ingrediente = _mapper.Map<Ingrediente>(i);
            ingrediente.ReceitaId = idReceita;
            return ingrediente;
        }).ToList();

        await _repositoryIngrediente.AdicionarIngredientes(ingredientes);
        await _unidadeDeTrabalho.Commit();
    }

    public void Validar(RequisicaoReceitaJson request)
    {
        var validator = new RegistrarReceitaValidator();
        var result = validator.Validate(request);
        if(!result.IsValid)
        {
            var mensagens = result.Errors.Select(erro => erro.ErrorMessage).ToList();
            throw new ErrosDeValidacaoException(mensagens);
        }
    }
}
