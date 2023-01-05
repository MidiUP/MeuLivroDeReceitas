using AutoMapper;
using MeuLivroDeReceitas.Application.Servicos.UsuarioLogado;
using MeuLivroDeReceitas.Comunicacao.Respostas;
using MeuLivroDeReceitas.Domain.Repositorios.Receita;

namespace MeuLivroDeReceitas.Application.UseCases.Receita.Recuperar;

public class RecuperarPorUsuarioUseCase : IRecuperarPorUsuarioUseCase
{
    private readonly IReceitaReadOnlyRepositorio _repositorio;
    private readonly IUsuarioLogado _usuarioLogado;
    private readonly IMapper _mapper;

    public RecuperarPorUsuarioUseCase(IReceitaReadOnlyRepositorio repositorio, IUsuarioLogado usuarioLogado, IMapper mapper)
    {
        _repositorio = repositorio;
        _usuarioLogado = usuarioLogado;
        _mapper = mapper;
    }

    public async Task<List<RespostaReceitaJson>> Executar()
    {

        var usuarioLogado = await _usuarioLogado.RecuperarUsuario();

        var receitas = await _repositorio.GetByUsuario(usuarioLogado.Id);

        if(receitas.Count > 0)
        {
            return receitas.Select((receita, index) =>
            {
                return _mapper.Map<RespostaReceitaJson>(receita);
            }).ToList();
        } else
        {
            return new List<RespostaReceitaJson> { };
        }

    }
}
