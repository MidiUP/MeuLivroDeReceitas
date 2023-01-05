using AutoMapper;
using HashidsNet;
using MeuLivroDeReceitas.Comunicacao.Requisicoes;
using MeuLivroDeReceitas.Comunicacao.Respostas;

namespace MeuLivroDeReceitas.Application.Servicos.Automapper;

public class AutoMapperConfiguracao : Profile
{
    private readonly IHashids _hsashids;
    public AutoMapperConfiguracao(IHashids hsashids)
    {
        _hsashids = hsashids;

        RequisicaoParaEntidade();
        EntidadeParaRequisicao();
    }

    private void RequisicaoParaEntidade()
    {
        CreateMap<RequisicaoRegistrarUsuarioJson, Domain.Entidades.Usuario>()
            .ForMember(destino => destino.Senha, config => config.Ignore());

        CreateMap<RequisicaoReceitaJson, Domain.Entidades.Receita>()
            .ForMember(destino => destino.UsuarioId, config => config.Ignore());

        CreateMap<RequisicaoIngredienteJson, Domain.Entidades.Ingrediente>()
            .ForMember(destino => destino.ReceitaId, config => config.Ignore());
    }
    private void EntidadeParaRequisicao()
    {
        CreateMap<Domain.Entidades.Receita, RespostaReceitaJson>();

        CreateMap<Domain.Entidades.Ingrediente, RespostaIngredienteJson>();

        CreateMap<Domain.Entidades.Receita, RespostaReceitaDashboardJson>()
            .ForMember(destino => destino.QuantidadeIngredientes, config => config.MapFrom(origem => origem.Ingredientes.Count));
    }
}
