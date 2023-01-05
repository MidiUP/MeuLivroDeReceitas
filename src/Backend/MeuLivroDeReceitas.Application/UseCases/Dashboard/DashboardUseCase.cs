using AutoMapper;
using MeuLivroDeReceitas.Application.Servicos.UsuarioLogado;
using MeuLivroDeReceitas.Comunicacao.Requisicoes;
using MeuLivroDeReceitas.Comunicacao.Respostas;
using MeuLivroDeReceitas.Domain.Repositorios.Receita;

namespace MeuLivroDeReceitas.Application.UseCases.Dashboard;

public class DashboardUseCase : IDashboardUseCase
{
    private readonly IReceitaReadOnlyRepositorio _repository;
    private readonly IUsuarioLogado _usuarioLogado;
    private readonly IMapper _mapper;
    public DashboardUseCase(IReceitaReadOnlyRepositorio repository, IUsuarioLogado usuarioLogado, IMapper mapper)
    {
        _repository = repository;
        _usuarioLogado = usuarioLogado;
        _mapper = mapper;
    }
    public async Task<RespostaDashboardJson> Executar(RequisicaoDashboardJson request)
    {
        var usuarioLogado = await _usuarioLogado.RecuperarUsuario();

        var receitas = await _repository.GetByUsuario(usuarioLogado.Id);

        receitas = Filtrar(request, receitas);

        return new RespostaDashboardJson
        {
            Receitas = _mapper.Map<List<RespostaReceitaDashboardJson>>(receitas)
        };
    }

    private IList<Domain.Entidades.Receita> Filtrar(RequisicaoDashboardJson request, IList<Domain.Entidades.Receita> receitas)
    {
        if (request.Categoria.HasValue)
        {
            receitas = receitas.Where(r => r.Categoria == (Domain.Enum.Categoria) request.Categoria.Value).ToList();
        }

        if (string.IsNullOrEmpty(request.TituloOuIngrediente))
        {
            return receitas;
        }

        return receitas.Where(r => r.Titulo.ToLower().Contains(request.TituloOuIngrediente.ToLower()) 
        || r.Ingredientes.Any(  i => i.Produto.ToLower().Contains(request.TituloOuIngrediente.ToLower()))).ToList();
    }
}
