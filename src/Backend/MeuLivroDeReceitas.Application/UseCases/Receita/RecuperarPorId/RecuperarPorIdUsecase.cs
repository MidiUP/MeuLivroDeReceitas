using AutoMapper;
using MeuLivroDeReceitas.Application.Servicos.UsuarioLogado;
using MeuLivroDeReceitas.Comunicacao.Respostas;
using MeuLivroDeReceitas.Domain.Repositorios.Receita;
using MeuLivroDeReceitas.Exceptions;
using MeuLivroDeReceitas.Exceptions.ExceptionsBase;

namespace MeuLivroDeReceitas.Application.UseCases.Receita.RecuperarPorId;

public class RecuperarPorIdUsecase : IRecuperarPorIdUsecase
{
    private readonly IReceitaReadOnlyRepositorio _repository;
    private readonly IMapper _mapper;
    private readonly IUsuarioLogado _usuarioLogado;
    public RecuperarPorIdUsecase (IReceitaReadOnlyRepositorio repository, IMapper mapper, IUsuarioLogado usuarioLogado)
    {
        _mapper= mapper;
        _repository = repository;
        _usuarioLogado = usuarioLogado;
    }

    public async Task<RespostaReceitaJson> Executar(long id)
    {
        var receita = await _repository.GetById(id);
        var usuarioLogado = await _usuarioLogado.RecuperarUsuario();

        if (receita == null || usuarioLogado.Id != receita.UsuarioId)
        {
            throw new ErrosDeValidacaoException(new List<string> {ResourceMensagensDeErro.RECEITA_NAO_ENCONTRADA});
        }

        return _mapper.Map<RespostaReceitaJson>(receita);
    }
}
