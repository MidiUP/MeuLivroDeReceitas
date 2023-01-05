using FluentValidation;
using MeuLivroDeReceitas.Comunicacao.Enum;
using MeuLivroDeReceitas.Comunicacao.Requisicoes;
using MeuLivroDeReceitas.Exceptions;

namespace MeuLivroDeReceitas.Application.UseCases.Receita.Registrar;

public class RegistrarReceitaValidator : AbstractValidator<RequisicaoReceitaJson>
{
    public RegistrarReceitaValidator()
    {
        RuleFor(c => c.Titulo).NotEmpty().WithMessage(ResourceMensagensDeErro.TITULO_RECEITA_EMBRANCO);

        
        RuleFor(c => c.Categoria).IsInEnum().WithMessage(ResourceMensagensDeErro.CATEGORIA_INEXISTENTE);
        

        RuleFor(c => c.ModoPreparo).NotEmpty().WithMessage(ResourceMensagensDeErro.MODO_PREPARO_RECEITA_EMBRANCO);

        RuleFor(c => c.Ingredientes).NotEmpty().WithMessage(ResourceMensagensDeErro.INGREDIENTES_RECEITA_EMBRANCO);
        RuleForEach(c => c.Ingredientes).ChildRules(ingrediente =>
        {
            ingrediente.RuleFor(x => x.Quantidade).NotEmpty();
            ingrediente.RuleFor(x => x.Produto).NotEmpty();
        });

        RuleFor(c => c.Ingredientes).Custom((ingredientes, contexto) =>
        {
            var produtosDistintos = ingredientes.Select(x => x.Produto).ToList().Distinct();
            if(produtosDistintos.Count() != ingredientes.Count())
            {
                contexto.AddFailure(new FluentValidation.Results.ValidationFailure("Ingredientes", ""));
            }
        });
  
    }
}

/*
  public string Titulo { get; set; }
    public Categoria Categoria { get; set; }
    public string ModoPreparo { get; set; }

    public List<RequisicaoRegistrarIngredienteJson> Ingredientes { get; set; }
 */
